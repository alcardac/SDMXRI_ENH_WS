// -----------------------------------------------------------------------
// <copyright file="DsdEntity.cs" company="EUROSTAT">
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
    /// This is the entity class representing a DSD
    /// </summary>
    public class DsdEntity : ArtefactEntity
    {
        #region Constants and Fields

        /// <summary>
        /// The list of data structure definition attributes
        /// </summary>
        private readonly Collection<ComponentEntity> _attributes = new Collection<ComponentEntity>();

        /// <summary>
        /// The list of data structure definition crossSectionalMeasures
        /// </summary>
        private readonly Collection<ComponentEntity> _crossSectionalMeasures = new Collection<ComponentEntity>();

        /// <summary>
        /// The list of data structure definition dimensions
        /// </summary>
        private readonly Collection<ComponentEntity> _dimensions = new Collection<ComponentEntity>();

        /// <summary>
        /// The list of data structure definition groups
        /// </summary>
        private readonly Collection<GroupEntity> _groups = new Collection<GroupEntity>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DsdEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public DsdEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of data structure definition attributes
        /// </summary>
        public Collection<ComponentEntity> Attributes
        {
            get
            {
                return this._attributes;
            }
        }

        /// <summary>
        /// Gets the list of data structure definition cross sectional measures
        /// </summary>
        public Collection<ComponentEntity> CrossSectionalMeasures
        {
            // MAT-130
            get
            {
                return this._crossSectionalMeasures;
            }
        }

        /// <summary>
        /// Gets the list of data structure definition dimensions
        /// </summary>
        public Collection<ComponentEntity> Dimensions
        {
            get
            {
                return this._dimensions;
            }
        }

        /// <summary>
        /// Gets the list of data structure definition groups
        /// </summary>
        public Collection<GroupEntity> Groups
        {
            get
            {
                return this._groups;
            }
        }

        /// <summary>
        /// Gets or sets the data structure definition primary measure
        /// </summary>
        public ComponentEntity PrimaryMeasure { get; set; }

        /// <summary>
        /// Gets or sets the data structure definition time dimension
        /// </summary>
        public ComponentEntity TimeDimension { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The method is used to add a <see cref="GroupEntity"/> 
        /// to the data structure definition list of groups
        /// </summary>
        /// <param name="group">
        /// The <see cref="GroupEntity"/> 
        /// that needs to be added
        /// </param>
        public void AddGroup(GroupEntity group)
        {
            this._groups.Add(group);
        }

        #endregion
    }
}