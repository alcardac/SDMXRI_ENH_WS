// -----------------------------------------------------------------------
// <copyright file="MappingSetEntity.cs" company="EUROSTAT">
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
    /// This value object represents a mappingset
    /// </summary>
    public class MappingSetEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The _mappings.
        /// </summary>
        private readonly Collection<MappingEntity> _mappings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingSetEntity"/> class. 
        /// Default constructor used to initialize the 
        /// <see cref="Mappings"/>
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public MappingSetEntity(long sysId)
            : base(sysId)
        {
            this._mappings = new Collection<MappingEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the mappingset dataset
        /// </summary>
        public DataSetEntity DataSet { get; set; }

        /// <summary>
        /// Gets or sets the mappingset dataflow
        /// </summary>
        public DataflowEntity Dataflow { get; set; }

        /// <summary>
        /// Gets or sets the mappingset description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the mappingset identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the mappingset list of mappings
        /// </summary>
        public Collection<MappingEntity> Mappings
        {
            get
            {
                return this._mappings;
            }
        }

        #endregion
    }
}