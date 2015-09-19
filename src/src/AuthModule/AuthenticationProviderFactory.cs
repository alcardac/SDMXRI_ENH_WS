// -----------------------------------------------------------------------
// <copyright file="AuthenticationProviderFactory.cs" company="EUROSTAT">
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
    /// <summary>
    /// Singleton class for creating <see cref="IAuthenticationProvider"/> objects using the implementation specified in <see cref="AuthConfigSection.AuthenticationImplementation"/>
    /// </summary>
    public class AuthenticationProviderFactory : AbstractFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly AuthenticationProviderFactory _instance = new AuthenticationProviderFactory();

        /// <summary>
        /// Holds the configured implementation to use
        /// </summary>
        private readonly string _configuredType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AuthenticationProviderFactory"/> class from being created. 
        /// Initialize a new instance of the <see cref="AuthenticationProviderFactory"/> class
        /// </summary>
        private AuthenticationProviderFactory()
        {
            this._configuredType = ConfigManager.Instance.Config.AuthenticationImplementation.ImplementationType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static AuthenticationProviderFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new <see cref="IAuthenticationProvider"/> implementation using the implementation specified in <see cref="AuthConfigSection.AuthenticationImplementation"/>
        /// </summary>
        /// <returns>
        /// A new <see cref="IAuthenticationProvider"/> implementation object
        /// </returns>
        public IAuthenticationProvider CreateAuthenticationProvider()
        {
            return this.CreateAuthenticationProvider(null);
        }

        /// <summary>
        /// Create a new <see cref="IAuthenticationProvider"/> implementation using the <paramref name="type"/> or the <see cref="AuthConfigSection.AuthenticationImplementation"/>
        /// </summary>
        /// <param name="type">
        /// The implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method. If it is null then it is ignored and this method behaves like <see cref="CreateAuthenticationProvider()"/>
        /// </param>
        /// <returns>
        /// A new <see cref="IAuthenticationProvider"/> implementation object
        /// </returns>
        public IAuthenticationProvider CreateAuthenticationProvider(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return Create<IAuthenticationProvider>(this._configuredType);
            }

            return Create<IAuthenticationProvider>(type);
        }

        #endregion
    }
}