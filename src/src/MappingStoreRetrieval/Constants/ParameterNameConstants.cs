// -----------------------------------------------------------------------
// <copyright file="ParameterNameConstants.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
    /// <summary>
    /// This class contains common parameter names
    /// </summary>
    internal static class ParameterNameConstants
    {
        #region Constants and Fields

        /// <summary>
        /// Agency prepared statement named parameter
        /// </summary>
        public const string AgencyParameter = "Agency";

        /// <summary>
        /// Prefix for code prepared statement named parameter
        /// </summary>
        public const string CodeParameterName = "code";

        /// <summary>
        /// ID/SysID prepared statement named parameter
        /// </summary>
        public const string IdParameter = "Id";

        /// <summary>
        /// Dataflow ID prepared statement named parameter
        /// </summary>
        public const string DataflowIdParameter = "DfId";

        /// <summary>
        /// Concept ID prepared statement named parameter
        /// </summary>
        public const string ConceptIdParameter = "cid";

        /// <summary>
        /// Transcoding ID prepared statement named parameter
        /// </summary>
        public const string TranscodingId = "trid";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter = "Version";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter1 = "Version1";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter2 = "Version2";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter3 = "Version3";

        /// <summary>
        /// Production prepared statement named parameter
        /// </summary>
        public const string ProductionParameter = "Production";

        #endregion
    }
}