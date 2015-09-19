// -----------------------------------------------------------------------
// <copyright file="SimpleHierarchicalItemEngineBase.cs" company="EUROSTAT">
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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The simple hierarchical item engine base.
    /// </summary>
    /// <typeparam name="TItem">
    /// The <see cref="IItemObject"/> based type
    /// </typeparam>
    /// <typeparam name="TProc">
    /// The  <see cref="HierarchicalItemProcedureBase"/> based type.
    /// </typeparam>
    public abstract class SimpleHierarchicalItemEngineBase<TItem, TProc> : ItemBaseEngine<TItem, TProc>
        where TProc : HierarchicalItemProcedureBase, new() where TItem : IItemObject
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SimpleHierarchicalItemEngineBase<TItem, TProc>));

        #endregion

        #region Fields

        /// <summary>
        ///     The _item procedure
        /// </summary>
        private readonly HierarchicalItemProcedureBase _itemProcedure;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleHierarchicalItemEngineBase{TItem,TProc}" /> class.
        /// </summary>
        protected SimpleHierarchicalItemEngineBase()
        {
            this._itemProcedure = new TProc();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Insert the specified <paramref name="items"/> to the mapping store with <paramref name="state"/>
        /// </summary>
        /// <param name="state">
        /// The MAPPING STORE connection and transaction state
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="parentArtefact">
        /// The parent 
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{ArtefactImportStatus}"/>.
        /// </returns>
        public override IEnumerable<long> Insert(DbTransactionState state, IEnumerable<TItem> items, long parentArtefact)
        {
            var parentMap = new Dictionary<string, long>(StringComparer.Ordinal);
            var linkedList = new LinkedList<TItem>(items);

            // First attempt to insert all codes without having to do an SQL UPDATE for parent. For each inserted code with remove it from linked list.
            this.InsertCodesNoUpdate(state, parentArtefact, linkedList, parentMap);

            // Second if it was not possible to insert all codes, possibly because of cyclic parent references try with update.
            // Normally this method is not needed with Common API. TODO remove it after verifying that parent code recursive loops are forbidden.
            this.InsertItemsWithUpdate(state, parentArtefact, linkedList, parentMap);

            return parentMap.Values;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the parent item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The parent item if <paramref name="item"/> has one; otherwise null.
        /// </returns>
        protected abstract string GetParentItem(TItem item);

        /// <summary>
        /// insert the specified <paramref name="item"/> .
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="parentScheme">
        /// The parent artefact primary.
        /// </param>
        /// <param name="parentItemPrimaryKey">
        /// The parent item primary key.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The primary key of the inserted code.
        /// </returns>
        private long InsertCode(DbTransactionState state, long parentScheme, long parentItemPrimaryKey, TItem item)
        {
            long itemID;

            using (DbCommand command = this._itemProcedure.CreateCommandWithDefaults(state))
            {
                DbParameter itemSchemeParameter = this._itemProcedure.CreateSchemeIdParameter(command);
                itemSchemeParameter.Value = parentScheme;

                var parentCodeParameter = this._itemProcedure.CreateParentItemIdParameter(command);
                var parentItem = this.GetParentItem(item);
                if (!string.IsNullOrEmpty(parentItem) && parentItemPrimaryKey != 0)
                {
                    parentCodeParameter.Value = parentItemPrimaryKey;
                }

                itemID = this.RunCommand(item, command, (TProc)this._itemProcedure, state);
            }

            return itemID;
        }

        /// <summary>
        /// The (fast) method that inserts codes from <paramref name="items"/> together with parent item information without
        ///     using an update SQL statement for parents.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="parentArtefact">
        /// The parent artefact.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="parentMap">
        /// The parent map.
        /// </param>
        private void InsertCodesNoUpdate(DbTransactionState state, long parentArtefact, LinkedList<TItem> items, IDictionary<string, long> parentMap)
        {
            int lastCount = items.Count;
            LinkedListNode<TItem> current = items.First;
            while (current != null)
            {
                var item = current.Value;
                long parentCodePrimaryKey = 0;
                var parentItem = this.GetParentItem(item);
                if (string.IsNullOrEmpty(parentItem) || parentMap.TryGetValue(parentItem, out parentCodePrimaryKey))
                {
                    var itemID = this.InsertCode(state, parentArtefact, parentCodePrimaryKey, item);

                    parentMap.Add(item.Id, itemID);
                    items.Remove(current);
                }

                current = current.Next;

                if (current == null && items.Count != lastCount)
                {
                    current = items.First;
                    lastCount = items.Count;
                }
            }
        }

        /// <summary>
        /// The (slow) method that inserts items from <paramref name="items"/> without parent code information with an update
        ///     SQL statement for parents.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="parentArtefact">
        /// The parent artefact.
        /// </param>
        /// <param name="items">
        /// The code collection
        /// </param>
        /// <param name="parentMap">
        /// The parent map.
        /// </param>
        /// <exception cref="MappingStoreException">
        /// Invalid parent code found.
        /// </exception>
        /// <remarks>
        /// This method normally shouldn't run because Common API <c>SDMX Source.NET 0.9.15</c> validation rules check for
        ///     codes with parent recursive loops. <see cref="ExceptionCode.ParentRecursiveLoop"/>
        /// </remarks>
        private void InsertItemsWithUpdate(DbTransactionState state, long parentArtefact, ICollection<TItem> items, IDictionary<string, long> parentMap)
        {
            if (items.Count == 0)
            {
                return;
            }

            _log.InfoFormat(CultureInfo.InvariantCulture, "Using slow InsertItemsWithUpdate method for {0} codes. Already inserted : {1}", items.Count, parentMap.Count);
            var itemsWithParents = new List<KeyValuePair<long, string>>(items.Count);
            foreach (var item in items)
            {
                var parentItem = this.GetParentItem(item);
                if (!string.IsNullOrEmpty(parentItem))
                {
                    long parentItemPrimaryKey;
                    long itemID;
                    if (parentMap.TryGetValue(parentItem, out parentItemPrimaryKey))
                    {
                        itemID = this.InsertCode(state, parentArtefact, parentItemPrimaryKey, item);
                    }
                    else
                    {
                        itemID = this.InsertCode(state, parentArtefact, 0, item);
                        itemsWithParents.Add(new KeyValuePair<long, string>(itemID, parentItem));
                    }

                    parentMap.Add(item.Id, itemID);
                }
                else
                {
                    Debug.Fail("Error in first algorithm. We should have never reached this point.");
                }
            }

            foreach (var itemWithParent in itemsWithParents)
            {
                long parentCodePrimaryKey;
                if (parentMap.TryGetValue(itemWithParent.Value, out parentCodePrimaryKey))
                {
                    state.ExecuteNonQueryFormat(
                        "update DSD_CODE set PARENT_CODE = {0} where LCD_ID = {1}", 
                        state.Database.CreateInParameter("parent", DbType.Int64, parentCodePrimaryKey), 
                        state.Database.CreateInParameter("code", DbType.Int64, itemWithParent.Key));
                }
                else
                {
                    var message = "Invalid parent code : " + itemWithParent.Value;
                    _log.Error(message);
                    throw new MappingStoreException(message);
                }
            }
        }

        #endregion
    }
}