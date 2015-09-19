// -----------------------------------------------------------------------
// <copyright file="PartialCodesCommandBuilder.cs" company="EUROSTAT">
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
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The partial codes command builder.
    /// </summary>
    internal class PartialCodesCommandBuilder : IAuthCommandBuilder<PartialCodesSqlQuery>
    {
        #region Fields

        /// <summary>
        ///     The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialCodesCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        public PartialCodesCommandBuilder(Database mappingStoreDb)
        {
            this._mappingStoreDb = mappingStoreDb;
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
        /// The allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        /// <remarks>
        /// It expects the input SQL query to have 7 format parameters.
        ///     1. <c>CL_ID</c>
        ///     2. <c>Dataflow ID</c>
        ///     3. <c>Dataflow Agency</c>
        ///     4. <c>Dataflow Version1</c>
        ///     5. <c>Dataflow Version2</c>
        ///     6. <c>Dataflow Version3</c>
        ///     7. <c>Concept ID</c>
        /// </remarks>
        public DbCommand Build(PartialCodesSqlQuery buildFrom, IList<IMaintainableRefObject> allowedDataflows)
        {
            string paramId = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);
            string dataflowId = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.DataflowIdParameter);
            string dataflowAgency = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.AgencyParameter);
            string dataflowVersion1 = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.VersionParameter1);
            string dataflowVersion2 = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.VersionParameter2);
            string dataflowVersion3 = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.VersionParameter3);
            string conceptId = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.ConceptIdParameter);

            var query = new StringBuilder();
            var securityCriteria = new StringBuilder();
            var securityParameters = new List<DbParameter>();
            SecurityHelper.AddWhereClauses(buildFrom.DataflowReference, this._mappingStoreDb, securityCriteria, securityParameters, allowedDataflows, WhereState.And);

            query.AppendFormat(CultureInfo.InvariantCulture, buildFrom.QueryInfo.ToString(), paramId, dataflowId, dataflowAgency, dataflowVersion1, dataflowVersion2, dataflowVersion3, conceptId, securityCriteria);

            var version = buildFrom.DataflowReference.SplitVersion(3);
            var parameters = new List<DbParameter>
                                 {
                                     this._mappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, buildFrom.ParentSysId),
                                     this._mappingStoreDb.CreateInParameter(ParameterNameConstants.DataflowIdParameter, DbType.AnsiString, buildFrom.DataflowReference.MaintainableId),
                                     this._mappingStoreDb.CreateInParameter(ParameterNameConstants.AgencyParameter, DbType.AnsiString, buildFrom.DataflowReference.AgencyId),
                                     this._mappingStoreDb.CreateInParameter(ParameterNameConstants.VersionParameter1, DbType.Int64, version[0].ToDbValue()),
                                     this._mappingStoreDb.CreateInParameter(ParameterNameConstants.VersionParameter2, DbType.Int64, version[1].ToDbValue(0)),
                                     this._mappingStoreDb.CreateInParameter(ParameterNameConstants.VersionParameter3, DbType.Int64, version[2].ToDbValue()),
                                     this._mappingStoreDb.CreateInParameter(ParameterNameConstants.ConceptIdParameter, DbType.AnsiString, buildFrom.ConceptId)
                                 };

            parameters.AddRange(securityParameters);
            return this._mappingStoreDb.GetSqlStringCommand(query.ToString(), parameters);
        }

        #endregion
    }
}