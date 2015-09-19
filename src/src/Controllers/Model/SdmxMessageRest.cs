// -----------------------------------------------------------------------
// <copyright file="SdmxMessageRest.cs" company="EUROSTAT">
//   Date Created : 2013-11-18
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
namespace Estat.Sri.Ws.Controllers.Model
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Controller;

    /// <summary>
    ///     The SDMX Message for REST (no soap envelope).
    /// </summary>
    public class SdmxMessageRest : SdmxMessageBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxMessageRest"/> class.
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        /// <param name="exceptionHandler">
        /// The exception handler.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// controller
        ///     or
        ///     exceptionHandler
        ///     or
        ///     messageVersion
        /// </exception>
        public SdmxMessageRest(IStreamController<XmlWriter> controller, Func<Exception, FaultException> exceptionHandler)
            : base(controller, exceptionHandler, MessageVersion.None)
        {
        }

        #endregion
    }
}