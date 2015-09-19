// -----------------------------------------------------------------------
// <copyright file="ConfigManager.cs" company="EUROSTAT">
//   Date Created : 2011-06-19
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
namespace Estat.Nsi.AuthModule
{
    using System.Configuration;

    /// <summary>
    /// Configuration manager for this module
    /// </summary>
    public class ConfigManager
    {
        #region Constants and Fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly ConfigManager _instance = new ConfigManager();

        /// <summary>
        /// The configuration section
        /// </summary>
        private readonly AuthConfigSection _config;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ConfigManager"/> class from being created. 
        /// Initialize a new instance of the <see cref="ConfigManager"/> class
        /// </summary>
        private ConfigManager()
        {
            this._config = (AuthConfigSection)ConfigurationManager.GetSection("estat.nsi.ws.config/auth");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Gets the current configuration section
        /// </summary>
        public AuthConfigSection Config
        {
            get
            {
                return this._config;
            }
        }

        #endregion
    }
}