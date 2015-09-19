// -----------------------------------------------------------------------
// <copyright file="StructureMapEngine.cs" company="EUROSTAT">
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
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The structure map engine.
    /// </summary>
    internal class StructureMapEngine : SchemeMapBaseEngine<IStructureMapObject, InsertStructureMapProcedure>
    {
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
        protected override void WriteItemMaps(DbTransactionState state, IStructureMapObject schemaMap, long primaryKey)
        {
            var componentMapProcedure = new InsertComponentMapProcedure();
            var sourceMap = StructureCache.GetComponentMapIds(state, schemaMap.SourceRef);
            var targetMap = StructureCache.GetComponentMapIds(state, schemaMap.TargetRef);

            using (var command = componentMapProcedure.CreateCommandWithDefaults(state))
            {
                componentMapProcedure.CreateParentIdParameter(command).Value = primaryKey;
                foreach (var componentMapObject in schemaMap.Components)
                {
                    componentMapProcedure.CreateSourceIdParameter(command).Value = sourceMap[componentMapObject.MapConceptRef];
                    componentMapProcedure.CreateTargetIdParameter(command).Value = targetMap[componentMapObject.MapTargetConceptRef];
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}