// -----------------------------------------------------------------------
// <copyright file="ItemSchemeRetrieverEngine.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The item scheme retriever engine.
    /// </summary>
    /// <typeparam name="TMaintaible">
    /// The <see cref="IItemSchemeMutableObject{T}"/> type
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The <typeparamref name="TMaintaible"/> Item type
    /// </typeparam>
    internal abstract class ItemSchemeRetrieverEngine<TMaintaible, TItem> : ArtefactRetrieverEngine<TMaintaible>
        where TMaintaible : IItemSchemeMutableObject<TItem> where TItem : IItemMutableObject
    {
        #region Fields

        /// <summary>
        ///     The item command builder.
        /// </summary>
        private readonly ItemCommandBuilder _itemCommandBuilder;

        /// <summary>
        /// The _item SQL query information
        /// </summary>
        private readonly SqlQueryInfo _itemSqlQueryInfo;

        /// <summary>
        /// The _identifiable annotation retriever engine
        /// </summary>
        private readonly IdentifiableAnnotationRetrieverEngine _identifiableAnnotationRetrieverEngine;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeRetrieverEngine{TMaintaible,TItem}"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="orderBy">
        /// The order By.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        protected ItemSchemeRetrieverEngine(Database mappingStoreDb, string orderBy = null)
            : base(mappingStoreDb, orderBy)
        {
            this._itemCommandBuilder = new ItemCommandBuilder(mappingStoreDb);
            var itemTableInfoBuilder = new ItemTableInfoBuilder();
            TableInfo tableInfo = new TableInfoBuilder().Build(typeof(TMaintaible));
            var itemTableInfo = itemTableInfoBuilder.Build(tableInfo.StructureType);
            var itemSqlQueryBuilder = new ItemSqlQueryBuilder(mappingStoreDb, null);
            this._itemSqlQueryInfo = itemSqlQueryBuilder.Build(itemTableInfo);

            this._identifiableAnnotationRetrieverEngine = new IdentifiableAnnotationRetrieverEngine(mappingStoreDb, itemTableInfo);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the item command builder.
        /// </summary>
        protected ItemCommandBuilder ItemCommandBuilder
        {
            get
            {
                return this._itemCommandBuilder;
            }
        }

        /// <summary>
        /// Gets the identifiable annotation retriever engine
        /// </summary>
        protected IdentifiableAnnotationRetrieverEngine IdentifiableAnnotationRetrieverEngine
        {
            get
            {
                return this._identifiableAnnotationRetrieverEngine;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// The version types.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<TMaintaible> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            var sqlInfo = versionConstraints == VersionQueryType.Latest ? this.SqlQueryInfoForLatest : this.SqlQueryInfoForAll;
            var sqlQuery = new ArtefactSqlQuery(sqlInfo, maintainableRef);
            return this.RetrieveItemScheme(detail, sqlQuery);
        }

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override TMaintaible RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            var sqlQuery = new ArtefactSqlQuery(this.SqlQueryInfoForLatest, maintainableRef);
            var mutableObjects = this.RetrieveItemScheme(detail, sqlQuery);
            switch (mutableObjects.Count)
            {
                case 0:
                    return default(TMaintaible);
                case 1:
                    return mutableObjects.First();
                default:
                    throw new ArgumentException(ErrorMessages.MoreThanOneArtefact, "maintainableRef");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create an item.
        /// </summary>
        /// <returns>
        ///     The <typeparamref name="TItem"/>
        /// </returns>
        protected abstract TItem CreateItem();

        /// <summary>
        /// The parse extra fields.
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="dataReader">
        /// The reader.
        /// </param>
        protected override void HandleArtefactExtraFields(TMaintaible artefact, IDataReader dataReader)
        {
            artefact.IsPartial = DataReaderHelper.GetBoolean(dataReader, "IS_PARTIAL");
        }

        /// <summary>
        /// When this method is overridden it is used to retrieve Items of a ItemScheme and populate the output List
        /// </summary>
        /// <param name="itemScheme">
        ///     The <see cref="IItemSchemeMutableObject{T}"/> to fill with <see cref="IItemMutableObject"/>
        /// </param>
        /// <param name="parentSysId">
        ///     The primary key of the Item Scheme from Mapping Store table ARTEFACT.ART_ID field
        /// </param>
        protected virtual void FillItems(TMaintaible itemScheme, long parentSysId)
        {
            var itemQuery = new ItemSqlQuery(this._itemSqlQueryInfo, parentSysId);

            var itemMap = new Dictionary<long, TItem>();
            using (DbCommand command = this.ItemCommandBuilder.Build(itemQuery))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    int sysIdIdx = dataReader.GetOrdinal("SYSID");
                    int idIdx = dataReader.GetOrdinal("ID");
                    int txtIdx = dataReader.GetOrdinal("TEXT");
                    int langIdx = dataReader.GetOrdinal("LANGUAGE");
                    int typeIdx = dataReader.GetOrdinal("TYPE");
                    while (dataReader.Read())
                    {
                        long sysId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
                        TItem item;
                        if (!itemMap.TryGetValue(sysId, out item))
                        {
                            item = this.CreateItem(); // we set them below.
                            item.Id = DataReaderHelper.GetString(dataReader, idIdx); // "ID"
                            itemScheme.AddItem(item);
                            itemMap.Add(sysId, item);
                        }

                        ReadLocalisedString(item, typeIdx, txtIdx, langIdx, dataReader);
                    }
                }
            } 

            this.IdentifiableAnnotationRetrieverEngine.RetrieveAnnotations(parentSysId, itemMap);
        }

        /// <summary>
        /// Retrieve details for the specified <paramref name="artefact"/> with MAPPING STORE ARTEFACT.ART_ID equal to
        ///     <paramref name="sysId"/>
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="sysId">
        /// The MAPPING STORE ARTEFACT.ART_ID value
        /// </param>
        /// <returns>
        /// The <typeparamref name="TMaintaible"/>.
        /// </returns>
        protected override TMaintaible RetrieveDetails(TMaintaible artefact, long sysId)
        {
            this.FillItems(artefact, sysId);
            return artefact;
        }

        /// <summary>
        /// The retrieve item scheme.
        /// </summary>
        /// <param name="detail">
        /// The detail.
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL query.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IItemSchemeObject}"/>.
        /// </returns>
        private ISet<TMaintaible> RetrieveItemScheme(ComplexStructureQueryDetailEnumType detail, ArtefactSqlQuery sqlQuery)
        {
            return this.RetrieveArtefacts(sqlQuery, detail);
        }

        #endregion
    }
}