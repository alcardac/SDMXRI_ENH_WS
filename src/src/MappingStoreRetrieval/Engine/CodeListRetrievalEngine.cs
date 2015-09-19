// -----------------------------------------------------------------------
// <copyright file="CodeListRetrievalEngine.cs" company="EUROSTAT">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;

    /// <summary>
    ///     The code list retrieval engine.
    /// </summary>
    internal class CodeListRetrievalEngine : HierarchicalItemSchemeRetrievalEngine<ICodelistMutableObject, ICodeMutableObject>
    {
        #region Fields

        /// <summary>
        ///     The _item SQL query info.
        /// </summary>
        private readonly SqlQueryInfo _itemSqlQueryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeListRetrievalEngine" /> class.
        /// </summary>
        /// <param name="mappingStoreDb">The mapping store DB.</param>
        /// <param name="itemOrderBy">The item order by.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mappingStoreDb" /> is null</exception>
        public CodeListRetrievalEngine(Database mappingStoreDb, string itemOrderBy = CodeListConstant.ItemOrderBy)
            : base(mappingStoreDb)
        {
            var itemSqlQueryBuilder = new ItemSqlQueryBuilder(mappingStoreDb, itemOrderBy);
            this._itemSqlQueryInfo = itemSqlQueryBuilder.Build(CodeListConstant.ItemTableInfo);
        }

        /// <summary>
        ///    Gets the Code SQL query info.
        /// </summary>
        protected SqlQueryInfo ItemSqlQueryInfo
        {
            get
            {
                return this._itemSqlQueryInfo;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="ICodelistMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICodelistMutableObject" />.
        /// </returns>
        protected override ICodelistMutableObject CreateArtefact()
        {
            return new CodelistMutableCore();
        }

        /// <summary>
        ///     Create an item.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICodeMutableObject" />.
        /// </returns>
        protected override ICodeMutableObject CreateItem()
        {
            return new CodeMutableCore();
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
        protected override void FillItems(ICodelistMutableObject itemScheme, long parentSysId)
        {
            var itemQuery = new ItemSqlQuery(this.ItemSqlQueryInfo, parentSysId);
            this.FillItemWithParent(itemScheme, itemQuery);
        }

        /// <summary>
        /// Handle item child method. Override to handle item relationships
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
        protected override void HandleItemChild(
            ICodelistMutableObject itemSchemeBean, IDictionary<long, ICodeMutableObject> allItems, IDictionary<long, long> childItems, long childSysId, ICodeMutableObject child)
        {
            long parentItemId;
            if (childItems.TryGetValue(childSysId, out parentItemId))
            {
                ICodeMutableObject parent;
                if (allItems.TryGetValue(parentItemId, out parent))
                {
                    child.ParentCode = parent.Id;
                }
            }

            // HACK Move descriptions to names. 
            // In SDMX  v2.0 codes had only descriptions while in SDMX v2.1 they need to have names.
            // In mapping store (at least up to 2.7) only SDMX v2.0 was supported and code description were saved
            // as descriptions. So in case there are no names we move descriptions to names.
            if (child.Names.Count == 0)
            {
                var description = child.Descriptions;
                while (description.Count > 0)
                {
                    var index = description.Count - 1;
                    var last = description[index];
                    description.RemoveAt(index);
                    child.Names.Add(last);
                }
            }

            itemSchemeBean.AddItem(child);
        }

        #endregion
    }
}