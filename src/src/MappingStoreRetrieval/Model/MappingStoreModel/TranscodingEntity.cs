// -----------------------------------------------------------------------
// <copyright file="TranscodingEntity.cs" company="EUROSTAT">
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
    /// This class represents a Transcoding rule
    /// </summary>
    public class TranscodingEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// Gets the time transcoding collection.
        /// </summary>
        private readonly TimeTranscodingCollection _timeTranscodingCollection;

        /// <summary>
        /// The transcoding dictionaries
        /// </summary>
        private TranscodingRulesEntity _transcodingRules = new TranscodingRulesEntity();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranscodingEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public TranscodingEntity(long sysId)
            : base(sysId)
        {
            this._timeTranscodingCollection = new TimeTranscodingCollection(sysId);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expression to evaluate in mapping time for transcoding
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the transcoding dictionaries
        /// </summary>
        public TranscodingRulesEntity TranscodingRules
        {
            get
            {
                return this._transcodingRules;
            }

            set
            {
                this._transcodingRules = value;
            }
        }

        /// <summary>
        /// Gets the time transcoding collection.
        /// </summary>
        public TimeTranscodingCollection TimeTranscodingCollection
        {
            get
            {
                return this._timeTranscodingCollection;
            }
        }

        #endregion
    }
}