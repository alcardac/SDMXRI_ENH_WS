// -----------------------------------------------------------------------
// <copyright file="AuthArtefactCommandBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-04-15
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
    using System.Data.Common;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The authorization aware artefact command builder.
    /// </summary>
    internal abstract class AuthArtefactCommandBuilder : ArtefactCommandBuilder, IAuthCommandBuilder<ArtefactSqlQuery>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthArtefactCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        protected AuthArtefactCommandBuilder(Database mappingStoreDb)
            : base(mappingStoreDb)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand Build(ArtefactSqlQuery buildFrom, IList<IMaintainableRefObject> allowedDataflows)
        {
            string sqlQuery = this.GetSqlQuery(buildFrom);
            var sqlCommand = new StringBuilder(sqlQuery);
            IList<DbParameter> parameters = this.CreateArtefactWhereClause(buildFrom.MaintainableRef, sqlCommand, buildFrom.QueryInfo.WhereStatus, allowedDataflows);

            if (!string.IsNullOrWhiteSpace(buildFrom.QueryInfo.OrderBy))
            {
                sqlCommand.Append(buildFrom.QueryInfo.OrderBy);
            }

            return this.MappingStoreDB.GetSqlStringCommand(sqlCommand.ToString(), parameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the SQL query.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The SQL query.
        /// </returns>
        protected virtual string GetSqlQuery(ArtefactSqlQuery query)
        {
            return query.QueryInfo.QueryFormat;
        }

        /// <summary>
        /// Create the WHERE clause from the <paramref name="maintainableRef"/>  and write it to <paramref name="sqlCommand"/>
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable Ref.
        /// </param>
        /// <param name="sqlCommand">
        /// The output string buffer
        /// </param>
        /// <param name="whereState">
        /// the current state of the WHERE clause in <paramref name="sqlCommand"/>
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The list of <see cref="DbParameter"/>
        /// </returns>
        protected abstract IList<DbParameter> CreateArtefactWhereClause(
            IMaintainableRefObject maintainableRef, StringBuilder sqlCommand, WhereState whereState, IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}