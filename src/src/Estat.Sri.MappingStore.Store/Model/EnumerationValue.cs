// -----------------------------------------------------------------------
// <copyright file="EnumerationValue.cs" company="EUROSTAT">
//   Date Created : 2013-04-05
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
namespace Estat.Sri.MappingStore.Store.Model
{
    /// <summary>
    /// The enumeration value.
    /// </summary>
    public class EnumerationValue
    {
        #region Fields

        /// <summary>
        /// The _id.
        /// </summary>
        private readonly long _id;

        /// <summary>
        /// The _name.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// The _value.
        /// </summary>
        private readonly string _value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationValue"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public EnumerationValue(long id, string name, string value)
        {
            this._id = id;
            this._name = name;
            this._value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        public long ID
        {
            get
            {
                return this._id;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this._value;
            }
        }

        #endregion
    }
}