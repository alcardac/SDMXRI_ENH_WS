// -----------------------------------------------------------------------
// <copyright file="SubmitStructureException.cs" company="Eurostat">
//   Date Created : 2014-03-06
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.Ws.SubmitStructure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The submit structure exception.
    /// </summary>
    public class SubmitStructureException : SdmxException
    {
        private readonly IStructureReference _structureReference;

        /// <summary>
        /// Creates Exception from an error String and an Error code
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public SubmitStructureException(string errorMessage, IStructureReference structureReference)
            : base(errorMessage)
        {
            this._structureReference = structureReference;
        }

        /// <summary>
        /// Creates an exception from a Throwable, if the Throwable is a SdmxException - then the
        ///             error code wil be used, if it is not, then InternalServerError will be used
        /// </summary>
        /// <param name="exception">The exception
        ///             </param><param name="errorMessage">the error message
        ///             </param>
        //public SubmitStructureException(Exception exception, string errorMessage, IStructureReference structureReference)
        //    : base(exception, errorMessage)
        //{
        //    this._structureReference = structureReference;
        //}

        public IStructureReference StructureReference
        {
            get
            {
                return this._structureReference;
            }
        }
    }
}