// -----------------------------------------------------------------------
// <copyright file="ConnectionStringHelper.cs" company="EUROSTAT">
//   Date Created : 2013-04-10
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
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System.Configuration;

    /// <summary>
    /// Helper singleton class to get and save configuration settings
    /// </summary>
    public sealed class ConnectionStringHelper
    {
        /// <summary>
        /// The connection string name
        /// </summary>
        public const string ConnectionStringName = "MappingStoreServer";

        /// <summary>
        /// The protection provider
        /// </summary>
        private const string ProtectionProvider = "ConfiguredProtectedConfigurationProvider";
        
        /// <summary>
        /// This should be "configProtectedData"
        /// </summary>
        private const string ProtectionProviderSectionName = "configProtectedData";

        /// <summary>
        /// Singleton instance 
        /// </summary>
        private static readonly ConnectionStringHelper _instance = new ConnectionStringHelper();

        /// <summary>
        /// Prevents a default instance of the <see cref="ConnectionStringHelper"/> class from being created.
        /// </summary>
        private ConnectionStringHelper()
        {
        }

        /// <summary>
        /// Gets the singleton instance 
        /// </summary>
        public static ConnectionStringHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Gets the Mapping Store connection string settings
        /// </summary>
        public ConnectionStringSettings MappingStoreConnectionStringSettings
        {
            get
            {
                // TODO use openmappedexeconfiguration and save to program data
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                ConnectionStringsSection configSection = config.ConnectionStrings;
                if (configSection.SectionInformation.IsProtected)
                {
                    configSection.SectionInformation.UnprotectSection();
                }

                ConnectionStringSettings connectionStringSetting = configSection.ConnectionStrings[ConnectionStringName];
                return connectionStringSetting;
            }
        }

        /// <summary>
        /// Save connection string settings together with Protect Provider.
        /// </summary>
        /// <param name="connectionString">
        /// The connection String.
        /// </param>
        /// <param name="providerName">
        /// The provider Name.
        /// </param>
        public void Save(string connectionString, string providerName)
        {
            var connectionStringSettings = new ConnectionStringSettings(ConnectionStringName, connectionString, providerName);

            // save config
            // TODO use openmappedexeconfiguration and save to program data
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConnectionStringsSection configSection = config.ConnectionStrings;

            if (configSection.SectionInformation.IsProtected)
            {
                configSection.SectionInformation.UnprotectSection();
            }

            // just changing the connection string doesn't seem to work...
            configSection.ConnectionStrings.Remove(ConnectionStringName);
            
            config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);
            CreateProtectProvider(config);
            
            configSection.SectionInformation.ProtectSection(ProtectionProvider);
            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("configuration");
        }

        /// <summary>
        /// Gets or creates protected provider section
        /// </summary>
        /// <param name="config">The configuration</param>
        private static void CreateProtectProvider(Configuration config)
        {
            var protectedConfigurationSection = config.GetSection(ProtectionProviderSectionName) as ProtectedConfigurationSection;

            bool found = false;
            if (protectedConfigurationSection != null)
            {
                foreach (ProviderSettings provider in protectedConfigurationSection.Providers)
                {
                    if (provider.Name.Equals(ProtectionProvider))
                    {
                        found = true;
                        break;
                    }
                }
            }
            else
            {
                protectedConfigurationSection = new ProtectedConfigurationSection();
                config.Sections.Add(ProtectionProviderSectionName, protectedConfigurationSection);
            }

            if (!found)
            {
                protectedConfigurationSection.Providers.Add(
                    new ProviderSettings(ProtectionProvider, typeof(DpapiProtectedConfigurationProvider).AssemblyQualifiedName));
            }
        }

        /// <summary>
        /// Determines whether [has connection string].
        /// </summary>
        /// <returns>True if there are connection string set</returns>
        public static bool HasConnectionString()
        {
            return Instance.MappingStoreConnectionStringSettings != null;
        }
    }
}