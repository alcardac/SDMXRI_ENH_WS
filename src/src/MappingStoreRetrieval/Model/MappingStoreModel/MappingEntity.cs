// -----------------------------------------------------------------------
// <copyright file="MappingEntity.cs" company="EUROSTAT">
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
    /// This class refers to a Mapping Set. It is in fact a value object to 
    /// contain the values retrieved by the MA store.
    /// </summary>
    public class MappingEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The list of local columns
        /// </summary>
        private readonly Collection<DataSetColumnEntity> _columns;

        /// <summary>
        /// The list of DSD components
        /// </summary>
        private readonly Collection<ComponentEntity> _components;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntity"/> class. 
        /// Default constructor used to initialize 
        /// <see cref="Components"/>
        /// and <see cref="Columns"/>
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public MappingEntity(long sysId)
            : base(sysId)
        {
            this._components = new Collection<ComponentEntity>();
            this._columns = new Collection<DataSetColumnEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the mapping columns
        /// </summary>
        public Collection<DataSetColumnEntity> Columns
        {
            get
            {
                return this._columns;
            }
        }

        /// <summary>
        /// Gets the mapping components
        /// </summary>
        public Collection<ComponentEntity> Components
        {
            get
            {
                return this._components;
            }
        }

        /// <summary>
        /// Gets or sets the mapping constant
        /// </summary>
        public string Constant { get; set; }

        /// <summary>
        /// Gets or sets the mapping type
        /// </summary>
        public string MappingType { get; set; }

        /// <summary>
        /// Gets or sets the mapping transcoding
        /// </summary>
        public TranscodingEntity Transcoding { get; set; }

        #endregion
    }
}