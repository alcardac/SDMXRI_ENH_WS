// -----------------------------------------------------------------------
// <copyright file="ImplementationElement.cs" company="EUROSTAT">
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
    using System;
    using System.Configuration;
    using System.Globalization;

    /// <summary>
    /// Abstract class for *Implementation elements
    /// </summary>
    public abstract class ImplementationElement : ConfigurationElement
    {
        /// <summary>
        /// The attribute name of the implementation type attribute
        /// </summary>
        private const string ImplementationTypeName = "type";

        #region Public Properties

        /// <summary>
        /// Gets or sets the implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method
        /// </summary>
        [ConfigurationProperty(ImplementationTypeName, IsRequired = true)]
        public string ImplementationType
        {
            get
            {
                return (string)this[ImplementationTypeName];
            }

            set
            {
                this["type"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if the implementation is valid
        /// </summary>
        /// <param name="value">
        /// The type name
        /// </param>
        /// <exception cref="AuthConfigurationException">
        /// <see cref="Errors.ImplementationMissingAttr"/>, <see cref="Errors.ImplementationCannotLoad"/>
        /// </exception>
        public static void TypeValidator(object value)
        {
            var typeName = value as string;
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(Errors.ImplementationMissingAttr);
            }

            Type type = Type.GetType(typeName, false);
            if (type == null)
            {
                throw new AuthConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Errors.ImplementationCannotLoad, value));
            }
        }

        #endregion
    }
}