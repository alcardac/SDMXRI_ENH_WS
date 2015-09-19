// -----------------------------------------------------------------------
// <copyright file="CategoryImportEngine.cs" company="EUROSTAT">
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
    using System.Collections.Generic;
    using System.Data.Common;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;

    /// <summary>
    /// The category import engine.
    /// </summary>
    public class CategoryImportEngine : ItemBaseEngine<ICategoryObject, InsertCategory>
    {
        /// <summary>
        /// The _stored procedures
        /// </summary>
        private static readonly StoredProcedures _storedProcedures;

        /// <summary>
        /// Initializes static members of the <see cref="CategoryImportEngine"/> class.
        /// </summary>
        static CategoryImportEngine()
        {
            _storedProcedures = new StoredProcedures();
        }

        /// <summary>
        /// Insert the specified <paramref name="items"/> to the mapping store with <paramref name="state"/>
        /// </summary>
        /// <param name="state">
        ///     The MAPPING STORE connection and transaction state
        /// </param>
        /// <param name="items">
        ///     The items.
        /// </param>
        /// <param name="parentArtefact"> The primary key of the parent artefact.</param>
        /// <returns>
        /// The <see cref="IEnumerable{ArtefactImportStatus}"/>.
        /// </returns>
        public override IEnumerable<long> Insert(DbTransactionState state, IEnumerable<ICategoryObject> items, long parentArtefact)
        {
            var categoryIds = new List<long>();
            var queue = new Queue<KeyValuePair<long, IEnumerable<ICategoryObject>>>();
            queue.Enqueue(new KeyValuePair<long, IEnumerable<ICategoryObject>>(0, items));
            while (queue.Count > 0)
            {
                var keyValuePair = queue.Dequeue();
                long parentCategoryId = keyValuePair.Key;
                foreach (var categoryObject in keyValuePair.Value)
                {
                    var itemID = this.InsertCategory(state, parentArtefact, parentCategoryId, categoryObject);

                    queue.Enqueue(new KeyValuePair<long, IEnumerable<ICategoryObject>>(itemID, categoryObject.Items));

                    categoryIds.Add(itemID);
                }
            }

            return categoryIds;
        }

        /// <summary>
        /// Insert the specified <paramref name="categoryObject"/> to mapping store
        /// </summary>
        /// <param name="state">
        /// The mapping store connection state.
        /// </param>
        /// <param name="parentArtefact">
        /// The parent artefact.
        /// </param>
        /// <param name="parentCategoryID">
        /// The parent category id.
        /// </param>
        /// <param name="categoryObject">
        /// The category object.
        /// </param>
        /// <returns>
        /// The inserted record primary key. <c>CATEGORY.CAT_ID</c>
        /// </returns>
        private long InsertCategory(DbTransactionState state, long parentArtefact, long parentCategoryID, ICategoryObject categoryObject)
        {
            long itemID;
            var itemProcedure = _storedProcedures.InsertCategory;
            using (DbCommand command = itemProcedure.CreateCommandWithDefaults(state))
            {
                DbParameter itemSchemeParameter = itemProcedure.CreateSchemeIdParameter(command);
                itemSchemeParameter.Value = parentArtefact;

                if (parentCategoryID > 0)
                {
                    itemProcedure.CreateParentItemIdParameter(command).Value = parentCategoryID;
                }

                itemID = this.RunCommand(categoryObject, command, itemProcedure, state);
            }

            return itemID;
        }
    }
}