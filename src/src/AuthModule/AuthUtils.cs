// -----------------------------------------------------------------------
// <copyright file="AuthUtils.cs" company="EUROSTAT">
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

    /// <summary>
    /// This class holds a collection of helper static methods
    /// </summary>
    public static class AuthUtils
    {
        #region Public Methods

        /// <summary>
        /// Convert the specified object base type to the generic type.
        /// </summary>
        /// <typeparam name="T">
        /// The output base type
        /// </typeparam>
        /// <param name="value">
        /// The value to convert
        /// </param>
        /// <returns>
        /// If <c>param</c> is <see cref="DBNull"/> then return the <c>default(T)</c> else the dbValue converted to (T) type
        /// </returns>
        public static T ConvertDBValue<T>(object value)
        {
            if (Convert.IsDBNull(value))
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if a configuration setting is set and throw an <see cref="AuthConfigurationException"/> if not
        /// </summary>
        /// <param name="config">
        /// The config artefact to check
        /// </param>
        /// <param name="type">
        /// The type of the calling module
        /// </param>
        /// <exception cref="AuthConfigurationException">
        /// See the <see cref="Errors.MissingConfiguration"/>
        /// </exception>
        public static void ValidateConfig(object config, Type type)
        {
            if (config == null)
            {
                throw new AuthConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Errors.MissingConfiguration, type));
            }
        }

        /// <summary>
        /// Validates that all the specified strings exist in the input string in any order
        /// </summary>
        /// <param name="input">
        /// The input string to check
        /// </param>
        /// <param name="values">
        /// A non-null and non empty collection of strings to check
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// strings is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// strings is empty
        /// </exception>
        /// <returns>
        /// True if all the specified strings are included in the input string. Else false.
        /// </returns>
        public static bool ValidateContains(string input, params string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length == 0)
            {
                throw new ArgumentException("strings is empty");
            }

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (string s in values)
            {
                if (!input.Contains(s))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}