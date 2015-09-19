// -----------------------------------------------------------------------
// <copyright file="ComponentMapping1to1T.cs" company="EUROSTAT">
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
#define MAT200

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// Handles 1-1 mappings with transconding
    /// </summary>
    internal class ComponentMapping1To1T : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields
        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query which is transcoded 
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the sdmx query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the sdmx query, "=" by default
        /// </param>
        /// <returns>
        /// A SQL where clause for the columns of the mapping
        /// </returns>
        public string GenerateComponentWhere(string conditionValue, string operatorValue = "=")
        {
            var ret = new StringBuilder();
            ret.Append(" (");

            CodeSetCollection localCodesSet =
                this.Mapping.Transcoding.TranscodingRules.GetLocalCodes(new CodeCollection(new[] { conditionValue }));
            DataSetColumnEntity column = this.Mapping.Columns[0];
            string mappedValue = EscapeString(conditionValue);

            // TODO check if columnIndex == 0 always
            int columnIndex = 0; // was this.Mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition[column.Name];
            if (localCodesSet.Count > 0)
            {
                for (int i = 0; i < localCodesSet.Count; i++)
                {
                    Collection<string> localCodes = localCodesSet[i];
                    if (localCodes != null && localCodes.Count > 0)
                    {
                        mappedValue = localCodes[columnIndex];
                    }

                    if (i != 0)
                    {
                        ret.Append(" or ");
                    }

                    ret.Append("( " + SqlOperatorComponent(column.Name, mappedValue, operatorValue) + ")");
                    //ret.AppendFormat("( {0} " + operatorValue + " '{1}' )", column.Name, mappedValue);
                }
            }
            else
            {
                ret.Append(" " + SqlOperatorComponent(column.Name, mappedValue, operatorValue));
                //ret.AppendFormat(" {0} " + operatorValue + " '{1}' ", column.Name, mappedValue);
            }

            ret.Append(") ");
            return ret.ToString();
        }

        /// <summary>
        /// Maps the column of the mapping to the component of this ComponentMapping1to1T object
        /// and transcodes it. 
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column.
        /// </param>
        /// <returns>
        /// The value of the component or null if no transcoding rule for the column values is found
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            var resultCodes = new CodeCollection();
            this.BuildOrdinals(reader);
            var column = this.ColumnOrdinals[0];
            string columnValue = DataReaderHelper.GetString(reader, column.Value);
            resultCodes.Add(columnValue);
            Collection<string> transcodedCodes = this.Mapping.Transcoding.TranscodingRules.GetDsdCodes(resultCodes);
            string ret = null;
            if (transcodedCodes != null && transcodedCodes.Count > 0)
            {
                ret = transcodedCodes[0];
            }

            return ret;
        }

        #endregion
    }
}
