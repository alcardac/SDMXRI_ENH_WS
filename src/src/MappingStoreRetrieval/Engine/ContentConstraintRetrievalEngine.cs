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
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// 
    /// </summary>
    internal class ContentConstraintRetrievalEngine : ArtefactRetrieverEngine<IContentConstraintMutableObject>
    {
        #region Constructors and Destructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mappingStoreDb"></param>
        public ContentConstraintRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb)
        {
            var sqlQueryBuilder = new ReferencedSqlQueryBuilder(this.MappingStoreDb, null);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maintainableRef"></param>
        /// <param name="detail"></param>
        /// <param name="versionConstraints"></param>
        /// <returns></returns>
        public override ISet<IContentConstraintMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            var sqlInfo = versionConstraints == VersionQueryType.Latest ? this.SqlQueryInfoForLatest : this.SqlQueryInfoForAll;
            return this.GetConstraintMutableObjects(maintainableRef, detail, sqlInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maintainableRef"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        public override IContentConstraintMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            ISet<IContentConstraintMutableObject> mutableObjects = this.GetConstraintMutableObjects(maintainableRef, detail, this.SqlQueryInfoForLatest);
            
            if(mutableObjects.Count > 0)
                foreach (IContentConstraintMutableObject cons in mutableObjects)
                {
                    return cons;
                }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IContentConstraintMutableObject CreateArtefact()
        {
            return new ContentConstraintMutableCore();
        }

 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        protected override IContentConstraintMutableObject RetrieveDetails(IContentConstraintMutableObject artefact, long sysId)
        {
            PopulateConstraintGeneral(artefact, sysId);
            PopulateCubeRegion(artefact, sysId);
            return artefact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void PopulateConstraintGeneral(IContentConstraintMutableObject artefact, long sysId)
        {
            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);

            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(ContentConstraintConstant.SqlConsInfo, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    bool bGeneral = true;

                    artefact.ReleaseCalendar = new ReleaseCalendarMutableCore();
                    artefact.ConstraintAttachment = new ContentConstraintAttachmentMutableCore();

                    while (dataReader.Read())
                    {
                        if (bGeneral)
                        {
                            if (dataReader["PERIODICITY"] != DBNull.Value)
                                artefact.ReleaseCalendar.Periodicity = dataReader["PERIODICITY"].ToString();

                            if (dataReader["OFFSET"] != DBNull.Value)
                                artefact.ReleaseCalendar.Offset = dataReader["OFFSET"].ToString();

                            if (dataReader["TOLERANCE"] != DBNull.Value)
                                artefact.ReleaseCalendar.Tolerance = dataReader["TOLERANCE"].ToString();

                            if (String.IsNullOrEmpty(artefact.ReleaseCalendar.Periodicity))
                                artefact.ReleaseCalendar = null;

                            bGeneral = false;
                        }


                        IStructureReference structRef = new StructureReferenceImpl(dataReader["AGENCY"].ToString(),
                                                    dataReader["ID"].ToString(), dataReader["VERSION"].ToString(),
                                                    SdmxStructureType.GetFromEnum((SdmxStructureEnumType)Enum.Parse(typeof(SdmxStructureEnumType), dataReader["ARTEFACT_TYPE"].ToString())),
                                                    new string[] { });

                        artefact.ConstraintAttachment.AddStructureReference(structRef);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artefact"></param>
        /// <param name="sysId"></param>
        private void PopulateCubeRegion(IContentConstraintMutableObject artefact, long sysId)
        {
            var inParameter = MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, sysId);
            using (DbCommand command = MappingStoreDb.GetSqlStringCommandParam(ContentConstraintConstant.SqlConsItem, inParameter))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {

                    string keyValueIDCurr = String.Empty;
                    string componentTypeCurr = String.Empty;

                    artefact.IncludedCubeRegion = new CubeRegionMutableCore();

                    IKeyValuesMutable key = null;

                    while (dataReader.Read())
                    {
                        if (dataReader["CUBE_REGION_KEY_VALUE_ID"].ToString() != keyValueIDCurr)
                        {
                            if (key != null)
                            {
                                if (componentTypeCurr == "Attribute")
                                    artefact.IncludedCubeRegion.AddAttributeValue(key);
                                if (componentTypeCurr == "Dimension")
                                    artefact.IncludedCubeRegion.AddKeyValue(key);
                            }

                            keyValueIDCurr = dataReader["CUBE_REGION_KEY_VALUE_ID"].ToString();
                            componentTypeCurr = dataReader["COMPONENT_TYPE"].ToString();

                            key = new KeyValuesMutableImpl();
                        }

                        if (String.IsNullOrEmpty(key.Id))
                            key.Id = dataReader["MEMBER_ID"].ToString();

                        key.AddValue(dataReader["MEMBER_VALUE"].ToString());
                    }

                    if (componentTypeCurr == "Attribute")
                        artefact.IncludedCubeRegion.AddAttributeValue(key);
                    if (componentTypeCurr == "Dimension")
                        artefact.IncludedCubeRegion.AddKeyValue(key);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maintainableRef"></param>
        /// <param name="detail"></param>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        private ISet<IContentConstraintMutableObject> GetConstraintMutableObjects(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, SqlQueryInfo queryInfo)
        {
            var artefactSqlQuery = new ArtefactSqlQuery(queryInfo, maintainableRef);

            return this.RetrieveArtefacts(artefactSqlQuery, detail);
        }

        #endregion
    }
}