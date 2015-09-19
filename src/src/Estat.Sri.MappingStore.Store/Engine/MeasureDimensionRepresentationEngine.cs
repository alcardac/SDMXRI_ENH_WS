// -----------------------------------------------------------------------
// <copyright file="MeasureDimensionRepresentationEngine.cs" company="EUROSTAT">
//   Date Created : 2013-09-26
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
    using System.Data;

    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Factory;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The measure dimension representation engine.
    /// HACK: Creates a dummy codelist for all SDMX v2.1 DSD MeasureDimensions.
    /// </summary>
    public class MeasureDimensionRepresentationEngine
    {
        #region Constants

        /// <summary>
        ///     The SQL query for getting all measure dimensions without codelist.
        /// </summary>
        private const string GetAllMeasureDimensionsWithoutCodelist =
            "select C.COMP_ID, A.AGENCY as CONCEPTSCHEME_AGENCY, A.ID as CONCEPTSCHEME_ID, A.VERSION as CONCEPTSCHEME_VERSION from COMPONENT C INNER JOIN ARTEFACT_VIEW A ON C.CON_SCH_ID = A.ART_ID where c.cl_id is null and c.con_sch_id is not null and is_measure_dim is not null and is_measure_dim = 1";

        /// <summary>
        ///     The SQL update statement that updates a component's codelist.
        /// </summary>
        private const string UpdateComponentCodelist =
            "update COMPONENT set CL_ID = {0} where COMP_ID = {1} and cl_id is null and con_sch_id is not null and is_measure_dim is not null and is_measure_dim = 1";

        #endregion

        #region Fields

        /// <summary>
        ///     The _database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        ///     The _factory.
        /// </summary>
        private readonly IMutableRetrievalManagerFactory _factory;

        /// <summary>
        ///     The _import engine.
        /// </summary>
        private readonly CodeListEngine _importEngine;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureDimensionRepresentationEngine"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        public MeasureDimensionRepresentationEngine(Database database)
        {
            this._database = database;
            this._importEngine = new CodeListEngine(database);
            this._factory = new MutableRetrievalManagerFactory();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Create a dummy codelist for all SDMX v2.1 DSD MeasureDimensions.
        /// </summary>
        public void CreateDummyCodelistForAll()
        {
            using (var state = DbTransactionState.Create(this._database))
            {
                var conceptSchemesPerMeasureDimension = new Dictionary<long, IMaintainableRefObject>();

                state.ExecuteReaderFormat(GetAllMeasureDimensionsWithoutCodelist, reader => PopulateConceptSchemePerMeasureDimension(reader, conceptSchemesPerMeasureDimension));

                this.ConvertConceptSchemes(state, conceptSchemesPerMeasureDimension);

                state.Commit();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the concept scheme per measure dimension.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="conceptSchemesPerMeasureDimension">
        /// The concept schemes per measure dimension.
        /// </param>
        private static void PopulateConceptSchemePerMeasureDimension(IDataReader reader, IDictionary<long, IMaintainableRefObject> conceptSchemesPerMeasureDimension)
        {
            int compIdIdx = reader.GetOrdinal("COMP_ID");
            int agencyIdx = reader.GetOrdinal("CONCEPTSCHEME_AGENCY");
            int idIdx = reader.GetOrdinal("CONCEPTSCHEME_ID");
            int versionIdx = reader.GetOrdinal("CONCEPTSCHEME_VERSION");

            while (reader.Read())
            {
                var compId = DataReaderHelper.GetInt64(reader, compIdIdx);
                var agencyId = DataReaderHelper.GetString(reader, agencyIdx);
                var id = DataReaderHelper.GetString(reader, idIdx);
                var version = DataReaderHelper.GetString(reader, versionIdx);
                var conceptSchemeReference = new MaintainableRefObjectImpl(agencyId, id, version);
                conceptSchemesPerMeasureDimension.Add(compId, conceptSchemeReference);
            }
        }

        /// <summary>
        /// Converts the concept schemes to code lists.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="conceptSchemesPerMeasureDimension">
        /// The concept schemes per measure dimension.
        /// </param>
        private void ConvertConceptSchemes(DbTransactionState state, IEnumerable<KeyValuePair<long, IMaintainableRefObject>> conceptSchemesPerMeasureDimension)
        {
            var transactionalDatabase = new Database(this._database, state.Transaction);
            var mutableObjectRetrievalManager = this.GetRetrievalManager(transactionalDatabase);
            var codelistPrimaryKeyCache = new Dictionary<IMaintainableRefObject, ArtefactFinalStatus>();
            foreach (var keyValuePair in conceptSchemesPerMeasureDimension)
            {
                ArtefactFinalStatus artefactFinalStatus;
                if (!codelistPrimaryKeyCache.TryGetValue(keyValuePair.Value, out artefactFinalStatus))
                {
                    artefactFinalStatus = ArtefactBaseEngine.GetFinalStatus(state, new StructureReferenceImpl(keyValuePair.Value, SdmxStructureEnumType.CodeList));

                    if (artefactFinalStatus.IsEmpty)
                    {
                        var conceptScheme = mutableObjectRetrievalManager.GetMutableConceptScheme(keyValuePair.Value, false, false);
                        var codelist = conceptScheme.ConvertToCodelist();
                        var importStatus = this._importEngine.Insert(state, codelist.ImmutableInstance);
                        artefactFinalStatus = new ArtefactFinalStatus(importStatus.PrimaryKeyValue, true);
                    }

                    codelistPrimaryKeyCache.Add(keyValuePair.Value, artefactFinalStatus);
                }

                state.ExecuteNonQueryFormat(
                    UpdateComponentCodelist, 
                    transactionalDatabase.CreateInParameter("clId", DbType.Int64, artefactFinalStatus.PrimaryKey), 
                    transactionalDatabase.CreateInParameter("compId", DbType.Int64, keyValuePair.Key));
            }
        }

        /// <summary>
        /// Returns the retrieval manager.
        /// </summary>
        /// <param name="transactionalDatabase">
        /// The transactional database.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxMutableObjectRetrievalManager"/>.
        /// </returns>
        private ISdmxMutableObjectRetrievalManager GetRetrievalManager(Database transactionalDatabase)
        {
            return this._factory.GetRetrievalManager(transactionalDatabase);
        }

        #endregion
    }
}