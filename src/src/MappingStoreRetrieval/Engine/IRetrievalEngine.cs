// -----------------------------------------------------------------------
// <copyright file="IRetrievalEngine.cs" company="EUROSTAT">
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
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Constants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The RetrievalEngine interface.
    /// </summary>
    /// <typeparam name="TMaint">
    /// The <see cref="IMaintainableMutableObject"/> type
    /// </typeparam>
    public interface IRetrievalEngine<TMaint>
        where TMaint : IMaintainableMutableObject
    {
        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">The version related constraints.</param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints);

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        TMaint RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail);

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references <paramref name="referencedStructure"/>.
        /// </summary>
        /// <param name="referencedStructure">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail);

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">The version related constraints</param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints, IList<IMaintainableRefObject> allowedDataflows);

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        TMaint RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows);

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references <paramref name="referencedStructure"/>.
        /// </summary>
        /// <param name="referencedStructure">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}