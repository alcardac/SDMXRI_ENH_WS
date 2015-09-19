// -----------------------------------------------------------------------
// <copyright file="TimeExpressionEntity.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     This class holds the TIME_TRANSCODING for Time transcoding
    /// </summary>
    public class TimeExpressionEntity
    {
        #region Static Fields

        /// <summary>
        ///     Regular expression to parse TIME_TRANSCODING.EXPRESION from Mapping Store
        /// </summary>
        private static readonly Regex _timeExpression =
            new Regex("(year=(?<year_start>-?[0-9]+),(?<year_len>[0-9]+);)?(period=(?<period_start>[0-9]+),(?<period_len>-?[0-9]+);)?(datetime=(?<datetime>1);)?");

        #endregion

        #region Public Properties

        /// <summary>
        ///  Gets the Regular expression to parse TIME_TRANSCODING.EXPRESION from Mapping Store
        /// </summary>
        public static Regex TimeExpressionRegex
        {
            get
            {
                return _timeExpression;
            }
        }

        /// <summary>
        ///     Gets the year DATASET_COLUMN.COL_ID from <c>TIME_TRANSCODING.DATE_COL_ID</c>
        /// </summary>
        public long DateColumnSysId { get; private set; }

        /// <summary>
        ///     Gets the frequency value from <c>TIME_TRANSCODING.FREQ</c>
        /// </summary>
        public TimeFormatEnumType Freq { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the isDateTime value from TRANSCODING.EXPRESSION is set
        /// </summary>
        public bool IsDateTime { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the same column is used for both <see cref="YearColumnSysId" /> and
        ///     <see
        ///         cref="PeriodColumnSysId" />
        ///     .
        /// </summary>
        public bool OneColumnMapping { get; private set; }

        /// <summary>
        ///     Gets the <c>periodColid </c> (The DATASET_COLUMN.COL_ID) value from <c>TIME_TRANSCODING.PERIOD_COL_ID</c>
        /// </summary>
        public long PeriodColumnSysId { get; private set; }

        /// <summary>
        ///     Gets the periodLen value from TRANSCODING.EXPRESSION
        /// </summary>
        public int PeriodLength { get; private set; }

        /// <summary>
        ///     Gets the periodStart value from TRANSCODING.EXPRESSION
        /// </summary>
        public int PeriodStart { get; private set; }

        /// <summary>
        ///     Gets the year DATASET_COLUMN.COL_ID from <c>TIME_TRANSCODING.YEAR_COL_ID</c>
        /// </summary>
        public long YearColumnSysId { get; private set; }

        /// <summary>
        ///     Gets the yearLen value from TRANSCODING.EXPRESSION
        /// </summary>
        public int YearLength { get; private set; }

        /// <summary>
        ///     Gets the yearStart value from TRANSCODING.EXPRESSION
        /// </summary>
        public int YearStart { get; private set; }

        /// <summary>
        /// Gets the transcoding rules.
        /// </summary>
        public TranscodingRulesEntity TranscodingRules { get; private set; }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create an <see cref="TimeExpressionEntity"/> instance from the specified <paramref name="timeTranscoding"/>
        /// </summary>
        /// <param name="timeTranscoding">
        /// The Mapping store Time Transcoding
        /// </param>
        /// <returns>
        /// An <see cref="TimeExpressionEntity"/>
        /// </returns>
        public static TimeExpressionEntity CreateExpression(TimeTranscodingEntity timeTranscoding)
        {
            var expr = new TimeExpressionEntity();
            Match match = TimeExpressionRegex.Match(timeTranscoding.Expression);
            expr.Freq = TimeFormat.GetTimeFormatFromCodeId(timeTranscoding.FrequencyValue).EnumType;
            expr.IsDateTime = match.Groups["datetime"].Value.Equals("1");
            expr.TranscodingRules = timeTranscoding.TranscodingRules;

            if (!expr.IsDateTime)
            {
                expr.YearStart = Convert.ToInt32(match.Groups["year_start"].Value, CultureInfo.InvariantCulture);
                expr.YearLength = Convert.ToInt32(match.Groups["year_len"].Value, CultureInfo.InvariantCulture);
                if (expr.YearLength == 0)
                {
                    expr.YearLength = 4;
                }

                expr.YearColumnSysId = timeTranscoding.YearColumnId;
                expr.PeriodStart = 0;
                expr.PeriodLength = 0;
                expr.PeriodColumnSysId = 0;
                if (expr.Freq != TimeFormatEnumType.Year)
                {
                    expr.PeriodColumnSysId = timeTranscoding.PeriodColumnId;
                    expr.PeriodStart = Convert.ToInt32(match.Groups["period_start"].Value, CultureInfo.InvariantCulture);
                    expr.PeriodLength = Convert.ToInt32(match.Groups["period_len"].Value, CultureInfo.InvariantCulture);
                    expr.OneColumnMapping = expr.YearColumnSysId == expr.PeriodColumnSysId;
                }
                else
                {
                    expr.OneColumnMapping = true;
                }
            }
            else
            {
                expr.DateColumnSysId = timeTranscoding.DateColumnId;
            }

            return expr;
        }

        #endregion
    }
}