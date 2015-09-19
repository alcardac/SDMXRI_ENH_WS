// -----------------------------------------------------------------------
// <copyright file="TextFormatTypesPool.cs" company="EUROSTAT">
//   Date Created : 2013-04-22
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
namespace Estat.Sri.MappingStore.Store.Helper
{
    using System.Collections.Concurrent;
    using System.Configuration;

    using Estat.Sri.MappingStore.Store.Engine;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    /// <summary>
    ///     A naive <see cref="TextFormatTypesQueryEngine" /> pool
    /// </summary>
    public static class TextFormatTypesPool
    {
        #region Static Fields

        /// <summary>
        ///     The _databases.
        /// </summary>
        private static readonly ConcurrentDictionary<ConnectionStringSettings, TextFormatTypesQueryEngine> _connectionStringMap =
            new ConcurrentDictionary<ConnectionStringSettings, TextFormatTypesQueryEngine>();

        /// <summary>
        ///     The _databases.
        /// </summary>
        private static readonly ConcurrentDictionary<Database, TextFormatTypesQueryEngine> _databaseMap = new ConcurrentDictionary<Database, TextFormatTypesQueryEngine>();

        #endregion

        ///// TODO add queue and pool limit
        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="TextFormatTypesQueryEngine"/> instance for the specified
        ///     <paramref name="connectionStringSettings"/>
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        /// <returns>
        /// The <see cref="TextFormatTypesQueryEngine"/>.
        /// </returns>
        public static TextFormatTypesQueryEngine GetTextFormatQuery(ConnectionStringSettings connectionStringSettings)
        {
            return _connectionStringMap.GetOrAdd(connectionStringSettings, settings => new TextFormatTypesQueryEngine(settings));
        }

        /// <summary>
        /// Returns a <see cref="TextFormatTypesQueryEngine"/> instance for the specified <paramref name="database"/>
        /// </summary>
        /// <param name="database">
        /// The mapping store database
        /// </param>
        /// <returns>
        /// The <see cref="TextFormatTypesQueryEngine"/>.
        /// </returns>
        public static TextFormatTypesQueryEngine GetTextFormatQuery(Database database)
        {
            return _databaseMap.GetOrAdd(database, settings => new TextFormatTypesQueryEngine(settings));
        }

        /// <summary>
        /// Returns a <see cref="TextFormatTypesQueryEngine" /> instance for the specified <paramref name="state" />
        /// </summary>
        /// <param name="state">The mapping store database</param>
        /// <returns>
        /// The <see cref="TextFormatTypesQueryEngine" />.
        /// </returns>
        public static TextFormatTypesQueryEngine GetTextFormatQuery(DbTransactionState state)
        {
            return _databaseMap.GetOrAdd(state.Database, settings => new TextFormatTypesQueryEngine(state));
        }

        #endregion
    }
}