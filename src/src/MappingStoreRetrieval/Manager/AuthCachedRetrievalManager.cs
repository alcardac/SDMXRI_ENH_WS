﻿// -----------------------------------------------------------------------
// <copyright file="AuthCachedRetrievalManager.cs" company="EUROSTAT">
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
    using System.Globalization;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Extensions;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    ///     The cached retrieval manager.
    /// </summary>
    public class AuthCachedRetrievalManager : AuthRetrievalManagerBase
    {
        #region Static Fields

        /// <summary>
        ///     The builder that builds a <see cref="IStructureReference" /> from a <see cref="IMaintainableMutableObject" />
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutable = new StructureReferenceFromMutableBuilder();

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthCachedRetrievalManager));

        #endregion

        #region Fields

        /// <summary>
        ///     The _request to artefacts.
        /// </summary>
        private readonly DictionaryOfSets<IStructureReference, IMaintainableMutableObject>[] _requestToArtefacts;

        /// <summary>
        ///     The _maintainable mutable objects used as cache.
        /// </summary>
        private readonly Dictionary<IStructureReference, IMaintainableMutableObject> _requestToLatestArtefacts;

        /// <summary>
        ///     The _maintainable mutable objects used as cache.
        /// </summary>
        private readonly Dictionary<IStructureReference, IMaintainableMutableObject> _requestToLatestArtefactsStub;

        /// <summary>
        ///     The retrieval manager with allowed dataflows.
        /// </summary>
        private readonly IAuthSdmxMutableObjectRetrievalManager _retrievalAuthManager;

        /// <summary>
        ///     The retrieval manager.
        /// </summary>
        private readonly IAuthSdmxMutableObjectRetrievalManager _retrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCachedRetrievalManager"/> class.
        /// </summary>
        /// <param name="maintainableMutableObjects">
        /// The mutable objects.
        /// </param>
        /// <param name="retrievalAuthManager">
        /// The retrieval manager with allowed dataflows.
        /// </param>
        public AuthCachedRetrievalManager(IEnumerable<IMaintainableMutableObject> maintainableMutableObjects, IAuthSdmxMutableObjectRetrievalManager retrievalAuthManager)
        {
            this._retrievalManager = retrievalAuthManager;
            this._retrievalAuthManager = retrievalAuthManager;
            this._requestToLatestArtefacts = new Dictionary<IStructureReference, IMaintainableMutableObject>();
            this._requestToArtefacts = new DictionaryOfSets<IStructureReference, IMaintainableMutableObject>[4];
            this._requestToArtefacts[GetCacheIndex(false, false)] = new DictionaryOfSets<IStructureReference, IMaintainableMutableObject>();
            this._requestToArtefacts[GetCacheIndex(false, true)] = new DictionaryOfSets<IStructureReference, IMaintainableMutableObject>();
            this._requestToArtefacts[GetCacheIndex(true, false)] = new DictionaryOfSets<IStructureReference, IMaintainableMutableObject>();
            this._requestToArtefacts[GetCacheIndex(true, true)] = new DictionaryOfSets<IStructureReference, IMaintainableMutableObject>();

            this._requestToLatestArtefactsStub = new Dictionary<IStructureReference, IMaintainableMutableObject>();

            if (maintainableMutableObjects != null)
            {
                foreach (IMaintainableMutableObject mutableObject in maintainableMutableObjects)
                {
                    var cache = this.GetCache(mutableObject.ExternalReference != null && mutableObject.ExternalReference.IsTrue, false);
                    cache.Add(_fromMutable.Build(mutableObject), new HashSet<IMaintainableMutableObject> { mutableObject });
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a single Agency Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IAgencySchemeMutableObject"/> .
        /// </returns>
        public override IAgencySchemeMutableObject GetMutableAgencyScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.AgencyScheme, this._retrievalManager.GetMutableAgencyScheme);
        }

        /// <summary>
        /// Gets AgencySchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.AgencyScheme, this._retrievalManager.GetMutableAgencySchemeObjects);
        }

        /// <summary>
        /// Gets a single Categorisation, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed
        ///     dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorisationMutableObject"/> .
        /// </returns>
        public override ICategorisationMutableObject GetMutableCategorisation(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.Categorisation, allowedDataflows, this._retrievalAuthManager.GetMutableCategorisation);
        }

        /// <summary>
        /// Gets CategorisationObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(
            IMaintainableRefObject xref, 
            bool returnLatest, 
            bool returnStub, 
            IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.Categorisation, allowedDataflows, this._retrievalAuthManager.GetMutableCategorisationObjects);
        }

        /// <summary>
        /// Gets a single CategoryScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorySchemeMutableObject"/> .
        /// </returns>
        public override ICategorySchemeMutableObject GetMutableCategoryScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.CategoryScheme, this._retrievalManager.GetMutableCategoryScheme);
        }

        /// <summary>
        /// Gets CategorySchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.CategoryScheme, this._retrievalManager.GetMutableCategorySchemeObjects);
        }

        /// <summary>
        /// Gets a single CodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.ICodelistMutableObject"/> .
        /// </returns>
        public override ICodelistMutableObject GetMutableCodelist(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.CodeList, this._retrievalManager.GetMutableCodelist);
        }

        /// <summary>
        /// Gets CodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICodelistMutableObject> GetMutableCodelistObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.CodeList, this._retrievalManager.GetMutableCodelistObjects);
        }

        /// <summary>
        /// Gets a single ConceptScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme.IConceptSchemeMutableObject"/> .
        /// </returns>
        public override IConceptSchemeMutableObject GetMutableConceptScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.ConceptScheme, this._retrievalManager.GetMutableConceptScheme);
        }

        /// <summary>
        /// Gets ConceptSchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.ConceptScheme, this._retrievalManager.GetMutableConceptSchemeObjects);
        }

        /// <summary>
        /// Returns a single Content Constraint, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The Content constraint.
        /// </returns>
        public override IContentConstraintMutableObject GetMutableContentConstraint(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.ContentConstraint, this._retrievalManager.GetMutableContentConstraint);
        }

        /// <summary>
        /// Returns ContentConstraintBeans that match the parameters in the ref bean.  If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ContentConstraintObjects
        /// </summary>
        /// <param name="xref">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public override ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.ContentConstraint, this._retrievalManager.GetMutableContentConstraintObjects);
        }

        /// <summary>
        /// Gets a single data consumer scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataConsumerSchemeMutableObject"/> .
        /// </returns>
        public override IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.DataConsumerScheme, this._retrievalManager.GetMutableDataConsumerScheme);
        }

        /// <summary>
        /// Gets DataConsumerSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.DataConsumerScheme, this._retrievalManager.GetMutableDataConsumerSchemeObjects);
        }

        /// <summary>
        /// Gets a single Data Provider Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataProviderSchemeMutableObject"/> .
        /// </returns>
        public override IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.DataProviderScheme, this._retrievalManager.GetMutableDataProviderScheme);
        }

        /// <summary>
        /// Gets DataProviderSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.DataProviderScheme, this._retrievalManager.GetMutableDataProviderSchemeObjects);
        }

        /// <summary>
        /// Gets a single DataStructure.
        ///     This expects the ref object either to contain a URN or all the attributes required to uniquely identify the object.
        ///     If version information is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataStructureMutableObject"/> .
        /// </returns>
        public override IDataStructureMutableObject GetMutableDataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.Dsd, this._retrievalManager.GetMutableDataStructure);
        }

        /// <summary>
        /// Gets DataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all dataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.Dsd, this._retrievalManager.GetMutableDataStructureObjects);
        }

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed
        ///     dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataflowMutableObject"/> .
        /// </returns>
        public override IDataflowMutableObject GetMutableDataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.Dataflow, allowedDataflows, this._retrievalAuthManager.GetMutableDataflow);
        }

        /// <summary>
        /// Gets DataflowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataflowObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataflowMutableObject> GetMutableDataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.Dataflow, allowedDataflows, this._retrievalAuthManager.GetMutableDataflowObjects);
        }

        /// <summary>
        /// Gets a single HierarchicCodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.IHierarchicalCodelistMutableObject"/> .
        /// </returns>
        public override IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.HierarchicalCodelist, this._retrievalManager.GetMutableHierarchicCodeList);
        }

        /// <summary>
        /// Gets HierarchicalCodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.HierarchicalCodelist, this._retrievalManager.GetMutableHierarchicCodeListObjects);
        }

        /// <summary>
        /// Gets a single MetadataStructure , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The
        ///     <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataStructureDefinitionMutableObject"/>
        ///     .
        /// </returns>
        public override IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.Msd, this._retrievalManager.GetMutableMetadataStructure);
        }

        /// <summary>
        /// Gets MetadataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.Msd, this._retrievalManager.GetMutableMetadataStructureObjects);
        }

        /// <summary>
        /// Gets a single Metadataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataFlowMutableObject"/> .
        /// </returns>
        public override IMetadataFlowMutableObject GetMutableMetadataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.MetadataFlow, this._retrievalManager.GetMutableMetadataflow);
        }

        /// <summary>
        /// Gets MetadataFlowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.MetadataFlow, this._retrievalManager.GetMutableMetadataflowObjects);
        }

        /// <summary>
        /// Gets a single organization scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IOrganisationUnitSchemeMutableObject"/> .
        /// </returns>
        public override IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.OrganisationUnitScheme, this._retrievalManager.GetMutableOrganisationUnitScheme);
        }

        /// <summary>
        /// Gets OrganisationUnitSchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null
        ///     or
        ///     has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeMutableObject
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.OrganisationUnitScheme, this._retrievalManager.GetMutableOrganisationUnitSchemeObjects);
        }

        /// <summary>
        /// Gets a process @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process.IProcessMutableObject"/> .
        /// </returns>
        public override IProcessMutableObject GetMutableProcessObject(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.Process, this._retrievalManager.GetMutableProcessObject);
        }

        /// <summary>
        /// Gets ProcessObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all IProcessObject
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IProcessMutableObject> GetMutableProcessObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.Process, this._retrievalManager.GetMutableProcessObjects);
        }

        /// <summary>
        /// Returns a provision agreement bean, this expects the ref object to contain
        ///     all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override IProvisionAgreementMutableObject GetMutableProvisionAgreement(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.ProvisionAgreement, this._retrievalManager.GetMutableProvisionAgreement);
        }

        /// <summary>
        /// Returns ProvisionAgreement beans that match the parameters in the ref bean. If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreement beans.
        /// </summary>
        /// <param name="xref">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public override ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.ProvisionAgreement, this._retrievalManager.GetMutableProvisionAgreementObjects);
        }

        /// <summary>
        /// Gets a reporting taxonomy @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.IReportingTaxonomyMutableObject"/> .
        /// </returns>
        public override IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.ReportingTaxonomy, this._retrievalManager.GetMutableReportingTaxonomy);
        }

        /// <summary>
        /// Gets ReportingTaxonomyObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.ReportingTaxonomy, this._retrievalManager.GetMutableReportingTaxonomyObjects);
        }

        /// <summary>
        /// Gets a structure set @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping.IStructureSetMutableObject"/> .
        /// </returns>
        public override IStructureSetMutableObject GetMutableStructureSet(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetLatest(xref, returnLatest, returnStub, SdmxStructureEnumType.StructureSet, this._retrievalManager.GetMutableStructureSet);
        }

        /// <summary>
        /// Gets StructureSetObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this.GetArtefacts(xref, returnLatest, returnStub, SdmxStructureEnumType.StructureSet, this._retrievalManager.GetMutableStructureSetObjects);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the cache index
        /// </summary>
        /// <param name="returnStub">
        /// if set to <c>true</c> select the stub cache.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> latest version cache.
        /// </param>
        /// <returns>
        /// The cache index depending on <paramref name="returnStub"/> and <paramref name="returnLatest"/>
        /// </returns>
        private static int GetCacheIndex(bool returnStub, bool returnLatest)
        {
            var stubIndex = returnStub ? 1 : 0;
            var latestIndex = returnLatest ? 2 : 0;
            var index = stubIndex + latestIndex;
            return index;
        }

        /// <summary>
        /// Returns the artefacts of <typeparamref name="T"/> that match <paramref name="xref"/>
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference. ID, Agency and/or Version can have a value or null. Null is considered a wildcard.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="sdmxStructure">
        /// The SDMX structure type.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed Dataflow.
        /// </param>
        /// <param name="getterAuth">
        /// The getter method to retrieve the artefacts if <see cref="_requestToArtefacts"/> doesn't not contain them with
        ///     authentication.
        /// </param>
        /// <typeparam name="T">
        /// The type of the returned artefacts.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        private ISet<T> GetArtefacts<T>(
            IMaintainableRefObject xref, 
            bool returnLatest, 
            bool returnStub, 
            SdmxStructureEnumType sdmxStructure, 
            IList<IMaintainableRefObject> allowedDataflow, 
            Func<IMaintainableRefObject, bool, bool, IList<IMaintainableRefObject>, ISet<T>> getterAuth) where T : class, IMaintainableMutableObject
        {
            this._retrievalAuthManager.ValidateAuthManager(allowedDataflow);

            return this.GetArtefacts(xref, returnLatest, returnStub, sdmxStructure, (o, latest, stub) => getterAuth(o, latest, stub, allowedDataflow));
        }

        /// <summary>
        /// Returns the artefacts of <typeparamref name="T"/> that match <paramref name="xref"/>
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference. ID, Agency and/or Version can have a value or null. Null is considered a wildcard.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="sdmxStructure">
        /// The SDMX structure type.
        /// </param>
        /// <param name="getter">
        /// The getter method to retrieve the artefacts if <see cref="_requestToArtefacts"/> doesn't not contain them.
        /// </param>
        /// <typeparam name="T">
        /// The type of the returned artefacts.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        private ISet<T> GetArtefacts<T>(IMaintainableRefObject xref, bool returnLatest, bool returnStub, SdmxStructureEnumType sdmxStructure, Func<IMaintainableRefObject, bool, bool, ISet<T>> getter)
            where T : class, IMaintainableMutableObject
        {
            ISet<IMaintainableMutableObject> mutableObjects;
            ISet<T> retrievedObjects;
            IStructureReference structureReference = new StructureReferenceImpl(xref, sdmxStructure);
            var cache = this.GetCache(returnStub, returnLatest);
            if (!cache.TryGetValue(structureReference, out mutableObjects))
            {
                _log.DebugFormat(CultureInfo.InvariantCulture, "Cache miss: {0}", structureReference);
                retrievedObjects = getter(xref, returnLatest, returnStub);
                cache.Add(structureReference, new HashSet<IMaintainableMutableObject>(retrievedObjects));
                foreach (T retrievedObject in retrievedObjects)
                {
                    var reference = _fromMutable.Build(retrievedObject);
                    cache.AddToSet(reference, retrievedObject);
                }
            }
            else
            {
                retrievedObjects = new HashSet<T>(mutableObjects.Cast<T>());
            }

            return retrievedObjects;
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <param name="returnStub">
        /// if set to <c>true</c> select the stub cache.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> latest version cache.
        /// </param>
        /// <returns>
        /// The cache depending on <paramref name="returnStub"/> and <paramref name="returnLatest"/>
        /// </returns>
        private DictionaryOfSets<IStructureReference, IMaintainableMutableObject> GetCache(bool returnStub, bool returnLatest)
        {
            var index = GetCacheIndex(returnStub, returnLatest);
            return this._requestToArtefacts[index];
        }

        /// <summary>
        /// Return the latest artefact of type <typeparamref name="T" /> that matches the <paramref name="xref" />.
        /// </summary>
        /// <typeparam name="T">The type of the requested artefact</typeparam>
        /// <param name="xref">The maintainable reference. The version must be null.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">Set to <c>true</c> to return only stubs.</param>
        /// <param name="sdmxStructure">The SDMX structure type.</param>
        /// <param name="getter">The getter method to retrieve the artefact if <see cref="_requestToLatestArtefacts" /> doesn't not contain it.</param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject" /> of type <typeparamref name="T" />; otherwise null
        /// </returns>
        private T GetLatest<T>(IMaintainableRefObject xref, bool returnLatest, bool returnStub, SdmxStructureEnumType sdmxStructure, Func<IMaintainableRefObject, bool, bool, T> getter)
            where T : class, IMaintainableMutableObject
        {
            IMaintainableMutableObject mutableObject;
            IStructureReference structureReference = new StructureReferenceImpl(xref, sdmxStructure);
            var cache = returnStub ? this._requestToLatestArtefactsStub : this._requestToLatestArtefacts;
            if (!cache.TryGetValue(structureReference, out mutableObject))
            {
                _log.DebugFormat(CultureInfo.InvariantCulture, "Cache miss: {0}", structureReference);
                T retrievedObject = getter(xref, returnLatest, returnStub);
                if (retrievedObject != null)
                {
                    cache.Add(structureReference, retrievedObject);
                    var reference = _fromMutable.Build(retrievedObject);

                    var allVersionCache = this.GetCache(returnStub, false);
                    allVersionCache.AddToSet(reference, retrievedObject);

                    var latestCache = this.GetCache(returnStub, true);
                    latestCache.AddToSet(reference, retrievedObject);

                    return retrievedObject;
                }

                return null;
            }

            return mutableObject as T;
        }

        /// <summary>
        /// Return the latest artefact of type <typeparamref name="T" /> that matches the <paramref name="xref" />.
        /// </summary>
        /// <typeparam name="T">The type of the requested artefact</typeparam>
        /// <param name="xref">The maintainable reference. The version must be null.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">Set to <c>true</c> to return only stubs.</param>
        /// <param name="sdmxStructure">The SDMX structure type.</param>
        /// <param name="allowedDataflow">The allowed Dataflow.</param>
        /// <param name="getterAuth">The getter method to retrieve the artefact if <see cref="_requestToLatestArtefacts" /> doesn't not contain it with
        /// used of the
        /// <paramref name="allowedDataflow" />
        /// .</param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject" /> of type <typeparamref name="T" />; otherwise null
        /// </returns>
        private T GetLatest<T>(
            IMaintainableRefObject xref, 
            bool returnLatest, 
            bool returnStub, 
            SdmxStructureEnumType sdmxStructure, 
            IList<IMaintainableRefObject> allowedDataflow, 
            Func<IMaintainableRefObject, bool, bool, IList<IMaintainableRefObject>, T> getterAuth) where T : class, IMaintainableMutableObject
        {
            this._retrievalAuthManager.ValidateAuthManager(allowedDataflow);

            return this.GetLatest(xref, returnLatest, returnStub, sdmxStructure, (o, rl, b) => getterAuth(o, rl, b, allowedDataflow));
        }

        #endregion
    }
}