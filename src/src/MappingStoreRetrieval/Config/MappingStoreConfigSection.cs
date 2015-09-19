// -----------------------------------------------------------------------
// <copyright file="MappingStoreConfigSection.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
    /// Mapping store related configuration
    /// </summary>
    public class MappingStoreConfigSection : ConfigurationSection
    {
        #region Public Properties

        /// <summary>
        /// Gets the dataflow Settings
        /// </summary>
        [ConfigurationProperty(SettingConstants.DataflowConfiguration)]
        public DataflowConfigurationSection DataflowConfiguration
        {
            get
            {
                return (DataflowConfigurationSection)this [SettingConstants.DataflowConfiguration];
            }
        }

        /// <summary>
        /// Gets Dissemination Database (DDB) Settings
        /// </summary>
        [ConfigurationProperty(SettingConstants.DisseminationDatabaseSettings)]
        public MastoreProviderMappingSettingCollection DisseminationDatabaseSettings
        {
            get
            {
                return (MastoreProviderMappingSettingCollection)this[SettingConstants.DisseminationDatabaseSettings];
            }
        }

        /// <summary>
        /// Gets Dissemination Database (DDB) Settings
        /// </summary>
        [ConfigurationProperty(SettingConstants.GeneralDatabaseSettings)]
        public DatabaseSettingCollection GeneralDatabaseSettings
        {
            get
            {
                return (DatabaseSettingCollection)this[SettingConstants.GeneralDatabaseSettings];
            }
        }

        #endregion
    }
}