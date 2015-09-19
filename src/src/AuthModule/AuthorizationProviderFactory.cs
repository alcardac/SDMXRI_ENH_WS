// -----------------------------------------------------------------------
// <copyright file="AuthorizationProviderFactory.cs" company="EUROSTAT">
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
    /// Class for creating <see cref="IAuthorizationProvider"/> objects using the implementation specified in <see cref="AuthConfigSection.AuthorizationImplementation"/>
    /// </summary>
    public class AuthorizationProviderFactory : AbstractFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The _configured type.
        /// </summary>
        private readonly string _configuredType;

        /// <summary>
        /// The _default authorization provider.
        /// </summary>
        private readonly IAuthorizationProvider _defaultAuthorizationProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationProviderFactory"/> class. 
        /// Create a new instance of the <see cref="AuthorizationProviderFactory"/> class 
        /// </summary>
        public AuthorizationProviderFactory()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationProviderFactory"/> class. 
        /// Create a new instance of the <see cref="AuthorizationProviderFactory"/> class with the specified default <see cref="IAuthorizationProvider"/>
        /// </summary>
        /// <param name="defaultAuthorizationProvider">
        /// The <see cref="IAuthorizationProvider"/> based object
        /// </param>
        public AuthorizationProviderFactory(IAuthorizationProvider defaultAuthorizationProvider)
        {
            if (ConfigManager.Instance.Config.AuthorizationImplementation != null)
            {
                this._configuredType = ConfigManager.Instance.Config.AuthorizationImplementation.ImplementationType;
            }

            this._defaultAuthorizationProvider = defaultAuthorizationProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new <see cref="IAuthorizationProvider"/> implementation using the implementation specified in <see cref="AuthConfigSection.AuthorizationImplementation"/>
        /// or the default <see cref="IAuthorizationProvider"/> specified in the constructor
        /// </summary>
        /// <returns>
        /// A new <see cref="IAuthorizationProvider"/> implementation object
        /// </returns>
        public IAuthorizationProvider CreateAuthorizationProvider()
        {
            return this.CreateAuthorizationProvider(null);
        }

        /// <summary>
        /// Create a new <see cref="IAuthorizationProvider"/> implementation using the <paramref name="type"/> or the <see cref="AuthConfigSection.AuthorizationImplementation"/>
        /// </summary>
        /// <param name="type">
        /// The implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method. If it is null then it is ignored and this method behaves like <see cref="CreateAuthorizationProvider()"/>
        /// </param>
        /// <returns>
        /// A new <see cref="IAuthorizationProvider"/> implementation object
        /// </returns>
        public IAuthorizationProvider CreateAuthorizationProvider(string type)
        {
            IAuthorizationProvider provider;
            switch (type)
            {
                case null:
                    if (!string.IsNullOrEmpty(this._configuredType))
                    {
                        provider = Create<IAuthorizationProvider>(this._configuredType);
                    }
                    else
                    {
                        return this._defaultAuthorizationProvider;
                    }

                    break;
                default:
                    provider = Create<IAuthorizationProvider>(type);
                    break;
            }

            return provider;
        }

        #endregion
    }
}