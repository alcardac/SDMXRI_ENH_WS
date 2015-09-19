// -----------------------------------------------------------------------
// <copyright file="NsiEstatV20Service.cs" company="EUROSTAT">
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
namespace Estat.Sri.Ws.Soap
{
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    using Estat.Sri.Ws.Controllers.Constants;

    /// <summary>
    ///     The SDMX v2.0 SOAP with ESTAT extensions implementation.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class NsiEstatV20Service : NsiV20ServiceBase, INSIEstatV20Service
    {
        #region Properties

        /// <summary>
        ///     Gets the type of the endpoint.
        /// </summary>
        /// <value>
        ///     The type of the endpoint.
        /// </value>
        protected override WebServiceEndpoint EndpointType
        {
            get
            {
                return WebServiceEndpoint.EstatEndpoint;
            }
        }

        /// <summary>
        ///     Gets the namespace
        /// </summary>
        /// <value>
        ///     The namespace
        /// </value>
        protected override string Ns
        {
            get
            {
                return SoapNamespaces.SdmxV20Estat;
            }
        }

        #endregion
    }
}