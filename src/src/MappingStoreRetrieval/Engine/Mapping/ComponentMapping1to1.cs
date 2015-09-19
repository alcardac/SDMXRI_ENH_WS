// -----------------------------------------------------------------------
// <copyright file="ComponentMapping1to1.cs" company="EUROSTAT">
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
    using System;
    using System.Data;
    using System.Globalization;

    /// <summary>
    /// Handles 1-1 mappings without transconding
    /// </summary>
    internal class ComponentMapping1To1 : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields

        /// <summary>
        /// The position of the column of this mapping inside the row
        /// in the reader
        /// </summary>
        private int _columnOrdinal = -1;

        /// <summary>
        /// The type of the column of this mapping E.g. string, float, int e.t.c.
        /// </summary>
        private Type _fieldType;

        /// <summary>
        /// The last data reader
        /// </summary>
        private IDataReader _lastReader;

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query as it is
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the SDMX query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the sdmx query, "=" by default
        /// </param>
        /// <returns>
        /// A SQL where clause for the column of the mapping
        /// </returns>
        public string GenerateComponentWhere(string conditionValue, string operatorValue = "=")
        {
            return " ( " + SqlOperatorComponent(this.Mapping.Columns[0].Name, EscapeString(conditionValue), operatorValue) + ") ";
            //return string.Format(CultureInfo.InvariantCulture, " ( {0} " + operatorValue + " '{1}' ) ", this.Mapping.Columns[0].Name, EscapeString(conditionValue));
        }

        /// <summary>
        /// Maps the column of the mapping to the component of this ComponentMapping1to1 object 
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column.
        /// </param>
        /// <returns>
        /// The value of the component or String.Empty in case the column value is null
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            if (!ReferenceEquals(reader, this._lastReader))
            {
                this._columnOrdinal = -1;
                this._lastReader = reader;
            }

            if (this._columnOrdinal == -1)
            {
                this._columnOrdinal = reader.GetOrdinal(this.Mapping.Columns[0].Name);
                this._fieldType = reader.GetFieldType(this._columnOrdinal);
            }

            string val = string.Empty;
            if (!reader.IsDBNull(this._columnOrdinal))
            {
                if (this._fieldType == typeof(double))
                {
                    val = reader.GetDouble(this._columnOrdinal).ToString(CultureInfo.InvariantCulture);
                }
                else if (this._fieldType == typeof(float))
                {
                    val = reader.GetFloat(this._columnOrdinal).ToString(CultureInfo.InvariantCulture);
                }
                else if (this._fieldType == typeof(string))
                {
                    val = reader.GetString(this._columnOrdinal);
                }
                else
                {
                    val = Convert.ToString(reader.GetValue(this._columnOrdinal), CultureInfo.InvariantCulture);
                }
            }

            return val;
        }

        #endregion
    }
}