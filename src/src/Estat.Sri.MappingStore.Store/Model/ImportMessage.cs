﻿// -----------------------------------------------------------------------
// <copyright file="ImportMessage.cs" company="EUROSTAT">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The import message.
    /// </summary>
    public class ImportMessage : IImportMessage
    {
        #region Fields

        /// <summary>
        /// The _message.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// The _structure reference.
        /// </summary>
        private readonly IStructureReference _structureReference;

        /// <summary>
        /// The _success.
        /// </summary>
        private readonly ImportMessageStatus _status;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportMessage"/> class.
        /// </summary>
        /// <param name="status">
        ///     The status of the import.
        /// </param>
        /// <param name="structureReference">
        ///     The structure reference.
        /// </param>
        /// <param name="message">
        ///     The message.
        /// </param>
        public ImportMessage(ImportMessageStatus status, IStructureReference structureReference, string message)
        {
            this._message = message;
            this._status = status;
            this._structureReference = structureReference;
        }

        #endregion

        #region Public Properties

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

        /// <summary>
        /// Gets the structure reference.
        /// </summary>
        public IStructureReference StructureReference
        {
            get
            {
                return this._structureReference;
            }
        }

        /// <summary>
        /// Gets the success.
        /// </summary>
        public ImportMessageStatus Status
        {
            get
            {
                return this._status;
            }
        }

        #endregion
    }
}