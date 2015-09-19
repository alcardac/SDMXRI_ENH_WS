// -----------------------------------------------------------------------
// <copyright file="ConfigManager.cs" company="EUROSTAT">
//   Date Created : 2011-09-08
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
namespace Estat.Sri.MappingStoreRetrieval.Config
{
    using System.Configuration;

    /// <summary>
    /// Mapping Store related configuration manager
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// The .config section that holds the configuration
        /// </summary>
        private const string EstatNsiMappingStore = "estat.sri/mapping.store";

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly ConfigManager _instance = new ConfigManager();

        /// <summary>
        /// The Mapping Store configuration section
        /// </summary>
        private readonly MappingStoreConfigSection _config;

        /// <summary>
        /// Prevents a default instance of the <see cref="ConfigManager"/> class from being created. 
        /// </summary>
        private ConfigManager()
        {
            this._config = (MappingStoreConfigSection)ConfigurationManager.GetSection(EstatNsiMappingStore)
                           ?? new MappingStoreConfigSection();
        }

        /// <summary>
        /// Gets the Mapping Store configuration section
        /// </summary>
        public static MappingStoreConfigSection Config
        {
            get
            {
                return _instance._config;
            }
        }

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
    }
}