// -----------------------------------------------------------------------
// <copyright file="IsFinalQueryBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-04-08
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
namespace Estat.Sri.MappingStore.Store.Builder
{
    using System;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;

    /// <summary>
    /// The is final query builder.
    /// </summary>
    public class IsFinalQueryBuilder : IBuilder<string, TableInfo>
    {
        /// <summary>
        /// The SQL query.
        /// </summary>
        private const string SqlQuery = "select A.ART_ID as primaryKey, A.IS_FINAL as isFinal from ARTEFACT A INNER JOIN {0} T ON A.ART_ID = T.{1} WHERE A.ID = {2} and A.AGENCY = {3} and ( ({4} is not null and dbo.isEqualVersion(A.VERSION1, A.VERSION2, A.VERSION3, {4}, {5}, {6})=1 ) or (({4} is null) and (SELECT COUNT(*) FROM ARTEFACT A2 INNER JOIN {0} T2 ON A2.ART_ID = T2.{1} where A2.ID=A.ID AND A2.AGENCY=A.AGENCY AND dbo.isGreaterVersion(A2.VERSION1, A2.VERSION2, A2.VERSION3, A.VERSION1, A.VERSION2, A.VERSION3)=1 ) = 0))";

        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsFinalQueryBuilder"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        public IsFinalQueryBuilder(Database database)
        {
            this._database = database;
        }

        /// <summary>
        /// Builds an object of type <see cref="string"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An <see cref="TableInfo"/> to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="string"/>
        /// </returns>
        public string Build(TableInfo buildFrom)
        {
            if (buildFrom == null)
            {
                throw new ArgumentNullException("buildFrom");
            }

            var id = this._database.BuildParameterName("id");
            var agency = this._database.BuildParameterName("agency");
            var version1 = this._database.BuildParameterName("version1");
            var version2 = this._database.BuildParameterName("version2");
            var version3 = this._database.BuildParameterName("version3");

            return string.Format(CultureInfo.InvariantCulture, SqlQuery, buildFrom.Table, buildFrom.PrimaryKey, id, agency, version1, version2, version3);
        }
    }
}