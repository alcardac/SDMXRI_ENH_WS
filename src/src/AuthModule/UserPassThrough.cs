// -----------------------------------------------------------------------
// <copyright file="UserPassThrough.cs" company="EUROSTAT">
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
    /// An implementation of this <see cref="IUser"/> interface. This implementation doesn't encypt or encode the password in any way
    /// </summary>
    public class UserPassThrough : IUser
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Domain/Realm
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the User name
        /// </summary>
        public string UserName { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method checks if the given password matches with the  <see cref="IUser.Password"/>
        /// </summary>
        /// <param name="password">
        /// The password from the authentication provider
        /// </param>
        /// <returns>
        /// True if specifed password == <see cref="IUser.Password"/>. Else false
        /// </returns>
        public virtual bool CheckPasswordEnc(string password)
        {
            return string.Equals(this.Password, password);
        }

        #endregion
    }
}