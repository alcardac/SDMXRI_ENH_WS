// -----------------------------------------------------------------------
// <copyright file="MaintainableRefRetrieverEngine.cs" company="EUROSTAT">
//   Date Created : 2013-07-11
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using Dapper;

    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Properties;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// The maintainable ref retriever engine.
    /// </summary>
    public class MaintainableRefRetrieverEngine
    {
        #region Constants

        /// <summary>
        /// The SQL query for from PK format. 1 parameter the Primary Key value
        /// </summary>
        private const string SqlQueryFromPk = "SELECT A.AGENCY as AgencyId, A.ID as MaintainableId, A.VERSION FROM ARTEFACT_VIEW A WHERE A.ART_ID = {0}";

        /// <summary>
        /// The SQL query template.7 parameter: ID, AGENCY, VERSION1, VERSION2, VERSION3, {primary key field}, {table name}
        /// </summary>
        private const string SqlQueryFromRef = "SELECT A.ART_ID FROM ARTEFACT A WHERE ({0} is null OR A.ID = {0}) AND ({1} is null OR A.AGENCY = {1}) and ({2} is NULL OR dbo.isEqualVersion(A.VERSION1, A.VERSION2, A.VERSION3, {2}, {3}, {4})=1) AND A.ART_ID in (SELECT {5} FROM {6} )";

        #endregion

        #region Fields

        /// <summary>
        /// The _mapping store database.
        /// </summary>
        private readonly Database _mappingStoreDatabase;

        /// <summary>
        ///     The table info builder.
        /// </summary>
        private readonly TableInfoBuilder _tableInfoBuilder = new TableInfoBuilder();

        /// <summary>
        /// The _mutable retrieval manager.
        /// </summary>
        private readonly ISdmxMutableObjectRetrievalManager _mutableRetrievalManager;

        /// <summary>
        /// The _item table information builder
        /// </summary>
        private readonly ItemTableInfoBuilder _itemTableInfoBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableRefRetrieverEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDatabase">
        /// The mapping store database.
        /// </param>
        public MaintainableRefRetrieverEngine(Database mappingStoreDatabase)
        {
            this._mappingStoreDatabase = mappingStoreDatabase;
            this._mutableRetrievalManager = new MappingStoreRetrievalManager(this._mappingStoreDatabase);
            this._itemTableInfoBuilder = new ItemTableInfoBuilder();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="IMaintainableRefObject"/> populate by the contents of the record in <c>ARTEFACT_VIEW</c> for <c>ART_ID = </c> <paramref name="primaryKeyValue"/>
        /// </summary>
        /// <param name="primaryKeyValue">
        /// The primary key value.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableRefObject"/>; otherwise null.
        /// </returns>
        public IMaintainableRefObject Retrieve(long primaryKeyValue)
        {
            string parameterName = this._mappingStoreDatabase.BuildParameterName("pk");
            var query = string.Format(CultureInfo.InvariantCulture, SqlQueryFromPk, parameterName);
            using (var connection = this._mappingStoreDatabase.CreateConnection())
            {
                connection.Open();
                return connection.Query<MaintainableRefObjectImpl>(query, new { pk = primaryKeyValue }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieve the primary key from mapping store for the given <paramref name="structureReference"/>
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <returns>
        /// The primary key value; otherwise <c>-1</c>.
        /// </returns>
        public long Retrieve(IStructureReference structureReference)
        {
            var maintainableRef = structureReference.MaintainableReference;
            var splitVersion = maintainableRef.SplitVersion(3);
            var tableInfo = this._tableInfoBuilder.Build(structureReference.MaintainableStructureEnumType.EnumType);
            var parameters = new List<DbParameter>();

            var query = string.Format(
                CultureInfo.InvariantCulture,
                SqlQueryFromRef,
                this._mappingStoreDatabase.BuildParameterName("ID"),
                this._mappingStoreDatabase.BuildParameterName("AGENCY"),
                this._mappingStoreDatabase.BuildParameterName("VERSION1"),
                this._mappingStoreDatabase.BuildParameterName("VERSION2"),
                this._mappingStoreDatabase.BuildParameterName("VERSION3"),
                tableInfo.PrimaryKey,
                tableInfo.Table);

            parameters.Add(this._mappingStoreDatabase.CreateInParameter("ID", DbType.AnsiString, maintainableRef.HasMaintainableId() ? (object)maintainableRef.MaintainableId : DBNull.Value));
            parameters.Add(this._mappingStoreDatabase.CreateInParameter("AGENCY", DbType.AnsiString, maintainableRef.HasAgencyId() ? (object)maintainableRef.AgencyId : DBNull.Value));

            // TODO fix that after the conclusion of MAT-579
            parameters.Add(this._mappingStoreDatabase.CreateInParameter("VERSION1", DbType.Int64, splitVersion[0].ToDbValue()));
            parameters.Add(this._mappingStoreDatabase.CreateInParameter("VERSION2", DbType.Int64, splitVersion[1].ToDbValue(0)));
            parameters.Add(this._mappingStoreDatabase.CreateInParameter("VERSION3", DbType.Int64, splitVersion[2].ToDbValue()));

            var executeScalar = this._mappingStoreDatabase.ExecuteScalar(query, parameters);

            return executeScalar is long ? (long)executeScalar : -1;
        }

        /// <summary>
        /// Retrieves the urn map.
        /// </summary>
        /// <param name="sdmxStructure">The SDMX structure.</param>
        /// <returns>
        /// The map between System ID to URN
        /// </returns>
        public IDictionary<long, string> RetrievesUrnMap(SdmxStructureType sdmxStructure)
        {
            string query;
            if (sdmxStructure.IsMaintainable)
            {
                TableInfo tableInfo = this._tableInfoBuilder.Build(sdmxStructure);
                query = string.Format(
                    CultureInfo.InvariantCulture,
                    "SELECT A.ART_ID as SYSID, A.AGENCY, A.ID, A.VERSION FROM ARTEFACT_VIEW A WHERE A.ART_ID in (SELECT {0} FROM {1})",
                    tableInfo.PrimaryKey,
                    tableInfo.Table);
                IDictionary<long, string> dictionary = new Dictionary<long, string>();
                foreach (dynamic o in this._mappingStoreDatabase.Query(query))
                {
                    long sysid = o.SYSID;

                    Uri generateUrn = sdmxStructure.GenerateUrn(o.AGENCY, o.ID, o.VERSION);
                    var result = new KeyValuePair<long, string>(sysid, generateUrn.ToString());
                    dictionary.Add(result);
                }

                return dictionary;
            }
            else
            {
                var itemTableInfo = this._itemTableInfoBuilder.Build(sdmxStructure);
                if (itemTableInfo == null)
                {
                    throw new NotImplementedException(sdmxStructure.ToString());
                }

                query = string.Format(
                    CultureInfo.InvariantCulture,
                    "SELECT I.ITEM_ID as SYSID, A.AGENCY, A.ID, A.VERSION, I.ID as IID FROM ARTEFACT_VIEW A INNER JOIN {1} T ON A.ART_ID = T.{2} INNER JOIN ITEM I ON I.ITEM_ID = T.{0}",
                    itemTableInfo.PrimaryKey,
                    itemTableInfo.Table,
                    itemTableInfo.ForeignKey);
                IDictionary<long, string> dictionary = new Dictionary<long, string>();
                foreach (dynamic o in this._mappingStoreDatabase.Query(query))
                {
                    Uri generateUrn = sdmxStructure.GenerateUrn(o.AGENCY, o.ID, o.VERSION, o.IID);
                    long systemId = o.SYSID;
                    var result = new KeyValuePair<long, string>(systemId, generateUrn.ToString());
                    dictionary.Add(result);
                }

                return dictionary;
            }
        }

        /// <summary>
        /// Retrieves the urn map.
        /// </summary>
        /// <param name="sdmxStructure">The SDMX structure.</param>
        /// <returns>
        /// The map between System ID to URN
        /// </returns>
        public IDictionary<string, long> RetrievesUrnToSysIdMap(SdmxStructureType sdmxStructure)
        {
            string query;
            if (sdmxStructure.IsMaintainable)
            {
                TableInfo tableInfo = this._tableInfoBuilder.Build(sdmxStructure);
                query = string.Format(
                    CultureInfo.InvariantCulture,
                    "SELECT A.ART_ID as SYSID, A.AGENCY, A.ID, A.VERSION FROM ARTEFACT_VIEW A WHERE A.ART_ID in (SELECT {0} FROM {1})",
                    tableInfo.PrimaryKey,
                    tableInfo.Table);
                IDictionary<string, long> dictionary = new Dictionary<string, long>(StringComparer.Ordinal);
                foreach (var result in this._mappingStoreDatabase.Query(query).Select(o => new KeyValuePair<string, long>(sdmxStructure.GenerateUrn(o.AGENCY, o.ID, o.VERSION).ToString(), o.SYSID)))
                {
                    Debug.Assert(!dictionary.ContainsKey(result.Key), "Already got key : {0}", result.Key);
                    dictionary.Add(result);
                }

                return dictionary;
            }
            else
            {
                var itemTableInfo = this._itemTableInfoBuilder.Build(sdmxStructure);
                if (itemTableInfo == null)
                {
                    throw new NotImplementedException(sdmxStructure.ToString());
                }

                query = string.Format(
                   CultureInfo.InvariantCulture,
                    "SELECT I.ITEM_ID as SYSID, A.AGENCY, A.ID, A.VERSION, I.ID as IID FROM ARTEFACT_VIEW A INNER JOIN {1} T ON A.ART_ID = T.{2} INNER JOIN ITEM I ON I.ITEM_ID = T.{0}",
                   itemTableInfo.PrimaryKey,
                   itemTableInfo.Table,
                   itemTableInfo.ForeignKey);
                IDictionary<string, long> dictionary = new Dictionary<string, long>(StringComparer.Ordinal);
                foreach (var result in this._mappingStoreDatabase.Query(query).Select(o => new KeyValuePair<string, long>(sdmxStructure.GenerateUrn(o.AGENCY, o.ID, o.VERSION, o.IID).ToString(), o.SYSID)))
                {
                    dictionary.Add(result);
                }

                return dictionary;
            }
        }

        /// <summary>
        /// Retrieve the primary key from mapping store for the given period code list.
        /// </summary>
        /// <param name="timeFormat">
        /// The time format.
        /// </param>
        /// <returns>
        /// The  primary key.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Value in <paramref name="timeFormat"/> not supported.
        /// </exception>
        public long RetrievePeriodCodelistId(TimeFormat timeFormat)
        {
            PeriodObject periodObject;
            if (PeriodCodelist.PeriodCodelistIdMap.TryGetValue(timeFormat.FrequencyCode, out periodObject))
            {
                return this.Retrieve(new StructureReferenceImpl(PeriodCodelist.Agency, periodObject.Id, PeriodCodelist.Version, SdmxStructureEnumType.CodeList));
            }

            throw new ArgumentOutOfRangeException("timeFormat", timeFormat, Resources.ErrorNotSupported);
        }

        /// <summary>
        /// Retrieve the primary key from mapping store for the given period code list.
        /// </summary>
        /// <param name="timeFormat">
        /// The time format.
        /// </param>
        /// <returns>
        /// The  primary key.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Value in <paramref name="timeFormat"/> not supported.
        /// </exception>
        public ICodelistMutableObject RetrievePeriodCodelist(TimeFormat timeFormat)
        {
            PeriodObject periodObject;
            if (PeriodCodelist.PeriodCodelistIdMap.TryGetValue(timeFormat.FrequencyCode, out periodObject))
            {
                var maintainableRef = new MaintainableRefObjectImpl(PeriodCodelist.Agency, periodObject.Id, PeriodCodelist.Version);
                return this._mutableRetrievalManager.GetMutableCodelist(maintainableRef, false, false);
            }

            throw new ArgumentOutOfRangeException("timeFormat", timeFormat, Resources.ErrorNotSupported);
        }
        #endregion
    }
}