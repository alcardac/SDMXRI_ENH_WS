// -----------------------------------------------------------------------
// <copyright file="AuthReferenceCommandBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-07-03
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
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The authorization reference command builder.
    /// </summary>
    internal class AuthReferenceCommandBuilder : IAuthCommandBuilder<ReferenceSqlQuery>
    {
        /// <summary>
        /// The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        /// <summary>
        /// The  dataflow filter.
        /// </summary>
        private readonly DataflowFilter _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthReferenceCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="filter">
        /// The dataflow filter.
        /// </param>
        public AuthReferenceCommandBuilder(Database mappingStoreDb, DataflowFilter filter)
        {
            this._mappingStoreDb = mappingStoreDb;
            this._filter = filter;
        }

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="SqlQueryBase"/> based class to build from.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand Build(ReferenceSqlQuery buildFrom, IList<IMaintainableRefObject> allowedDataflows)
        {
            IList<DbParameter> parameters = new List<DbParameter>();
            var sqlCommand = new StringBuilder();
            var whereState = WhereState.Nothing;
            if (this._filter == DataflowFilter.Production)
            {
                SqlHelper.AddWhereClause(sqlCommand, whereState, CategorisationConstant.ProductionWhereClause);
                whereState = WhereState.And;
            }

            SecurityHelper.AddWhereClauses(null, this._mappingStoreDb, sqlCommand, parameters, allowedDataflows, whereState);

            var sqlQuery = string.Format(CultureInfo.InvariantCulture, buildFrom.QueryInfo.QueryFormat, sqlCommand);
            return this._mappingStoreDb.GetSqlStringCommand(sqlQuery, parameters);
        }
    }
}