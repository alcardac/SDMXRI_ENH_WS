// -----------------------------------------------------------------------
// <copyright file="ComponentMapping1C.cs" company="EUROSTAT">
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

    /// <summary>
    /// Handles constant mapping
    /// </summary>
    internal class ComponentMapping1C : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the constant value used in this mapping
        /// and the condition value from SDMX Query which is transcoded
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the SDMX query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the SDMX query, "=" by default
        /// </param>
        /// <returns>
        /// A SQL where clause for the column of the mapping
        /// </returns>
        public string GenerateComponentWhere(string conditionValue, string operatorValue = "=")
        {
            var quotedConstantValue = string.Format(CultureInfo.InvariantCulture, "'{0}'", this.Mapping.Constant);
            var escapeString = EscapeString(conditionValue);
            if (operatorValue.Contains("value"))
            {
                return string.Format(CultureInfo.InvariantCulture, " ( {0} ) ", SqlOperatorComponent(quotedConstantValue, escapeString, operatorValue));
            }

            return string.Format(CultureInfo.InvariantCulture, " ( {0} {2} '{1}' ) ", quotedConstantValue, escapeString, operatorValue);
        }

        /// <summary>
        /// Maps the constant value of the mapping to the component of this ComponentMapping1C object 
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column.
        /// </param>
        /// <returns>
        /// The constant value
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            return this.Mapping.Constant;
        }
        #endregion
    }
}