// -----------------------------------------------------------------------
// <copyright file="QueryStructureController.cs" company="EUROSTAT">
//   Date Created : 2011-12-07
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
namespace Estat.Sri.Ws.Controllers.Controller
{
    using System;

    using Estat.Nsi.AuthModule;
    using Estat.Sri.MappingStoreRetrieval.Config;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.Ws.Controllers.Manager;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    /// The query structure controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public abstract class QueryStructureController<TWriter>
    {
        #region Fields

        /// <summary>
        ///     The _dataflow principal
        /// </summary>
        private readonly DataflowPrincipal _dataflowPrincipal;

        /// <summary>
        ///     The _response generator.
        /// </summary>
        private readonly IResponseGenerator<TWriter, ISdmxObjects> _responseGenerator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureController{TWriter}"/> class.
        /// </summary>
        /// <param name="responseGenerator">
        /// The response generator.
        /// </param>
        /// <param name="dataflowPrincipal">
        /// The dataflow principal.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted with query used
        /// </exception>
        protected QueryStructureController(IResponseGenerator<TWriter, ISdmxObjects> responseGenerator, DataflowPrincipal dataflowPrincipal)
        {
            this._responseGenerator = responseGenerator;
            this._dataflowPrincipal = dataflowPrincipal;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parses the request
        /// </summary>
        /// <param name="getMutablesFunc">
        /// The get mutable function.
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        protected IStreamController<TWriter> ParseRequestPrivate(Func<DataflowPrincipal, IMutableObjects> getMutablesFunc)
        {
            // 1 call the StructureRetriever
            // 1.1 see if we configured a specific default DDB oracle provider. It will be overrided if mapping store is also on oracle
            if (!string.IsNullOrEmpty(SettingsManager.OracleProvider))
            {
                DatabaseType.Mappings[MappingStoreDefaultConstants.OracleName].Provider = SettingsManager.OracleProvider;
            }

            IMutableObjects mutableObjects = getMutablesFunc(this._dataflowPrincipal);

            var immutableObj = mutableObjects.ImmutableObjects;
            immutableObj.Header = SettingsManager.Header;

            return new StreamController<TWriter>(this._responseGenerator.GenerateResponseFunction(immutableObj));
        }

        #endregion
    }
}