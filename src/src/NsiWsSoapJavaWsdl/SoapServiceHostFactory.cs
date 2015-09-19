// -----------------------------------------------------------------------
// <copyright file="SoapServiceHostFactory.cs" company="EUROSTAT">
//   Date Created : 2013-10-07
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
namespace Estat.Sri.Ws.Soap
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Description;

    using Estat.Sri.Ws.Controllers.Builder;

    using log4net;

    /// <summary>
    /// The soap service host factory.
    /// </summary>
    public class SoapServiceHostFactory : ServiceHostFactory
    {
        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SoapServiceHostFactory));

        #endregion

        #region Fields

        /// <summary>
        /// The _fault soap SDMX v2.0 builder.
        /// </summary>
        private readonly MessageFaultSoapv20Builder _faultSoapv20Builder = new MessageFaultSoapv20Builder();

        /// <summary>
        /// The _type.
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// The WSDL location.
        /// </summary>
        private readonly string _wsdlLocation;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SoapServiceHostFactory"/> class.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="wsdlLocation">
        /// The WSDL location.
        /// </param>
        public SoapServiceHostFactory(Type type, string wsdlLocation)
        {
            this._type = type;
            this._wsdlLocation = wsdlLocation;
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
                _log.DebugFormat("Creating SOAP service host for {0} for uri : {1}", serviceType, baseAddresses[0]);
                // System.Diagnostics.Debugger.Break();
                ServiceHost serviceHost = base.CreateServiceHost(serviceType, baseAddresses);

                var binding = new BasicHttpBinding { TransferMode = TransferMode.Streamed, MessageEncoding = WSMessageEncoding.Text, MaxReceivedMessageSize = 10485760 };

                var baseAddress = baseAddresses[0];
                serviceHost.AddServiceEndpoint(this._type, binding, baseAddress);
              
                serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
                serviceHost.Description.Behaviors.Add(new SdmxErrorServiceBehaviour(exception => this._faultSoapv20Builder.BuildException(exception, "Unknown")));

                //// serviceHost.Description.Behaviors.Add(new SdmxInspectorBehaviour());
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