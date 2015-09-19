// -----------------------------------------------------------------------
// <copyright file="DapperDatabaseExtension.cs" company="EUROSTAT">
//   Date Created : 2014-12-01
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
namespace Estat.Sri.MappingStore.Store.Extension
{
    using System;
    using System.Collections.Generic;

    using Dapper;

    using Estat.Sri.MappingStoreRetrieval.Manager;

    /// <summary>
    /// This class contains extensions for using <see cref="Dapper"/> with <see cref="Database"/>
    /// </summary>
    public static class DapperDatabaseExtension
    {
        /// <summary>
        /// Queries the specified database.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns>
        /// The <see cref="IEnumerable{dynamic}"/>
        /// </returns>
        public static IEnumerable<dynamic> Query(this Database database, string sqlQuery)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                return connection.Query(sqlQuery);
            }
        }

        /// <summary>
        /// Queries the specified database.
        /// </summary>
        /// <typeparam name="T">The type of the result</typeparam>
        /// <param name="database">The database.</param>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>
        /// </returns>
        public static IEnumerable<T> Query<T>(this Database database, string sqlQuery)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                return connection.Query<T>(sqlQuery);
            }
        }

        /// <summary>
        /// Queries the specified database.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="database">The database.</param>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="map">The map function.</param>
        /// <param name="splitOn">The split on.</param>
        /// <returns>
        /// The <see cref="IEnumerable{T1}"/>
        /// </returns>
        public static IEnumerable<T1> Query<T1, T2>(this Database database, string sqlQuery, Func<T1, T2, T1> map, string splitOn)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                return connection.Query(sqlQuery, map, splitOn);
            }
        }
    }
}