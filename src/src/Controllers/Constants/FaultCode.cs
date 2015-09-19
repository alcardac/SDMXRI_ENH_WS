// -----------------------------------------------------------------------
// <copyright file="FaultCode.cs" company="EUROSTAT">
//   Date Created : 2013-10-24
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
namespace Estat.Sri.Ws.Controllers.Constants
{
    using System.ServiceModel;

    /// <summary>
    /// The fault code defaults.
    /// </summary>
    public static class FaultCodeDefaults
    {
        #region Static Fields

        /// <summary>
        /// The _client.
        /// </summary>
        private static readonly FaultCode _client;

        /// <summary>
        /// The _server.
        /// </summary>
        private static readonly FaultCode _server;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="FaultCodeDefaults"/> class.
        /// </summary>
        static FaultCodeDefaults()
        {
            _client = new FaultCode("Client", "http://schemas.xmlsoap.org/soap/envelope/");
            _server = new FaultCode("Server", "http://schemas.xmlsoap.org/soap/envelope/");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the client.
        /// </summary>
        public static FaultCode Client
        {
            get
            {
                return _client;
            }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public static FaultCode Server
        {
            get
            {
                return _server;
            }
        }

        #endregion
    }
}