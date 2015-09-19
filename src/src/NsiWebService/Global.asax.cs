// -----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="EUROSTAT">
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
namespace Estat.Nsi.DataDisseminationWS
{
    using System;
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Web.Routing;

    using Estat.Sri.Ws.Rest;
    using Estat.Sri.Ws.Soap;
    using Estat.Sri.Ws.Wsdl;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Util.ResourceBundle;
    using Org.Sdmxsource.Util.Url;

    /// <summary>
    /// The global.
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(Global));

        #region Methods

        /// <summary>
        /// The application_ authenticate request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application_ begin request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string path = Request.PhysicalPath;
            var rawUrl = Request.RawUrl;
            var url = Request.Url.AbsolutePath;
            _log.DebugFormat("path {0}, raw URL : {1}, url: {2}", path, rawUrl, url);

            // rewrite URL for WCF WSDL requests.
            if (rawUrl.EndsWith("Service?wsdl", StringComparison.OrdinalIgnoreCase))
            {
                string originalPath = HttpContext.Current.Request.Path.ToLowerInvariant();
                var index = url.LastIndexOf('/');
                if (index > -1)
                {
                    var lastPart = url.Substring(index + 1);
                    var typeName = WsdlRegistry.Instance.GetWsdlInfo(lastPart);
                    
                    if (typeName != null)
                    {
                        var wsdlUri = originalPath.Replace(lastPart.ToLowerInvariant(), "wsdl/" + typeName.Name);
                        Context.RewritePath(wsdlUri, null, null);
                    }
                }
            }
        }

        /// <summary>
        /// The application_ end.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_End(object sender, EventArgs e)
        {
            _log.Debug("Ending application");
        }

        /// <summary>
        /// The application_ error.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application_ start.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_Start(object sender, EventArgs e)
        {
            _log.Debug("Starting application");
            SdmxException.SetMessageResolver(new MessageDecoder());
            UriUtils.FixSystemUriDotBug();

            var wsdlRegistry = WsdlRegistry.Instance;
            var sdmxRestServiceHostFactory = new SdmxRestServiceHostFactory(typeof(IStaticWsdlService));

            var serviceRoute = new ServiceRoute("wsdl", sdmxRestServiceHostFactory, typeof(StaticWsdlService));
            RouteTable.Routes.Add(serviceRoute);
            wsdlRegistry.Add(
                new WsdlInfo { Name  = "NSIEstatV20Service", OriginalPath = "sdmx_estat/NSI.wsdl" },
                new WsdlInfo { Name = "NSIStdV20Service", OriginalPath = "sdmx_org/NSI.wsdl" },
                new WsdlInfo { Name = "SdmxService", OriginalPath = "sdmxv21/SDMX-WS.wsdl" });

            RouteTable.Routes.Add(new ServiceRoute("NSIEstatV20Service", new SoapServiceHostFactory(typeof(INSIEstatV20Service), "sdmx_estat/NSI.wsdl"), typeof(NsiEstatV20Service)));
            RouteTable.Routes.Add(new ServiceRoute("NSIStdV20Service", new SoapServiceHostFactory(typeof(INSIStdV20Service), "sdmx_org/NSI.wsdl"), typeof(NsiStdV20Service)));
            RouteTable.Routes.Add(new ServiceRoute("SdmxService", new SoapServiceHostFactory(typeof(INSIStdV21Service), "sdmxv21/SDMX-WS.wsdl"), typeof(NSIStdV21Service)));
            RouteTable.Routes.Add(new ServiceRoute("rest/data", new SdmxRestServiceHostFactory(typeof(IDataResource)), typeof(DataResource)));
            RouteTable.Routes.Add(new ServiceRoute("rest", new SdmxRestServiceHostFactory(typeof(IStructureResource)), typeof(StructureResource)));
        }

        /// <summary>
        /// The session_ end.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The session_ start.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        #endregion
    }
}