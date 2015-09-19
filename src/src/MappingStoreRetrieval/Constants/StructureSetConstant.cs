// -----------------------------------------------------------------------
// <copyright file="StructureSetConstant.cs" company="EUROSTAT">
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
    internal static class StructureSetConstant
    {
        /// <summary>
        /// Gets the item order by.
        /// </summary>
        public const string ItemOrderBy = " ORDER BY T.SS_ID";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.StructureSet) { Table = "STRUCTURE_SET", PrimaryKey = "SS_ID" };

        /// <summary>
        /// The CodeListMap Item Table Info
        /// </summary>
        private static readonly ItemTableInfo _clmItemTableInfo = new ItemTableInfo(SdmxStructureEnumType.CodeListMap) { Table = "CODELIST_MAP", PrimaryKey = "CLM_ID", ForeignKey = "SS_ID" };

        /// <summary>
        /// The StructureMap Item Table Info
        /// </summary>
        private static readonly ItemTableInfo _smItemTableInfo = new ItemTableInfo(SdmxStructureEnumType.StructureMap) { Table = "STRUCTURE_MAP", PrimaryKey = "SM_ID", ForeignKey = "SS_ID" };

        #region "CodeListMap"

        /// <summary>
        /// StructureMap Info 
        /// </summary>
        public const string SqlSMInfo = "SELECT B.ITEM_ID,ID,TYPE,TEXT,LANGUAGE " +
                                            "FROM dbo.STRUCTURE_MAP A  " +
                                            "    INNER JOIN dbo.ITEM B ON " +
                                            "        A.SM_ID = B.ITEM_ID	 " +
                                            "     LEFT OUTER JOIN dbo.LOCALISED_STRING C ON  " +
                                            "        A.SM_ID = C.ITEM_ID  " +
                                            "WHERE SS_ID = @Id";


        /// <summary>
        /// StructureMap Structure Reference
        /// </summary>
        public const string SqlSMReference = "SELECT " +
                                               "     'S_ID' = B.ID, " +
                                               "     'S_AGENCY' = B.AGENCY, " +
                                               "     'S_VERSION' = CAST(B.VERSION1 AS VARCHAR(2)) +'.'+ CAST(B.VERSION2 AS VARCHAR(2)), " +
                                               "	 'S_ARTEFACT_TYPE' = CASE WHEN D.DSD_ID IS NOT NULL THEN 'Dsd' ELSE 'Dataflow' END, " +
                                               "     'T_ID' = C.ID, " +
                                               "     'T_AGENCY' = C.AGENCY, " +
                                               "     'T_VERSION' = CAST(C.VERSION1 AS VARCHAR(2)) +'.'+ CAST(C.VERSION2 AS VARCHAR(2)), " +
                                               "	 'T_ARTEFACT_TYPE' = CASE WHEN E.DSD_ID IS NOT NULL THEN 'Dsd' ELSE 'Dataflow' END " +
                                               " FROM dbo.STRUCTURE_MAP A " +
                                               "     INNER JOIN dbo.ARTEFACT B ON " +
                                               "         A.SOURCE_STR_ID = B.ART_ID " +
                                               "     INNER JOIN dbo.ARTEFACT C ON " +
                                               "         A.TARGET_STR_ID = C.ART_ID " +
                                               "     LEFT OUTER JOIN dbo.DSD D ON  " +
                                               "        d.DSD_ID = a.SOURCE_STR_ID  " +
                                               "     LEFT OUTER JOIN dbo.DSD e ON  " +
                                               "        e.DSD_ID = a.TARGET_STR_ID  " +
                                               " WHERE A.SM_ID = @Id";


        /// <summary>
        /// StructureMap Item
        /// </summary>
        public const string SqlSMItem = " SELECT   " +
                                        "       'S_ID' = C.ID,   " +
                                        "       'T_ID' = D.ID   " +
                                        "   FROM dbo.STRUCTURE_MAP A   " +
                                        "       INNER JOIN COMPONENT_MAP B ON   " +
                                        "           B.SM_ID = A.SM_ID   " +
                                        "       INNER JOIN dbo.COMPONENT C ON   " +
                                        "           B.SOURCE_COMP_ID = C.COMP_ID   " +
                                        "       INNER JOIN dbo.COMPONENT D ON   " +
                                        "           B.TARGET_COMP_ID = D.COMP_ID  " +
                                        "   WHERE A.SM_ID = @Id ";


        // 1. Info CodeListMap(id, name, description)
        /// <summary>
        /// CodeListMap Info 
        /// </summary>
        public const string SqlCLMInfo = "SELECT B.ITEM_ID,ID,TYPE,TEXT,LANGUAGE " +
                                            "FROM dbo.CODELIST_MAP A " +
                                            "    INNER JOIN dbo.ITEM B ON " +
                                            "        A.CLM_ID = B.ITEM_ID	  " +
                                            "	 LEFT OUTER JOIN dbo.LOCALISED_STRING C ON " +
                                            "        A.CLM_ID = C.ITEM_ID " +
                                            "WHERE A.SS_ID = @Id";


        /// 2. SourceRef.CrossReferenceImpl		
        /// <summary>
        /// CodeListMap CL Reference
        /// </summary>
        public const string SqlCLMReference = "SELECT " +
                                               "     'S_ID' = B.ID, " +
                                               "     'S_AGENCY' = B.AGENCY, " +
                                               "     'S_VERSION' = CAST(B.VERSION1 AS VARCHAR(2)) +'.'+ CAST(B.VERSION2 AS VARCHAR(2)), " +
                                               "     'T_ID' = C.ID, " +
                                               "     'T_AGENCY' = C.AGENCY, " +
                                               "     'T_VERSION' = CAST(C.VERSION1 AS VARCHAR(2)) +'.'+ CAST(C.VERSION2 AS VARCHAR(2)) " +
                                               " FROM dbo.CODELIST_MAP A " +
                                               "     INNER JOIN dbo.ARTEFACT B ON " +
                                               "         A.SOURCE_CL_ID = B.ART_ID " +
                                               "     INNER JOIN dbo.ARTEFACT C ON " +
                                               "         A.TARGET_CL_ID = C.ART_ID " +
                                               " WHERE A.CLM_ID = @Id";
        //" WHERE SS_ID = @Id";

        /// <summary>
        /// CodeListMap Item
        /// </summary>
        public const string SqlCLMItem = "SELECT " +
                                         "       'S_ID' = C.ID, " +
                                         "       'T_ID' = D.ID " +
                                         "   FROM CODELIST_MAP A " +
                                         "       INNER JOIN CODE_MAP B ON " +
                                         "           B.CLM_ID = A.CLM_ID " +
                                         "       INNER JOIN ITEM C ON " +
                                         "           B.SOURCE_LCD_ID = C.ITEM_ID " +
                                         "       INNER JOIN ITEM D ON " +
                                         "           B.TARGET_LCD_ID = D.ITEM_ID " +
                                         "   WHERE A.CLM_ID = @Id ";
        //"   WHERE SS_ID = @Id ";


        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the table info.
        /// </summary>
        public static TableInfo TableInfo
        {
            get
            {
                return _tableInfo;
            }
        }

        /// <summary>
        ///     Gets The CodeListMap Item Table Info
        /// </summary>
        public static ItemTableInfo CLMItemTableInfo
        {
            get
            {
                return _clmItemTableInfo;
            }
        }

        /// <summary>
        ///     Gets The StructureMap Item Table Info
        /// </summary>
        public static ItemTableInfo SMItemTableInfo
        {
            get
            {
                return _smItemTableInfo;
            }
        }

        #endregion

    }
}