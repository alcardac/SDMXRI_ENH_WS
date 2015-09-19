// -----------------------------------------------------------------------
// <copyright file="TableInfoBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-04-08
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
    using System;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The table info builder.
    /// </summary>
    public class TableInfoBuilder : IBuilder<TableInfo, SdmxStructureEnumType>, IBuilder<TableInfo, Type>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="TableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An <see cref="SdmxStructureEnumType"/> to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="TableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </returns>
        public TableInfo Build(SdmxStructureEnumType buildFrom)
        {
            TableInfo tableInfo = null;
            switch (buildFrom)
            {
                case SdmxStructureEnumType.Categorisation:
                    tableInfo = CategorisationConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.CodeList:
                    tableInfo = CodeListConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    tableInfo = CategorySchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    tableInfo = ConceptSchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.Dataflow:
                    tableInfo = DataflowConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.Dsd:
                    tableInfo = DsdConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    tableInfo = HclConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    tableInfo = OrganisationUnitSchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.AgencyScheme:
                    tableInfo = AgencySchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.DataConsumerScheme:
                    tableInfo = DataConsumerSchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.DataProviderScheme:
                    tableInfo = DataProviderSchemeConstant.TableInfo;
                    break;

                case SdmxStructureEnumType.StructureSet:
                    tableInfo = StructureSetConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.ContentConstraint:
                    tableInfo = ContentConstraintConstant.TableInfo;
                    break;
            }

            return tableInfo;
        }

        /// <summary>
        /// Builds an <see cref="TableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">The type to build from.</param>
        /// <returns>
        /// an <see cref="TableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </returns>
        public TableInfo Build(Type buildFrom)
        {
            var propertyInfo = buildFrom.GetProperty("ImmutableInstance");
            var returnType = propertyInfo.GetGetMethod().ReturnType;
            var sdmxStructureType = SdmxStructureType.ParseClass(returnType);
            return this.Build(sdmxStructureType);
        }

        #endregion
    }
}
