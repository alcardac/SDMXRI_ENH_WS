// -----------------------------------------------------------------------
// <copyright file="SdmxRestServiceHostFactory.cs" company="EUROSTAT">
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
namespace Estat.Sri.Ws.Rest
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Description;
    using System.ServiceModel.Web;
    using System.Text;

    using log4net;

    /// <summary>
    /// The sdmx rest service host factory.
    /// </summary>
    public class SdmxRestServiceHostFactory : WebServiceHostFactory
    {
        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SdmxRestServiceHostFactory));

        #endregion

        #region Fields

        /// <summary>
        /// The _type.
        /// </summary>
        private readonly Type _type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxRestServiceHostFactory"/> class.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        public SdmxRestServiceHostFactory(Type type)
        {
            _log.DebugFormat("Init SdmxRestServiceHostFactory({0})", type);
            this._type = type;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create service host.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="baseAddresses">
        /// The base addresses.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceHost"/>.
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            try
            {
                _log.DebugFormat("Creating REST service host for {0} for uri : {1}", serviceType, baseAddresses[0]);
                ServiceHost serviceHost = base.CreateServiceHost(serviceType, baseAddresses);

                var webBehavior = new WebHttpBehavior { AutomaticFormatSelectionEnabled = false, HelpEnabled = true, FaultExceptionEnabled = false, DefaultBodyStyle = WebMessageBodyStyle.Bare };
                var binding = new WebHttpBinding { TransferMode = TransferMode.Streamed, ContentTypeMapper = new SdmxContentMapper()};
                var endpoint = serviceHost.AddServiceEndpoint(this._type, binding, baseAddresses[0]);

                endpoint.Behaviors.Add(webBehavior);

                return serviceHost;
            }
            catch (Exception e)
            {
                _log.Error("While creating service host", e);
                throw;
            }
        }

        #endregion
    }
}