// -----------------------------------------------------------------------
// <copyright file="CrossReferenceRetrievalManager.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Factory;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The cross reference mutable retrieval manager.
    /// </summary>
    public class CrossReferenceRetrievalManager : ICrossReferenceMutableRetrievalManager
    {
        #region Fields

        /// <summary>
        /// The AUTH cross reference manager
        /// </summary>
        private readonly IAuthCrossReferenceMutableRetrievalManager _authCrossReferenceManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceRetrievalManager"/> class.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        public CrossReferenceRetrievalManager(ISdmxMutableObjectRetrievalManager retrievalManager, ConnectionStringSettings connectionStringSettings)
        {
            if (retrievalManager == null)
            {
                throw new ArgumentNullException("retrievalManager");
            }

            this._authCrossReferenceManager = GetAuthCrossReferenceMutableRetrievalManager(retrievalManager, connectionStringSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceRetrievalManager"/> class.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="mappingStoreDatabase">
        /// The mapping Store Database.
        /// </param>
        public CrossReferenceRetrievalManager(ISdmxMutableObjectRetrievalManager retrievalManager, Database mappingStoreDatabase)
        {
            if (retrievalManager == null)
            {
                throw new ArgumentNullException("retrievalManager");
            }

            this._authCrossReferenceManager = GetAuthCrossReferenceMutableRetrievalManager(retrievalManager, mappingStoreDatabase);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceRetrievalManager"/> class.
        /// </summary>
        /// <param name="authCrossReferenceManager">The authentication cross reference manager.</param>
        public CrossReferenceRetrievalManager(IAuthCrossReferenceMutableRetrievalManager authCrossReferenceManager)
        {
            this._authCrossReferenceManager = authCrossReferenceManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a tree structure representing the <paramref name="maintainableObject"/> and all the structures that cross reference it, and the structures that reference them, and so on.
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable object to build the tree for.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableCrossReferencingTree"/>.
        /// </returns>
        public IMutableCrossReferencingTree GetCrossReferenceTree(IMaintainableMutableObject maintainableObject)
        {
            return this._authCrossReferenceManager.GetCrossReferenceTree(maintainableObject, null);
        }

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the structure(s) that match the reference parameter
        /// </summary>
        /// <param name="structureReference">
        /// - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        public IList<IMaintainableMutableObject> GetCrossReferencedStructures(IStructureReference structureReference, bool returnStub, params SdmxStructureType[] structures)
        {
            return this._authCrossReferenceManager.GetCrossReferencedStructures(structureReference, returnStub, null, structures);
        }

        /// <summary>
        /// Returns a list of MaintainableObject that are cross referenced by the given identifiable structure
        /// </summary>
        /// <param name="identifiable">
        /// the identifiable bean to retrieve the references for - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        public IList<IMaintainableMutableObject> GetCrossReferencedStructures(IIdentifiableMutableObject identifiable, bool returnStub, params SdmxStructureType[] structures)
        {
            return this._authCrossReferenceManager.GetCrossReferencedStructures(identifiable, returnStub, null, structures);
        }

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the structure(s) that match the reference parameter
        /// </summary>
        /// <param name="structureReference">
        /// Who References Me?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        public IList<IMaintainableMutableObject> GetCrossReferencingStructures(
            IStructureReference structureReference, bool returnStub,  params SdmxStructureType[] structures)
        {
            return this._authCrossReferenceManager.GetCrossReferencingStructures(structureReference, returnStub, null, structures);
        }

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the given identifiable structure
        /// </summary>
        /// <param name="identifiable">
        /// the identifiable bean to retrieve the references for - Who References Me?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        public IList<IMaintainableMutableObject> GetCrossReferencingStructures(
            IIdentifiableMutableObject identifiable, bool returnStub, params SdmxStructureType[] structures)
        {
            return this._authCrossReferenceManager.GetCrossReferencingStructures(identifiable, returnStub, null, structures);
        }

        #endregion

        /// <summary>
        /// Gets the authentication cross reference mutable retrieval manager.
        /// </summary>
        /// <typeparam name="T">The <paramref name="settings"/> type</typeparam>
        /// <param name="retrievalManager">The retrieval manager.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>the authentication cross reference mutable retrieval manager.</returns>
        private static IAuthCrossReferenceMutableRetrievalManager GetAuthCrossReferenceMutableRetrievalManager<T>(
            ISdmxMutableObjectRetrievalManager retrievalManager,
            T settings)
        {
            IAuthCrossRetrievalManagerFactory factory = new AuthCrossMutableRetrievalManagerFactory();
            IAuthMutableRetrievalManagerFactory retrievalManagerFactory = new AuthMutableRetrievalManagerFactory();
            var authSdmxMutableObjectRetrievalManager = retrievalManagerFactory.GetRetrievalManager(settings, retrievalManager);

            var authCrossReferenceMutableRetrievalManager = factory.GetCrossRetrievalManager(settings, authSdmxMutableObjectRetrievalManager);
            return authCrossReferenceMutableRetrievalManager;
        }
    }
}