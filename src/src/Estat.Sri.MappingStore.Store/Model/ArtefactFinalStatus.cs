// -----------------------------------------------------------------------
// <copyright file="ArtefactFinalStatus.cs" company="EUROSTAT">
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
    /// The artefact final status.
    /// </summary>
    public class ArtefactFinalStatus
    {
        /// <summary>
        /// The _empty <see cref="ArtefactFinalStatus"/>
        /// </summary>
        private static readonly ArtefactFinalStatus _empty;

        #region Fields

        /// <summary>
        /// The _is final.
        /// </summary>
        private readonly bool _isFinal;

        /// <summary>
        /// The _primary key.
        /// </summary>
        private readonly long _primaryKey;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ArtefactFinalStatus"/> class.
        /// </summary>
        static ArtefactFinalStatus()
        {
            _empty = new ArtefactFinalStatus(-1, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactFinalStatus"/> class. 
        /// </summary>
        /// <param name="primaryKey">
        /// The primary Key.
        /// </param>
        /// <param name="isFinal">
        /// The is Final.
        /// </param>
        public ArtefactFinalStatus(long primaryKey, bool isFinal)
        {
            this._primaryKey = primaryKey;
            this._isFinal = isFinal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactFinalStatus"/> class. 
        /// </summary>
        /// <param name="primaryKey">
        /// The primary Key.
        /// </param>
        /// <param name="isFinal">
        /// The is Final.
        /// </param>
        public ArtefactFinalStatus(long primaryKey, int isFinal)
        {
            this._primaryKey = primaryKey;
            this._isFinal = isFinal != 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactFinalStatus"/> class. 
        /// </summary>
        /// <param name="primaryKey">
        /// The primary Key.
        /// </param>
        /// <param name="isFinal">
        /// The is Final.
        /// </param>
        public ArtefactFinalStatus(long primaryKey, short isFinal)
        {
            this._primaryKey = primaryKey;
            this._isFinal = isFinal != 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the empty <see cref="ArtefactFinalStatus"/>.
        /// </summary>
        public static ArtefactFinalStatus Empty
        {
            get
            {
                return _empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is final.
        /// </summary>
        public bool IsFinal
        {
            get
            {
                return this._isFinal;
            }
        }

        /// <summary>
        /// Gets the primary key.
        /// </summary>
        public long PrimaryKey
        {
            get
            {
                return this._primaryKey;
            }
        }

        /// <summary>
        /// Gets a value indicating whether it is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is empty]; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return ReferenceEquals(_empty, this);
            }
        }

        #endregion
    }
}