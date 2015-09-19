// -----------------------------------------------------------------------
// <copyright file="ArtefactImportStatus.cs" company="EUROSTAT">
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
    /// The artefact import status.
    /// </summary>
    public class ArtefactImportStatus
    {
        #region Fields

        /// <summary>
        /// The _import message.
        /// </summary>
        private readonly IImportMessage _importMessage;

        /// <summary>
        /// The _primary key value.
        /// </summary>
        private readonly long _primaryKeyValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactImportStatus"/> class. 
        /// </summary>
        /// <param name="primaryKeyValue">
        /// The primary Key Value.
        /// </param>
        /// <param name="importMessage">
        /// The import Message.
        /// </param>
        public ArtefactImportStatus(long primaryKeyValue, IImportMessage importMessage)
        {
            this._primaryKeyValue = primaryKeyValue;
            this._importMessage = importMessage;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the import message.
        /// </summary>
        public IImportMessage ImportMessage
        {
            get
            {
                return this._importMessage;
            }
        }

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        public long PrimaryKeyValue
        {
            get
            {
                return this._primaryKeyValue;
            }
        }

        #endregion
    }
}