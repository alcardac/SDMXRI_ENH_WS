// -----------------------------------------------------------------------
// <copyright file="IImportEngine.cs" company="EUROSTAT">
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
    /// The Import Engine interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the <see cref="IMaintainableObject"/>
    /// </typeparam>
    public interface IImportEngine<in T>
        where T : IMaintainableObject
    {
        #region Public Methods and Operators

        /// <summary>
        /// Insert the specified <paramref name="maintainables"/> to the mapping store.
        /// </summary>
        /// <param name="maintainables">
        ///     The maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{ArtefactImportStatus}"/>.
        /// </returns>
        IEnumerable<ArtefactImportStatus> Insert(IEnumerable<T> maintainables);

        #endregion
    }
}