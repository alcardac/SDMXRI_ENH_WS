// -----------------------------------------------------------------------
// <copyright file="HierarchicalItemSchemeRetrievalEngine.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    /// The hierarchical item scheme retrieval engine.
    /// </summary>
    /// <typeparam name="TMaintaible">
    /// The <see cref="IItemSchemeMutableObject{T}"/> type
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The <typeparamref name="TMaintaible"/> Item type
    /// </typeparam>
    internal abstract class HierarchicalItemSchemeRetrievalEngine<TMaintaible, TItem> : ItemSchemeRetrieverEngine<TMaintaible, TItem>
        where TMaintaible : IItemSchemeMutableObject<TItem> where TItem : IItemMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalItemSchemeRetrievalEngine{TMaintaible,TItem}"/> class. 
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="orderBy">
        /// The order By.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        protected HierarchicalItemSchemeRetrievalEngine(Database mappingStoreDb, string orderBy = null)
            : base(mappingStoreDb, orderBy)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fill the specified <paramref name="itemSchemeBean"/> with parent items.
        /// </summary>
        /// <param name="itemSchemeBean">
        ///     The parent <see cref="IItemSchemeMutableObject{T}"/>
        /// </param>
        /// <param name="itemQuery">
        ///     The item Query.
        /// </param>
        protected void FillItemWithParent(TMaintaible itemSchemeBean, ItemSqlQuery itemQuery)
        {
            var allItems = new Dictionary<long, TItem>();
            var orderedItems = new List<KeyValuePair<long, TItem>>();
            var childItems = new Dictionary<long, long>();
            using (DbCommand command = this.ItemCommandBuilder.Build(itemQuery))
            {
                this.ReadItems(allItems, orderedItems, command, childItems);
            }

            this.FillParentItems(itemSchemeBean, childItems, allItems, orderedItems);

            this.IdentifiableAnnotationRetrieverEngine.RetrieveAnnotations(itemQuery.ParentSysId, allItems);
        }

        /// <summary>
        /// Handle item child method. Override to handle parent relationships 
        /// </summary>
        /// <param name="itemSchemeBean">
        /// The item scheme bean.
        /// </param>
        /// <param name="allItems">
        /// The all items.
        /// </param>
        /// <param name="childItems">
        /// The child items.
        /// </param>
        /// <param name="childSysId">
        /// The child sys id.
        /// </param>
        /// <param name="child">
        /// The child.
        /// </param>
        protected abstract void HandleItemChild(TMaintaible itemSchemeBean, IDictionary<long, TItem> allItems, IDictionary<long, long> childItems, long childSysId, TItem child);

        /// <summary>
        /// When overridden  it will handle extra fields. By default it does nothing
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="reader">
        /// The reader.
        /// </param>
        protected virtual void HandleItemExtraFields(TItem item, IDataReader reader)
        {
        }

        /// <summary>
        /// Fill parent items
        /// </summary>
        /// <param name="itemSchemeBean">
        /// The item scheme bean.
        /// </param>
        /// <param name="childItems">
        /// The child items.
        /// </param>
        /// <param name="allItems">
        /// All items.
        /// </param>
        /// <param name="orderedItems">
        /// The ordered items.
        /// </param>
        protected void FillParentItems(TMaintaible itemSchemeBean, IDictionary<long, long> childItems, IDictionary<long, TItem> allItems, IEnumerable<KeyValuePair<long, TItem>> orderedItems)
        {
            foreach (KeyValuePair<long, TItem> item in orderedItems)
            {
                long sysId = item.Key;
                TItem itemBean = item.Value;
                this.HandleItemChild(itemSchemeBean, allItems, childItems, sysId, itemBean);
            }
        }

        /// <summary>
        /// Read all items with parent item
        /// </summary>
        /// <param name="allItems">
        /// The all items.
        /// </param>
        /// <param name="orderedItems">
        /// The ordered items.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="childItems">
        /// The child items.
        /// </param>
        protected void ReadItems(IDictionary<long, TItem> allItems, ICollection<KeyValuePair<long, TItem>> orderedItems, DbCommand command, IDictionary<long, long> childItems)
        {
            using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
            {
                int sysIdIdx = dataReader.GetOrdinal("SYSID");
                int idIdx = dataReader.GetOrdinal("ID");
                int parentIdx = dataReader.GetOrdinal("PARENT");
                int txtIdx = dataReader.GetOrdinal("TEXT");
                int langIdx = dataReader.GetOrdinal("LANGUAGE");
                int typeIdx = dataReader.GetOrdinal("TYPE");
                while (dataReader.Read())
                {
                    long sysId = dataReader.GetInt64(sysIdIdx); // not a null.
                    TItem item;
                    if (!allItems.TryGetValue(sysId, out item))
                    {
                        item = this.CreateItem();
                        item.Id = DataReaderHelper.GetString(dataReader, idIdx); // "ID"
                        this.HandleItemExtraFields(item, dataReader);
                        orderedItems.Add(new KeyValuePair<long, TItem>(sysId, item));
                        
                        allItems.Add(sysId, item);
                        long parentItemId = DataReaderHelper.GetInt64(dataReader, parentIdx);
                        if (parentItemId > long.MinValue)
                        {
                            childItems.Add(sysId, parentItemId);
                        }
                    }

                    ReadLocalisedString(item, typeIdx, txtIdx, langIdx, dataReader);
                }

                this.MappingStoreDb.CancelSafe(command);
            }
        }

        #endregion
    }
}