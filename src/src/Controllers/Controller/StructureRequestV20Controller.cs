// -----------------------------------------------------------------------
// <copyright file="StructureRequestV20Controller.cs" company="EUROSTAT">
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
    using System.ServiceModel.Channels;
    using System.Xml;
    using System.Xml.Linq;

    using Estat.Nsi.AuthModule;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.CustomRequests.Builder;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Properties;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.Query;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Structureparser.Workspace;

    /// <summary>
    /// The structure request SDMX v20 controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the output writer
    /// </typeparam>
    public class StructureRequestV20Controller<TWriter> : QueryStructureController<TWriter>, 
                                                          IController<XmlNode, TWriter>, 
                                                          IController<IReadableDataLocation, TWriter>, 
                                                          IController<XElement, TWriter>, 
                                                          IController<Message, TWriter>
    {
        #region Fields

        /// <summary>
        ///     The AUTH structure search manager.
        /// </summary>
        private readonly IAuthMutableStructureSearchManager _authStructureSearchManager;

        /// <summary>
        ///     The manager.
        /// </summary>
        private readonly IQueryParsingManager _manager;

        /// <summary>
        ///     The _structure search manager.
        /// </summary>
        private readonly IMutableStructureSearchManager _structureSearchManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRequestV20Controller{TWriter}"/> class.
        /// </summary>
        /// <param name="responseGenerator">
        /// The response generator.
        /// </param>
        /// <param name="structureSearchManager">
        /// The structure search manager.
        /// </param>
        /// <param name="authStructureSearchManager">
        /// The authentication structure search manager.
        /// </param>
        /// <param name="dataflowPrincipal">
        /// The dataflow principal.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted with query used
        /// </exception>
        public StructureRequestV20Controller(
            IResponseGenerator<TWriter, ISdmxObjects> responseGenerator, 
            IMutableStructureSearchManager structureSearchManager, 
            IAuthMutableStructureSearchManager authStructureSearchManager, 
            DataflowPrincipal dataflowPrincipal)
            : base(responseGenerator, dataflowPrincipal)
        {
            this._structureSearchManager = structureSearchManager;
            this._authStructureSearchManager = authStructureSearchManager;
            SdmxSchema sdmxSchemaV20 = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo);
            this._manager = new QueryParsingManager(sdmxSchemaV20.EnumType, new QueryBuilder(null, new ConstrainQueryBuilderV2(), null));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(IReadableDataLocation input)
        {
            return this.ParseRequestPrivate(principal => this.GetMutableObjectsV20(input, principal));
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
        public IStreamController<TWriter> ParseRequest(XmlNode input)
        {
            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation())
            {
                return this.ParseRequest(xmlReadable);
            }
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
        public IStreamController<TWriter> ParseRequest(XElement input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation())
            {
                return this.ParseRequest(xmlReadable);
            }
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
        public IStreamController<TWriter> ParseRequest(Message input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation(new XmlQualifiedName(SdmxConstants.RegistryInterfaceRootNode, SdmxConstants.MessageNs20)))
            {
                return this.ParseRequest(xmlReadable);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the mutable objects V20.
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
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted
        /// </exception>
        private IMutableObjects GetMutableObjectsV20(IReadableDataLocation input, DataflowPrincipal dataflowPrincipal)
        {
            IQueryWorkspace queryWorkspace = this._manager.ParseQueries(input);

            if (queryWorkspace == null)
            {
                // throw new SdmxSemmanticException(Properties.Resources.MissingRegistryOrInvalidSoap);
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            IMutableObjects mutableObjects = dataflowPrincipal != null
                                                 ? this._authStructureSearchManager.RetrieveStructures(
                                                     queryWorkspace.SimpleStructureQueries, 
                                                     queryWorkspace.ResolveReferences, 
                                                     false, 
                                                     dataflowPrincipal.AllowedDataflows.ToList())
                                                 : this._structureSearchManager.RetrieveStructures(queryWorkspace.SimpleStructureQueries, queryWorkspace.ResolveReferences, false);

            return mutableObjects;
        }

        #endregion
    }
}