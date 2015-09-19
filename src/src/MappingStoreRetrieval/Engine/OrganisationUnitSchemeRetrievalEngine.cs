// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationUnitSchemeRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2014-07-18
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The organisation unit scheme retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Data;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///     The organisation unit scheme retrieval engine.
    /// </summary>
    internal class OrganisationUnitSchemeRetrievalEngine : ItemSchemeRetrieverEngine<IOrganisationUnitSchemeMutableObject, IOrganisationUnitMutableObject>
    {
        #region Fields

        /// <summary>
        ///     The _item <see cref="SqlQueryInfo" /> builder.
        /// </summary>
        private readonly ItemSqlQueryBuilder _itemSqlQueryBuilder;

        /// <summary>
        ///     The _item SQL query info.
        /// </summary>
        private readonly SqlQueryInfo _itemSqlQueryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationUnitSchemeRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public OrganisationUnitSchemeRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb)
        {
            this._itemSqlQueryBuilder = new ItemSqlQueryBuilder(mappingStoreDb, OrganisationUnitSchemeConstant.ItemOrderBy);
            this._itemSqlQueryInfo = this._itemSqlQueryBuilder.Build(OrganisationUnitSchemeConstant.ItemTableInfo);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="IOrganisationUnitSchemeMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="IOrganisationUnitSchemeMutableObject" />.
        /// </returns>
        protected override IOrganisationUnitSchemeMutableObject CreateArtefact()
        {
            return new OrganisationUnitSchemeMutableCore();
        }

        /// <summary>
        /// When this method is overridden it is used to retrieve Items of a ItemScheme and populate the output List
        /// </summary>
        /// <param name="itemScheme">
        /// The <see cref="IItemSchemeMutableObject{T}"/> to fill with <see cref="IItemMutableObject"/>
        /// </param>
        /// <param name="parentSysId">
        /// The primary key of the Item Scheme from Mapping Store table ARTEFACT.ART_ID field
        /// </param>
        //protected override void FillItems(IOrganisationUnitSchemeMutableObject itemScheme, long parentSysId)
        //{
            //var itemQuery = new ItemSqlQuery(this._itemSqlQueryInfo, parentSysId);

            //AnnotationRetrievalEngine annRetrieval = new AnnotationRetrievalEngine(MappingStoreDb, OrganisationUnitSchemeConstant.TableInfo, OrganisationUnitSchemeConstant.ItemTableInfo, AnnotationCommandBuilder.AnnotationType.Item, parentSysId);

            //using (DbCommand command = this.ItemCommandBuilder.Build(itemQuery))
            //{
            //    using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
            //    {
            //        int sysIdIdx = dataReader.GetOrdinal("SYSID");
            //        int idIdx = dataReader.GetOrdinal("ID");
            //        int txtIdx = dataReader.GetOrdinal("TEXT");
            //        int langIdx = dataReader.GetOrdinal("LANGUAGE");
            //        int typeIdx = dataReader.GetOrdinal("TYPE");
            //        long lastSysId = long.MinValue;
            //        IOrganisationUnitMutableObject item = null;
            //        while (dataReader.Read())
            //        {
            //            long sysId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
            //            if (item == null || sysId != lastSysId)
            //            {
            //                lastSysId = sysId;
            //                item = new OrganisationUnitMutableCore();
            //                PopulateItem(item, dataReader, idIdx);
            //                annRetrieval.AddAnnotation(item, sysId);
            //                itemScheme.AddItem(item);
            //            }
                        
            //            ReadLocalisedString(item, typeIdx, txtIdx, langIdx, dataReader);
            //        }
            //    }
            //}
        //}

        protected override IOrganisationUnitMutableObject CreateItem()
        {
            return new OrganisationUnitMutableCore();
        }

        #endregion


    }
}