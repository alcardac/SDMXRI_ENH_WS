// -----------------------------------------------------------------------
// <copyright file="DatabaseType.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Config;

    /// <summary>
    /// Provides storage all the DataBase types and their corresponding Provider Names 
    /// that are recognized by the DataRetriever.
    /// Currently Microsoft SQL Server, Oracle, MySQL, , PC-Axis
    /// </summary>
    public class DatabaseType
    {
        #region Constants and Fields

        /// <summary>
        /// Holds the singleton instance 
        /// </summary>
        private static readonly DatabaseType _instance = new DatabaseType();

        /// <summary>
        /// The Mapping Store configuration section
        /// </summary>
        private readonly MappingStoreConfigSection _config;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DatabaseType"/> class from being created. 
        /// Initializes a new instance of the <see cref="DatabaseType"/> class
        /// </summary>
        private DatabaseType()
        {
            this._config = ConfigManager.Config;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static DatabaseType Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Gets the Database type provider mappings
        /// </summary>
        public static MastoreProviderMappingSettingCollection Mappings
        {
            get
            {
                return _instance._config.DisseminationDatabaseSettings;
            }
        }

        /// <summary>
        /// Gets the general database settings
        /// </summary>
        public static DatabaseSettingCollection DatabaseSettings
        {
            get
            {
                return _instance._config.GeneralDatabaseSettings;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the corresponding providers name of the required database type
        /// In case the list of dictionary is not populated it populates it before.
        /// In case the Database type is not present in the dictionary, currently 
        /// is different then Microsoft SQL Server, Oracle, MySQL, , PCAxis an exception is thrown
        /// </summary>
        /// <param name="databaseType">
        /// The name of the Database. The accepted values are
        /// <list type="bullet">
        /// <item>
        /// SqlServer
        /// </item>
        /// <item>
        /// Oracle
        /// </item>
        /// <item>
        /// MySQL
        /// </item>
        /// <item>
        /// PCAxis
        /// </item>
        /// </list>
        /// </param>
        /// <returns>
        /// The name of the Database Provider. The default values are
        /// <list type="bullet">
        /// <item>
        /// <c>System.Data.SqlClient</c>
        /// </item>
        /// <item>
        /// <c>System.Data.OracleClient</c>
        /// </item>
        /// <item>
        /// <c>MySql.Data.MySqlClient</c> 
        /// </item>
        /// <item>
        /// <c>org.estat.PcAxis.PcAxisProvider</c>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="databaseType"/> is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Unknown Database type specified at <paramref name="databaseType"/>
        /// </exception>
        public static string GetProviderName(string databaseType)
        {
            if (databaseType == null)
            {
                throw new ArgumentNullException("databaseType");
            }

            var setting = _instance._config.DisseminationDatabaseSettings[databaseType];
            if (setting == null)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, ErrorMessages.UnknownDatabaseTypeFormat1, databaseType));
            }

            return setting.Provider;
        }

        #endregion
    }
}