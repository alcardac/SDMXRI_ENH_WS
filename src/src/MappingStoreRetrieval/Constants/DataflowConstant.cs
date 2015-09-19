// -----------------------------------------------------------------------
// <copyright file="DataflowConstant.cs" company="EUROSTAT">
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
    /// The Dataflow table constant.
    /// </summary>
    internal static class DataflowConstant
    {
        /// <summary>
        /// Gets the SQL Query format/template for retrieving the key family reference from a dataflow id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string KeyFamilyRefQueryFormat =
            "SELECT DSD.DSD_ID, ART.ID, ART.VERSION, ART.AGENCY FROM DSD, DATAFLOW DF, ARTEFACT_VIEW ART WHERE DF.DSD_ID = DSD.DSD_ID AND DSD.DSD_ID = ART.ART_ID AND DF.DF_ID = {0} ";

        /// <summary>
        /// Gets the PRODUCTION clause
        /// </summary>
        public const string ProductionWhereClause = " T.PRODUCTION = 1";

        /// <summary>
        /// Gets the PRODUCTION clause
        /// </summary>
        public const string ProductionWhereLatestClause = " AND T2.PRODUCTION = 1";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.Dataflow) { Table = "DATAFLOW", PrimaryKey = "DF_ID", ExtraFields = ", PRODUCTION" };

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