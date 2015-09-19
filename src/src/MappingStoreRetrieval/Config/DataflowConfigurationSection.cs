// -----------------------------------------------------------------------
// <copyright file="DataflowConfigurationSection.cs" company="EUROSTAT">
//   Date Created : 2013-05-01
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

    using Estat.Sri.MappingStoreRetrieval.Engine;

    /// <summary>
    ///     The dataflow configuration section.
    /// </summary>
    public class DataflowConfigurationSection : ConfigurationElement
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether ignore production for data (<see cref="MappingSetRetriever" />).
        /// </summary>
        [ConfigurationProperty(SettingConstants.IgnoreProductionForData, IsRequired = false, DefaultValue = false)]
        public bool IgnoreProductionForData
        {
            get
            {
                return (bool)this[SettingConstants.IgnoreProductionForData];
            }

            set
            {
                this[SettingConstants.IgnoreProductionForData] = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether ignore production for dataflow retrieval. Except for
        ///     <see
        ///         cref="MappingSetRetriever" />
        ///     . For that use <see cref="IgnoreProductionForData" />
        /// </summary>
        [ConfigurationProperty(SettingConstants.IgnoreProduction, IsRequired = false, DefaultValue = false)]
        public bool IgnoreProductionForStructure
        {
            get
            {
                return (bool)this[SettingConstants.IgnoreProduction];
            }

            set
            {
                this[SettingConstants.IgnoreProduction] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        #endregion
    }
}