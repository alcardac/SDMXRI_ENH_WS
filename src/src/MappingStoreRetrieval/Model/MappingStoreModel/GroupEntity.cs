// -----------------------------------------------------------------------
// <copyright file="GroupEntity.cs" company="EUROSTAT">
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
    /// The class models a group
    /// </summary>
    public class GroupEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The list of group dimensions
        /// </summary>
        private readonly Collection<ComponentEntity> _dimensions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupEntity"/> class. 
        /// Standard constructor used to initialize the dimensions internal list
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public GroupEntity(long sysId)
            : base(sysId)
        {
            this._dimensions = new Collection<ComponentEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of Dimensions
        /// </summary>
        public Collection<ComponentEntity> Dimensions
        {
            get
            {
                return this._dimensions;
            }
        }

        /// <summary>
        /// Gets or sets the Id of the Group
        /// </summary>
        public string Id { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a dimension to the Dimensions list
        /// </summary>
        /// <param name="dimension">
        /// &gt;The <see cref="ComponentEntity"/> 
        /// that needs to be added
        /// </param>
        public void AddDimensions(ComponentEntity dimension)
        {
            this._dimensions.Add(dimension);
        }

        #endregion
    }
}