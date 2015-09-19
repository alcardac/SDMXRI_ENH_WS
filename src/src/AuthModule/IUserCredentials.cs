// -----------------------------------------------------------------------
// <copyright file="IUserCredentials.cs" company="EUROSTAT">
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
    using System.Web;

    /// <summary>
    /// Interface for implementations that retrieve user name and password
    /// </summary>
    public interface IUserCredentials
    {
        #region Public Methods

        /// <summary>
        /// Parse the HTTP response from <paramref name="application"/> and populate the <paramref name="user"/>
        /// </summary>
        /// <param name="application">
        /// The current <see cref="HttpApplication"/> instance
        /// </param>
        /// <param name="user">
        /// The <see cref="IUser"/> object to populate
        /// </param>
        /// <returns>
        /// True if retrieving the user credentials were successfull
        /// </returns>
        bool ParseResponse(HttpApplication application, IUser user);

        /// <summary>
        /// Request authentication from client. This might be a no-op for some implementations
        /// </summary>
        /// <param name="application">
        /// The current <see cref="HttpApplication"/> instance
        /// </param>
        /// <param name="domain">
        /// The domain/realm to use when requesting authentication
        /// </param>
        void RequestAuthentication(HttpApplication application, string domain);

        #endregion
    }
}