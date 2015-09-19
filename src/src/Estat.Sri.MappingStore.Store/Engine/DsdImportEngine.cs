// -----------------------------------------------------------------------
// <copyright file="DsdImportEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-20
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Factory;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// The DSD import engine.
    /// </summary>
    public class DsdImportEngine : ArtefactImportEngine<IDataStructureObject>
    {
        /// <summary>
        /// The id parameter.
        /// </summary>
        private const string IdParameter = "id";

        /// <summary>
        /// The group id parameter.
        /// </summary>
        private const string GroupIdParameter = "groupId";

        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(DsdImportEngine));

        /// <summary>
        /// The _artefact stored procedure
        /// </summary>
        private static readonly InsertDsd _artefactStoredProcedure;

        #endregion

        #region Fields

        /// <summary>
        /// The _component import.
        /// </summary>
        private readonly IIdentifiableImportEngine<IComponent> _componentImport;

        /// <summary>
        /// The _group import.
        /// </summary>
        private readonly IIdentifiableImportEngine<IGroup> _groupImport;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="DsdImportEngine"/> class.
        /// </summary>
        static DsdImportEngine()
        {
            _artefactStoredProcedure = new StoredProcedures().InsertDsd;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DsdImportEngine"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The mapping store database instance.
        /// </param>
        public DsdImportEngine(Database connectionStringSettings)
            : this(connectionStringSettings, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DsdImportEngine"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The mapping store database instance.
        /// </param>
        /// <param name="componentImport">
        /// The component Import.
        /// </param>
        /// <param name="groupImport">
        /// The group Import.
        /// </param>
        public DsdImportEngine(Database connectionStringSettings, IIdentifiableImportFactory<IComponent> componentImport, IIdentifiableImportFactory<IGroup> groupImport)
            : base(connectionStringSettings)
        {
            var componentImportFactory = componentImport ?? new IdentifiableImportFactory<IComponent>();
            var groupImportFactory = groupImport ?? new IdentifiableImportFactory<IGroup>();
            this._componentImport = componentImportFactory.GetIdentifiableImport();
            this._groupImport = groupImportFactory.GetIdentifiableImport();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Insert the specified <paramref name="maintainable"/> to the mapping store with <paramref name="state"/>
        /// </summary>
        /// <param name="state">
        /// The MAPPING STORE connection and transaction state
        /// </param>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        public override ArtefactImportStatus Insert(DbTransactionState state, IDataStructureObject maintainable)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, "Importing artefact {0}", maintainable.Urn);
            var artefactStoredProcedure = _artefactStoredProcedure;
            var artefactStatus = this.InsertArtefactInternal(state, maintainable, artefactStoredProcedure);
            ItemStatusCollection groups = this._groupImport.Insert(state, maintainable.Groups, artefactStatus.PrimaryKeyValue);
            ItemStatusCollection components = this._componentImport.Insert(state, maintainable.GetAllComponents(), artefactStatus.PrimaryKeyValue);
            InsertDimensionGroup(state, maintainable, groups, components);
            InsertAttributeGroup(state, maintainable, groups, components);
            InsertAttributeDimensions(state, maintainable, components);
            InsertAttributeAttachmentMeasures(state, maintainable, components);
            this.InsertAsDataflow(state, maintainable, artefactStatus);

            return artefactStatus;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the child items.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="primaryKey">The primary key.</param>
        protected override void DeleteChildStructures(DbTransactionState state, long primaryKey)
        {
            state.ExecuteNonQueryFormat(
                "DELETE FROM ANNOTATION WHERE ANN_ID IN (SELECT DISTINCT ANN_ID FROM COMPONENT_ANNOTATION WHERE COMP_ID IN (SELECT COMP_ID FROM COMPONENT WHERE DSD_ID = {0}))",
                state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));
            state.ExecuteNonQueryFormat(
                "DELETE FROM ANNOTATION WHERE ANN_ID IN (SELECT DISTINCT ANN_ID FROM GROUP_ANNOTATION WHERE GR_ID IN (SELECT GR_ID FROM DSD_GROUP WHERE DSD_ID = {0}))",
                state.Database.CreateInParameter("p_fk", DbType.Int64, primaryKey));
        }

        /// <summary>
        /// Insert dimension group.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="dsd">
        /// The DSD.
        /// </param>
        /// <param name="groups">
        /// The groups.
        /// </param>
        /// <param name="components">
        /// The components.
        /// </param>
        private static void InsertDimensionGroup(DbTransactionState state, IDataStructureObject dsd, ItemStatusCollection groups, ItemStatusCollection components)
        {
            var parameterList = new List<DbParameter[]>();
            foreach (var dsdGroup in dsd.Groups)
            {
                ItemStatus dsdGroupStatus;
                if (groups.TryGetValue(dsdGroup.Id, out dsdGroupStatus))
                {
                    foreach (var dimensionRef in dsdGroup.DimensionRefs)
                    {
                        ItemStatus dimensionStatus;
                        if (components.TryGetValue(dimensionRef, out dimensionStatus))
                        {
                            var parameters = new DbParameter[2];
                            parameters[0] = state.Database.CreateInParameter(IdParameter, DbType.Int64, dimensionStatus.SysID);
                            parameters[1] = state.Database.CreateInParameter(GroupIdParameter, DbType.Int64, dsdGroupStatus.SysID);
                            parameterList.Add(parameters);
                        }
                    }
                }
            }

            state.ExecuteNonQueryFormat("insert into DIM_GROUP (COMP_ID, GR_ID) VALUES ({0}, {1})", parameterList);
        }

        /// <summary>
        /// Insert attribute group.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="dsd">
        /// The DSD.
        /// </param>
        /// <param name="components">
        /// The components.
        /// </param>
        private static void InsertAttributeDimensions(DbTransactionState state, IDataStructureObject dsd, ItemStatusCollection components)
        {
            var parameterList = new List<DbParameter[]>();
            foreach (var attributeObject in dsd.DimensionGroupAttributes)
            {
                ItemStatus attributeStatus;
                if (components.TryGetValue(attributeObject.Id, out attributeStatus))
                {
                    foreach (var dimensionReference in attributeObject.DimensionReferences)
                    {
                        ItemStatus dimensionStatus;
                        if (components.TryGetValue(dimensionReference, out dimensionStatus))
                        {
                            var parameters = new DbParameter[2];
                            parameters[0] = state.Database.CreateInParameter(IdParameter, DbType.Int64, attributeStatus.SysID);
                            parameters[1] = state.Database.CreateInParameter(GroupIdParameter, DbType.Int64, dimensionStatus.SysID);
                            parameterList.Add(parameters);
                        }
                    }
                }
            }

            state.ExecuteNonQueryFormat("insert into ATTR_DIMS (ATTR_ID, DIM_ID) VALUES ({0}, {1})", parameterList);
        }

        /// <summary>
        /// Inserts the attribute attachment measures. (SDMX V2.0 only)
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="dsd">The DSD.</param>
        /// <param name="components">The components.</param>
        private static void InsertAttributeAttachmentMeasures(DbTransactionState state, IDataStructureObject dsd, ItemStatusCollection components)
        {
            var cross = dsd as ICrossSectionalDataStructureObject;
            if (cross == null)
            {
                // it is not a SDXM v2.0 cross sectional DSD therefor there are no Measure dimensions or CrossSectional measures.
                return;
            }

            var parameterList = new List<DbParameter[]>();

            foreach (var attributeObject in cross.Attributes)
            {
                ItemStatus attributeStatus;
                if (components.TryGetValue(attributeObject.Id, out attributeStatus))
                {
                    foreach (var crossSectionalMeasure in cross.GetAttachmentMeasures(attributeObject))
                    {
                        ItemStatus measureStatus;
                        if (components.TryGetValue(crossSectionalMeasure.Id, out measureStatus))
                        {
                            var parameters = new DbParameter[2];
                            parameters[0] = state.Database.CreateInParameter(IdParameter, DbType.Int64, attributeStatus.SysID);
                            parameters[1] = state.Database.CreateInParameter("measureId", DbType.Int64, measureStatus.SysID);
                            parameterList.Add(parameters);
                        }
                    }
                }
            }

            state.ExecuteNonQueryFormat("insert into ATT_MEASURE (ATT_COMP_ID, MEASURE_COMP_ID) VALUES ({0}, {1})", parameterList);
        }

        /// <summary>
        /// Insert attribute group.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="dsd">
        /// The DSD.
        /// </param>
        /// <param name="groups">
        /// The groups.
        /// </param>
        /// <param name="components">
        /// The components.
        /// </param>
        private static void InsertAttributeGroup(DbTransactionState state, IDataStructureObject dsd, ItemStatusCollection groups, ItemStatusCollection components)
        {
            var parameterList = new List<DbParameter[]>();
            foreach (var attributeObject in dsd.GroupAttributes)
            {
                ItemStatus dsdGroupStatus;
                if (attributeObject.AttachmentGroup != null && groups.TryGetValue(attributeObject.AttachmentGroup, out dsdGroupStatus))
                {
                    ItemStatus attributeStatus;
                    if (components.TryGetValue(attributeObject.Id, out attributeStatus))
                    {
                        var parameters = new DbParameter[2];
                        parameters[0] = state.Database.CreateInParameter(IdParameter, DbType.Int64, attributeStatus.SysID);
                        parameters[1] = state.Database.CreateInParameter(GroupIdParameter, DbType.Int64, dsdGroupStatus.SysID);
                        parameterList.Add(parameters);
                    }
                }
            }

            state.ExecuteNonQueryFormat("insert into ATT_GROUP (COMP_ID, GR_ID) VALUES ({0}, {1})", parameterList);
        }

        /// <summary>
        /// Insert the specified <paramref name="maintainable"/> as dataflow if <paramref name="maintainable"/> is final.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="maintainable">
        /// The <see cref="IDataStructureObject"/>.
        /// </param>
        /// <param name="artefactStatus">
        /// The artefact status.
        /// </param>
        /// <remarks>
        /// This method inserts the specified data structure object (DSD) as dataflow. It is not an error. 
        /// </remarks>
        private void InsertAsDataflow(DbTransactionState state, IDataStructureObject maintainable, ArtefactImportStatus artefactStatus)
        {
            if (maintainable.IsFinal.IsTrue)
            {
                var dataflowStoredProcedure = new StoredProcedures().InsertDataflow;

                var artefactFinalStatus = GetFinalStatus(state, new StructureReferenceImpl(maintainable.AsReference, SdmxStructureEnumType.Dataflow));
                if (artefactFinalStatus.IsEmpty)
                {
                    this.InsertArtefactInternal(
                        state,
                        maintainable,
                        dataflowStoredProcedure,
                        command =>
                            {
                                dataflowStoredProcedure.CreateDsdIdParameter(command).Value = artefactStatus.PrimaryKeyValue;
                                dataflowStoredProcedure.CreateMapSetIdParameter(command);
                            });
                }
            }
        }

        #endregion
    }
}