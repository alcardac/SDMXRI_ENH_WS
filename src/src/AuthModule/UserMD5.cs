// -----------------------------------------------------------------------
// <copyright file="UserMD5.cs" company="EUROSTAT">
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
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The user md 5.
    /// </summary>
    public class UserMd5 : IUser
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
        /// This method checks if the given password matches with the MD5 hash of the <see cref="IUser.Password"/>
        /// </summary>
        /// <param name="password">
        /// The password from the authentication provider
        /// </param>
        /// <returns>
        /// True if specifed password == g(<see cref="IUser.Password"/>) where g is the MD5 hash method
        /// </returns>
        public bool CheckPasswordEnc(string password)
        {
            return string.Equals(GetPasswordHash(this.Password), password, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get MD5 hash of the specified <paramref name="txtNewPassword"/>
        /// </summary>
        /// <param name="txtNewPassword">
        /// The text to get the MD5 hash
        /// </param>
        /// <returns>
        /// The MD5 hash of the specified <paramref name="txtNewPassword"/>
        /// </returns>
        private static string GetPasswordHash(string txtNewPassword)
        {
            // Create a byte array from source data.
            byte[] tmpSource = Encoding.UTF8.GetBytes(txtNewPassword);

            // Compute hash based on source data.
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            var output = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                output.Append(tmpHash[i].ToString("X2", CultureInfo.InvariantCulture));
            }

            return output.ToString();
        }

        #endregion
    }
}