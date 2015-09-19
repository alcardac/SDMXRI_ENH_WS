﻿// -----------------------------------------------------------------------
// <copyright file="DbAuthenticationProvider.cs" company="EUROSTAT">
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
    using System.Data;
    using System.Data.Common;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Manager;

    /// <summary>
    /// An implementation of the <see cref="IAuthenticationProvider"/> interface.
    /// This implementation uses a database and a configurable sql query to authenticate the user.
    /// The database connection string and sql query are configured in .config files.
    /// </summary>
    public class DbAuthenticationProvider : IAuthenticationProvider
    {
        #region Constants and Fields

        /// <summary>
        /// The authentication database
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// The _query.
        /// </summary>
        private readonly string _query;

        /// <summary>
        /// The sql query as specified in the config file
        /// </summary>
        private readonly string _selectQuery = ConfigManager.Instance.Config.DBAuth.Authentication.Sql;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbAuthenticationProvider"/> class
        /// </summary>
        /// <exception cref="AuthConfigurationException">Missing or invalid configuration at .config</exception>
        public DbAuthenticationProvider()
        {
            AuthUtils.ValidateConfig(ConfigManager.Instance.Config.DBAuth, this.GetType());
            AuthUtils.ValidateConfig(ConfigManager.Instance.Config.DBAuth.Authentication, this.GetType());
            string connectionStringName = ConfigManager.Instance.Config.DBAuth.Authentication.ConnectionStringName;
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new AuthConfigurationException(Errors.DbAuthMissingConnectionStringName);
            }

            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
            {
                throw new AuthConfigurationException(Errors.DbAuthMissingConnectionStringName);
            }

            if (!AuthUtils.ValidateContains(this._selectQuery, DbConstants.UserMacro))
            {
                string invalidQuery = string.Format(
                    CultureInfo.CurrentCulture, Errors.DbAuthenticationInvalidSqlQuery, DbConstants.UserMacro);
                throw new AuthConfigurationException(
                    string.Format(
                        CultureInfo.CurrentCulture, 
                        Errors.DbAuthInvalidSqlQuery, 
                        this.GetType().Name, 
                        invalidQuery));
            }

            this._database = new Database(connectionStringSettings);
            string userParam = this._database.BuildParameterName(DbConstants.UserParamName);

            this._query = this._selectQuery.Replace(DbConstants.UserMacro, userParam);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Authenticate the specified user
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> instance containing the user information
        /// </param>
        /// <returns>
        /// If the user is authenticated <see cref="AuthorizationProviderFactory"/> else null
        /// </returns>
        public AuthorizationProviderFactory Authenticate(IUser user)
        {
            if (this.Exists(user))
            {
                return
                    new AuthorizationProviderFactory(
                        HasDbAuthorizationConfig() ? new DbAuthorizationProvider(user) : null);
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if <see cref="DbAuthorizationProvider"/> configuration exists
        /// </summary>
        /// <returns>
        /// True if <see cref="DbAuthorizationProvider"/> configuration exists. Else false.
        /// </returns>
        private static bool HasDbAuthorizationConfig()
        {
            return ConfigManager.Instance.Config.DBAuth.Authorization != null;
        }

        /// <summary>
        /// Check if the specified user with user name and password exists.
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> object that holds the user credentials
        /// </param>
        /// <returns>
        /// True if the specified user with user name and password exists. Else false
        /// </returns>
        private bool Exists(IUser user)
        {
            object o = this._database.ExecuteScalar(this._query, new[] { this._database.CreateInParameter(DbConstants.UserParamName, DbType.String, user.UserName) });
            if (o != null)
            {
                return user.CheckPasswordEnc(AuthUtils.ConvertDBValue<string>(o));
            }

            return false;
        }

        #endregion
    }
}