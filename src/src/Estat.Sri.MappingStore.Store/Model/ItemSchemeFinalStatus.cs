// -----------------------------------------------------------------------
// <copyright file="ItemSchemeFinalStatus.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Model
{
    /// <summary>
    /// The cached item.
    /// </summary>
    public class ItemSchemeFinalStatus
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeFinalStatus"/> class.
        /// </summary>
        /// <param name="finalStatus">
        /// The final Status.
        /// </param>
        public ItemSchemeFinalStatus(ArtefactFinalStatus finalStatus)
            : this(finalStatus, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeFinalStatus"/> class.
        /// </summary>
        /// <param name="finalStatus">
        /// The final Status.
        /// </param>
        /// <param name="itemIdMap">
        /// The item Id Map.
        /// </param>
        public ItemSchemeFinalStatus(ArtefactFinalStatus finalStatus, ItemStatusCollection itemIdMap)
        {
            this.ItemIdMap = itemIdMap;
            this.FinalStatus = finalStatus;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the final status.
        /// </summary>
        public ArtefactFinalStatus FinalStatus { get; private set; }

        /// <summary>
        /// Gets the item id map.
        /// </summary>
        public ItemStatusCollection ItemIdMap { get; private set; }

        #endregion
    }
}