// -----------------------------------------------------------------------
// <copyright file="CategorySchemeRetrievalEngine.cs" company="EUROSTAT">
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

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;

    /// <summary>
    ///     The concept scheme retrieval engine.
    /// </summary>
    internal class CategorySchemeRetrievalEngine : HierarchicalItemSchemeRetrievalEngine<ICategorySchemeMutableObject, ICategoryMutableObject>
    {
        #region Fields

        /// <summary>
        ///     The _item <see cref="SqlQueryInfo" /> builder.
        /// </summary>
        private readonly ItemSqlQueryBuilder _itemSqlQueryBuilder;

        /// <summary>
        ///     The _item SQL query info.
        /// </summary>
        private readonly SqlQueryInfo _itemSqlQueryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public CategorySchemeRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb, null)
        {
            this._itemSqlQueryBuilder = new ItemSqlQueryBuilder(mappingStoreDb, null);
            this._itemSqlQueryInfo = this._itemSqlQueryBuilder.Build(CategorySchemeConstant.ItemTableInfo);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="ICategorySchemeMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICategorySchemeMutableObject" />.
        /// </returns>
        protected override ICategorySchemeMutableObject CreateArtefact()
        {
            return new CategorySchemeMutableCore();
        }

        /// <summary>
        ///     Create an item.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICategoryMutableObject" />.
        /// </returns>
        protected override ICategoryMutableObject CreateItem()
        {
            return new CategoryMutableCore();
        }

        /// <summary>
        /// When this method is overridden it is used to retrieve Items of a ItemScheme and populate the output List
        /// </summary>
        /// <param name="itemScheme">
        ///     The <see cref="IItemSchemeMutableObject{T}"/> to fill with <see cref="IItemMutableObject"/>
        /// </param>
        /// <param name="parentSysId">
        ///     The primary key of the Item Scheme from Mapping Store table ARTEFACT.ART_ID field
        /// </param>
        protected override void FillItems(ICategorySchemeMutableObject itemScheme, long parentSysId)
        {
            var itemQuery = new ItemSqlQuery(this._itemSqlQueryInfo, parentSysId);
            this.FillItemWithParent(itemScheme, itemQuery);
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
        /// <param name="category">
        /// The child.
        /// </param>
        protected override void HandleItemChild(
            ICategorySchemeMutableObject itemSchemeBean, IDictionary<long, ICategoryMutableObject> allItems, IDictionary<long, long> childItems, long childSysId, ICategoryMutableObject category)
        {
            long parentItemId;
            if (category.Names.Count == 0)
            {
                category.AddName("en", category.Id);
            }

            if (childItems.TryGetValue(childSysId, out parentItemId))
            {
                // has parent
                ICategoryMutableObject parent = allItems[parentItemId];

                parent.AddItem(category);

                //// TODO Common API has no ParentId
                //// category.ParentId = parent.Id;
            }
            else
            {
                // add only root elements
                itemSchemeBean.AddItem(category);
            }

            //// TODO handle this at CategorisationRetrievalEngine
            //// this.PopulateDataflowRef(sysId, category.DataflowRef);
        }

        #endregion
    }
}