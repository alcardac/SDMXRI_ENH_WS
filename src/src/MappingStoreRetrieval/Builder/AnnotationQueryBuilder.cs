// -----------------------------------------------------------------------
// <copyright file="AnnotationQueryBuilder.cs" company="EUROSTAT">
//   Date Created : 2014-11-05
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
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    internal class AnnotationQueryBuilder : ISqlQueryInfoBuilder<TableInfo>, ISqlQueryInfoBuilder<ItemTableInfo>
    {
        /// <summary>
        /// Builds the specified artefact table information.
        /// </summary>
        /// <param name="artefactTableInfo">The artefact table information.</param>
        /// <returns>THe <see cref="SqlQueryInfo"/></returns>
        public SqlQueryInfo Build(TableInfo artefactTableInfo)
        {
            var format = string.Format(CultureInfo.InvariantCulture, AnnotationConstants.AnnotationQuery, artefactTableInfo.PrimaryKey, AnnotationConstants.ArtefactAnnotationTable, artefactTableInfo.Table, "ART_ID");
            
            return new SqlQueryInfo() { QueryFormat = string.Format(CultureInfo.InvariantCulture, "{0} WHERE T.{1} = {{0}}", format, artefactTableInfo.PrimaryKey), WhereStatus = WhereState.And };
        }

        /// <summary>
        /// Builds the specified nameable table information.
        /// </summary>
        /// <param name="nameableTableInfo">The nameable table information.</param>
        /// <returns>THe <see cref="SqlQueryInfo"/></returns>
        public SqlQueryInfo Build(ItemTableInfo nameableTableInfo)
        {
            // we have some non-nameable and/or special cases...
            string annotationRelationTable;
            string annotationRelationTableForeignKey;
            switch (nameableTableInfo.StructureType)
            {
                case SdmxStructureEnumType.Component:
                    annotationRelationTable = AnnotationConstants.ComponentAnnotationTable;
                    annotationRelationTableForeignKey = "COMP_ID";
                    break;
                case SdmxStructureEnumType.Group:
                    annotationRelationTable = AnnotationConstants.DsdGroupAnnotationTable;
                    annotationRelationTableForeignKey = "GR_ID";
                    break;
                 case SdmxStructureEnumType.Level:
                 case SdmxStructureEnumType.Hierarchy:
                 case SdmxStructureEnumType.HierarchicalCode:
                    annotationRelationTable = AnnotationConstants.ArtefactAnnotationTable;
                    annotationRelationTableForeignKey = "ART_ID";
                    break;                   
                default:
                    annotationRelationTable = AnnotationConstants.ItemAnnotationTable;
                    annotationRelationTableForeignKey = "ITEM_ID";
                    break;
            }

            var format = string.Format(CultureInfo.InvariantCulture, AnnotationConstants.AnnotationQuery, nameableTableInfo.PrimaryKey, annotationRelationTable, nameableTableInfo.Table, annotationRelationTableForeignKey);
            return new SqlQueryInfo() { QueryFormat = string.Format(CultureInfo.InvariantCulture, "{0} WHERE T.{1} = {{0}}", format, nameableTableInfo.ForeignKey), WhereStatus = WhereState.And };
        }
    }
}