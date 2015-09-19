// -----------------------------------------------------------------------
// <copyright file="WebServiceInfo.cs" company="EUROSTAT">
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
namespace Estat.Sri.Ws.Controllers.Model
{
    /// <summary>
    ///     This class holds Endpoint/service information for use with the index page
    /// </summary>
    public class WebServiceInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the Namespace
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        ///     Gets or sets the SDMXMessage.xsd path
        /// </summary>
        public string SchemaPath { get; set; }

        #endregion
    }
}