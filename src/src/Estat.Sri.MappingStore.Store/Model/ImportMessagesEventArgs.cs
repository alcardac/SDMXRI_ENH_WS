// -----------------------------------------------------------------------
// <copyright file="ImportMessagesEventArgs.cs" company="EUROSTAT">
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
    using System;

    /// <summary>
    /// The import messages event arguments.
    /// </summary>
    public class ImportMessagesEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// The _global id.
        /// </summary>
        private readonly string _globalId;

        /// <summary>
        /// The _message.
        /// </summary>
        private readonly string _message;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportMessagesEventArgs"/> class. 
        /// </summary>
        /// <param name="globalId">
        /// The artefact global Id.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public ImportMessagesEventArgs(string globalId, string message)
        {
            this._globalId = globalId;
            this._message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the artefact global id.
        /// </summary>
        public string GlobalId
        {
            get
            {
                return this._globalId;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get
            {
                return this._message;
            }
        }

        #endregion
    }
}