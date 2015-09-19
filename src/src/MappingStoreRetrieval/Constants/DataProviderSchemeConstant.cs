// -----------------------------------------------------------------------
// <copyright file="DataProviderSchemeConstant.cs" company="EUROSTAT">
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
    internal static class DataProviderSchemeConstant
    {
        /// <summary>
        /// Gets the item order by.
        /// </summary>
        public const string ItemOrderBy = " ORDER BY T.DP_ID";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.DataProviderScheme) { Table = "DATAPROVIDER_SCHEME", PrimaryKey = "DP_SCH_ID", ExtraFields = ", IS_PARTIAL" };

        /// <summary>
        /// The item table info. i.e. for table DATAPROVIDER
        /// </summary>
        private static readonly ItemTableInfo _itemTableInfo = new ItemTableInfo(SdmxStructureEnumType.DataProvider) { Table = "DATAPROVIDER", PrimaryKey = "DP_ID", ForeignKey = "DP_SCH_ID", ParentItem = null };

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
        /// Gets the item table info. i.e. for table DATAPROVIDER
        /// </summary>
        public static ItemTableInfo ItemTableInfo
        {
            get
            {
                return _itemTableInfo;
            }
        }
    }
}