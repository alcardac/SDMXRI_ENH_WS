// -----------------------------------------------------------------------
// <copyright file="MappingStoreManager.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Ma.Helpers;
    using Estat.Sri.MappingStore.Store.Engine;
    using Estat.Sri.MappingStore.Store.Factory;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Manager.Persist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// The mapping store manager.
    /// </summary>
    public class MappingStoreManager : IStructurePersistenceManager
    {
        #region Fields

        /// <summary>
        /// The _artefact import statuses.
        /// </summary>
        private readonly IList<ArtefactImportStatus> _artefactImportStatuses;

        /// <summary>
        /// The _connection string settings.
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        /// The _factories.
        /// </summary>
        private readonly IEngineFactories _factories;

        /// <summary>
        /// The _measure dimension representation engine
        /// </summary>
        private readonly MeasureDimensionRepresentationEngine _measureDimensionRepresentationEngine;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreManager"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        /// <param name="artefactImportStatuses">
        /// The artefact import statuses.
        /// </param>
        public MappingStoreManager(ConnectionStringSettings connectionStringSettings, IList<ArtefactImportStatus> artefactImportStatuses)
            : this(connectionStringSettings, null, artefactImportStatuses)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreManager"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        /// <param name="factories">
        /// The factories.
        /// </param>
        /// <param name="artefactImportStatuses">
        /// The artefact import statuses.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionStringSettings"/> is null
        /// -or-
        /// <paramref name="artefactImportStatuses"/> is null
        /// </exception>
        public MappingStoreManager(ConnectionStringSettings connectionStringSettings, IEngineFactories factories, IList<ArtefactImportStatus> artefactImportStatuses)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            if (artefactImportStatuses == null)
            {
                throw new ArgumentNullException("artefactImportStatuses");
            }

            this._connectionStringSettings = connectionStringSettings;
            this._artefactImportStatuses = artefactImportStatuses;
            this._factories = factories ?? new EngineFactories();
            Database database = DatabasePool.GetDatabase(connectionStringSettings);
            this._measureDimensionRepresentationEngine = new MeasureDimensionRepresentationEngine(database);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Deletes the maintainable structures in the supplied objects
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable to delete
        /// </param>
        public void DeleteStructure(IMaintainableObject maintainable)
        {
            ISdmxObjects objects = new SdmxObjectsImpl(maintainable);
            this.DeleteStructures(objects);
        }

        /// <summary>
        /// Deletes the maintainable structures in the supplied sdmxObjects
        /// </summary>
        /// <param name="sdmxObjects">
        /// SDMX objects
        /// </param>
        public void DeleteStructures(ISdmxObjects sdmxObjects)
        {
            this.DeleteMaintainable(sdmxObjects.Categorisations);
            this.DeleteMaintainable(sdmxObjects.StructureSets);
            this.DeleteMaintainable(sdmxObjects.ContentConstraintObjects);
            this.DeleteMaintainable(sdmxObjects.AgenciesSchemes);
            this.DeleteMaintainable(sdmxObjects.DataProviderSchemes);
            this.DeleteMaintainable(sdmxObjects.DataConsumerSchemes);
            this.DeleteMaintainable(sdmxObjects.OrganisationUnitSchemes);
            this.DeleteMaintainable(sdmxObjects.HierarchicalCodelists);
            this.DeleteMaintainable(sdmxObjects.Dataflows);
            
            this.DeleteMaintainable(sdmxObjects.CategorySchemes);
            this.DeleteMaintainable(sdmxObjects.DataStructures);
            this.DeleteMaintainable(sdmxObjects.ConceptSchemes);
            this.DeleteMaintainable(sdmxObjects.Codelists);
        }

        /// <summary>
        /// Saves the maintainable
        /// </summary>
        /// <param name="maintainable">
        /// The <see cref="IMaintainableObject"/>
        /// </param>
        public void SaveStructure(IMaintainableObject maintainable)
        {
            ISdmxObjects objects = new SdmxObjectsImpl(maintainable);
            this.SaveStructures(objects);
        }

        /// <summary>
        /// Saves the maintainable structures in the supplied sdmxObjects
        /// </summary>
        /// <param name="sdmxObjects">
        /// SDMX objects
        /// </param>
        public void SaveStructures(ISdmxObjects sdmxObjects)
        {
            this.InsertMaintainable(sdmxObjects.Codelists);
            this.InsertMaintainable(sdmxObjects.ConceptSchemes);
            this.InsertMaintainable(sdmxObjects.DataStructures);
            this.InsertMaintainable(sdmxObjects.Dataflows);
            this.InsertMaintainable(sdmxObjects.CategorySchemes);
            this.InsertMaintainable(sdmxObjects.Categorisations);
            this.InsertMaintainable(sdmxObjects.HierarchicalCodelists);
            this.InsertMaintainable(sdmxObjects.AgenciesSchemes);
            this.InsertMaintainable(sdmxObjects.DataProviderSchemes);
            this.InsertMaintainable(sdmxObjects.DataConsumerSchemes);
            this.InsertMaintainable(sdmxObjects.OrganisationUnitSchemes);
            this.InsertMaintainable(sdmxObjects.StructureSets);
            this.InsertMaintainable(sdmxObjects.ContentConstraintObjects);

            // HACK. Handle SDMX v2.1 Measure Dimensions until proper support is added.
            // We create a dummy codelist for all SDMX v2.1 Measure dimensions.
            this._measureDimensionRepresentationEngine.CreateDummyCodelistForAll();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete the specified <paramref name="maintainables"/> 
        /// </summary>
        /// <param name="maintainables">
        /// The maintainable.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based type.
        /// </typeparam>
        private void DeleteMaintainable<T>(IEnumerable<T> maintainables) where T : IMaintainableObject
        {
            var maintainableDeleteMethod = this._factories.GetMaintainableDeleteMethod<T>(this._connectionStringSettings);
            maintainableDeleteMethod(maintainables);
        }

        /// <summary>
        /// Insert the specified <paramref name="maintainables"/>.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainables.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based type.
        /// </typeparam>
        private void InsertMaintainable<T>(IEnumerable<T> maintainables) where T : IMaintainableObject
        {
            this._artefactImportStatuses.AddAll(this._factories.GetMaintainableImportMethod<T>(this._connectionStringSettings)(maintainables));
        }

        #endregion
    }
}