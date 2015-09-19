// -----------------------------------------------------------------------
// <copyright file="OrganisationUnitSchemeConstant.cs" company="EUROSTAT">
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
    internal static class OrganisationUnitSchemeConstant
    {
        /// <summary>
        /// Gets the item order by.
        /// </summary>
        public const string ItemOrderBy = " ORDER BY T.ORG_UNIT_ID";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.OrganisationUnitScheme) { Table = "ORGANISATION_UNIT_SCHEME", PrimaryKey = "ORG_UNIT_SCH_ID", ExtraFields = ", IS_PARTIAL" };

        /// <summary>
        /// The item table info. i.e. for table ORGANISATION_UNIT
        /// </summary>
        private static readonly ItemTableInfo _itemTableInfo = new ItemTableInfo(SdmxStructureEnumType.OrganisationUnit) { Table = "ORGANISATION_UNIT", PrimaryKey = "ORG_UNIT_ID", ForeignKey = "ORG_UNIT_SCH_ID", ParentItem = "PARENT_ORG_UNIT_ID" };

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
        /// Gets the item table info. i.e. for table ORGANISATION_UNIT
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