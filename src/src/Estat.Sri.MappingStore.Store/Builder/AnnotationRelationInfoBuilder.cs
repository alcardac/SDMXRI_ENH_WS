// -----------------------------------------------------------------------
// <copyright file="AnnotationRelationInfoBuilder.cs" company="EUROSTAT">
//   Date Created : 2014-11-21
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
namespace Estat.Sri.MappingStore.Store.Builder
{
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The annotation relation info builder.
    /// </summary>
    public class AnnotationRelationInfoBuilder : IBuilder<TableInfo, SdmxStructureType>
    {
        /// <summary>
        /// Builds the <see cref="TableInfo"/> for the annotation relation tables from the specified <paramref name="structureType"/>
        /// </summary>
        /// <param name="structureType">Type of the structure.</param>
        /// <returns>
        /// The <see cref="TableInfo"/>
        /// </returns>
        public TableInfo Build(SdmxStructureType structureType)
        {
            var annotationRelationTable = new TableInfo(structureType);

            if (structureType.IsMaintainable)
            {
                annotationRelationTable.Table = "ARTEFACT_ANNOTATION";
                annotationRelationTable.PrimaryKey = "ART_ID";
            }
            else
            {
                switch (structureType.EnumType)
                {
                    case SdmxStructureEnumType.Dimension:
                    case SdmxStructureEnumType.TimeDimension:
                    case SdmxStructureEnumType.MeasureDimension:
                    case SdmxStructureEnumType.PrimaryMeasure:
                    case SdmxStructureEnumType.DataAttribute:
                    case SdmxStructureEnumType.CrossSectionalMeasure:
                    case SdmxStructureEnumType.Component:
                        annotationRelationTable.Table = "COMPONENT_ANNOTATION";
                        annotationRelationTable.PrimaryKey = "COMP_ID";
                        break;
                    case SdmxStructureEnumType.Group:
                        annotationRelationTable.Table = "GROUP_ANNOTATION";
                        annotationRelationTable.PrimaryKey = "GR_ID";
                        break;
                    case SdmxStructureEnumType.Hierarchy:
                    case SdmxStructureEnumType.Level:
                    case SdmxStructureEnumType.CodeListRef:
                    case SdmxStructureEnumType.HierarchicalCode:
                        annotationRelationTable.Table = "ARTEFACT_ANNOTATION";
                        annotationRelationTable.PrimaryKey = "ART_ID";
                        break;
                    default:
                        annotationRelationTable.Table = "ITEM_ANNOTATION";
                        annotationRelationTable.PrimaryKey = "ITEM_ID";
                        break;
                }
            }

            return annotationRelationTable;
        }
    }
}