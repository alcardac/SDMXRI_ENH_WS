// -----------------------------------------------------------------------
// <copyright file="AnnotationCommandBuilder.cs" company="EUROSTAT">
//   Date Created : 2014-11-06
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
namespace Estat.Sri.MappingStoreRetrieval
{
    using System.Data;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    /// <summary>
    /// The annotation command builder.
    /// </summary>
    internal class AnnotationCommandBuilder : ICommandBuilder<PrimaryKeySqlQuery>
    {
        /// <summary>
        /// The _database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationCommandBuilder"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public AnnotationCommandBuilder(Database database)
        {
            this._database = database;
        }

        /// <summary>
        /// Builds the specified SQL query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns>The <see cref="DbCommand"/> for the specified <paramref name="sqlQuery"/></returns>
        public DbCommand Build(PrimaryKeySqlQuery sqlQuery)
        {
            return this._database.GetSqlStringCommandFormat(sqlQuery.QueryInfo.ToString(), this._database.CreateInParameter("p_id", DbType.Int64, sqlQuery.PrimaryKeyValue));
        }
    }
}