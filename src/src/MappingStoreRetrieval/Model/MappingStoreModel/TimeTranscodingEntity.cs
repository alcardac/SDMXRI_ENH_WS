// -----------------------------------------------------------------------
// <copyright file="TimeTranscodingEntity.cs" company="EUROSTAT">
//   Date Created : 2013-07-10
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
    ///     The time transcoding entity.
    /// </summary>
    public class TimeTranscodingEntity
    {
        #region Fields

        /// <summary>
        /// Gets the frequency value.
        /// </summary>
        private readonly string _frequencyValue;

        /// <summary>
        /// Gets the transcoding id.
        /// </summary>
        private readonly long _transcodingId;

        /// <summary>
        /// The transcoding rules.
        /// </summary>
        private TranscodingRulesEntity _transcodingRules;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTranscodingEntity"/> class. 
        /// </summary>
        /// <param name="frequencyValue">
        /// The frequency Value.
        /// </param>
        /// <param name="transcodingId">
        /// The transcoding Id.
        /// </param>
        public TimeTranscodingEntity(string frequencyValue, long transcodingId)
        {
            this._frequencyValue = frequencyValue;
            this._transcodingId = transcodingId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the frequency value.
        /// </summary>
        public string FrequencyValue
        {
            get
            {
                return this._frequencyValue;
            }
        }

        /// <summary>
        /// Gets the transcoding id.
        /// </summary>
        public long TranscodingId
        {
            get
            {
                return this._transcodingId;
            }
        }

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the period column id.
        /// </summary>
        public long PeriodColumnId { get; set; }

        /// <summary>
        /// Gets or sets the year column id.
        /// </summary>
        public long YearColumnId { get; set; }

        /// <summary>
        /// Gets or sets the date column id.
        /// </summary>
        public long DateColumnId { get; set; }

        /// <summary>
        /// Gets or sets the transcoding rules.
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

        #endregion

        /// <summary>
        /// Deep copy the current object.
        /// </summary>
        /// <param name="transcodingId">
        /// The transcoding id.
        /// </param>
        /// <returns>
        /// The <see cref="TimeTranscodingEntity"/>.
        /// </returns>
        public TimeTranscodingEntity DeepCopy(long transcodingId)
        {
            return new TimeTranscodingEntity(this._frequencyValue, transcodingId) { Expression = this.Expression, DateColumnId = this.DateColumnId, PeriodColumnId = this.PeriodColumnId, YearColumnId = this.YearColumnId }; 
        }
    }
}