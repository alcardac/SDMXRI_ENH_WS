// -----------------------------------------------------------------------
// <copyright file="DataWriterBuilder.cs" company="EUROSTAT">
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
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.ServiceModel;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Engine;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    ///     The <see cref="IDataWriterEngine" /> engine builder from <see cref="XmlWriter" /> or <see cref="Stream" />
    /// </summary>
    public class DataWriterBuilder : IWriterBuilder<IDataWriterEngine, XmlWriter>, IWriterBuilder<IDataWriterEngine, Stream>
    {
        #region Fields

        /// <summary>
        ///     The _data format.
        /// </summary>
        private readonly BaseDataFormat _dataFormat;

        /// <summary>
        ///     The _sdmx schema.
        /// </summary>
        private readonly SdmxSchema _sdmxSchema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterBuilder"/> class.
        /// </summary>
        /// <param name="dataFormat">
        /// The data Format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The sdmx Schema.
        /// </param>
        public DataWriterBuilder(BaseDataFormat dataFormat, SdmxSchema sdmxSchema)
        {
            this._dataFormat = dataFormat;
            this._sdmxSchema = sdmxSchema;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="IDataWriterEngine" />.
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">Not supported IDataWriterEngine for XmlWriter output</exception>
        public IDataWriterEngine Build(XmlWriter writer, Queue<Action> actions)
        {
            switch (this._dataFormat.EnumType)
            {
                case BaseDataFormatEnumType.Generic:
                    return new DelayedDataWriterEngine(new GenericDataWriterEngine(writer, this._sdmxSchema), actions);
                case BaseDataFormatEnumType.Compact:
                    return new DelayedDataWriterEngine(new CompactDataWriterEngine(writer, this._sdmxSchema), actions);
                default:
                    var message = string.Format(CultureInfo.InvariantCulture, "Not supported IDataWriterEngine for XmlWriter output {0}", this._dataFormat);
                    throw new SdmxNotImplementedException(message);
            }
        }

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">
        /// Not supported IDataWriterEngine for Stream output
        /// </exception>
        /// <returns>
        /// The <see cref="IDataWriterEngine"/>.
        /// </returns>
        public IDataWriterEngine Build(Stream writer, Queue<Action> actions)
        {
            switch (this._dataFormat.EnumType)
            {
                case BaseDataFormatEnumType.Generic:
                case BaseDataFormatEnumType.Compact:
                    return this.Build(XmlWriter.Create(writer), actions);

                    //// case BaseDataFormatEnumType.Edi:
                    ////     // TODO
                    ////      return  GesmesTimeSeriesWriter(writer);
                    ////     break;
                default:
                    var message = string.Format(CultureInfo.InvariantCulture, "Not supported IDataWriterEngine for Stream output {0}", this._dataFormat);
                    throw new SdmxNotImplementedException(message);
            }
        }

        #endregion
    }
}