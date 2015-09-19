// -----------------------------------------------------------------------
// <copyright file="AnnotationDeleteEngine.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Engine.Delete
{
    using System.Data;
    using System.Globalization;

    using Estat.Sri.MappingStore.Store.Builder;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The annotation delete engine.
    /// </summary>
    public class AnnotationDeleteEngine
    {
        /// <summary>
        /// The _mapping store database
        /// </summary>
        private readonly Database _mappingStoreDatabase;

        /// <summary>
        /// The _annotation relation information builder
        /// </summary>
        private readonly AnnotationRelationInfoBuilder _annotationRelationInfoBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationDeleteEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDatabase">
        /// The mapping store database.
        /// </param>
        public AnnotationDeleteEngine(Database mappingStoreDatabase)
        {
            this._mappingStoreDatabase = mappingStoreDatabase;
            this._annotationRelationInfoBuilder = new AnnotationRelationInfoBuilder();
        }

        /// <summary>
        /// Deletes annotations for annotatoble with the specified <paramref name="annotableSysId"/> and the specified <paramref name="type"/>
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="annotableSysId">The annotable system identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="structureType">Type of the structure.</param>
        /// <returns>The number of annotations deleted.</returns>
        public int DeleteByType(DbTransactionState state, long annotableSysId, string type, SdmxStructureType structureType)
        {
            var annotationRelationTable = this._annotationRelationInfoBuilder.Build(structureType);

            var query = string.Format(
                CultureInfo.InvariantCulture,
                "DELETE FROM ANNOTATION WHERE ANN_ID IN (SELECT DISTINCT ANN_ID FROM {0} WHERE {1} = {{0}} AND TYPE={{1}})",
                annotationRelationTable.Table,
                annotationRelationTable.PrimaryKey);
            return state.Database.ExecuteNonQueryFormat(query, this._mappingStoreDatabase.CreateInParameter("p_id", DbType.Int64, annotableSysId), this._mappingStoreDatabase.CreateInParameter("p_type", DbType.String, type));
        }
    }
}