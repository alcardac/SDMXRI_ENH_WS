// -----------------------------------------------------------------------
// <copyright file="DatabaseExtension.cs" company="EUROSTAT">
//   Date Created : 2013-07-29
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
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Data;

    /// <summary>
    /// This class contains various database extension methods.
    /// </summary>
    public static class DatabaseExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Convert the specified <paramref name="value"/> to a value suitable for Mapping Store.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ToDbValue(this long value)
        {
            return ToDbValue(value, DBNull.Value);
        }

        /// <summary>
        /// Convert the specified <paramref name="value"/> to a value suitable for Mapping Store.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ToDbValue(this long value, object defaultValue)
        {
            if (value < 0)
            {
                return DBNull.Value;
            }

            return value;
        }

        /// <summary>
        /// Convert the specified <paramref name="value" /> to a value suitable for Mapping Store.
        /// </summary>
        /// <typeparam name="T">The type of</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The normalized for DB object.
        /// </returns>
        public static object ToDbValue<T>(this T? value) where T : struct
        {
            return ToDbValue(value, DBNull.Value);
        }

        /// <summary>
        /// Convert the specified <paramref name="value" /> to a value suitable for Mapping Store.
        /// </summary>
        /// <typeparam name="T">The type of</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The normalized for DB object.
        /// </returns>
        public static object ToDbValue<T>(this T? value, object defaultValue) where T : struct
        {
            if (!value.HasValue)
            {
                return defaultValue;
            }

            return value.Value;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="reader"/> has a field with the specified <paramref name="fieldName"/>
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>True if there is a field with name <paramref name="fieldName"/>; otherwise false</returns>
        public static bool HasFieldName(this IDataReader reader, string fieldName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion 
    }
}