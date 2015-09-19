// -----------------------------------------------------------------------
// <copyright file="ItemSchemeMapImportEngine.cs" company="EUROSTAT">
//   Date Created : 2014-10-10
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
    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// The <see cref="IItemSchemeMapObject"/> based import engine.
    /// </summary>
    /// <typeparam name="TSchemaMap">
    /// The type of the schema map.
    /// </typeparam>
    /// <typeparam name="TProc">
    /// The type of the procedure.
    /// </typeparam>
    /// <typeparam name="TMapItemProc">
    /// The type of the map item procedure.
    /// </typeparam>
    internal abstract class ItemSchemeMapImportEngine<TSchemaMap, TProc, TMapItemProc> : SchemeMapBaseEngine<TSchemaMap, TProc>
        where TSchemaMap : IItemSchemeMapObject where TProc : SchemeMapProcedureBase, new() where TMapItemProc : MapProcedureBase, new()
    {
        #region Fields

        /// <summary>
        ///     The _child type
        /// </summary>
        private readonly SdmxStructureEnumType _childType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMapImportEngine{TSchemaMap,TProc,TMapItemProc}"/> class.
        /// </summary>
        /// <param name="childType">
        /// Type of the child.
        /// </param>
        protected ItemSchemeMapImportEngine(SdmxStructureEnumType childType)
        {
            this._childType = childType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes the item maps.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="schemaMap">
        /// The schema map.
        /// </param>
        /// <param name="primaryKey">
        /// The primary key.
        /// </param>
        protected override void WriteItemMaps(DbTransactionState state, TSchemaMap schemaMap, long primaryKey)
        {
            var cache = new StructureCache();
            var mapProcedure = new TMapItemProc();
            var sourceItemSchemeRef = schemaMap.SourceRef;
            var targetItemSchemeRef = schemaMap.TargetRef;
            using (var command = mapProcedure.CreateCommandWithDefaults(state))
            {
                mapProcedure.CreateParentIdParameter(command).Value = primaryKey;
                foreach (var itemMap in schemaMap.Items)
                {
                    var sourceRef = new StructureReferenceImpl(sourceItemSchemeRef.AgencyId, sourceItemSchemeRef.MaintainableId, sourceItemSchemeRef.Version, this._childType, itemMap.SourceId);
                    var sourceRefStatus = this.GetReferenceStatus(state, sourceRef, cache);
                    mapProcedure.CreateSourceIdParameter(command).Value = sourceRefStatus.ItemIdMap[itemMap.SourceId].SysID;

                    var targetRef = new StructureReferenceImpl(targetItemSchemeRef.AgencyId, targetItemSchemeRef.MaintainableId, targetItemSchemeRef.Version, this._childType, itemMap.TargetId);
                    var targetRefStatus = this.GetReferenceStatus(state, targetRef, cache);
                    mapProcedure.CreateTargetIdParameter(command).Value = targetRefStatus.ItemIdMap[itemMap.TargetId].SysID;

                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}