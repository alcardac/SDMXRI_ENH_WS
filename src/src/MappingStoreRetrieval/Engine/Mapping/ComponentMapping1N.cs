// -----------------------------------------------------------------------
// <copyright file="ComponentMapping1N.cs" company="EUROSTAT">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// Handles the mapping between 1 component and N columns where N &gt; 1
    /// </summary>
    internal class ComponentMapping1N : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields
        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query which is transcoded
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the SDMX query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the SDMX query, "=" by default
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
            if (localCodesSet.Count > 0)
            {
                for (int i = 0; i < localCodesSet.Count; i++)
                {
                    if (i != 0)
                    {
                        ret.Append(" OR ");
                    }

                    ret.Append(" (");
                    Collection<string> localCodes = localCodesSet[i];

                    // component to columns
                    var mappedClause = new List<string>();
                    foreach (DataSetColumnEntity column in this.Mapping.Columns)
                    {
                        string mappedId = column.Name;
                        string mappedValue = EscapeString(conditionValue);
                        int columnPosition = this.Mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition[column.SysId];
                        if (localCodes != null && columnPosition < localCodes.Count)
                        {
                            mappedValue = localCodes[columnPosition];
                        }

                        mappedClause.Add(SqlOperatorComponent(mappedId, mappedValue, operatorValue));
                        //mappedClause.Add(
                        //    string.Format(CultureInfo.InvariantCulture, "{0} " + operatorValue + " '{1}' ", mappedId, mappedValue));
                    }

                    ret.Append(string.Join(" AND ", mappedClause.ToArray()));
                    ret.Append(" ) ");
                }
            }
            else
            {
                var mappedClause = new List<string>();
                foreach (DataSetColumnEntity column in this.Mapping.Columns)
                {
                    string mappedId = column.Name;
                    string mappedValue = EscapeString(conditionValue);
                    mappedClause.Add(SqlOperatorComponent(mappedId, mappedValue, operatorValue));
                    //mappedClause.Add(string.Format(CultureInfo.InvariantCulture, "{0} " + operatorValue + " '{1}' ", mappedId, mappedValue));
                }

                ret.Append(string.Join(" AND ", mappedClause.ToArray()));
            }

            ret.Append(" )");
            return ret.ToString();
        }

        /// <summary>
        /// Maps the columns of the mapping to the component of this ComponentMapping1N object
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
            var resultCodes = new string[this.Mapping.Columns.Count];
            this.BuildOrdinals(reader);

            foreach (var column in this.ColumnOrdinals)
            {
                resultCodes[column.ColumnPosition] =
                    DataReaderHelper.GetString(reader, column.Value);
            }

            Collection<string> transcodedCodes =
                this.Mapping.Transcoding.TranscodingRules.GetDsdCodes(new CodeCollection(resultCodes));
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