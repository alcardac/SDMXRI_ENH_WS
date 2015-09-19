// -----------------------------------------------------------------------
// <copyright file="NSIEstatV20Service.asmx.cs" company="EUROSTAT">
//   Date Created : 2011-10-12
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
namespace Estat.Nsi.DataDisseminationWS
{
    using System.ComponentModel;
    using System.Web.Services;

    using Estat.Sri.Ws.Controllers.Constants;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.0 Schema files with Eurostat extensions
    /// </summary>
    /// <example>
    /// A simple <see cref="NSIStdV20Service"/> client in C#
    /// <code source="ReUsingExamples\NsiWebService\ReUsingWebService.cs" lang="cs"/>
    /// The <code>WSDLSettings</code> class used by the client
    /// <code source="ReUsingExamples\NsiWebService\WSDLSettings.cs" lang="cs"/>
    /// </example>
    [WebService(Namespace = "http://ec.europa.eu/eurostat/sri/service/2.0/extended", 
        Description =
            "Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.0 Schema files with Eurostat extensions")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class NSIEstatV20Service : NSIStdV20Service
    {
        /// <summary>
        /// Gets the <see cref="SdmxSchema"/> for this service
        /// </summary>
        protected override WebServiceEndpoint Endpoint 
        {
            get
            {
                return WebServiceEndpoint.EstatEndpoint;
            }
        }
    }
}