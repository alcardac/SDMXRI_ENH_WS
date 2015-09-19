// -----------------------------------------------------------------------
// <copyright file="SdmxErrorServiceBehaviour.cs" company="EUROSTAT">
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
namespace Estat.Sri.Ws.Soap
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using Estat.Sri.Ws.Controllers.Model;

    /// <summary>
    /// The SDMX error service behavior.
    /// </summary>
    public class SdmxErrorServiceBehaviour : IServiceBehavior
    {
        #region Fields

        /// <summary>
        ///     The _message fault builder
        /// </summary>
        private readonly Func<Exception, FaultException<SdmxFault>> _messageFaultBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxErrorServiceBehaviour"/> class.
        /// </summary>
        /// <param name="messageFaultBuilder">
        /// The message fault builder.
        /// </param>
        public SdmxErrorServiceBehaviour(Func<Exception, FaultException<SdmxFault>> messageFaultBuilder)
        {
            this._messageFaultBuilder = messageFaultBuilder;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description of the service.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host of the service.
        /// </param>
        /// <param name="endpoints">
        /// The service endpoints.
        /// </param>
        /// <param name="bindingParameters">
        /// Custom objects to which binding elements have access.
        /// </param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            foreach (ChannelDispatcher chanDisp in serviceHostBase.ChannelDispatchers)
            {
                chanDisp.ErrorHandlers.Add(new ErrorHandler(this._messageFaultBuilder));
            }
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers,
        ///     message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host that is currently being built.
        /// </param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run
        ///     successfully.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The service host that is currently being constructed.
        /// </param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}