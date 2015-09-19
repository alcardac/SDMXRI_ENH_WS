// -----------------------------------------------------------------------
// <copyright file="MastoreProviderMappingSetting.cs" company="EUROSTAT">
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
    /// This element allow to configure database related settings. Such mapping store vendor name and database provider
    /// </summary>
    public class MastoreProviderMappingSetting : ConfigurationElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MastoreProviderMappingSetting"/> class with the specified <paramref name="name"/>
        /// </summary>
        /// <param name="name">
        /// Sets the <see cref="Name"/>.
        /// </param>
        public MastoreProviderMappingSetting(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MastoreProviderMappingSetting"/> class.
        /// </summary>
        public MastoreProviderMappingSetting()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Mapping Store DDB vendor name
        /// </summary>
        [ConfigurationProperty(SettingConstants.NameAttributeName, IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this[SettingConstants.NameAttributeName];
            }

            set
            {
                this[SettingConstants.NameAttributeName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Mapping Store DDB provider (driver name)
        /// </summary>
        [ConfigurationProperty(SettingConstants.ProviderAttributeName, IsRequired = true)]
        public string Provider
        {
            get
            {
                return (string)this[SettingConstants.ProviderAttributeName];
            }

            set
            {
                this[SettingConstants.ProviderAttributeName] = value;
            }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}