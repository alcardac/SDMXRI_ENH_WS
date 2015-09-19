// -----------------------------------------------------------------------
// <copyright file="SdmxQueryPeriod.cs" company="EUROSTAT">
//   Date Created : 2013-10-30
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
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    /// <summary>
    /// The SDMX query period.
    /// </summary>
    public class SdmxQueryPeriod
    {
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public int Period { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [has period].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has period]; otherwise, <c>false</c>.
        /// </value>
        public bool HasPeriod { get; set; }
    }
}