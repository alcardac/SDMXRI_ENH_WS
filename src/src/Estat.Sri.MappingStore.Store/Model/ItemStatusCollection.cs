// -----------------------------------------------------------------------
// <copyright file="ItemStatusCollection.cs" company="EUROSTAT">
//   Date Created : 2013-04-22
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
namespace Estat.Sri.MappingStore.Store.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    ///     The item status collection.
    /// </summary>
    public class ItemStatusCollection : KeyedCollection<string, ItemStatus>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemStatusCollection" /> class.
        /// </summary>
        public ItemStatusCollection()
            : base(StringComparer.Ordinal, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStatusCollection"/> class.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        public ItemStatusCollection(IEnumerable<ItemStatus> items)
        {
            foreach (var itemStatuse in items)
            {
                this.Add(itemStatuse);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the <paramref name="status"/> associated with the specified <paramref name="id"/>
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="id"/> is null.
        /// </exception>
        /// <returns>
        /// True is there is a <paramref name="status"/> associated with the specified <paramref name="id"/>; otherwise false.
        /// </returns>
        public bool TryGetValue(string id, out ItemStatus status)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            var itemStatuses = this.Dictionary;
            if (itemStatuses != null)
            {
                return itemStatuses.TryGetValue(id, out status);
            }

            status = this.Items.FirstOrDefault(itemStatus => itemStatus.Id.Equals(id));
            return status != null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        /// <param name="item">
        /// The element from which to extract the key.
        /// </param>
        protected override string GetKeyForItem(ItemStatus item)
        {
            return item.Id;
        }

        #endregion
    }
}