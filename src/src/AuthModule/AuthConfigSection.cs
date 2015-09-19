// -----------------------------------------------------------------------
// <copyright file="AuthConfigSection.cs" company="EUROSTAT">
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

    using Estat.Nsi.AuthModule.Config;

    /// <summary>
    /// The main configuration section of the AuthModule
    /// </summary>
    public class AuthConfigSection : ConfigurationSection
    {
        #region Constants and Fields

        /// <summary>
        /// The anon user name.
        /// </summary>
        private const string AnonUserName = "anonymousUser";

        /// <summary>
        /// The authentication impl name.
        /// </summary>
        private const string AuthenticationImplName = "authenticationImplementation";

        /// <summary>
        /// The authorization impl name.
        /// </summary>
        private const string AuthorizationImplName = "authorizationImplementation";

        /// <summary>
        /// The db auth name.
        /// </summary>
        private const string DbAuthName = "dbAuth";

        // attribute names

        /// <summary>
        /// The realm name.
        /// </summary>
        private const string RealmName = "realm";

        /// <summary>
        /// The user cred impl name.
        /// </summary>
        private const string UserCredImplName = "userCredentialsImplementation";

        /// <summary>
        /// The user impl name.
        /// </summary>
        private const string UserImplName = "userImplementation";

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Anonymous user. if its not set or is empty or null, it's disabled
        /// </summary>
        [ConfigurationProperty(AnonUserName)]
        public string AnonymousUser
        {
            get
            {
                return (string)this[AnonUserName];
            }

            set
            {
                this[AnonUserName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IAuthenticationProvider"/> implementation element
        /// </summary>
        [ConfigurationProperty(AuthenticationImplName, IsRequired = true)]
        public AuthenticationImplementationElement AuthenticationImplementation
        {
            get
            {
                return (AuthenticationImplementationElement)this[AuthenticationImplName];
            }

            set
            {
                this[AuthenticationImplName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IAuthorizationProvider"/> implementation element
        /// </summary>
        [ConfigurationProperty(AuthorizationImplName, IsRequired = false)]
        public AuthorizationImplementationElement AuthorizationImplementation
        {
            get
            {
                return (AuthorizationImplementationElement)this[AuthorizationImplName];
            }

            set
            {
                this[AuthorizationImplName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DbAuthenticationProvider"/> and <see cref="DbAuthorizationProvider"/> configuration section
        /// </summary>
        [ConfigurationProperty(DbAuthName, IsRequired = false)]
        public DBAuthElement DBAuth
        {
            get
            {
                return (DBAuthElement)this[DbAuthName];
            }

            set
            {
                this[DbAuthName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Realm/Domain
        /// </summary>
        [ConfigurationProperty(RealmName, DefaultValue = "nsi")]
        public string Realm
        {
            get
            {
                return (string)this[RealmName];
            }

            set
            {
                this[RealmName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IUserCredentials"/> implementation element
        /// </summary>
        [ConfigurationProperty(UserCredImplName, IsRequired = true)]
        public UserCredentialsImplementationElement UserCredentialsImplementation
        {
            get
            {
                return (UserCredentialsImplementationElement)this[UserCredImplName];
            }

            set
            {
                this[UserCredImplName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IUser"/> implementation element
        /// </summary>
        [ConfigurationProperty(UserImplName, IsRequired = true)]
        public UserImplementationElement UserImplementation
        {
            get
            {
                return (UserImplementationElement)this[UserImplName];
            }

            set
            {
                this[UserImplName] = value;
            }
        }

        #endregion
    }
}