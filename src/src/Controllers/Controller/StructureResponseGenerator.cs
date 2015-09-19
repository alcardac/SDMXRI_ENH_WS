// -----------------------------------------------------------------------
// <copyright file="StructureResponseGenerator.cs" company="EUROSTAT">
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
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Builder;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;

    /// <summary>
    ///     The structure response generator.
    /// </summary>
    public class StructureResponseGenerator : IResponseGenerator<XmlWriter, ISdmxObjects>
    {
        #region Fields

        /// <summary>
        ///     The _sdmx structure format.
        /// </summary>
        private readonly SdmxStructureFormat _sdmxStructureFormat;

        /// <summary>
        ///     The _structure manager builder.
        /// </summary>
        private readonly IWriterBuilder<IStructureWriterManager, XmlWriter> _structureManagerBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureResponseGenerator"/> class.
        /// </summary>
        /// <param name="structureManagerBuilder">
        /// The structure manager builder.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        public StructureResponseGenerator(IWriterBuilder<IStructureWriterManager, XmlWriter> structureManagerBuilder, StructureOutputFormatEnumType format)
        {
            this._structureManagerBuilder = structureManagerBuilder;
            this._sdmxStructureFormat = new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(format));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Generates the response function.
        /// </summary>
        /// <param name="query">
        /// </param>
        /// <returns>
        /// The <see cref="Action"/> that will write the response.
        /// </returns>
        public Action<XmlWriter, Queue<Action>> GenerateResponseFunction(ISdmxObjects query)
        {
            return (writer,actions) => this.StreamTo(query, writer, actions);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The stream to.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <param name="writer">
        ///     The writer.
        /// </param>
        /// <param name="actions"></param>
        private void StreamTo(ISdmxObjects query, XmlWriter writer, Queue<Action> actions)
        {
            IStructureWriterManager structureWritingManager = this._structureManagerBuilder.Build(writer, actions);

            structureWritingManager.WriteStructures(query, this._sdmxStructureFormat, null);
        }

        #endregion
    }
}