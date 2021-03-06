// -----------------------------------------------------------------------
// <copyright file="DatabaseSetting.cs" company="EUROSTAT">
//   Date Created : 2011-09-08
//   Copyright (c) 2009, 2015 by the European Commission, represented by Eurostat.   All rights reserved.
// 
// Licensed under the EUPL, Version 1.1 or � as soon they
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
    /// This element allow to configure database related settings per database provider. Such as parameter format
    /// </summary>
    public class DatabaseSetting : ConfigurationElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSetting"/> class with the specified <paramref name="providerName"/>
        /// </summary>
        /// <param name="providerName">
        /// Sets the <see cref="Provider"/>.
        /// </param>
        public DatabaseSetting(string providerName)
        {
            this.Provider = providerName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSetting"/> class.
        /// </summary>
        public DatabaseSetting()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the cast to add at WHERE clauses when comparing against DATE fields
        /// </summary>
        [ConfigurationProperty(SettingConstants.CastToString, DefaultValue = "{0}")]
        public string CastToString
        {
            get
            {
                return (string)this[SettingConstants.CastToString];
            }

            set
            {
                this[SettingConstants.CastToString] = value;
            }
        }

        /// <summary>
        /// Gets or sets the cast to add at WHERE clauses when comparing against DATE fields
        /// </summary>
        [ConfigurationProperty(SettingConstants.DateCast, DefaultValue = "")]
        public string DateCast
        {
            get
            {
                return (string)this[SettingConstants.DateCast];
            }

            set
            {
                this[SettingConstants.DateCast] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Mapping Store DDB provider (driver name)
        /// </summary>
        [ConfigurationProperty(SettingConstants.ProviderAttributeName, IsKey = true, IsRequired = true)]
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

        /// <summary>
        /// Gets or sets the substring command
        /// </summary>
        [ConfigurationProperty(
            SettingConstants.SubStringAttributeName, DefaultValue = SettingConstants.StandardSubString)]
        public string SubstringCommand
        {
            get
            {
                return (string)this[SettingConstants.SubStringAttributeName];
            }

            set
            {
                this[SettingConstants.SubStringAttributeName] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the substring command requires the length parameter
        /// </summary>
        [ConfigurationProperty(SettingConstants.SubStringNeedsLength, DefaultValue = false)]
        public bool SubstringCommandRequiresLength
        {
            get
            {
                return (bool)this[SettingConstants.SubStringNeedsLength];
            }

            set
            {
                this[SettingConstants.SubStringNeedsLength] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an SQL ORDER BY is allowed in a sub query.
        /// </summary>
        /// <remarks>Using ORDER BY is not usually a good idea. This flag just indicates if it is allowed.</remarks>
        [ConfigurationProperty(SettingConstants.SubQueryOrderBy, DefaultValue = true)]
        public bool SubQueryOrderByAllowed
        {
            get
            {
                return (bool)this[SettingConstants.SubQueryOrderBy];
            }

            set
            {
                this[SettingConstants.SubQueryOrderBy] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the substring command requires the length parameter
        /// </summary>
        [ConfigurationProperty(SettingConstants.ParameterMarkerFormat, DefaultValue = "")]
        public string ParameterMarkerFormat
        {
            get
            {
                return (string)this[SettingConstants.ParameterMarkerFormat];
            }

            set
            {
                this[SettingConstants.ParameterMarkerFormat] = value;
            }
        }

        #endregion
    }
}