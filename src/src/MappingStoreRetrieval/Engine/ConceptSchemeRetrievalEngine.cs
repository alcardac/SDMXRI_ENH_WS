// -----------------------------------------------------------------------
// <copyright file="ConceptSchemeRetrievalEngine.cs" company="EUROSTAT">
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

    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme;

    /// <summary>
    ///     The concept scheme retrieval engine.
    /// </summary>
    internal class ConceptSchemeRetrievalEngine : ItemSchemeRetrieverEngine<IConceptSchemeMutableObject, IConceptMutableObject>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public ConceptSchemeRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create an item.
        /// </summary>
        /// <returns>
        ///     The <see cref="IConceptMutableObject"/>
        /// </returns>
        protected override IConceptMutableObject CreateItem()
        {
            return new ConceptMutableCore();
        }

        /// <summary>
        ///     Create a new instance of <see cref="IConceptSchemeMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="IConceptSchemeMutableObject" />.
        /// </returns>
        protected override IConceptSchemeMutableObject CreateArtefact()
        {
            return new ConceptSchemeMutableCore();
        }

        #endregion
    }
}