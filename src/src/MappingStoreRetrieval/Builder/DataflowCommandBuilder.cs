// -----------------------------------------------------------------------
// <copyright file="DataflowCommandBuilder.cs" company="EUROSTAT">
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
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The dataflow command builder.
    /// </summary>
    internal class DataflowCommandBuilder : AuthArtefactCommandBuilder
    {
        #region Fields

        /// <summary>
        /// The filter.
        /// </summary>
        private readonly DataflowFilter _filter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="filter">
        /// The filter. (Optional defaults to <see cref="DataflowFilter.Production"/>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public DataflowCommandBuilder(Database mappingStoreDb,  DataflowFilter filter = DataflowFilter.Production)
            : base(mappingStoreDb)
        {
            this._filter = filter;
        }

        #endregion

        #region Methods

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
        protected override IList<DbParameter> CreateArtefactWhereClause(IMaintainableRefObject maintainableRef, StringBuilder sqlCommand, WhereState whereState, IList<IMaintainableRefObject> allowedDataflows)
        {
            IList<DbParameter> parameters = this.CreateArtefactWhereClause(maintainableRef, sqlCommand, whereState);
            if (parameters.Count > 0)
            {
                whereState = WhereState.And;
            }

            if (this._filter == DataflowFilter.Production)
            {
                SqlHelper.AddWhereClause(sqlCommand, whereState, DataflowConstant.ProductionWhereClause);
                whereState = WhereState.And;
            }

            return SecurityHelper.AddWhereClauses(maintainableRef, this.MappingStoreDB, sqlCommand, parameters, allowedDataflows, whereState);
        }

        #endregion
    }
}