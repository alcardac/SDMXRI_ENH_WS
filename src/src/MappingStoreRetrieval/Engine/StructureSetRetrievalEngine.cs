// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSetRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2014-07-24
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The StructureSet retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Extensions;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping;

    /// <summary>
    ///     The StructureSet engine.
    /// </summary>
    internal class StructureSetRetrievalEngine : ArtefactRetrieverEngine<IStructureSetMutableObject>
    {

        // add for estat annotation
        /// <summary>
        /// The _identifiable annotation retriever engine
        /// </summary>
        private IdentifiableAnnotationRetrieverEngine _identifiableAnnotationRetrieverEngine;

        // add for estat annotation
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

        // add for estat annotation
        private Database _mappingStoreDb;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSetRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null.
        /// </exception>
        public StructureSetRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb)
        {
            // add for estat annotation
            _mappingStoreDb = this.MappingStoreDb;

            var sqlQueryBuilder = new ReferencedSqlQueryBuilder(this.MappingStoreDb, null);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IStructureSetMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// The version constraints.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IStructureSetMutableObject}"/>.
        /// </returns>
        public override ISet<IStructureSetMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            var sqlInfo = versionConstraints == VersionQueryType.Latest ? this.SqlQueryInfoForLatest : this.SqlQueryInfoForAll;
            return this.GetStructureSetMutableObjects(maintainableRef, detail, sqlInfo);
        }

        /// <summary>
        /// Retrieve the <see cref="IStructureSetMutableObject"/> with the latest version group by ID and AGENCY from Mapping
        ///     Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureSetMutableObject"/>.
        /// </returns>
        public override IStructureSetMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            ISet<IStructureSetMutableObject> mutableObjects = this.GetStructureSetMutableObjects(maintainableRef, detail, this.SqlQueryInfoForLatest);
            return mutableObjects.GetOneOrNothing();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="IStructureSetMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="IStructureSetMutableObject" />.
        /// </returns>
        protected override IStructureSetMutableObject CreateArtefact()
        {
            return new StructureSetMutableCore();
        }

 
        ///// <summary>
        ///// Retrieve details for the specified <paramref name="artefact"/> with MAPPING STORE ARTEFACT.ART_ID equal to
        /////     <paramref name="sysId"/>
        ///// </summary>
        ///// <param name="artefact">
        ///// The artefact.
        ///// </param>
        ///// <param name="sysId">
        ///// The MAPPING STORE ARTEFACT.ART_ID value
        ///// </param>
        ///// <returns>
        ///// The <see cref="IStructureSetMutableObject"/>.
        ///// </returns>
        protected override IStructureSetMutableObject RetrieveDetails(IStructureSetMutableObject artefact, long sysId)
        {
            PopulateCodeListMap(artefact, sysId);
            PopulateStructureMap(artefact, sysId);

            return artefact;
        }

        #region "StructureMap Methods"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void PopulateStructureMap(IStructureSetMutableObject artefact, long sysId)
        {
            GetStructureMapInfo(artefact, sysId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        private bool GetStructureMapInfo(IStructureSetMutableObject artefact, long sysId)
        {
            // add for estat annotation
            this._identifiableAnnotationRetrieverEngine = new IdentifiableAnnotationRetrieverEngine(_mappingStoreDb, StructureSetConstant.SMItemTableInfo);
            var itemMap = new Dictionary<long, IStructureMapMutableObject>();

            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);
            //AnnotationRetrievalEngine annRetrieval = new AnnotationRetrievalEngine(MappingStoreDb, StructureSetConstant.TableInfo, StructureSetConstant.SMItemTableInfo, AnnotationCommandBuilder.AnnotationType.Item, sysId);

            bool smFound = false;

            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(StructureSetConstant.SqlSMInfo, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    int txtIdx = dataReader.GetOrdinal("TEXT");
                    int langIdx = dataReader.GetOrdinal("LANGUAGE");
                    int typeIdx = dataReader.GetOrdinal("TYPE");
                    long smID;
                    string ID;
                    long currSM = 0;

                    IStructureMapMutableObject sm = null;

                    while (dataReader.Read())
                    {
                        smFound = true;

                        smID = (long)dataReader["ITEM_ID"];
                        if (smID != currSM)
                        {
                            if (sm != null)
                                GetSMItemAndReference(artefact, sm, currSM);

                            sm = new StructureMapMutableCore();
                            ID = dataReader["ID"].ToString();
                            sm.Id = ID;
                            //annRetrieval.AddAnnotation(sm, smID);

                            // add for estat annotation
                            itemMap.Add(smID, sm);

                            currSM = smID;
                        }

                        ReadLocalisedString(sm, typeIdx, txtIdx, langIdx, dataReader);
                    }

                    if (sm != null)
                        GetSMItemAndReference(artefact, sm, currSM);
                }
            }
            // add for estat annotation
            this.IdentifiableAnnotationRetrieverEngine.RetrieveAnnotations(sysId, itemMap);

            return smFound;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sm"></param>
        /// <param name="smID"></param>
        private void GetSMItemAndReference(IStructureSetMutableObject artefact, IStructureMapMutableObject sm, long smID)
        {
            GetStructureMapCrossReference(artefact, sm, smID);
            GetStructureMapItems(artefact, sm, smID);
            artefact.AddStructureMap(sm);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void GetStructureMapCrossReference(IStructureSetMutableObject artefact, IStructureMapMutableObject sm, long sysId)
        {
            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);

            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(StructureSetConstant.SqlSMReference, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    if (dataReader.Read())
                    {
                        IStructureReference sourceRef;
                        IStructureReference targetRef;

                        string s_ID, s_Agency, s_Version, s_ArtType;
                        string t_ID, t_Agency, t_Version, t_ArtType;

                        s_ID = dataReader["S_ID"].ToString();
                        s_Agency = dataReader["S_AGENCY"].ToString();
                        s_Version = dataReader["S_Version"].ToString();
                        s_ArtType = dataReader["S_ARTEFACT_TYPE"].ToString();

                        t_ID = dataReader["T_ID"].ToString();
                        t_Agency = dataReader["T_AGENCY"].ToString();
                        t_Version = dataReader["T_Version"].ToString();
                        t_ArtType = dataReader["T_ARTEFACT_TYPE"].ToString();

                        sourceRef = new StructureReferenceImpl(s_Agency, s_ID, s_Version, (SdmxStructureEnumType)Enum.Parse(typeof(SdmxStructureEnumType), s_ArtType), "");
                        targetRef = new StructureReferenceImpl(t_Agency, t_ID, t_Version, (SdmxStructureEnumType)Enum.Parse(typeof(SdmxStructureEnumType), t_ArtType), "");

                        sm.SourceRef = sourceRef;
                        sm.TargetRef = targetRef;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void GetStructureMapItems(IStructureSetMutableObject artefact, IStructureMapMutableObject sm, long sysId)
        {
            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);

            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(StructureSetConstant.SqlSMItem, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    ComponentMapMutableCore smComp;

                    while (dataReader.Read())
                    {
                        smComp = new ComponentMapMutableCore();

                        smComp.MapConceptRef = dataReader["S_ID"].ToString();
                        smComp.MapTargetConceptRef = dataReader["T_ID"].ToString();

                        sm.AddComponent(smComp);
                    }
                }
            }
        }

        #endregion

        #region "CodeListMap Methods"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void PopulateCodeListMap(IStructureSetMutableObject artefact, long sysId)
        {
            GetCodeListMapInfo(artefact, sysId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        private bool GetCodeListMapInfo(IStructureSetMutableObject artefact, long sysId)
        {
            // add for estat annotation
            this._identifiableAnnotationRetrieverEngine = new IdentifiableAnnotationRetrieverEngine(_mappingStoreDb, StructureSetConstant.CLMItemTableInfo);
            var itemMap = new Dictionary<long, ICodelistMapMutableObject>();

            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);
            //AnnotationRetrievalEngine annRetrieval = new AnnotationRetrievalEngine(MappingStoreDb, StructureSetConstant.TableInfo, StructureSetConstant.CLMItemTableInfo, AnnotationCommandBuilder.AnnotationType.Item, sysId);

            bool clmFound = false;

            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(StructureSetConstant.SqlCLMInfo, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    int txtIdx = dataReader.GetOrdinal("TEXT");
                    int langIdx = dataReader.GetOrdinal("LANGUAGE");
                    int typeIdx = dataReader.GetOrdinal("TYPE");
                    long clmID = 0;
                    string ID;
                    long currCLM = 0;

                    ICodelistMapMutableObject clm = null;

                    while (dataReader.Read())
                    {
                        clmFound = true;
                        clmID = (long)dataReader["ITEM_ID"];
                        if (clmID != currCLM)
                        {
                            if(clm != null)
                                GetCLMItemAndReference(artefact, clm, currCLM);

                            clm = new CodelistMapMutableCore();
                            ID = dataReader["ID"].ToString();
                            clm.Id = ID;
                            //annRetrieval.AddAnnotation(clm, clmID);

                            // add for estat annotation
                            itemMap.Add(clmID, clm);

                            currCLM = clmID;
                        }

                        ReadLocalisedString(clm, typeIdx, txtIdx, langIdx, dataReader);
                    }

                    if (clm != null)
                        GetCLMItemAndReference(artefact, clm, currCLM);
                }
            }

            // add for estat annotation
            this.IdentifiableAnnotationRetrieverEngine.RetrieveAnnotations(sysId, itemMap);
            return clmFound;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="clm"></param>
        /// <param name="clmID"></param>
        private void GetCLMItemAndReference(IStructureSetMutableObject artefact, ICodelistMapMutableObject clm, long clmID)
        {
            GetCodeListMapCrossReference(artefact, clm, clmID);
            GetCodeListMapItems(artefact, clm, clmID);
            artefact.AddCodelistMap(clm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void GetCodeListMapCrossReference(IStructureSetMutableObject artefact, ICodelistMapMutableObject clm, long sysId)
        {
            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);

            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(StructureSetConstant.SqlCLMReference, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    if (dataReader.Read())
                    {
                        IStructureReference sourceRef;
                        IStructureReference targetRef;

                        string s_ID, s_Agency, s_Version;
                        string t_ID, t_Agency, t_Version;

                        s_ID = dataReader["S_ID"].ToString();
                        s_Agency = dataReader["S_AGENCY"].ToString();
                        s_Version = dataReader["S_Version"].ToString();

                        t_ID = dataReader["T_ID"].ToString();
                        t_Agency = dataReader["T_AGENCY"].ToString();
                        t_Version = dataReader["T_Version"].ToString();

                        sourceRef = new StructureReferenceImpl(s_Agency,s_ID,s_Version,SdmxStructureEnumType.CodeList,"");
                        targetRef = new StructureReferenceImpl(t_Agency, t_ID, t_Version, SdmxStructureEnumType.CodeList, "");

                        clm.SourceRef = sourceRef;
                        clm.TargetRef = targetRef;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void GetCodeListMapItems(IStructureSetMutableObject artefact, ICodelistMapMutableObject clm, long sysId)
        {
            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);

            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(StructureSetConstant.SqlCLMItem, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    ItemMapMutableCore clmItem;

                    while (dataReader.Read())
                    {
                        clmItem = new ItemMapMutableCore();

                        clmItem.SourceId = dataReader["S_ID"].ToString();
                        clmItem.TargetId = dataReader["T_ID"].ToString();

                        clm.AddItem(clmItem);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Retrieve the <see cref="IStructureSetMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="queryInfo">
        /// The query Info.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IStructureSetMutableObject}"/>.
        /// </returns>
        private ISet<IStructureSetMutableObject> GetStructureSetMutableObjects(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, SqlQueryInfo queryInfo)
        {
            var artefactSqlQuery = new ArtefactSqlQuery(queryInfo, maintainableRef);

            return this.RetrieveArtefacts(artefactSqlQuery, detail);
        }

        #endregion
    }
}