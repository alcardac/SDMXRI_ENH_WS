// -----------------------------------------------------------------------
// <copyright file="IdentifiableAnnotationRetrieverEngine.cs" company="EUROSTAT">
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
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    /// The annotation retriever engine.
    /// </summary>
    internal class IdentifiableAnnotationRetrieverEngine
    {
        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(IdentifiableAnnotationRetrieverEngine));

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
        /// Initializes a new instance of the <see cref="IdentifiableAnnotationRetrieverEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">The mapping store database.</param>
        /// <param name="tableInfo">The table information.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="tableInfo"/> is null -or- <paramref name="mappingStoreDb"/> is null.</exception>
        public IdentifiableAnnotationRetrieverEngine(Database mappingStoreDb, ItemTableInfo tableInfo)
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

            ISqlQueryInfoBuilder<ItemTableInfo> annotationQueryBuilder = new AnnotationQueryBuilder();
            this._annotationSqlQueryInfo = annotationQueryBuilder.Build(tableInfo);

            this._annotationCommandBuilder = new AnnotationCommandBuilder(this._mappingStoreDb);
        }

        /// <summary>
        /// Retrieve annotations for all specified <paramref name="annotateables"/> with parent
        /// </summary>
        /// <typeparam name="T">The sub type of <see cref="IAnnotableMutableObject"/></typeparam>
        /// <param name="parentSysId">The parent primary key value.</param>
        /// <param name="annotateables">The SDMX objects that accept annotations.</param>
        public void RetrieveAnnotations<T>(long parentSysId, IDictionary<long, T> annotateables) 
            where T : IAnnotableMutableObject
        {
            using (var command = this._annotationCommandBuilder.Build(new PrimaryKeySqlQuery(this._annotationSqlQueryInfo, parentSysId)))
            {
                _log.InfoFormat(CultureInfo.InvariantCulture,"Executing query for identifiable annotations : {0}", command.CommandText);
                using (var dataReader = this._mappingStoreDb.ExecuteReader(command))
                {
                    int sysIdIdx = dataReader.GetOrdinal("SYSID");
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

                            var sysId = dataReader.GetInt64(sysIdIdx);
                            T annotable;
                            if (annotateables.TryGetValue(sysId, out annotable))
                            {
                                annotable.AddAnnotation(annotation);
                            }
                            else
                            {
                                _log.WarnFormat(CultureInfo.InvariantCulture, "Possible bug detected while retrieving annotations. Could not find SDMX object of type ({0}) with primary key value {1}", typeof(T), sysId);
                            }
                        }

                        var text = DataReaderHelper.GetString(dataReader, txtIdx);
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            annotation.AddText(DataReaderHelper.GetString(dataReader, langIdx), text);
                        }
                    }

                    this._mappingStoreDb.CancelSafe(command);
                }
            }
        }
    }
}