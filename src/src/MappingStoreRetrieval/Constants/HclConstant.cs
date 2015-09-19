// -----------------------------------------------------------------------
// <copyright file="HclConstant.cs" company="EUROSTAT">
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
    /// The HCL, HIERARCHY, HCL_CODE and HLEVEL tables constants.
    /// </summary>
    internal static class HclConstant
    {
        /// <summary>
        /// Gets the SQL Query format/template for retrieving the HIERARCY from a HCL id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string HierarchyQueryFormat =
                "SELECT T.H_ID as SYSID, A.ID, LN.TEXT, LN.LANGUAGE, LN.TYPE FROM HIERARCHY T, ARTEFACT A  LEFT OUTER JOIN LOCALISED_STRING LN ON LN.ART_ID = A.ART_ID WHERE T.H_ID = A.ART_ID AND T.HCL_ID ={0} ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the codelist references from the HCL id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string CodelistRefQueryFormat =
            "SELECT  distinct A.ID, A.AGENCY, A.VERSION FROM HCL_CODE T INNER JOIN DSD_CODE C ON T.LCD_ID = C.LCD_ID INNER JOIN ARTEFACT_VIEW A ON C.CL_ID = A.ART_ID INNER JOIN HIERARCHY H ON T.H_ID = H.H_ID WHERE  H.HCL_ID ={0} ";


        /// <summary>
        /// Gets the SQL Query format/template for retrieving the Hierarchical codelist level from a HIERARCHY id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string LevelQueryFormat =
              "SELECT T.LEVEL_ID as SYSID, A.ID, T.PARENT_LEVEL_ID AS PARENT, LN.TEXT, LN.LANGUAGE, LN.TYPE FROM HLEVEL T, ARTEFACT A  LEFT OUTER JOIN LOCALISED_STRING LN ON LN.ART_ID = A.ART_ID WHERE T.LEVEL_ID = A.ART_ID AND T.H_ID ={0} ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the code references from a HIERARCHY id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string CodeRefQueryFormat =
              "SELECT T.HCODE_ID as SYSID, A.ID as NodeAliasID, T.PARENT_HCODE_ID AS PARENT, I.ID as CODE_ID, AL.ID as LEVEL_REF, CL.ID as CLID, CL.VERSION as CLVERSION, CL.AGENCY as CLAGENCY, A.VALID_FROM, A.VALID_TO FROM HCL_CODE T  INNER JOIN ARTEFACT A ON T.HCODE_ID = A.ART_ID INNER JOIN DSD_CODE DC ON DC.LCD_ID = T.LCD_ID  INNER JOIN ITEM I ON I.ITEM_ID = DC.LCD_ID  INNER JOIN ARTEFACT_VIEW CL ON DC.CL_ID = CL.ART_ID  LEFT OUTER JOIN ARTEFACT AL ON AL.ART_ID = T.LEVEL_ID WHERE  T.H_ID ={0} ";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.HierarchicalCodelist) { Table = "HCL", PrimaryKey = "HCL_ID" };

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