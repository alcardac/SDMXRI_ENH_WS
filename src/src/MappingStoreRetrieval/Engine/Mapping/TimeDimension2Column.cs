﻿// -----------------------------------------------------------------------
// <copyright file="TimeDimension2Column.cs" company="EUROSTAT">
//   Date Created : 2013-04-10
//   Copyright (c) 2009, 2015 by the European Commission, represented by Eurostat.   All rights reserved.
// 
// Licensed under the EUPL, Version 1.1 or – as soon they
// will be approved by the European Commission - subsequent
// versions of the EUPL (the "Licence");
// You may not use this work except in compliance with the
// Licence.
// You may obtain a copy of the Licence at:
// 
// https://joinup.ec.europa.eu/software/page/eupl 
// 
// Unless required by applicable law or agreed to in
// writing, software distributed under the Licence is
// distributed on an "AS IS" basis,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied.
// See the Licence for the specific language governing
// permissions and limitations under the Licence.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Data;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    /// <summary>
    /// This Time Dimension Transcoding class is used for 1-2 mappings between
    /// a Time Dimension and two dissemination columns as generated from DATASET.QUERY
    /// </summary>
    internal class TimeDimension2Column : TimeDimensionMapping, ITimeDimensionMapping
    {
        #region Constants and Fields
        /// <summary>
        /// Holds the current local codes for period
        /// </summary>
        private readonly CodeCollection _periodLocalCode;

        /// <summary>
        /// The where builder.
        /// </summary>
        private readonly TimeTranscodingWhereBuilder _whereBuilder;

        /// <summary>
        /// The field ordinals
        /// </summary>
        private readonly TimeTranscodingFieldOrdinal _fieldOrdinals;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDimension2Column"/> class. 
        /// </summary>
        /// <param name="mapping">
        /// The time dimension mapping
        /// </param>
        /// <param name="expression">
        /// The TRANSCODING.EXPRESSION contents
        /// </param>
        /// <param name="databaseType">
        /// The dissemination database vendor from  DB_CONNECTION.DB_TYPE at Mapping Store database. It is used to determine the substring command to use
        /// </param>
        /// <exception cref="TranscodingException">
        /// Occurs when transcoding cannot performed due to incorrect mapping store data
        /// </exception>
        public TimeDimension2Column(MappingEntity mapping, TimeExpressionEntity expression, string databaseType)
            : base(mapping, expression, databaseType)
        {
            string yearOnlyWhereFormat;
            string yearOnlyStart;
            string yearOnlyEnd;
            string whereFormat;
            this._periodLocalCode = new CodeCollection();
            string yearColumn = GetColumnName(mapping, expression.YearColumnSysId);
            string periodColumn = GetColumnName(mapping, expression.PeriodColumnSysId);

            string periodClause = expression.PeriodLength == 0
                                      ? string.Format(FormatProvider, "( {0} = '{1}' )", periodColumn, "{0}")
                                      : this.CreateSubStringClause(periodColumn, expression.PeriodStart + 1, expression.PeriodLength, "=");

            if (expression.YearLength == 0)
            {
                yearOnlyStart = string.Format(FormatProvider, " ( {0} >= '{1}' )", yearColumn, "{0}");
                yearOnlyEnd = string.Format(FormatProvider, " ( {0} <= '{1}' )", yearColumn, "{0}");
                yearOnlyWhereFormat = string.Format(FormatProvider, "( {0} = '{1}' )", yearColumn, "{0}");

                // whereFormat = String.Format(FormatProvider,"({0} = '{1}' and {2} )",yearColumn, "{0}",periodClause);
                whereFormat = periodClause;
            }
            else
            {
                yearOnlyStart = this.CreateSubStringClause(yearColumn, expression.YearStart + 1, expression.YearLength, ">=");
                yearOnlyEnd = this.CreateSubStringClause(yearColumn, expression.YearStart + 1, expression.YearLength, "<=");

                whereFormat = periodClause;
                yearOnlyWhereFormat = this.CreateSubStringClause(yearColumn, expression.YearStart + 1, expression.YearLength, "=");
            }

            this._whereBuilder = new TimeTranscodingWhereBuilder(this.Periodicity, this.Expression, whereFormat, yearOnlyEnd, yearOnlyStart, yearOnlyWhereFormat);
            this._fieldOrdinals = new TimeTranscodingFieldOrdinal(mapping, this.Expression);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Query where condition from the SDMX Query TimeBean <see cref="ISdmxDate"/>
        /// </summary>
        /// <param name="dateFrom">The start time period</param>
        /// <param name="dateTo">The end time period</param>
        /// <returns>
        /// The string containing SQL Query where condition
        /// </returns>
        public string GenerateWhere(ISdmxDate dateFrom, ISdmxDate dateTo)
        {
            return this._whereBuilder.WhereBuild(dateFrom, dateTo);
        }

        /// <summary>
        /// Transcodes the time period returned by the local database to SDMX Time period
        /// </summary>
        /// <param name="reader">
        /// The data reader reading the Dissemination database
        /// </param>
        /// <returns>
        /// The transcoded time period, as in SDMX Time period type
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            this._fieldOrdinals.BuildOrdinal(reader);
            string year = DataReaderHelper.GetString(reader, this._fieldOrdinals.YearOrdinal);
            if (this.Expression.YearLength > 0)
            {
                year = year.Substring(this.Expression.YearStart, this.Expression.YearLength);
            }

            string period = DataReaderHelper.GetString(reader, this._fieldOrdinals.PeriodOrdinal);
            if (this.Expression.PeriodLength > 0)
            {
                int rowPeriodLen = this.Expression.PeriodLength;
                if (this.Expression.PeriodLength + this.Expression.PeriodStart > period.Length)
                {
                    rowPeriodLen = period.Length - this.Expression.PeriodStart;
                }

                period = period.Substring(this.Expression.PeriodStart, rowPeriodLen);
            }

            this._periodLocalCode.Clear();
            this._periodLocalCode.Add(period);
            CodeCollection periodDsdCode = this.Expression.TranscodingRules.GetDsdCodes(this._periodLocalCode);
            if (periodDsdCode == null)
            {
                return null; // MAT-495

                // periodDsdCode = periodLocalCode;
            }

            string ret = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", year, periodDsdCode[0]);

            // ret = _timePeriodTranscoding[String.Format(CultureInfo.InvariantCulture,"{0}-{1}", year, period)];
            return ret;
        }

        #endregion
    }
}
