// -----------------------------------------------------------------------
// <copyright file="PeriodCodelist.cs" company="EUROSTAT">
//   Date Created : 2013-07-30
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
namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The period codelist constants.
    /// </summary>
    public static class PeriodCodelist
    {
        #region Constants

        /// <summary>
        /// The agency.
        /// </summary>
        public const string Agency = "MA";

        /// <summary>
        /// The version.
        /// </summary>
        public const string Version = "1.0";

        #endregion

        #region Static Fields

        /// <summary>
        /// The _period codelist id.
        /// </summary>
        private static readonly IDictionary<string, PeriodObject> _periodCodelistIdMap;

        /// <summary>
        /// The ordered codelist ids.
        /// </summary>
        private static readonly string[] _supportedPeriodFrequencies;

        /// <summary>
        /// The version particles.
        /// </summary>
        private static readonly IList<int?> _versionParticles = new int?[] { 1, 0, null };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="PeriodCodelist"/> class.
        /// </summary>
        static PeriodCodelist()
        {
            _supportedPeriodFrequencies = new[] { TimeFormat.GetFromEnum(TimeFormatEnumType.Month).FrequencyCode, TimeFormat.GetFromEnum(TimeFormatEnumType.QuarterOfYear).FrequencyCode, TimeFormat.GetFromEnum(TimeFormatEnumType.HalfOfYear).FrequencyCode };
            var periodLengths = new[] { 12, 4, 2 };
            var periodFormats = new[] { "00", "\\Q0", "\\B0" };
            _periodCodelistIdMap = new Dictionary<string, PeriodObject>(StringComparer.Ordinal);
            for (int i = 0; i < _supportedPeriodFrequencies.Length; i++)
            {
                var freq = _supportedPeriodFrequencies[i];
                var id = string.Format(CultureInfo.InvariantCulture, "SDMX_{0}_PERIODS", freq);

                _periodCodelistIdMap.Add(freq, new PeriodObject(periodLengths[i], periodFormats[i], id));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the period codelist id.
        /// </summary>
        public static IDictionary<string, PeriodObject> PeriodCodelistIdMap
        {
            get
            {
                return _periodCodelistIdMap;
            }
        }

        /// <summary>
        /// Gets the ordered codelist ids.
        /// </summary>
        public static IList<string> SupportedPeriodFrequencies
        {
            get
            {
                return _supportedPeriodFrequencies;
            }
        }

        /// <summary>
        /// Gets the version particles.
        /// </summary>
        public static IList<int?> VersionParticles
        {
            get
            {
                return _versionParticles;
            }
        }

        #endregion
    }
}