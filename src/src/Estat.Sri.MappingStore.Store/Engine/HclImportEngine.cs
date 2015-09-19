// -----------------------------------------------------------------------
// <copyright file="HclImportEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-24
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Resources;

    using Estat.Ma.Helpers;
    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The HCL import engine.
    /// </summary>
    public class HclImportEngine : ArtefactImportEngine<IHierarchicalCodelistObject>
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(HclImportEngine));

        /// <summary>
        /// The _stored procedures
        /// </summary>
        private static readonly StoredProcedures _storedProcedures;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initializes static members of the <see cref="HclImportEngine"/> class.
        /// </summary>
        static HclImportEngine()
        {
            _storedProcedures = new StoredProcedures();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HclImportEngine"/> class. 
        /// </summary>
        /// <param name="database">
        /// The mapping store database instance.
        /// </param>
        public HclImportEngine(Database database)
            : base(database)
        {
        }

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
        public override ArtefactImportStatus Insert(DbTransactionState state, IHierarchicalCodelistObject maintainable)
        {
            var codelistCache = new StructureCache();
            foreach (var codelistRef in maintainable.CodelistRef)
            {
                ICrossReference codelistReference = codelistRef.CodelistReference;

                ItemSchemeFinalStatus itemSchemeFinalStatus = codelistCache.GetStructure(state, codelistReference);
                var codelistMaintainableReference = codelistReference.MaintainableReference;
                if (itemSchemeFinalStatus.FinalStatus.PrimaryKey <= 0)
                {
                    string message = string.Format(
                        "HierarchicalCodeList {0} uses the CodeList:\r\n ID: {1}\r\n VERSION:{2}\r\n AGENCY: {3}\r\n which doesn't exist in the Mapping Store",
                        maintainable.Id,
                        codelistMaintainableReference.MaintainableId,
                        codelistMaintainableReference.Version,
                        codelistMaintainableReference.AgencyId);
                    return new ArtefactImportStatus(-1, new ImportMessage(ImportMessageStatus.Error, codelistReference, message));
                }

                if (!itemSchemeFinalStatus.FinalStatus.IsFinal)
                {
                    string message = string.Format(
                        "HierarchicalCodeList {0} uses the CodeList:\r\n ID: {1}\r\n VERSION:{2}\r\n AGENCY: {3}\r\n which is not Final",
                        maintainable.Id,
                        codelistMaintainableReference.MaintainableId,
                        codelistMaintainableReference.Version,
                        codelistMaintainableReference.AgencyId);
                    return new ArtefactImportStatus(-1, new ImportMessage(ImportMessageStatus.Error, codelistReference, message));
                }
            }

            _log.DebugFormat(CultureInfo.InvariantCulture, "Importing artefact {0}", maintainable.Urn);
            var artefactStoredProcedure = _storedProcedures.InsertHcl;
            var artefactStatus = this.InsertArtefactInternal(state, maintainable, artefactStoredProcedure);
            foreach (KeyValuePair<long, IHierarchy> hierarchy in this.InsertHierarchies(state, maintainable.Hierarchies, artefactStatus.PrimaryKeyValue))
            {
                var levelIds = this.InsertLevels(state, hierarchy.Value.Level, hierarchy.Key);
                this.InsertCodeReferences(state, hierarchy.Value.HierarchicalCodeObjects, hierarchy.Key, codelistCache, levelIds);
            }

            return artefactStatus;
        }

        /// <summary>
        /// Deletes the child items.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="primaryKey">The primary key.</param>
        protected override void DeleteChildStructures(DbTransactionState state, long primaryKey)
        {
            // There is no "ON DELETE CASCADE" between ARTEFACT and any of HIERARCHY, HLEVEL and HCL tables. 
            // So we need first to retrieve the records  HIERARCHY, HLEVEL and HCL tables. 
            var itemTableInfo = new ItemTableInfo(SdmxStructureEnumType.Hierarchy) { ForeignKey = "HCL_ID", PrimaryKey = "H_ID", Table = "HIERARCHY" };
            var hierarchySubQuery = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} WHERE {2} = {{0}}", itemTableInfo.PrimaryKey, itemTableInfo.Table, itemTableInfo.ForeignKey);

            // the following two array/list must match...
            string[] statements =
                {
                    hierarchySubQuery,
                    hierarchySubQuery,
                    "{0}"
                };

            var tablePrimaryKey = new List<ItemTableInfo> { new ItemTableInfo(SdmxStructureEnumType.HierarchicalCode) { Table = "HCL_CODE", PrimaryKey = "HCODE_ID", ForeignKey = "H_ID"}, new ItemTableInfo(SdmxStructureEnumType.Level) { Table = "HLEVEL", PrimaryKey = "LEVEL_ID", ForeignKey = "H_ID" }, itemTableInfo };
            var primaryKeys = new Stack<long>();
            for (int i = 0; i < statements.Length; i++)
            {
                var statement = statements[i];
                var tupleTableKey = tablePrimaryKey[i];
                var tableName = tupleTableKey.Table;
                var foreignKey = tupleTableKey.ForeignKey;
                var subQuery = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} WHERE {2} IN ({3})", tupleTableKey.PrimaryKey, tableName, foreignKey, statement);

                // First get the list of primary keys. We need those because we need to delete the HIERARCHY, HCL_CODE and HLEVEL records first and then the corresponding ARTEFACT records.
                state.ExecuteReaderFormat(
                    subQuery,
                    reader =>
                        {
                            while (reader.Read())
                            {
                                primaryKeys.Push(reader.GetInt64(0));
                            }
                        },
                    state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));

                // Delete the annotations
                var annotationDeleteStatement = string.Format("DELETE FROM ANNOTATION WHERE ANN_ID IN (SELECT DISTINCT ANN_ID FROM ARTEFACT_ANNOTATION WHERE ART_ID IN ({0}))", subQuery);
                state.ExecuteNonQueryFormat(annotationDeleteStatement, state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));


                // Delete the HIERARCHY, HCL_CODE and HLEVEL records 
                var deleteStatement = string.Format("DELETE FROM {0} WHERE {1} IN ({2})", tableName, foreignKey, statement);
                var executeNonQueryFormat = state.ExecuteNonQueryFormat(deleteStatement, state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));
            }

            // last delete the artefact records. Must be last else you get foreign key constraint violations. No "on delete cascade" thanks to SQL Server. 
            DbHelper.BulkDelete(state.Database, "ARTEFACT", "ART_ID", primaryKeys);
        }

        /// <summary>
        /// Insert code references.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="hierarchicalCodeObjects">
        /// The hierarchical code objects.
        /// </param>
        /// <param name="hierarchyID">
        /// The hierarchy id.
        /// </param>
        /// <param name="codelistCache">
        /// The codeList cache.
        /// </param>
        /// <param name="levelIds">
        /// The level ids.
        /// </param>
        private void InsertCodeReferences(DbTransactionState state, IEnumerable<IHierarchicalCode> hierarchicalCodeObjects, long hierarchyID, StructureCache codelistCache, IDictionary<string, long> levelIds)
        {
            var queue = new Queue<KeyValuePair<long, IHierarchicalCode>>(hierarchicalCodeObjects.Select(code => new KeyValuePair<long, IHierarchicalCode>(0, code)));
            while (queue.Count > 0)
            {
                var keyValue = queue.Dequeue();
                long primaryKey = this.InsertCodeReference(state, keyValue.Value, hierarchyID, keyValue.Key, codelistCache, levelIds);
                foreach (var hierarchicalCode in keyValue.Value.CodeRefs)
                {
                    queue.Enqueue(new KeyValuePair<long, IHierarchicalCode>(primaryKey, hierarchicalCode));
                }
            }
        }

        /// <summary>
        /// Insert a <see cref="ILevelObject"/> to mapping store table LEVEL.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="hierarchyId">
        /// The hierarchy id.
        /// </param>
        /// <param name="parentID">
        /// The parent level id.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        private long InsertLevel(DbTransactionState state, ILevelObject level, long hierarchyId, long parentID)
        {
            long artefactImportStatus;
            var artefactStoredProcedure = _storedProcedures.InsertHlevel;
            using (DbCommand command = artefactStoredProcedure.CreateCommandWithDefaults(state))
            {
                DbParameter hierarchyIdParameter = artefactStoredProcedure.CreateHIdParameter(command);
                hierarchyIdParameter.Value = hierarchyId;

                DbParameter parentLevelIdParameter = artefactStoredProcedure.CreateParentLevelIdParameter(command);
                if (parentID > 0)
                {
                    parentLevelIdParameter.Value = parentID;
                }

                if (level.Uri != null)
                {
                    artefactStoredProcedure.CreateUriParameter(command).Value = level.Uri.ToString();
                }

                artefactImportStatus = this.RunNameableArtefactCommand(level, command, artefactStoredProcedure);
            }

            return artefactImportStatus;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert levels to Mapping Store database.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="rootLevel">
        /// The root level.
        /// </param>
        /// <param name="parentHierarchyID">
        /// The parent hierarchy id.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary{String, Long}"/> with the level id as key and the mapping store level primary key as value.
        /// </returns>
        private IDictionary<string, long> InsertLevels(DbTransactionState state, ILevelObject rootLevel, long parentHierarchyID)
        {
            var levelIds = new Dictionary<string, long>(StringComparer.Ordinal);
            ILevelObject level = rootLevel;
            long parentLevel = 0;
            while (level != null)
            {
                var id = this.InsertLevel(state, level, parentHierarchyID, parentLevel);
                levelIds.Add(level.Id, id);
                parentLevel = id;
                level = level.HasChild() ? level.ChildLevel : null;
            }

            return levelIds;
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="hierarchies">
        /// The hierarchies.
        /// </param>
        /// <param name="parentArtefact">
        /// The parent artefact.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<KeyValuePair<long, IHierarchy>> InsertHierarchies(DbTransactionState state, IEnumerable<IHierarchy> hierarchies, long parentArtefact)
        {
            foreach (IHierarchy item in hierarchies)
            {
                yield return new KeyValuePair<long, IHierarchy>(this.InsertHierarchy(state, item, parentArtefact), item);
            }
        }

        /// <summary>
        /// Insert a <see cref="IHierarchicalCode"/> to mapping store table <c>HCL_CODE</c>.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="coderef">
        /// The Hierarchical code reference.
        /// </param>
        /// <param name="hierarchyID">
        /// The hierarchy ID.
        /// </param>
        /// <param name="parentID">
        /// The parent id.
        /// </param>
        /// <param name="codelistReferences">
        /// The codelist references.
        /// </param>
        /// <param name="levelIds">
        /// The level Ids.
        /// </param>
        /// <returns>
        /// The primary key of the ne
        /// </returns>
        private long InsertCodeReference(DbTransactionState state, IHierarchicalCode coderef, long hierarchyID, long parentID, StructureCache codelistReferences, IDictionary<string, long> levelIds)
        {
            var artefactStoredProcedure = _storedProcedures.InsertHclCode;
            long artefactImportStatus;

            using (DbCommand command = artefactStoredProcedure.CreateCommandWithDefaults(state))
            {
                DbParameter parentHcodeIDIdParameter = artefactStoredProcedure.CreateParentHcodeIdParameter(command);
                parentHcodeIDIdParameter.Value = parentID > 0 ? (object)parentID : null;

                DbParameter lcdIddParameter = artefactStoredProcedure.CreateLcdIdParameter(command);

                ItemSchemeFinalStatus itemSchemeFinalStatus = codelistReferences.GetStructure(state, coderef.CodeReference);
                ItemStatus codeStatus;
                if (itemSchemeFinalStatus.ItemIdMap.TryGetValue(coderef.CodeReference.ChildReference.Id, out codeStatus))
                {
                    lcdIddParameter.Value = codeStatus.SysID;
                }

                DbParameter hierarchyIdParameter = artefactStoredProcedure.CreateHIdParameter(command);
                hierarchyIdParameter.Value = hierarchyID;

                DbParameter levelIdParameter = artefactStoredProcedure.CreateLevelIdParameter(command);
                ILevelObject levelObject = coderef.GetLevel(false);
                long levelPrimaryKey;
                if (levelObject != null && levelIds.TryGetValue(levelObject.Id, out levelPrimaryKey))
                {
                    levelIdParameter.Value = levelPrimaryKey;
                }

                if (coderef.Uri != null)
                {
                    artefactStoredProcedure.CreateUriParameter(command).Value = coderef.Uri.ToString();
                }
                
                artefactImportStatus = this.RunIdentifiableArterfactCommand(coderef, command, artefactStoredProcedure);
            }

            return artefactImportStatus;
        }

        /// <summary>
        /// Insert the specified <paramref name="hierarchy"/> to Mapping store table <c>HIERARCHY</c>
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="hierarchy">
        /// The hierarchy.
        /// </param>
        /// <param name="parentArtefact">
        /// The parent artefact.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        private long InsertHierarchy(DbTransactionState state, IHierarchy hierarchy, long parentArtefact)
        {
            long artID;
            var artefactStoredProcedure = _storedProcedures.InsertHierachy;
            using (DbCommand command = artefactStoredProcedure.CreateCommandWithDefaults(state))
            {
                DbParameter hierarchyIdParameter = artefactStoredProcedure.CreateHclIdParameter(command);
                hierarchyIdParameter.Value = parentArtefact;

                if (hierarchy.Uri != null)
                {
                    artefactStoredProcedure.CreateUriParameter(command).Value = hierarchy.Uri.ToString();
                }

                artID = this.RunNameableArtefactCommand(hierarchy, command, artefactStoredProcedure);
            }

            return artID;
        }

        #endregion
    }
}
