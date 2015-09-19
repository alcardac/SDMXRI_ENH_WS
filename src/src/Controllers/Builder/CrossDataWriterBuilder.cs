// -----------------------------------------------------------------------
// <copyright file="CrossDataWriterBuilder.cs" company="EUROSTAT">
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
    using System.IO;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Engine;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    ///     The cross data writer builder.
    /// </summary>
    public class CrossDataWriterBuilder : IWriterBuilder<ICrossSectionalWriterEngine, XmlWriter>, IWriterBuilder<ICrossSectionalWriterEngine, Stream>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="ICrossSectionalWriterEngine" />.
        /// </returns>
        public ICrossSectionalWriterEngine Build(Stream writer, Queue<Action> actions)
        {
            return this.Build(XmlWriter.Create(writer), actions);
        }

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="ICrossSectionalWriterEngine" />.
        /// </returns>
        public ICrossSectionalWriterEngine Build(XmlWriter writer, Queue<Action> actions)
        {
            return new DelayedCrossWriterEngine(actions, new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)));
        }

        #endregion
    }
}