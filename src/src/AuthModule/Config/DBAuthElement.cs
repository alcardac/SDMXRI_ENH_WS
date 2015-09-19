// -----------------------------------------------------------------------
// <copyright file="DBAuthElement.cs" company="EUROSTAT">
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
    /// The <see cref="DbAuthenticationProvider"/> and <see cref="DbAuthorizationProvider"/> configuration section
    /// </summary>
    public class DBAuthElement : ConfigurationElement
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="DbAuthenticationProvider"/> configuration element
        /// </summary>
        [ConfigurationProperty("authentication", IsRequired = true)]
        public DBAuthenticationElement Authentication
        {
            get
            {
                return (DBAuthenticationElement)this["authentication"];
            }

            set
            {
                this["authentication"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DbAuthorizationProvider"/> configuration element
        /// </summary>
        [ConfigurationProperty("authorization")]
        public DBAuthorizationElement Authorization
        {
            get
            {
                return (DBAuthorizationElement)this["authorization"];
            }

            set
            {
                this["authorization"] = value;
            }
        }

        #endregion
    }
}