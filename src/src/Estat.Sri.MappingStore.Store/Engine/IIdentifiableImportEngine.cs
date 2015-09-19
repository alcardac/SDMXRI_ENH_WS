// -----------------------------------------------------------------------
// <copyright file="IIdentifiableImportEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-09
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using System.Collections.Generic;

    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The NameableImportEngine interface.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="INameableObject"/> based type
    /// </typeparam>
    public interface IIdentifiableImportEngine<in T> where T : IIdentifiableObject 
    {
        /// <summary>
        /// Insert the specified <paramref name="items"/> to the mapping store with <paramref name="state"/>
        /// </summary>
        /// <param name="state">
        ///     The MAPPING STORE connection and transaction state
        /// </param>
        /// <param name="items">
        ///     The items.
        /// </param>
        /// <param name="parentArtefact"> The primary key of the parent artefact.</param>
        /// <returns>
        /// The <see cref="IEnumerable{ArtefactImportStatus}"/>.
        /// </returns>
        ItemStatusCollection Insert(DbTransactionState state, IEnumerable<T> items, long parentArtefact); 
    }
}