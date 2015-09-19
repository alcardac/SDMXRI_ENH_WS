// -----------------------------------------------------------------------
// <copyright file="ContentConstraintConstant.cs" company="EUROSTAT">
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
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The concept scheme and concept tables constant.
    /// </summary>
    internal static class ContentConstraintConstant
    {
        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.ContentConstraint) { Table = "CONTENT_CONSTRAINT", PrimaryKey = "CONT_CONS_ID" };

        /// <summary>
        /// Recupero dati Constraint Attachment + Recupero dati Release Calendar
        /// </summary>
        public const string SqlConsInfo = "SELECT B.ID,B.AGENCY, " +
                                                    "'VERSION' = CAST(B.VERSION1 AS VARCHAR(2))  +'.'+  CAST(B.VERSION2 AS VARCHAR(2)), " +
                                                    "'ARTEFACT_TYPE' = CASE " +
                                                    "                    WHEN C.DSD_ID IS NOT NULL THEN 'Dsd'  " +
                                                    "                    WHEN D.DF_ID IS NOT NULL THEN 'Dataflow'  " +
                                                    "                END, " +
                                                    "E.ACTUAL_DATA,E.PERIODICITY,E.OFFSET,E.TOLERANCE " +
                                            "FROM CONTENT_CONSTRAINT_ATTACHMENT A " +
                                            "    INNER JOIN ARTEFACT B ON " +
                                            "        A.ART_ID = B.ART_ID " +
                                            "    LEFT OUTER JOIN DSD C ON	 " +
                                            "        B.ART_ID = C.DSD_ID " +
                                            "    LEFT OUTER JOIN DATAFLOW D ON " +
                                            "       B.ART_ID = D.DF_ID " +
                                            "    INNER JOIN CONTENT_CONSTRAINT E ON " +
                                            "        A.CONT_CONS_ID = E.CONT_CONS_ID " +
                                            "WHERE A.CONT_CONS_ID = @Id ";

        /// <summary>
        /// Recupero dati CubeRegion
        /// </summary>
        public const string SqlConsItem = "SELECT 'CUBE_REGION_INCLUDE'= A.INCLUDE, " +
                                                   "'CUBE_REGION_KEY_VALUE_INCLUDE'= B.INCLUDE, " +
                                                   "'CUBE_REGION_VALUE_INCLUDE'= C.INCLUDE, " +
                                                   " B.CUBE_REGION_KEY_VALUE_ID, " +
                                                   " B.MEMBER_ID, B.COMPONENT_TYPE, C.MEMBER_VALUE " +
                                            "FROM CUBE_REGION A " +
                                            "    INNER JOIN CUBE_REGION_KEY_VALUE B ON " +
                                            "        A.CUBE_REGION_ID = B.CUBE_REGION_ID " +
                                            "    INNER JOIN CUBE_REGION_VALUE C ON " +
                                            "        B.CUBE_REGION_KEY_VALUE_ID = C.CUBE_REGION_KEY_VALUE_ID " +
                                            "WHERE A.CONT_CONS_ID = @Id " +
                                            "ORDER BY CUBE_REGION_KEY_VALUE_ID ";


        /// <summary>
        /// Gets the table info.
        /// </summary>
        public static TableInfo TableInfo
        {
            get
            {
                return _tableInfo;
            }
        }
    }
}