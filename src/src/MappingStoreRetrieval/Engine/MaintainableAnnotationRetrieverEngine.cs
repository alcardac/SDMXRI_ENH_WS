// -----------------------------------------------------------------------
// <copyright file="MaintainableAnnotationRetrieverEngine.cs" company="EUROSTAT">
//   Date Created : 2014-11-06
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
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    /// The annotation retriever engine.
    /// </summary>
    internal class MaintainableAnnotationRetrieverEngine
    {
        /// <summary>
        ///     This field holds the Mapping Store Database object.
        /// </summary>
        private readonly Database _mappingStoreDb;

        /// <summary>
        /// The _annotation SQL query information
        /// </summary>
        private readonly SqlQueryInfo _annotationSqlQueryInfo;

        /// <summary>
        /// The _annotation command builder
        /// </summary>
        private readonly AnnotationCommandBuilder _annotationCommandBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableAnnotationRetrieverEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">The mapping store database.</param>
        /// <param name="tableInfo">The table information.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="tableInfo"/> is null -or- <paramref name="mappingStoreDb"/> is null.</exception>
        public MaintainableAnnotationRetrieverEngine(Database mappingStoreDb, TableInfo tableInfo)
        {
            if (mappingStoreDb == null)
            {
                throw new ArgumentNullException("mappingStoreDb");
            }

            if (tableInfo == null)
            {
                throw new ArgumentNullException("tableInfo");
            }

            this._mappingStoreDb = mappingStoreDb;

            ISqlQueryInfoBuilder<TableInfo> annotationQueryBuilder = new AnnotationQueryBuilder();
            this._annotationSqlQueryInfo = annotationQueryBuilder.Build(tableInfo);

            this._annotationCommandBuilder = new AnnotationCommandBuilder(this._mappingStoreDb);
        }

        /// <summary>
        /// Retrieve annotations for the specified SDMX <paramref name="annotable"/> object
        /// </summary>
        /// <param name="sysId">The artefact primary key value.</param>
        /// <param name="annotable">The SDMX object.</param>
        public void RetrieveAnnotations(long sysId, IAnnotableMutableObject annotable)
        {
            using (var command = this._annotationCommandBuilder.Build(new PrimaryKeySqlQuery(this._annotationSqlQueryInfo, sysId)))
            using (var dataReader = this._mappingStoreDb.ExecuteReader(command))
            {
                int annIdIdx = dataReader.GetOrdinal("ANN_ID");
                int idIdx = dataReader.GetOrdinal("ID");
                int txtIdx = dataReader.GetOrdinal("TEXT");
                int langIdx = dataReader.GetOrdinal("LANGUAGE");
                int typeIdx = dataReader.GetOrdinal("TYPE");
                int titleIdx = dataReader.GetOrdinal("TITLE");
                int urlIdx = dataReader.GetOrdinal("URL");

                IDictionary<long, IAnnotationMutableObject> annotationMap = new Dictionary<long, IAnnotationMutableObject>();

                while (dataReader.Read())
                {
                    var annId = dataReader.GetInt64(annIdIdx);
                    IAnnotationMutableObject annotation;
                    if (!annotationMap.TryGetValue(annId, out annotation))
                    {
                        annotation = new AnnotationMutableCore
                        {
                            Id = DataReaderHelper.GetString(dataReader, idIdx),
                            Title = DataReaderHelper.GetString(dataReader, titleIdx),
                            Type = DataReaderHelper.GetString(dataReader, typeIdx)
                        };
                        var url = DataReaderHelper.GetString(dataReader, urlIdx);
                        Uri uri;
                        if (!string.IsNullOrWhiteSpace(url) && Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
                        {
                            annotation.Uri = uri;
                        }

                        annotable.AddAnnotation(annotation);
                    }

                    var text = DataReaderHelper.GetString(dataReader, txtIdx);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        annotation.AddText(DataReaderHelper.GetString(dataReader, langIdx), text);
                    }
                }
            }
        }
    }
}