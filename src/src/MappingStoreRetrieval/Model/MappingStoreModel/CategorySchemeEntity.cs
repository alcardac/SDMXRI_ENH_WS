// -----------------------------------------------------------------------
// <copyright file="CategorySchemeEntity.cs" company="EUROSTAT">
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
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class represents a Mapping Store CategoryScheme.
    /// </summary>
    public class CategorySchemeEntity : ArtefactEntity
    {
        #region Constants and Fields

        /// <summary>
        /// List of categories that belong to this category scheme
        /// </summary>
        private readonly Collection<CategoryEntity> _categories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public CategorySchemeEntity(long sysId)
            : base(sysId)
        {
            this._categories = new Collection<CategoryEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of categories that belong to this category scheme
        /// </summary>
        public Collection<CategoryEntity> Categories
        {
            get
            {
                return this._categories;
            }
        }

        #endregion
    }
}