// -----------------------------------------------------------------------
// <copyright file="RetrievalEngineContainer.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using Estat.Sri.MappingStoreRetrieval.Config;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;

    /// <summary>
    /// The retrieval engine container.
    /// </summary>
    internal class RetrievalEngineContainer
    {
        #region Fields

        /// <summary>
        ///     The constraint list retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IContentConstraintMutableObject> _contentConstraintListRetrievalEngine;

        /// <summary>
        ///     The agency scheme list retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IAgencySchemeMutableObject> _agencySchemeListRetrievalEngine;

        /// <summary>
        ///     The organisation unit scheme list retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IOrganisationUnitSchemeMutableObject> _organisationUnitSchemeListRetrievalEngine;

        /// <summary>
        ///     The data provider scheme list retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IDataProviderSchemeMutableObject> _dataProviderSchemeListRetrievalEngine;

        /// <summary>
        ///     The data consumer scheme list retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IDataConsumerSchemeMutableObject> _dataConsumerSchemeListRetrievalEngine;

        /// <summary>
        ///     The StructureSet retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IStructureSetMutableObject> _structureSetRetrievalEngine;

        /// <summary>
        ///     The categorisation from dataflow retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<ICategorisationMutableObject> _categorisationRetrievalEngine;

        /// <summary>
        ///     The category scheme retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<ICategorySchemeMutableObject> _categorySchemeRetrievalEngine;

        /// <summary>
        ///     The code list retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<ICodelistMutableObject> _codeListRetrievalEngine;

        /// <summary>
        ///     The concept scheme retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IConceptSchemeMutableObject> _conceptSchemeRetrievalEngine;

        /// <summary>
        ///     The dataflow retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IDataflowMutableObject> _dataflowRetrievalEngine;

        /// <summary>
        ///     The DSD retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IDataStructureMutableObject> _dsdRetrievalEngine;

        /// <summary>
        ///     The HCL retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IHierarchicalCodelistMutableObject> _hclRetrievalEngine;

        /// <summary>
        /// The _partial code list retrieval engine.
        /// </summary>
        private readonly PartialCodeListRetrievalEngine _partialCodeListRetrievalEngine;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrievalEngineContainer"/> class. 
        /// </summary>
        /// <param name="mappingStoreDB">
        /// The mapping Store DB.
        /// </param>
        public RetrievalEngineContainer(Database mappingStoreDB)
        {
            DataflowFilter filter = ConfigManager.Config.DataflowConfiguration.IgnoreProductionForStructure ? DataflowFilter.Any : DataflowFilter.Production;
            this._categorisationRetrievalEngine = new CategorisationRetrievalEngine(mappingStoreDB, filter);
            this._categorySchemeRetrievalEngine = new CategorySchemeRetrievalEngine(mappingStoreDB);
            this._codeListRetrievalEngine = new CodeListRetrievalEngine(mappingStoreDB);
            this._conceptSchemeRetrievalEngine = new ConceptSchemeRetrievalEngine(mappingStoreDB);
            this._dataflowRetrievalEngine = new DataflowRetrievalEngine(mappingStoreDB, filter);
            this._dsdRetrievalEngine = new DsdRetrievalEngine(mappingStoreDB);
            this._hclRetrievalEngine = new HierarchicalCodeListRetrievealEngine(mappingStoreDB);
            this._partialCodeListRetrievalEngine = new PartialCodeListRetrievalEngine(mappingStoreDB);
            this._agencySchemeListRetrievalEngine = new AgencySchemeRetrievalEngine(mappingStoreDB);
            this._organisationUnitSchemeListRetrievalEngine = new OrganisationUnitSchemeRetrievalEngine(mappingStoreDB);
            this._dataProviderSchemeListRetrievalEngine = new DataProviderSchemeRetrievalEngine(mappingStoreDB);
            this._dataConsumerSchemeListRetrievalEngine = new DataConsumerSchemeRetrievalEngine(mappingStoreDB);
            this._structureSetRetrievalEngine = new StructureSetRetrievalEngine(mappingStoreDB);
            this._contentConstraintListRetrievalEngine = new ContentConstraintRetrievalEngine(mappingStoreDB);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the constraint list retrieval engine.
        /// </summary>
        public IRetrievalEngine<IContentConstraintMutableObject> ContentConstraintRetrievalEngine
        {
            get
            {
                return this._contentConstraintListRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the agency scheme list retrieval engine.
        /// </summary>
        public IRetrievalEngine<IAgencySchemeMutableObject> AgencySchemeRetrievalEngine
        {
            get
            {
                return this._agencySchemeListRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the organisation unit scheme list retrieval engine.
        /// </summary>
        public IRetrievalEngine<IOrganisationUnitSchemeMutableObject> OrganisationUnitSchemeRetrievalEngine
        {
            get
            {
                return this._organisationUnitSchemeListRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the data provider scheme list retrieval engine.
        /// </summary>
        public IRetrievalEngine<IDataProviderSchemeMutableObject> DataProviderSchemeRetrievalEngine
        {
            get
            {
                return this._dataProviderSchemeListRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the data consumer scheme list retrieval engine.
        /// </summary>
        public IRetrievalEngine<IDataConsumerSchemeMutableObject> DataConsumerSchemeRetrievalEngine
        {
            get
            {
                return this._dataConsumerSchemeListRetrievalEngine;
            }
        }


        /// <summary>
        ///     Gets the StructureSet retrieval engine.
        /// </summary>
        public IRetrievalEngine<IStructureSetMutableObject> StructureSetRetrievalEngine
        {
            get
            {
                return this._structureSetRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the categorisation from dataflow retrieval engine.
        /// </summary>
        public IRetrievalEngine<ICategorisationMutableObject> CategorisationRetrievalEngine
        {
            get
            {
                return this._categorisationRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the category scheme retrieval engine.
        /// </summary>
        public IRetrievalEngine<ICategorySchemeMutableObject> CategorySchemeRetrievalEngine
        {
            get
            {
                return this._categorySchemeRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the code list retrieval engine.
        /// </summary>
        public IRetrievalEngine<ICodelistMutableObject> CodeListRetrievalEngine
        {
            get
            {
                return this._codeListRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the concept scheme retrieval engine.
        /// </summary>
        public IRetrievalEngine<IConceptSchemeMutableObject> ConceptSchemeRetrievalEngine
        {
            get
            {
                return this._conceptSchemeRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the DSD retrieval engine.
        /// </summary>
        public IRetrievalEngine<IDataStructureMutableObject> DSDRetrievalEngine
        {
            get
            {
                return this._dsdRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the dataflow retrieval engine.
        /// </summary>
        public IRetrievalEngine<IDataflowMutableObject> DataflowRetrievalEngine
        {
            get
            {
                return this._dataflowRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the HCL retrieval engine.
        /// </summary>
        public IRetrievalEngine<IHierarchicalCodelistMutableObject> HclRetrievalEngine
        {
            get
            {
                return this._hclRetrievalEngine;
            }
        }

        /// <summary>
        /// Gets the partial code list retrieval engine.
        /// </summary>
        public PartialCodeListRetrievalEngine PartialCodeListRetrievalEngine
        {
            get
            {
                return this._partialCodeListRetrievalEngine;
            }
        }

        #endregion
    }
}