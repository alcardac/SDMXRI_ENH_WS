// -----------------------------------------------------------------------
// <copyright file="ItemEntity.cs" company="EUROSTAT">
//   Date Created : 2013-04-10
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
namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This class represents an SDMX Item. Items have a string id.
    /// </summary>
    public abstract class ItemEntity : PersistentEntityBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        protected ItemEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the item identifier
        /// </summary>
        public string Id { get; set; }

        #endregion
    }
}