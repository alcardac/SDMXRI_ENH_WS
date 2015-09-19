// -----------------------------------------------------------------------
// <copyright file="ReferencedSqlQueryBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    /// <summary>
    /// The item SQL query builder.
    /// </summary>
    internal class ReferencedSqlQueryBuilder : ISqlQueryInfoBuilder<string>
    {
        #region Fields

        /// <summary>
        ///     The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        /// <summary>
        ///     The _order by.
        /// </summary>
        private readonly string _orderBy;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencedSqlQueryBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="orderBy">
        /// The order by
        /// </param>
        public ReferencedSqlQueryBuilder(Database mappingStoreDb, string orderBy)
        {
            this._mappingStoreDb = mappingStoreDb;
            this._orderBy = orderBy;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="SqlQueryInfo"/> from the specified <paramref name="queryFormat"/>
        /// </summary>
        /// <param name="queryFormat">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="SqlQueryInfo"/> build from the specified <paramref name="queryFormat"/>
        /// </returns>
        public SqlQueryInfo Build(string queryFormat)
        {
            string paramId = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            string query = string.Format(CultureInfo.InvariantCulture, queryFormat, paramId);
            return new SqlQueryInfo { QueryFormat = query, OrderBy = this._orderBy };
        }

        #endregion
    }
}