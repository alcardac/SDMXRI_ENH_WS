// -----------------------------------------------------------------------
// <copyright file="ComponentMappingNto1.cs" company="EUROSTAT">
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
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// Handles the mapping between N components to 1 column where N&gt;1
    /// </summary>
    internal class ComponentMappingNto1 : ComponentMapping, IComponentMapping
    {
        #region Fields

        /// <summary>
        /// The component index from <see cref="TranscodingRulesEntity.ComponentAsKeyPosition"/>
        /// </summary>
        private readonly int _componentIndex;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMappingNto1"/> class.
        /// </summary>
        /// <param name="componentIndex">
        /// The component position from <see cref="TranscodingRulesEntity.ComponentAsKeyPosition"/>.
        /// </param>
        public ComponentMappingNto1(int componentIndex)
        {
            this._componentIndex = componentIndex;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping and the condition value from SDMX Query which is transcoded
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the sdmx query 
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the sdmx query, "=" by default
        /// </param>
        /// <returns>
        /// A SQL where clause for the column of the mapping 
        /// </returns>
        public string GenerateComponentWhere(string conditionValue, string operatorValue = "=")
        {
            var ret = new StringBuilder();

            IEnumerable<string> localCodes =
                this.Mapping.Transcoding.TranscodingRules.GetLocalCodes(this.Component.SysId, conditionValue);

            ret.Append(" (");
            var mappedClause = new List<string>();

            foreach (string localCode in localCodes)
            {
                string mappedId = this.Mapping.Columns[0].Name;
                string mappedValue = localCode;
                mappedClause.Add(SqlOperatorComponent(mappedId, mappedValue, operatorValue));
                //// mappedClause.Add(string.Format(CultureInfo.InvariantCulture, "{0} " + operatorValue + " '{1}' ", mappedId, mappedValue));
            }

            ret.Append(string.Join(" OR ", mappedClause.ToArray()));
            ret.Append(" )");

            return ret.ToString();
        }

        /// <summary>
        /// Maps the column of the mapping to the components of this <see cref="ComponentMappingNto1"/> object and transcodes it.
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column. 
        /// </param>
        /// <returns>
        /// The value of the component or null if no transcoding rule for the column values is found 
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            string ret = null;
            this.BuildOrdinals(reader);
            ColumnOrdinal column = this.ColumnOrdinals[0];
            var resultCodes = new CodeCollection { DataReaderHelper.GetString(reader, column.Value) };

            CodeCollection transcodedCodes = this.Mapping.Transcoding.TranscodingRules.GetDsdCodes(resultCodes);

            if (transcodedCodes != null && this._componentIndex < transcodedCodes.Count)
            {
                ret = transcodedCodes[this._componentIndex];
            }

            return ret;
        }

        #endregion
    }
}
