// -----------------------------------------------------------------------
// <copyright file="AdvancedDataController.cs" company="EUROSTAT">
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

    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Properties;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The advanced data controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the writer
    /// </typeparam>
    public class AdvancedDataController<TWriter> : AbstractDataControllerDecorator<IComplexDataQuery, TWriter>,
                                                   IController<XmlNode, TWriter>, 
                                                   IController<IReadableDataLocation, TWriter>, 
                                                   IController<Message, TWriter>
    {
        #region Fields

        /// <summary>
        ///     The _root node
        /// </summary>
        private readonly XmlQualifiedName _rootNode;

        /// <summary>
        ///     The _sdmx retrieval manager.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _sdmxRetrievalManager;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedDataController{TWriter}" /> class.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="decoratedController">The decorated Controller.</param>
        /// <param name="sdmxRetrievalManager">The sdmx Retrieval Manager.</param>
        /// <exception cref="System.ArgumentNullException"><see cref="_sdmxRetrievalManager"/> is null.</exception>
        /// <exception cref="SdmxSemmanticException">Operation not accepted with query used</exception>
        public AdvancedDataController(SoapOperation operation, IController<IComplexDataQuery, TWriter> decoratedController, ISdmxObjectRetrievalManager sdmxRetrievalManager)
            : base(decoratedController)
        {
            if (sdmxRetrievalManager == null)
            {
                throw new ArgumentNullException("sdmxRetrievalManager");
            }

            this._sdmxRetrievalManager = sdmxRetrievalManager;
            this._rootNode = operation.GetQueryRootElementV21();
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
            using (IReadableDataLocation xmlReadable = new XmlDocReadableDataLocation(input))
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
            IDataQueryParseManager dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwoPointOne);
            var dataQuery = dataQueryParseManager.BuildComplexDataQuery(input, this._sdmxRetrievalManager).FirstOrDefault();
            if (dataQuery == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

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
        public IStreamController<TWriter> ParseRequest(Message input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation(this._rootNode))
            {
                return this.ParseRequest(xmlReadable);
            }
        }

        #endregion
    }
}