// -----------------------------------------------------------------------
// <copyright file="SdmxQueryTimeVO.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// A Value Object used when transcoding SDMX Query Time period 
    /// to Dissemination database time period to store SDMX Query Time element
    /// year and periods
    /// </summary>
    public class SdmxQueryTimeVO
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the period part of EndTime Element e.g. the "1" from 2003-Q1
        /// </summary>
        public int EndPeriod { get; set; }

        /// <summary>
        /// Gets or sets the year part of EndTime Element e.g. the "2003" from 2003-Q1
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the SDMX Query Time.EndTime as a period. e.g. "2003" hasn't but "2004-01" has
        /// </summary>        
        public bool HasEndPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  the SDMX Query Time.StartTime has a period. e.g. "2003" hasn't but "2004-01" has
        /// </summary>
        public bool HasStartPeriod { get; set; }

        /// <summary>
        /// Gets or sets the period part of StartTime Element e.g. the "1" from 2003-Q1
        /// </summary>
        public int StartPeriod { get; set; }

        /// <summary>
        /// Gets or sets the year part of StartTime Element e.g. the "2003" from 2003-Q1
        /// </summary>
        public int StartYear { get; set; }

        #endregion
    }
}