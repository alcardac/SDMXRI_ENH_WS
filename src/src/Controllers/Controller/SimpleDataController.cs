// -----------------------------------------------------------------------
// <copyright file="SimpleDataController.cs" company="EUROSTAT">
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
    using System;
    using System.Linq;
    using System.ServiceModel.Channels;
    using System.Xml;
    using System.Xml.Linq;

    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Properties;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;

    /// <summary>
    /// The SDMX v20 SOAP controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public class SimpleDataController<TWriter> : AbstractDataControllerDecorator<IDataQuery, TWriter>, 
                                                 IController<XmlNode, TWriter>, 
                                                 IController<IReadableDataLocation, TWriter>, 
                                                 IController<IRestDataQuery, TWriter>, 
                                                 IController<XElement, TWriter>, 
                                                 IController<Message, TWriter>
    {
        #region Fields

        /// <summary>
        ///     The _log.
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(SimpleDataController<TWriter>));

        /// <summary>
        ///     The _sdmx retrieval manager.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _sdmxRetrievalManager;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataController{TWriter}" /> class.
        /// </summary>
        /// <param name="decoratedController">The decorated Controller.</param>
        /// <param name="sdmxRetrievalManager">The sdmx Retrieval Manager.</param>
        /// <exception cref="System.ArgumentNullException"><see cref="_sdmxRetrievalManager"/> is null</exception>
        public SimpleDataController(IController<IDataQuery, TWriter> decoratedController, ISdmxObjectRetrievalManager sdmxRetrievalManager)
            : base(decoratedController)
        {
            if (sdmxRetrievalManager == null)
            {
                throw new ArgumentNullException("sdmxRetrievalManager");
            }

            this._sdmxRetrievalManager = sdmxRetrievalManager;
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
        public IStreamController<TWriter> ParseRequest(IReadableDataLocation input)
        {
            var dataQuery = this.GetDataQueryFromStream(input);
            return this.ParseRequest(dataQuery);
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
        public override IStreamController<TWriter> ParseRequest(IDataQuery input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            return base.ParseRequest(input);
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
        public IStreamController<TWriter> ParseRequest(IRestDataQuery input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            IDataQuery dataQuery = new DataQueryImpl(input, this._sdmxRetrievalManager);
            this._log.DebugFormat("Converted REST data query '{0}' to IDataQuery {1}", input, dataQuery);
            return this.ParseRequest(dataQuery);
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

            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation(new XmlQualifiedName(SdmxConstants.QueryMessageRootNode, SdmxConstants.MessageNs20)))
            {
                return this.ParseRequest(xmlReadable);
            }
        }

        #endregion

        /// <summary>
        /// Gets the data query from stream.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException">
        /// Operation not accepted
        /// </exception>
        /// <returns>
        /// The <see cref="IDataQuery"/>.
        /// </returns>
        protected IDataQuery GetDataQueryFromStream(IReadableDataLocation input)
        {
            IDataQueryParseManager dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwo);
            var dataQuery = dataQueryParseManager.BuildDataQuery(input, this._sdmxRetrievalManager).FirstOrDefault();
            if (dataQuery == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            return dataQuery;
        }
    }
}