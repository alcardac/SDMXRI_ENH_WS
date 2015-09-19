// -----------------------------------------------------------------------
// <copyright file="MappingStoreSqlStatements.cs" company="EUROSTAT">
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
    /// This class holds a collection of SQL Statements
    /// </summary>
    internal static class MappingStoreSqlStatements
    {
        /// <summary>
        /// The SQL Statement that returns MAP_ID, TR_RULE_ID, COMP_ID as COMPONENT and ITEM.ID as CODE (DSD_CODE) and accepts two parameters : the MAP_SET_ID (Int64) and the  the TR_ID (Int64 or null) of the time dimension mapping
        /// </summary>
        public const string TranscodingRulesDsdCodes =
            "SELECT COMPONENT_MAPPING.MAP_ID, TRANSCODING_RULE.TR_RULE_ID, COM_COL_MAPPING_COMPONENT.COMP_ID AS COMPONENT, ITEM.ID AS CODE  "
            + "FROM TRANSCODING_RULE INNER JOIN  "
            +
            "TRANSCODING_RULE_DSD_CODE ON TRANSCODING_RULE.TR_RULE_ID = TRANSCODING_RULE_DSD_CODE.TR_RULE_ID INNER JOIN  "
            + "TRANSCODING ON TRANSCODING_RULE.TR_ID = TRANSCODING.TR_ID INNER JOIN  "
            + "DSD_CODE ON TRANSCODING_RULE_DSD_CODE.CD_ID = DSD_CODE.LCD_ID INNER JOIN  "
            + "ITEM ON DSD_CODE.LCD_ID = ITEM.ITEM_ID INNER JOIN  "
            + "COMPONENT_MAPPING ON TRANSCODING.MAP_ID = COMPONENT_MAPPING.MAP_ID INNER JOIN  "
            + "COM_COL_MAPPING_COMPONENT ON COMPONENT_MAPPING.MAP_ID = COM_COL_MAPPING_COMPONENT.MAP_ID INNER JOIN "
            + "COMPONENT ON COMPONENT.COMP_ID = COM_COL_MAPPING_COMPONENT.COMP_ID and DSD_CODE.CL_ID = COMPONENT.CL_ID "
            + "WHERE COMPONENT_MAPPING.MAP_SET_ID = {0} and (({1} is null) or (TRANSCODING.TR_ID != {1})) "
            + "ORDER BY COMPONENT_MAPPING.MAP_ID,TRANSCODING_RULE_DSD_CODE.TR_RULE_ID, COMPONENT";

        /// <summary>
        /// The SQL Statement that returns MAP_ID, TR_RULE_ID, COLUMN_ID as LOCAL_COLUMN and ITEM.ID as CODE (LOCAL_CODE) and accepts two parameters : the MAP_SET_ID (Int64) and the  the TR_ID (Int64 or null) of the time dimension mapping
        /// </summary>
        public const string TranscodingRulesLocalCodes =
            "SELECT COMPONENT_MAPPING.MAP_ID, TRANSCODING_RULE_LOCAL_CODE.TR_RULE_ID, LOCAL_CODE.COLUMN_ID AS LOCAL_COLUMN, ITEM.ID AS CODE  "
            + "FROM TRANSCODING_RULE INNER JOIN  "
            +
            "TRANSCODING_RULE_LOCAL_CODE ON TRANSCODING_RULE.TR_RULE_ID = TRANSCODING_RULE_LOCAL_CODE.TR_RULE_ID INNER JOIN  "
            + "LOCAL_CODE ON TRANSCODING_RULE_LOCAL_CODE.LCD_ID = LOCAL_CODE.LCD_ID INNER JOIN  "
            + "ITEM ON LOCAL_CODE.LCD_ID = ITEM.ITEM_ID INNER JOIN "
            + "TRANSCODING ON TRANSCODING_RULE.TR_ID = TRANSCODING.TR_ID INNER JOIN "
            + "COMPONENT_MAPPING ON TRANSCODING.MAP_ID = COMPONENT_MAPPING.MAP_ID "
            + "WHERE COMPONENT_MAPPING.MAP_SET_ID = {0} and (({1} is null) or (TRANSCODING.TR_ID != {1})) "
            + "ORDER BY COMPONENT_MAPPING.MAP_ID, TRANSCODING_RULE_LOCAL_CODE.TR_RULE_ID, LOCAL_COLUMN";

        /// <summary>
        /// The SQL Statement that returns TR_RULE_IDITEM_1.ID AS LOCALCODE, ITEM.ID AS DSDCODE, LOCAL_CODE.COLUMN_ID as LOCAL_COLUMN and accepts one parameter : the TR_ID (Int64) of the time dimension mapping
        /// </summary>
        public const string TranscodingRulesTimeDimension =
            "SELECT TRANSCODING_RULE.TR_RULE_ID, ITEM_1.ID AS LOCALCODE, ITEM.ID AS DSDCODE, LOCAL_CODE.COLUMN_ID as LOCAL_COLUMN, CL.ID as CODELIST_ID "
            + "FROM TRANSCODING_RULE INNER JOIN  "
            +
            "TRANSCODING_RULE_DSD_CODE ON TRANSCODING_RULE.TR_RULE_ID = TRANSCODING_RULE_DSD_CODE.TR_RULE_ID INNER JOIN  "
            +
            "TRANSCODING_RULE_LOCAL_CODE ON TRANSCODING_RULE.TR_RULE_ID = TRANSCODING_RULE_LOCAL_CODE.TR_RULE_ID INNER JOIN  "
            + "ITEM ON TRANSCODING_RULE_DSD_CODE.CD_ID = ITEM.ITEM_ID INNER JOIN  "
            + "LOCAL_CODE ON LOCAL_CODE.LCD_ID = TRANSCODING_RULE_LOCAL_CODE.LCD_ID INNER JOIN  "
            + "ITEM ITEM_1 ON LOCAL_CODE.LCD_ID = ITEM_1.ITEM_ID  INNER JOIN "
            + "DSD_CODE DC ON TRANSCODING_RULE_DSD_CODE.CD_ID = DC.LCD_ID INNER JOIN "
            + "ARTEFACT CL ON DC.CL_ID = CL.ART_ID "
            + "WHERE TRANSCODING_RULE.TR_ID={0} ";

        /// <summary>
        /// The time transcoding SQL Query which returns <c>FREQ, YEAR_COL_ID, PERIOD_COL_ID, EXPRESSION</c>
        /// </summary>
        public const string TimeTranscoding = "SELECT FREQ, YEAR_COL_ID, PERIOD_COL_ID, DATE_COL_ID, EXPRESSION FROM TIME_TRANSCODING WHERE TR_ID = {0}";

        /// <summary>
        /// The SQL statement for getting mapping set and dataflow.
        /// </summary>
        public const string MappingSetDataflow =
            "SELECT MSET.MAP_SET_ID, MSET.ID, MSET.DESCRIPTION, MSET.DS_ID, A.ID as DFID, dbo.versionToString(A.VERSION1, A.VERSION2, A.VERSION3) as DFVER, A.AGENCY as DFAG, T.DF_ID  FROM MAPPING_SET MSET, DATAFLOW T, ARTEFACT A WHERE MSET.MAP_SET_ID = T.MAP_SET_ID AND T.DF_ID = A.ART_ID ";
    }
}