// -----------------------------------------------------------------------
// <copyright file="RequestLogDataController.cs" company="EUROSTAT">
//   Date Created : 2014-11-03
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
    using Estat.Sri.Ws.Controllers.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    /// The request log data controller.
    /// </summary>
    /// <typeparam name="TQuery">
    /// The sub type of <see cref="IBaseDataQuery"/>
    /// </typeparam>
    /// <typeparam name="TWriter">
    /// The writer type.
    /// </typeparam>
    public class RequestLogDataController<TQuery, TWriter> : AbstractDataControllerDecorator<TQuery, TWriter>
        where TQuery : IBaseDataQuery
    {
        /// <summary>
        /// The _dataflow log manager
        /// </summary>
        private readonly IDataflowLogManager _dataflowLogManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLogDataController{TQuery, TWriter}"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="dataflowLogManager">The dataflow log manager.</param>
        public RequestLogDataController(IController<TQuery, TWriter> controller, IDataflowLogManager dataflowLogManager)
            : base(controller)
        {
            this._dataflowLogManager = dataflowLogManager;
        }

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public override IStreamController<TWriter> ParseRequest(TQuery input)
        {
            this._dataflowLogManager.Log(input.Dataflow);
            return base.ParseRequest(input);
        }
    }
}