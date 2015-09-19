// -----------------------------------------------------------------------
// <copyright file="ICrossReferenceRetrievalBuilder.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    /// The <see cref="ICrossReferenceMutableRetrievalManager"/> builder interface.
    /// </summary>
    public interface ICrossReferenceRetrievalBuilder : IBuilder<ICrossReferenceMutableRetrievalManager, ISdmxMutableObjectRetrievalManager>
    {
        /// <summary>
        /// Build a <see cref="ICrossReferenceMutableRetrievalManager"/> from the specified <paramref name="retrievalManager"/> for retrieving stub artefacts
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        ICrossReferenceMutableRetrievalManager BuildStub(ISdmxMutableObjectRetrievalManager retrievalManager);

        /// <summary>
        /// Build a <see cref="ICrossReferenceMutableRetrievalManager"/> from the specified <paramref name="retrievalManager"/> for retrieving stub artefacts
        /// </summary>
        /// <param name="retrievalManager">
        ///     The retrieval manager.
        /// </param>
        /// <param name="retrievalAuthManager">The authorization aware retrieval manager</param>
        /// <returns>
        /// The <see cref="IAuthCrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        IAuthCrossReferenceMutableRetrievalManager BuildStub(ISdmxMutableObjectRetrievalManager retrievalManager, IAuthSdmxMutableObjectRetrievalManager retrievalAuthManager);

        /// <summary>
        /// Build a <see cref="ICrossReferenceMutableRetrievalManager"/> from the specified <paramref name="retrievalManager"/> for retrieving full artefacts
        /// </summary>
        /// <param name="retrievalManager">
        ///     The retrieval manager.
        /// </param>
        /// <param name="retrievalAuthManager">The authorization aware retrieval manager</param>
        /// <returns>
        /// The <see cref="IAuthCrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        IAuthCrossReferenceMutableRetrievalManager Build(ISdmxMutableObjectRetrievalManager retrievalManager, IAuthSdmxMutableObjectRetrievalManager retrievalAuthManager);
    }
}