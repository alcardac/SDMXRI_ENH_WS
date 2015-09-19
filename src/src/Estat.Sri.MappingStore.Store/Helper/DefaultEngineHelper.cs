// -----------------------------------------------------------------------
// <copyright file="DefaultEngineHelper.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Ma.Helpers;
    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Engine;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    /// The default engine helper.
    /// </summary>
    public static class DefaultEngineHelper
    {
        #region Static Fields

        /// <summary>
        /// The artefact engines.
        /// </summary>
        private static readonly IDictionary<Type, Func<Database, object>> _artefactEngines;

        /// <summary>
        /// The _identifiable engines.
        /// </summary>
        private static readonly IDictionary<Type, object> _identifiableEngines;

        /// <summary>
        /// The item engines.
        /// </summary>
        private static readonly IDictionary<Type, object> _itemEngines;

        /// <summary>
        /// The _nameable engines.
        /// </summary>
        private static readonly IDictionary<Type, object> _nameableEngines;

        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="DefaultEngineHelper"/> class.
        /// </summary>
        static DefaultEngineHelper()
        {
            //// TODO use some injector instead. We use currently use SimpleInjector.
            _artefactEngines = new Dictionary<Type, Func<Database, object>>
                                   {
                                       { typeof(ICategorisationObject), settings => new CategorisationImportEngine(settings) },
                                       { typeof(ICategorySchemeObject), settings => new CategorySchemeImportEngine(settings) },
                                       { typeof(ICodelistObject), settings => new CodeListEngine(settings) },
                                       { typeof(IAgencyScheme), settings => new AgencySchemeImportEngine(settings) },
                                       { typeof(IConceptSchemeObject), settings => new ConceptSchemeImportEngine(settings) },
                                       { typeof(IDataflowObject), settings => new DataflowImportEngine(settings) },
                                       { typeof(IDataStructureObject), settings => new DsdImportEngine(settings) },
                                       { typeof(ICrossSectionalDataStructureObject), settings => new DsdImportEngine(settings) },
                                       { typeof(IHierarchicalCodelistObject), settings => new HclImportEngine(settings) },
                                       { typeof(IDataProviderScheme), settings => new DataProviderSchemeImportEngine(settings) },
                                       { typeof(IDataConsumerScheme), settings => new DataConsumerSchemeImportEngine(settings) },
                                       { typeof(IOrganisationUnitSchemeObject), settings => new OrganisationUnitSchemeImportEngine(settings) },
                                       { typeof(IStructureSetObject), settings => new StructureSetImportEngine(settings) },
                                       { typeof(IContentConstraintObject), settings => new ContentConstraintImportEngine(settings) },
                                   };
            _identifiableEngines = new Dictionary<Type, object> { { typeof(IComponent), new ComponentImportEngine() }, { typeof(IGroup), new DsdGroupImportEngine() } };


            _itemEngines = new Dictionary<Type, object>
                                   {
                                       { typeof(ICode), new CodeEngine() },
                                       { typeof(IConceptObject), new ConceptImportEngine() },
                                       { typeof(ICategoryObject), new CategoryImportEngine() },
                                       { typeof(IAgency), new AgencyImportEngine() },
                                       { typeof(IOrganisationUnit), new OrganisationUnitImportEngine() },
                                       { typeof(IDataProvider), new DataProviderImportEngine() },
                                       { typeof(IDataConsumer), new DataConsumerImportEngine() },
                                   };
            _nameableEngines = new Dictionary<Type, object>();
        }

        #region Public Methods and Operators

        /// <summary>
        /// Returns the default artefact delete method for <typeparamref name="T"/>
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IDeleteEngine{T}"/>.
        /// </returns>
        public static IDeleteEngine<T> GetArtefactDeleteEngine<T>(ConnectionStringSettings connectionStringSettings) where T : IMaintainableObject
        {
            Func<Database, object> engine;
            if (_artefactEngines.TryGetValue(typeof(T), out engine))
            {
                var database = DatabasePool.GetDatabase(connectionStringSettings);
                return engine(database) as IDeleteEngine<T>;
            }

            return null;
        }

        /// <summary>
        /// Returns the default insert artefact engine for <typeparamref name="T"/>
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IImportEngine{T}"/>.
        /// </returns>
        public static IImportEngine<T> GetArtefactEngine<T>(ConnectionStringSettings connectionStringSettings) where T : IMaintainableObject
        {
            Func<Database, object> engine;
            if (_artefactEngines.TryGetValue(typeof(T), out engine))
            {
                var database = DatabasePool.GetDatabase(connectionStringSettings);
                return engine(database) as IImportEngine<T>;
            }

            return null;
        }

        /// <summary>
        /// Returns the default identifiable engine for type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="IIdentifiableObject"/> based type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IIdentifiableImportEngine{T}"/>.
        /// </returns>
        public static IIdentifiableImportEngine<T> GetIdentifiableEngine<T>() where T : IIdentifiableObject
        {
            object engine;
            if (_identifiableEngines.TryGetValue(typeof(T), out engine))
            {
                return engine as IIdentifiableImportEngine<T>;
            }

            return null;
        }

        /// <summary>
        /// Returns the default item engine for type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="INameableObject"/> based type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IItemImportEngine{T}"/>.
        /// </returns>
        public static IItemImportEngine<T> GetItemEngine<T>() where T : IItemObject
        {
            object engine;
            if (_itemEngines.TryGetValue(typeof(T), out engine))
            {
                return engine as IItemImportEngine<T>;
            }

            return null;
        }

        /// <summary>
        /// Returns the default item engine for type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The <see cref="INameableObject" /> based type.</typeparam>
        /// <typeparam name="TProc">The type of the procedure.</typeparam>
        /// <returns>
        /// The <see cref="INameableImportEngine{T, TProc}" />.
        /// </returns>
        public static INameableImportEngine<T, TProc> GetNameableEngine<T, TProc>() where T : INameableObject where TProc : IIdentifiableProcedure
        {
            object engine;
            if (_nameableEngines.TryGetValue(typeof(T), out engine))
            {
                return engine as INameableImportEngine<T, TProc>;
            }

            return null;
        }

        #endregion
    }
}