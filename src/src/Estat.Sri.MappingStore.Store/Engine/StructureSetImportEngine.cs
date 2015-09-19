// -----------------------------------------------------------------------
// <copyright file="StructureSetImportEngine.cs" company="EUROSTAT">
//   Date Created : 2014-10-08
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
    using System.Data;
    using System.Globalization;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    /// The DSD import engine.
    /// </summary>
    public class StructureSetImportEngine : ArtefactImportEngine<IStructureSetObject>
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(StructureSetImportEngine));

        /// <summary>
        /// The artefact stored procedure
        /// </summary>
        private static readonly InsertStructureSet _artefactStoredProcedure;

        #endregion

        #region Fields

        /// <summary>
        /// The _component import.
        /// </summary>
        private readonly CodeListMapImportEngine _codelistMapImport;

        /// <summary>
        /// The _group import.
        /// </summary>
        private readonly StructureMapEngine _structureMapImportEngine;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="StructureSetImportEngine"/> class.
        /// </summary>
        static StructureSetImportEngine()
        {
            _artefactStoredProcedure = new StoredProcedures().InsertStructureSet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSetImportEngine"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The mapping store database instance.
        /// </param>
        public StructureSetImportEngine(Database connectionStringSettings)
            : base(connectionStringSettings)
        {
            this._codelistMapImport = new CodeListMapImportEngine();
            this._structureMapImportEngine = new StructureMapEngine();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Insert the specified <paramref name="maintainable"/> to the mapping store with <paramref name="state"/>
        /// </summary>
        /// <param name="state">
        /// The MAPPING STORE connection and transaction state
        /// </param>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        public override ArtefactImportStatus Insert(DbTransactionState state, IStructureSetObject maintainable)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, "Importing artefact {0}", maintainable.Urn);
            var artefactStoredProcedure = _artefactStoredProcedure;
            var artefactStatus = this.InsertArtefactInternal(state, maintainable, artefactStoredProcedure);

            this._codelistMapImport.Insert(state, maintainable.CodelistMapList, artefactStatus.PrimaryKeyValue);
            this._structureMapImportEngine.Insert(state, maintainable.StructureMapList, artefactStatus.PrimaryKeyValue);

            return artefactStatus;
        }

        #endregion

        /// <summary>
        /// Deletes the child items.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="primaryKey">The primary key.</param>
        protected override void DeleteChildStructures(DbTransactionState state, long primaryKey)
        {
            var itemTableInfos = new[] { new ItemTableInfo(SdmxStructureEnumType.StructureMap) { ForeignKey = "SS_ID", PrimaryKey = "SM_ID", Table = "STRUCTURE_MAP" }, new ItemTableInfo(SdmxStructureEnumType.CodeListMap) { ForeignKey = "SS_ID", PrimaryKey = "CLM_ID", Table = "CODELIST_MAP" } };
            foreach (var itemTableInfo in itemTableInfos)
            {
                var annotationQuery = string.Format("DELETE FROM ANNOTATION WHERE ANN_ID IN (SELECT DISTINCT ANN_ID FROM ITEM_ANNOTATION WHERE ITEM_ID IN (SELECT {0} FROM {1} WHERE {2} = {{0}})) ", itemTableInfo.PrimaryKey, itemTableInfo.Table, itemTableInfo.ForeignKey);
                state.ExecuteNonQueryFormat(annotationQuery, state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));
                var query = string.Format("DELETE FROM ITEM WHERE ITEM_ID IN (SELECT DISTINCT {0} FROM {1} WHERE {2} = {{0}}) ", itemTableInfo.PrimaryKey, itemTableInfo.Table, itemTableInfo.ForeignKey);
                state.ExecuteNonQueryFormat(query, state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));               
            }
        }
    }
}