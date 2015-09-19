// -----------------------------------------------------------------------
// <copyright file="ArtefactSqlQuery.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using Estat.Sri.MappingStoreRetrieval.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// The artefact SQL Query.
    /// </summary>
    internal class ArtefactSqlQuery : SqlQueryBase
    {
        #region Fields

        /// <summary>
        /// The artefact SQL builder.
        /// </summary>
        private static readonly ArtefactSqlBuilder _artefactSqlBuilder = new ArtefactSqlBuilder();

        /// <summary>
        ///     The _maintainable ref.
        /// </summary>
        private readonly IMaintainableRefObject _maintainableRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactSqlQuery"/> class.
        /// </summary>
        /// <param name="tableInfo">
        /// The table Info.
        /// </param>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION
        /// </param>
        public ArtefactSqlQuery(TableInfo tableInfo, IMaintainableRefObject maintainableRef)
            : this(_artefactSqlBuilder.Build(tableInfo), maintainableRef)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactSqlQuery"/> class.
        /// </summary>
        /// <param name="queryInfo">
        /// The query info.
        /// </param>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION
        /// </param>
        public ArtefactSqlQuery(SqlQueryInfo queryInfo, IMaintainableRefObject maintainableRef)
            : base(queryInfo)
        {
            this._maintainableRef = maintainableRef ?? new MaintainableRefObjectImpl();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the maintainable reference.
        /// </summary>
        public IMaintainableRefObject MaintainableRef
        {
            get
            {
                return this._maintainableRef;
            }
        }

        #endregion

    }
}