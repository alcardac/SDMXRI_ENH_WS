// -----------------------------------------------------------------------
// <copyright file="StructureBuilder.cs" company="EUROSTAT">
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
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxStructureMutableParser.Factory;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Structureparser.Factory;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;

    /// <summary>
    ///     The structure builder.
    /// </summary>
    public class StructureBuilder : IWriterBuilder<IStructureWriterManager, XmlWriter>
    {
        #region Fields

        /// <summary>
        ///     The _endpoint.
        /// </summary>
        private readonly WebServiceEndpoint _endpoint;

        /// <summary>
        ///     The _schema.
        /// </summary>
        private readonly SdmxSchema _schema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureBuilder"/> class.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public StructureBuilder(WebServiceEndpoint endpoint, SdmxSchema schema)
        {
            this._endpoint = endpoint;
            this._schema = schema;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="IStructureWriterManager" />.
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException">Unsupported format.</exception>
        public IStructureWriterManager Build(XmlWriter writer, Queue<Action> actions)
        {
            actions.RunAll();
            IStructureWriterManager structureWritingManager;
            switch (this._schema.EnumType)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    structureWritingManager = this._endpoint == WebServiceEndpoint.EstatEndpoint
                                                  ? new StructureWriterManager(new SdmxStructureWriterV2Factory(writer))
                                                  : new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    structureWritingManager = new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    break;
                default:
                    throw new SdmxSemmanticException(string.Format("Unsupported format {0}", this._schema));
            }

            return structureWritingManager;
        }

        #endregion
    }
}