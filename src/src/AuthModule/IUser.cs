// -----------------------------------------------------------------------
// <copyright file="IUser.cs" company="EUROSTAT">
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
    /// Interface for storing user credential and checking passwords
    /// </summary>
    public interface IUser
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Domain/Realm
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the User name
        /// </summary>
        string UserName { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method checks if the given password matches with the possibly encypted <see cref="Password"/>
        /// </summary>
        /// <param name="password">
        /// The password from the authentication provider
        /// </param>
        /// <returns>
        /// True if specifed password == g(<see cref="Password"/>) where g is the encryption method depending on the implementation
        /// </returns>
        bool CheckPasswordEnc(string password);

        #endregion
    }
}