// -----------------------------------------------------------------------
// <copyright file="ICrossReferenceResolverMutableEngine.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The purpose of the <c>ICrossReferenceResolverMutableEngine</c> is to resolve cross references for mutable, either Maintainable,
    ///     beans within a <see cref="IMutableObjects" /> container.
    /// </summary>
    public interface ICrossReferenceResolverMutableEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a Dictionary of <see cref="IIdentifiableMutableObject"/> alongside any cross references they declare that could not be found in the set of
        ///     <paramref name="beans"/>
        ///     provided, and the <paramref name="retriever"/> (if given).
        ///     <p/>
        ///     <b>NOTE :</b>An empty Map is returned if all cross references are present.
        /// </summary>
        /// <param name="beans">
        /// - the objects to return the Map of missing references for
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing  until it has found every directly and indirectly referenced
        /// </param>
        /// <param name="retriever">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <returns>
        /// Map of IIdentifiableMutableObject with a Set of CrossReferences that could not be resolved for the IIdentifiableMutableObject - an empty Map is returned if all cross references are present
        /// </returns>
        MaintainableReferenceDictionary GetMissingCrossReferences(IMutableObjects beans, int numberLevelsDeep, Func<IStructureReference, IMaintainableMutableObject> retriever);

        /// <summary>
        /// Resolves all references and returns a Map containing all the input beans and the objects that are cross referenced,
        ///     the Map's key set contains the Identifiable that is the referencing object and the Map's value collection contains the referenced artifacts.
        /// </summary>
        /// <param name="beans">
        /// - the <see cref="IMutableObjects"/> container, containing all the beans to check references for
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this.
        /// </param>
        /// <param name="retriever">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <returns>
        /// Map of referencing versus  references
        /// </returns>
        /// <exception cref="SdmxReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        MaintainableDictionary<IMaintainableMutableObject> ResolveReferences(IMutableObjects beans, int numberLevelsDeep, Func<IStructureReference, IMaintainableMutableObject> retriever);

        /// <summary>
        /// Returns a set of <see cref="IMaintainableMutableObject"/> that the IMaintainableMutableObject cross references
        /// </summary>
        /// <param name="artefact">
        /// The bean.
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this.
        /// </param>
        /// <param name="retriever">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <exception cref="SdmxReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        /// <returns>
        /// a set of <see cref="IMaintainableMutableObject"/> that the IMaintainableMutableObject cross references
        /// </returns>
        ISet<IMaintainableMutableObject> ResolveReferences(IMaintainableMutableObject artefact, int numberLevelsDeep, Func<IStructureReference, IMaintainableMutableObject> retriever);

        #endregion
    }
}