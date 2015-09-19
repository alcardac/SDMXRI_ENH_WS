// -----------------------------------------------------------------------
// <copyright file="ITimeDimension.cs" company="EUROSTAT">
//   Date Created : 2013-04-10
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
namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Data;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    /// <summary>
    /// A common interface for Time Dimension Transcoding classes
    /// </summary>
    public interface ITimeDimension
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the DSD Component.
        /// </summary>
        ComponentEntity Component { get; set; }

        /// <summary>
        /// Gets or sets the mapping
        /// </summary>
        MappingEntity Mapping { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Query where condition from the SDMX Query Time
        /// </summary>
        /// <param name="dateFrom">The start time</param>
        /// <param name="dateTo">The end time</param>
        /// <param name="frequencyValue">
        /// The frequency value
        /// </param>
        /// <returns>
        /// The string containing SQL Query where condition
        /// </returns>
        string GenerateWhere(ISdmxDate dateFrom, ISdmxDate dateTo, string frequencyValue);

        /// <summary>
        /// Map component.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="frequencyValue">
        /// The frequency value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string MapComponent(IDataReader reader, string frequencyValue);

        #endregion
    }
}