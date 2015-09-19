// -----------------------------------------------------------------------
// <copyright file="StructureRequestRestController.cs" company="EUROSTAT">
//   Date Created : 2013-10-10
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
    using System.Linq;

    using Estat.Nsi.AuthModule;
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    /// <summary>
    /// The structure request rest controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the output writer
    /// </typeparam>
    public class StructureRequestRestController<TWriter> : QueryStructureController<TWriter>, IController<IRestStructureQuery, TWriter>
    {
        #region Fields

        /// <summary>
        ///     The AUTH structure search manager
        /// </summary>
        private readonly IAuthMutableStructureSearchManager _authStructureSearchManager;

        /// <summary>
        ///     The structure search manager
        /// </summary>
        private readonly IMutableStructureSearchManager _structureSearchManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRequestRestController{TWriter}"/> class.
        /// </summary>
        /// <param name="responseGenerator">
        /// The response generator.
        /// </param>
        /// <param name="structureSearchManager">
        /// The structure Search Manager.
        /// </param>
        /// <param name="authStructureSearchManager">
        /// The AUTH Structure Search Manager.
        /// </param>
        /// <param name="dataflowPrincipal">
        /// The dataflow principal.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted with query used
        /// </exception>
        public StructureRequestRestController(
            IResponseGenerator<TWriter, ISdmxObjects> responseGenerator, 
            IMutableStructureSearchManager structureSearchManager, 
            IAuthMutableStructureSearchManager authStructureSearchManager, 
            DataflowPrincipal dataflowPrincipal)
            : base(responseGenerator, dataflowPrincipal)
        {
            this._structureSearchManager = structureSearchManager;
            this._authStructureSearchManager = authStructureSearchManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(IRestStructureQuery input)
        {
            return this.ParseRequestPrivate(principal => this.GetMutableObjectsRest(input, principal));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the mutable objects from rest.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="dataflowPrincipal">
        /// The dataflow principal.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        private IMutableObjects GetMutableObjectsRest(IRestStructureQuery input, DataflowPrincipal dataflowPrincipal)
        {
            IMutableObjects mutableObjects = dataflowPrincipal != null
                                                 ? this._authStructureSearchManager.GetMaintainables(input, dataflowPrincipal.AllowedDataflows.ToList())
                                                 : this._structureSearchManager.GetMaintainables(input);

            return mutableObjects;
        }

        #endregion
    }
}