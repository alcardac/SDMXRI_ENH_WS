// -----------------------------------------------------------------------
// <copyright file="SoapFaultExceptionBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-10-19
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
    using System.Globalization;
    using System.Web.Services.Protocols;

    using Estat.Nsi.SdmxSoapValidatorExtension;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Properties;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///     The soap fault exception builder. This is used for old ASMX based Web Services. Do not use this for WCF.
    /// </summary>
    public class SoapFaultExceptionBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SoapFaultExceptionBuilder));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds an object of type  <see cref="SoapException"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <param name="uri">
        /// The URI.
        /// </param>
        /// <returns>
        /// Object of type <see cref="SoapException"/>
        /// </returns>
        /// <exception cref="T:Org.Sdmxsource.Sdmx.Api.Exception.SdmxException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public SoapException Build(Exception buildFrom, string uri)
        {
            var sdmxException = buildFrom.ToSdmxException();
            if (sdmxException != null)
            {
                return this.Build(sdmxException, uri);
            }

            _log.ErrorFormat(CultureInfo.InvariantCulture, Resources.ErrorUnhandledFormat2, buildFrom.GetType(), buildFrom.Message);
            _log.Error(buildFrom.ToString());
            return SoapFaultFactory.CreateSoapException(uri, string.Empty, Resources.ErrorInternalError, SdmxV20Errors.ErrorNumberServer, uri, false, buildFrom.Message);
        }

        /// <summary>
        /// Builds an object of type  <see cref="SoapException"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <param name="uri">
        /// The URI.
        /// </param>
        /// <returns>
        /// Object of type <see cref="SoapException"/>
        /// </returns>
        /// <exception cref="T:Org.Sdmxsource.Sdmx.Api.Exception.SdmxException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public SoapException Build(SdmxException buildFrom, string uri)
        {
            _log.ErrorFormat("SdmxError : {0}, code : {1}", buildFrom.SdmxErrorCode.ErrorString, buildFrom.SdmxErrorCode.ClientErrorCode);
            _log.Error(buildFrom.FullMessage, buildFrom);
            if (buildFrom.SdmxErrorCode.EnumType.IsClientError())
            {
                var errorMessage = string.Format("{0}: {1}", buildFrom.SdmxErrorCode.ErrorString, buildFrom.Message);
                return SoapFaultFactory.CreateSoapException(uri, string.Empty, errorMessage, SdmxV20Errors.ErrorNumberClient, uri, true, Resources.ErrorClientMessage);
            }

            return SoapFaultFactory.CreateSoapException(uri, string.Empty, Resources.ErrorInternalError, SdmxV20Errors.ErrorNumberServer, uri, false, buildFrom.SdmxErrorCode.ErrorString);
        }

        #endregion
    }
}