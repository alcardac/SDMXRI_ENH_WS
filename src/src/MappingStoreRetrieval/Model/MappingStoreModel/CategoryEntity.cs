// -----------------------------------------------------------------------
// <copyright file="CategoryEntity.cs" company="EUROSTAT">
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
    /// This class represents a Mapping Store Category.
    /// </summary>
    public class CategoryEntity : ItemEntity
    {
        #region Constants and Fields

        /// <summary>
        /// The _categories.
        /// </summary>
        private readonly Collection<CategoryEntity> _categories;

        /// <summary>
        /// The _dataflows.
        /// </summary>
        private readonly Collection<DataflowEntity> _dataflows;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryEntity"/> class. 
        /// Mapping Store SDMX CategoryEntity constructor.
        /// This constructor initialises the child categories and Dataflows collection
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public CategoryEntity(long sysId)
            : base(sysId)
        {
            this._categories = new Collection<CategoryEntity>();
            this._dataflows = new Collection<DataflowEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of child categories
        /// </summary>
        public Collection<CategoryEntity> Categories
        {
            get
            {
                return this._categories;
            }
        }

        /// <summary>
        /// Gets the list of linked dataflows
        /// </summary>
        public Collection<DataflowEntity> Dataflows
        {
            get
            {
                return this._dataflows;
            }
        }

        #endregion
    }
}