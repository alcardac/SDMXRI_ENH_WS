// -----------------------------------------------------------------------
// <copyright file="ItemSchemeImportEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-20
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
    using System.Data;
    using System.Data.Common;
    using System.Globalization;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Factory;
    using Estat.Sri.MappingStore.Store.Helper;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The item scheme import engine.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="IItemSchemeObject{TItem}"/> type
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The <see cref="IItemObject"/> type
    /// </typeparam>
    public abstract class ItemSchemeImportEngine<T, TItem> : ArtefactImportEngine<T>
        where T : IItemSchemeObject<TItem> where TItem : IItemObject
    {
        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ItemSchemeImportEngine<T, TItem>));

        /// <summary>
        /// The _nameable import engine.
        /// </summary>
        private readonly IItemImportEngine<TItem> _itemImportEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeImportEngine{T,TItem}"/> class. 
        /// </summary>
        /// <param name="database">
        /// The mapping store database instance.
        /// </param>
        protected ItemSchemeImportEngine(Database database)
            : this(database, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeImportEngine{T,TItem}"/> class. 
        /// </summary>
        /// <param name="database">
        /// The mapping store database instance.
        /// </param>
        /// <param name="factory">
        /// The <see cref="IItemImportEngine{T}"/> factory. Optional
        /// </param>
        protected ItemSchemeImportEngine(Database database, IItemImportFactory<TItem> factory)
            : base(database)
        {
            factory = factory ?? new ItemImportFactory<TItem>();
            this._itemImportEngine = factory.GetItemImport();
        }

        /// <summary>
        /// Gets the nameable import engine.
        /// </summary>
        protected IItemImportEngine<TItem> ItemImportEngine
        {
            get
            {
                return this._itemImportEngine;
            }
        }

        /// <summary>
        /// Gets the item table.
        /// </summary>
        protected virtual ItemTableInfo ItemTable
        {
            get
            {
                var sdmxStructureType = SdmxStructureType.ParseClass(typeof(T));
                var tableInfoBuilder = new ItemTableInfoBuilder();
                return tableInfoBuilder.Build(sdmxStructureType);
            }
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
        /// <param name="artefactStoredProcedure">
        /// The artefact Stored Procedure.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected ArtefactImportStatus InsertInternal(DbTransactionState state, T maintainable, ItemSchemeProcedureBase artefactStoredProcedure)
        {
            return this.InsertInternal(state, maintainable, artefactStoredProcedure, null);
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
        /// <param name="artefactStoredProcedure">
        /// The artefact Stored Procedure.
        /// </param>
        /// <param name="setupCommand">
        /// The setup Command.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected ArtefactImportStatus InsertInternal(DbTransactionState state, T maintainable, ItemSchemeProcedureBase artefactStoredProcedure, Action<DbCommand> setupCommand)
        {
            Action<DbCommand> itemSchemeSetup = command =>
                {
                    artefactStoredProcedure.CreateIsPartialParameter(command).Value = maintainable.Partial ? 1 : 0;
                    if (setupCommand != null)
                    {
                        setupCommand(command);
                    }
                };
            var artefactImportStatus = this.InsertArtefactInternal(state, maintainable, artefactStoredProcedure, itemSchemeSetup);

            var itemIds = this.ItemImportEngine.Insert(state, maintainable.Items, artefactImportStatus.PrimaryKeyValue);
            ValidationHelper.Validate(maintainable, itemIds);
            return artefactImportStatus;
        }

        /// <summary>
        /// Deletes the child items.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="primaryKey">The primary key.</param>
        protected override void DeleteChildStructures(DbTransactionState state, long primaryKey)
        {
            var itemTableInfo = this.ItemTable;

            // delete annotations first
            var annotationQuery = string.Format("DELETE FROM ANNOTATION WHERE ANN_ID IN (SELECT DISTINCT ANN_ID FROM ITEM_ANNOTATION WHERE ITEM_ID IN (SELECT {0} FROM {1} WHERE {2} = {{0}})) ", itemTableInfo.PrimaryKey, itemTableInfo.Table, itemTableInfo.ForeignKey);
            var annotationsDeleted = state.ExecuteNonQueryFormat(annotationQuery, state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));
            _log.DebugFormat(CultureInfo.InvariantCulture, "Item Annotations records deleted {0}", annotationsDeleted);

            // set parent item to null to avoid issues with MySQL
            if (!string.IsNullOrWhiteSpace(itemTableInfo.ParentItem))
            {
                var noparentStatement = string.Format("UPDATE {0} SET {1} = NULL WHERE {2} = {{0}} ", itemTableInfo.Table, itemTableInfo.ParentItem, itemTableInfo.ForeignKey);
                var parentSetToNull = state.ExecuteNonQueryFormat(noparentStatement, state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));
                _log.DebugFormat(CultureInfo.InvariantCulture, "Parents set to null : {0}", parentSetToNull);
            }

            var query = string.Format("DELETE FROM ITEM WHERE ITEM_ID IN (SELECT DISTINCT {0} FROM {1} WHERE {2} = {{0}}) ", itemTableInfo.PrimaryKey, itemTableInfo.Table, itemTableInfo.ForeignKey);
            var itemsDeleted = state.ExecuteNonQueryFormat(query, state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));
            _log.DebugFormat(CultureInfo.InvariantCulture, "Item records deleted {0}", itemsDeleted);
        }
    }
}