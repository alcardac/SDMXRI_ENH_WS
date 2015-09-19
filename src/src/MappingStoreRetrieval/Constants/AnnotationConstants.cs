// -----------------------------------------------------------------------
// <copyright file="AnnotationConstants.cs" company="EUROSTAT">
//   Date Created : 2014-11-05
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
    /// The annotation constants.
    /// </summary>
    internal static class AnnotationConstants
    {
        /// <summary>
        /// The annotation format. It takes 3 parameters:
        /// <para></para>
        /// 0. Primary key field of SDMX annotate-able object table. e.g. DSD, DSD_CODE
        /// <para></para>
        /// 1. The *_ANNOTATION table. It can be one of the <see cref="ItemAnnotationTable"/>, <see cref="ArtefactAnnotationTable"/>, <see cref="DsdGroupAnnotationTable"/>, <see cref="ComponentAnnotationTable"/>
        /// <para></para>
        /// 2. The SDMX annotate-able object table. e.g. DSD, DSD_CODE, CODELIST
        /// <para></para>
        /// 3. Foreign key field of the *_ANNOTATION table specified in parameter 1.
        /// <para></para>
        /// </summary>
        public const string AnnotationQuery = "select T.{0} as SYSID, AN.ANN_ID, AN.ID, AN.TITLE, AN.TYPE, AN.URL, AT.LANGUAGE, AT.TEXT from ANNOTATION AN LEFT OUTER JOIN ANNOTATION_TEXT AT ON AN.ANN_ID = AT.ANN_ID INNER JOIN {1} AA ON AA.ANN_ID = AN.ANN_ID INNER JOIN {2} T ON T.{0} = AA.{3}";

        /// <summary>
        /// The item annotation table
        /// </summary>
        public const string ItemAnnotationTable = "ITEM_ANNOTATION";
        
        /// <summary>
        /// The artefact annotation table
        /// </summary>
        public const string ArtefactAnnotationTable = "ARTEFACT_ANNOTATION";
        
        /// <summary>
        /// The component annotation table
        /// </summary>
        public const string ComponentAnnotationTable = "COMPONENT_ANNOTATION";

        /// <summary>
        /// The DSD group annotation table
        /// </summary>
        public const string DsdGroupAnnotationTable = "GROUP_ANNOTATION";
    }
}