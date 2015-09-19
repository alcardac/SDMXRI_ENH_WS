// -----------------------------------------------------------------------
// <copyright file="SimpleCrossDataResponseGenerator.cs" company="EUROSTAT">
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
    using System.Collections.Generic;

    using Estat.Sri.Ws.Controllers.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;

    /// <summary>
    /// The simple xml response generator.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the output writer e.g. XmlWriter or Stream
    /// </typeparam>
    public class SimpleCrossDataResponseGenerator<T> : IResponseGenerator<T, IDataQuery>
    {
        #region Fields

        /// <summary>
        ///     The _data writer builder.
        /// </summary>
        private readonly IWriterBuilder<ICrossSectionalWriterEngine, T> _dataWriterBuilder;

        /// <summary>
        ///     The _sdmx data retrieval with writer.
        /// </summary>
        private readonly ISdmxDataRetrievalWithCrossWriter _sdmxDataRetrievalWithWriter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCrossDataResponseGenerator{T}"/> class.
        /// </summary>
        /// <param name="sdmxDataRetrievalWithWriter">
        /// The SDMX data retrieval with writer.
        /// </param>
        /// <param name="dataWriterBuilder">
        /// The data Writer Builder.
        /// </param>
        public SimpleCrossDataResponseGenerator(ISdmxDataRetrievalWithCrossWriter sdmxDataRetrievalWithWriter, IWriterBuilder<ICrossSectionalWriterEngine, T> dataWriterBuilder)
        {
            this._sdmxDataRetrievalWithWriter = sdmxDataRetrievalWithWriter;
            this._dataWriterBuilder = dataWriterBuilder;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Generates the response function.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        /// The <see cref="Action"/> that will write the response.
        /// </returns>
        public Action<T, Queue<Action>> GenerateResponseFunction(IDataQuery query)
        {
            return (writer, action) => this._sdmxDataRetrievalWithWriter.GetData(query, this._dataWriterBuilder.Build(writer, action));
        }

        #endregion
    }
}