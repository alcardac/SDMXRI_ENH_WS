// -----------------------------------------------------------------------
// <copyright file="IEngineFactories.cs" company="EUROSTAT">
//   Date Created : 2013-04-29
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
namespace Estat.Sri.MappingStore.Store.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The ImportEngineFactories interface.
    /// </summary>
    public interface IEngineFactories
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the identifiable factory.
        /// </summary>
        /// <typeparam name="TId">
        /// The <see cref="IIdentifiableObject"/> based type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IIdentifiableImportFactory{Tid}"/>.
        /// </returns>
        IIdentifiableImportFactory<TId> GetIdentifiableFactory<TId>() where TId : IIdentifiableObject;

        /// <summary>
        /// Returns the maintainable factory.
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based type
        /// </typeparam>
        /// <returns>
        /// The <see cref="IImportEngineFactory{T}"/>.
        /// </returns>
        IImportEngineFactory<T> GetMaintainableFactory<T>() where T : IMaintainableObject;


        /// <summary>
        /// Returns the maintainable insert method.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based type
        /// </typeparam>
        /// <returns>
        /// The method that can insert <typeparamref name="T"/> objects.
        /// </returns>
        Func<IEnumerable<T>, IEnumerable<ArtefactImportStatus>> GetMaintainableImportMethod<T>(ConnectionStringSettings connectionStringSettings) where T : IMaintainableObject;

        /// <summary>
        /// Returns the maintainable delete method.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based type
        /// </typeparam>
        /// <returns>
        /// The method that can delete <typeparamref name="T"/> objects.
        /// </returns>
        Action<IEnumerable<T>> GetMaintainableDeleteMethod<T>(ConnectionStringSettings connectionStringSettings) where T : IMaintainableObject;

        /// <summary>
        /// Returns the nameable factory.
        /// </summary>
        /// <typeparam name="TNam">
        /// The <see cref="INameableObject"/> based type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IItemImportFactory{T}"/>.
        /// </returns>
        IItemImportFactory<TNam> GetNameableFactory<TNam>() where TNam : IItemObject;

        #endregion
    }
}