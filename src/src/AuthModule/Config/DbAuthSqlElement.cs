// -----------------------------------------------------------------------
// <copyright file="DbAuthSqlElement.cs" company="EUROSTAT">
//   Date Created : 2011-09-09
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
namespace Estat.Nsi.AuthModule.Config
{
    using System.Configuration;

    /// <summary>
    /// Abstract class for DbAuth* elements which contains the common configuration properties
    /// </summary>
    public abstract class DBAuthSqlElement : ConfigurationElement
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ConnectionString Name. Must exist a connectionString with this name at connectionStrings section
        /// </summary>
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get
            {
                return (string)this["connectionStringName"];
            }

            set
            {
                this["connectionStringName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the SQL Query with macros
        /// </summary>
        [ConfigurationProperty("sql", IsRequired = true)]
        public string Sql
        {
            get
            {
                return (string)this["sql"];
            }

            set
            {
                this["sql"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if the specified value is empty
        /// </summary>
        /// <param name="value">
        /// The sql query string
        /// </param>
        /// <exception cref="AuthConfigurationException">
        /// See the <see cref="Errors.DbAuthValidateMissingSql"/>
        /// </exception>
        public static void SqlValidate(object value)
        {
            var sql = value as string;
            if (string.IsNullOrEmpty(sql))
            {
                throw new AuthConfigurationException(Errors.DbAuthValidateMissingSql);
            }
        }

        #endregion
    }
}