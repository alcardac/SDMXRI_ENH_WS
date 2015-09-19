// -----------------------------------------------------------------------
// <copyright file="AuthDataController.cs" company="EUROSTAT">
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
    using System.Globalization;

    using Estat.Nsi.AuthModule;
    using Estat.Nsi.DataRetriever.Properties;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The authorization decorator data controller.
    /// </summary>
    /// <typeparam name="TQuery">
    /// The sub type of <see cref="IBaseDataQuery"/>
    /// </typeparam>
    /// <typeparam name="TWriter">
    /// The writer type.
    /// </typeparam>
    public class AuthDataController<TQuery, TWriter> : AbstractDataControllerDecorator<TQuery, TWriter>
        where TQuery : IBaseDataQuery
    {
        /// <summary>
        ///     The dataflow principal
        /// </summary>
        private readonly DataflowPrincipal _principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthDataController{TQuery, TWriter}"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="principal">The principal.</param>
        public AuthDataController(IController<TQuery, TWriter> controller, DataflowPrincipal principal) : base(controller)
        {
            this._principal = principal;
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
            this.Authorize(input.Dataflow);
            return base.ParseRequest(input);
        }

        /// <summary>
        /// Try to authorize using the dataflow from <see cref="IDataflowObject"/>.
        ///     It uses and checks the <see cref="_principal"/> if there is an authorized user.
        /// </summary>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <exception cref="SdmxUnauthorisedException">
        /// Not authorized
        /// </exception>
        protected void Authorize(IDataflowObject dataflow)
        {
            if (this._principal != null)
            {
                IMaintainableRefObject dataflowRefBean = this._principal.GetAllowedDataflow(dataflow);

                if (dataflowRefBean == null)
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.NoMappingForDataflowFormat1, dataflow.Id);
                    throw new SdmxUnauthorisedException(errorMessage);
                }
            }
        }
    }
}