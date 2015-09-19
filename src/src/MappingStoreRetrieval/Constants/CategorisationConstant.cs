// -----------------------------------------------------------------------
// <copyright file="CategorisationConstant.cs" company="EUROSTAT">
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
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The categorisation constant.
    /// </summary>
    internal static class CategorisationConstant
    {
        /// <summary>
        /// Gets the SQL Query format/template for retrieving the artefact reference from a categorisation id. Use with <see cref="string.Format(string,object)"/> and one parameter 
        /// 1. the <see cref="ProductionWhereClause"/> or <see cref="string.Empty"/> 
        /// </summary>
        public const string ArtefactRefQueryFormat =
            "SELECT C.CATN_ID, AV.ID, AV.VERSION, AV.AGENCY, T.STYPE FROM CATEGORISATION C INNER JOIN ARTEFACT_VIEW AV ON C.ART_ID = AV.ART_ID INNER JOIN (SELECT D.DF_ID as SID, 'Dataflow' as STYPE FROM DATAFLOW D INNER JOIN ARTEFACT A ON A.ART_ID = D.DF_ID {0} UNION ALL SELECT D.DSD_ID as SID, 'Dsd' as STYPE FROM DSD D UNION ALL SELECT D.CON_SCH_ID as SID, 'ConceptScheme' as STYPE FROM Concept_Scheme D UNION ALL SELECT D.CL_ID as SID, 'Codelist' as STYPE FROM Codelist D UNION ALL SELECT D.HCL_ID as SID, 'Hcl' as STYPE FROM HCL D) T  ON T.SID = C.ART_ID ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the category reference from a categorisation id.
        /// </summary>
        public const string CategoryRefQueryFormat =
            "SELECT C.CATN_ID, A.ID, A.VERSION, A.AGENCY, I.ID as CATID FROM CATEGORISATION C INNER JOIN CATEGORY CY ON C.CAT_ID = CY.CAT_ID INNER JOIN ARTEFACT_VIEW A ON CY.CAT_SCH_ID = A.ART_ID INNER JOIN ITEM I ON I.ITEM_ID = CY.CAT_ID ";

        /// <summary>
        /// The referenced by CATEGORISATION P table the "parent" and A is the referenced <see cref="ArtefactParentsSqlBuilder.SqlQueryFormat"/>.
        /// </summary>
        public const string ReferencedByCategorisation = " INNER JOIN CATEGORISATION T ON T.CATN_ID = P.ART_ID INNER JOIN ARTEFACT A ON T.ART_ID = A.ART_ID ";

        /// <summary>
        /// Gets the PRODUCTION clause
        /// </summary>
        public const string ProductionWhereClause = " D.PRODUCTION = 1 ";

        /// <summary>
        /// The categorisation version.
        /// </summary>
        public const string CategorisationVersion = "1.0";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.Categorisation) { Table = "CATEGORISATION", PrimaryKey = "CATN_ID" };

        /// <summary>
        /// The _artefact reference.
        /// </summary>
        private static readonly SqlQueryInfo _artefactReference = new SqlQueryInfo { QueryFormat = ArtefactRefQueryFormat, WhereStatus = WhereState.Nothing };

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

        /// <summary>
        /// Gets the artefact reference.
        /// </summary>
        public static SqlQueryInfo ArtefactReference
        {
            get
            {
                return _artefactReference;
            }
        }
    }
}