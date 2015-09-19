// -----------------------------------------------------------------------
// <copyright file="WebServiceInfoBuilder.cs" company="EUROSTAT">
//   Date Created : 2012-04-11
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
namespace Estat.Nsi.DataDisseminationWS.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Web.Services;

    using Estat.Sri.Ws.Controllers.Model;
    using Estat.Sri.Ws.Soap;

    using Resources;

    /// <summary>
    /// Build the collection of <see cref="WebServiceInfo"/>
    /// </summary>
    internal static class WebServiceInfoBuilder
    {
        #region Constants and Fields

        /// <summary>
        ///   The service descriptions
        /// </summary>
        private static readonly string[] _names = { Messages.label_stdv20, Messages.label_estatv20, Messages.label_stdv20_wcf, Messages.label_estatv20_wcf, Messages.label_stdv21 };

        /// <summary>
        ///   The schema folders TODO those are configurable
        /// </summary>
        private static readonly string[] _schemas = { "sdmx_org", "sdmx_estat", "sdmx_org", "sdmx_estat", "sdmxv21" };

        /// <summary>
        ///   The current endpoint list
        /// </summary>
        private static readonly Type[] _types = { typeof(NSIStdV20Service), typeof(NSIEstatV20Service), typeof(INSIStdV20Service), typeof(INSIEstatV20Service), typeof(INSIStdV21Service) };

        /// <summary>
        ///   The collection of web service info
        /// </summary>
        private static readonly WebServiceInfo[] _webServices = BuildEndpoints();

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the collection of web service info
        /// </summary>
        public static IList<WebServiceInfo> WebServices
        {
            get
            {
                return _webServices;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build the endpoint array
        /// </summary>
        /// <returns>
        /// The endpoint info array 
        /// </returns>
        private static WebServiceInfo[] BuildEndpoints()
        {
            var endpointInfos = new WebServiceInfo[_types.Length];
            for (int i = 0; i < _types.Length; i++)
            {
                endpointInfos[i] = GetEndpoint(_types[i]);
                if (endpointInfos[i] != null)
                {
                    endpointInfos[i].SchemaPath = _schemas[i];
                    endpointInfos[i].Description = _names[i];
                }
            }

            return endpointInfos;
        }

        /// <summary>
        /// Gets the <see cref="WebServiceInfo"/> from the specified <paramref name="type"/> if it has the <see cref="WebServiceAttribute"/>
        /// </summary>
        /// <param name="type">
        /// The type which should have the <see cref="WebServiceInfo"/> 
        /// </param>
        /// <returns>
        /// the <see cref="WebServiceInfo"/> from the specified <paramref name="type"/> if it has the <see cref="WebServiceAttribute"/> ; otherwise null 
        /// </returns>
        private static WebServiceInfo GetEndpoint(Type type)
        {
            var attributes = type.GetCustomAttributes(true);

            // ASMX
            var webServiceAttribute = attributes.OfType<WebServiceAttribute>().FirstOrDefault();
            if (webServiceAttribute != null)
            {
                var info = new WebServiceInfo { Name = type.Name + ".asmx", Namespace = webServiceAttribute.Namespace };
                return info;
            }

            // WCF
            var serviceContractAttribute = attributes.OfType<ServiceContractAttribute>().FirstOrDefault();
            if (serviceContractAttribute != null)
            {
                return new WebServiceInfo { Name = serviceContractAttribute.Name, Namespace = serviceContractAttribute.Namespace };
            }

            return null;
        }

        #endregion
    }
}