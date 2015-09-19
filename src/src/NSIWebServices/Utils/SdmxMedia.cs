// -----------------------------------------------------------------------
// <copyright file="SdmxMedia.cs" company="EUROSTAT">
//   Date Created : 2013-10-07
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
namespace Estat.Sri.Ws.Rest.Utils
{
    /// <summary>
    ///     MIME types
    /// </summary>
    public static class SdmxMedia
    {
        #region Static Fields

        /// <summary>
        /// The application xml.
        /// </summary>
        public const string ApplicationXml = "application/xml";

        /// <summary>
        /// The Text xml.
        /// </summary>
        public const string TextXml = "text/xml";

        /// <summary>
        /// The compact data.
        /// </summary>
        public const string CompactData = "application/vnd.sdmx.compactdata+xml";

        /// <summary>
        /// The cross sectional data.
        /// </summary>
        public const string CrossSectionalData = "application/vnd.sdmx.crosssectionaldata+xml";

        /// <summary>
        /// The CSV data.
        /// </summary>
        public const string CsvData = "text/csv";

        /// <summary>
        /// The EDI data.
        /// </summary>
        public const string EdiData = "application/vnd.sdmx.edidata";

        /// <summary>
        /// The EDI structure.
        /// </summary>
        public const string EdiStructure = "application/vnd.sdmx.edistructure";

        /// <summary>
        /// The generic data.
        /// </summary>
        public const string GenericData = "application/vnd.sdmx.genericdata+xml";

        /// <summary>
        /// The structure.
        /// </summary>
        public const string Structure = "application/vnd.sdmx.structure+xml";

        /// <summary>
        /// The structure specific data.
        /// </summary>
        public const string StructureSpecificData = "application/vnd.sdmx.structurespecificdata+xml";

        #endregion
    }
}