﻿// -----------------------------------------------------------------------
// <copyright file="MessageFaultSoapv21Builder.cs" company="EUROSTAT">
//   Date Created : 2013-10-25
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
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.ServiceModel;

    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///     The web fault exception rest builder.
    /// </summary>
    public class MessageFaultSoapv21Builder
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(MessageFaultSoapv21Builder));

        /// <summary>
        ///     The _sdmx error code internal error.
        /// </summary>
        private static readonly SdmxErrorCode _sdmxErrorCodeIntenalError;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="MessageFaultSoapv21Builder" /> class.
        /// </summary>
        static MessageFaultSoapv21Builder()
        {
            _sdmxErrorCodeIntenalError = SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds an object of type  <see cref="FaultException"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <param name="uri">
        /// The URI.
        /// </param>
        /// <returns>
        /// Object of type <see cref="FaultException{String}"/>
        /// </returns>
        public FaultException<SdmxFault> BuildException(SdmxException buildFrom, string uri)
        {
            _log.ErrorFormat("SdmxError : {0}, code : {1}", buildFrom.SdmxErrorCode.ErrorString, buildFrom.SdmxErrorCode.ClientErrorCode);
            _log.Error(buildFrom.FullMessage, buildFrom);
            var sdmxFault = new SdmxFault(buildFrom.SdmxErrorCode.ErrorString, buildFrom.SdmxErrorCode.ClientErrorCode, uri);
            if (buildFrom.SdmxErrorCode.EnumType.IsClientError())
            {
                return CreateExceptionClient(sdmxFault);
            }

            return CreateExceptionServer(sdmxFault);
        }

        /// <summary>
        /// Builds an object of type  <see cref="FaultException{String}"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <param name="uri">
        /// The URI.
        /// </param>
        /// <returns>
        /// Object of type <see cref="FaultException{String}"/>
        /// </returns>
        public FaultException<SdmxFault> BuildException(Exception buildFrom, string uri)
        {
            var faultException = buildFrom as FaultException<SdmxFault>;
            if (faultException != null)
            {
                return faultException;
            }

            var webException = buildFrom as FaultException;
            if (webException != null)
            {
                SdmxFault fault;
                if (webException.Code.IsReceiverFault)
                {
                    fault = new SdmxFault(_sdmxErrorCodeIntenalError.ErrorString, _sdmxErrorCodeIntenalError.ClientErrorCode, uri);
                    return CreateExceptionServer(fault);
                }

                fault = new SdmxFault(buildFrom.Message, _sdmxErrorCodeIntenalError.ClientErrorCode, uri);
                return CreateExceptionClient(fault);
            }

            var sdmxException = buildFrom.ToSdmxException();
            if (sdmxException != null)
            {
                return this.BuildException(sdmxException, uri);
            }

            return CreateExceptionServer(new SdmxFault(_sdmxErrorCodeIntenalError.ErrorString, _sdmxErrorCodeIntenalError.ClientErrorCode, uri));
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create exception client.
        /// </summary>
        /// <param name="sdmxFault">
        /// The sdmx fault.
        /// </param>
        /// <returns>
        /// The <see cref="FaultException"/>.
        /// </returns>
        private static FaultException<SdmxFault> CreateExceptionClient(SdmxFault sdmxFault)
        {
            var faultReason = new FaultReason(sdmxFault.ErrorMessage);
            return new FaultException<SdmxFault>(sdmxFault, faultReason, FaultCodeDefaults.Client);
        }

        /// <summary>
        /// The create exception server.
        /// </summary>
        /// <param name="sdmxFault">
        /// The sdmx fault.
        /// </param>
        /// <returns>
        /// The <see cref="FaultException"/>.
        /// </returns>
        private static FaultException<SdmxFault> CreateExceptionServer(SdmxFault sdmxFault)
        {
            var faultReason = new FaultReason(sdmxFault.ErrorMessage);
            return new FaultException<SdmxFault>(sdmxFault, faultReason, FaultCodeDefaults.Server);
        }

        #endregion
    }
}