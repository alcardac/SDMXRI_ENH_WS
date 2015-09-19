// -----------------------------------------------------------------------
// <copyright file="PeriodObject.cs" company="EUROSTAT">
//   Date Created : 2013-07-30
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
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    ///     The period object.
    /// </summary>
    public class PeriodObject
    {
        #region Fields

        /// <summary>
        ///     The _codes.
        /// </summary>
        private readonly IList<string> _codes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodObject"/> class.
        /// </summary>
        /// <param name="periodLength">
        /// The period Length.
        /// </param>
        /// <param name="periodFormat">
        /// The period Format.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        public PeriodObject(int periodLength, string periodFormat, string id)
        {
            this.Id = id;
            this._codes = new string[periodLength];
            for (int i = 0; i < periodLength; i++)
            {
                this.Codes[i] = (i + 1).ToString(periodFormat, CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the codes.
        /// </summary>
        public IList<string> Codes
        {
            get
            {
                return this._codes;
            }
        }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        public string Id { get; private set; }

        #endregion
    }
}