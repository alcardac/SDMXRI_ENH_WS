// -----------------------------------------------------------------------
// <copyright file="ItemTableInfoBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-04-09
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
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Builds an <see cref="ItemTableInfo"/> 
    /// </summary>
    public class ItemTableInfoBuilder : IBuilder<ItemTableInfo, SdmxStructureEnumType>
    {
        /// <summary>
        /// Builds an <see cref="ItemTableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An <see cref="SdmxStructureEnumType"/> to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="ItemTableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </returns>
        public ItemTableInfo Build(SdmxStructureEnumType buildFrom)
        {
            ItemTableInfo tableInfo = null;
            switch (buildFrom)
            {
                case SdmxStructureEnumType.Categorisation:
                    break;
                case SdmxStructureEnumType.CodeList:
                case SdmxStructureEnumType.Code:
                    tableInfo = CodeListConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                case SdmxStructureEnumType.Category:
                    tableInfo = CategorySchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                case SdmxStructureEnumType.Concept:
                    tableInfo = ConceptSchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.DataProvider:
                case SdmxStructureEnumType.DataProviderScheme:
                    tableInfo = DataProviderSchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.DataConsumer:
                case SdmxStructureEnumType.DataConsumerScheme:
                    tableInfo = DataConsumerSchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.Agency:
                case SdmxStructureEnumType.AgencyScheme:
                    tableInfo = AgencySchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.OrganisationUnit:
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    tableInfo = OrganisationUnitSchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.CodeListMap:
                    tableInfo = StructureSetConstant.CLMItemTableInfo;
                    break;
                case SdmxStructureEnumType.StructureMap:
                    tableInfo = StructureSetConstant.SMItemTableInfo;
                    break;
            }
            return tableInfo;
        }
    }
}