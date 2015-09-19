// -----------------------------------------------------------------------
// <copyright file="IComponentMapping.cs" company="EUROSTAT">
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
    /// <summary>
    /// A common interface for all component mapping classes
    /// </summary>
    public interface IComponentMapping : IMapping
    {
        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query
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
        string GenerateComponentWhere(string conditionValue, string operatorValue = "=");

        #endregion
    }
}