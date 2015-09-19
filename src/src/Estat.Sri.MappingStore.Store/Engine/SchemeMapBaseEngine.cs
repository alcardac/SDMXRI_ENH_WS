// -----------------------------------------------------------------------
// <copyright file="SchemeMapBaseEngine.cs" company="EUROSTAT">
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
    using System.Collections.Generic;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The Schema base engine.
    /// </summary>
    /// <typeparam name="TSchemaMap">The type of the schema map.</typeparam>
    /// <typeparam name="TProc">The type of the procedure.</typeparam>
    internal abstract class SchemeMapBaseEngine<TSchemaMap, TProc>
        where TSchemaMap : ISchemeMapObject
        where TProc : SchemeMapProcedureBase, new()
    {
        /// <summary>
        /// The _nameable import engine
        /// </summary>
        private readonly INameableImportEngine<TSchemaMap, TProc> _nameableImportEngine;

        /// <summary>
        /// The _validate status engine
        /// </summary>
        private readonly ValidateStatusEngine _validateStatusEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemeMapBaseEngine{TSchemaMap,TProc}"/> class.
        /// </summary>
        protected SchemeMapBaseEngine()
        {
            this._nameableImportEngine = new NameableBaseEngine<TSchemaMap, TProc>();
            this._validateStatusEngine = new ValidateStatusEngine();
        }

        /// <summary>
        /// Inserts the specified item.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="schemaMaps">The schema maps.</param>
        /// <param name="structureSetId">The structure set identifier.</param>
        /// <returns>
        /// The primary key value
        /// </returns>
        public IEnumerable<long> Insert(DbTransactionState state, IEnumerable<TSchemaMap> schemaMaps, long structureSetId)
        {
            var procedure = new TProc();
            var cache = new StructureCache();
            var primaryKeys = new List<long>();
            foreach (var schemaMap in schemaMaps)
            {
                long primaryKey;
                using (var command = procedure.CreateCommandWithDefaults(state))
                {
                    procedure.CreateParentIdParameter(command).Value = structureSetId;
                    var sourceCodelistStatus = this._validateStatusEngine.GetReferenceStatus(state, schemaMap.SourceRef, cache);
                    var targetCodelistStatus = this._validateStatusEngine.GetReferenceStatus(state, schemaMap.TargetRef, cache);
                    procedure.CreateSourceIdParameter(command).Value = sourceCodelistStatus.FinalStatus.PrimaryKey;
                    procedure.CreateTargetIdParameter(command).Value = targetCodelistStatus.FinalStatus.PrimaryKey;
                    primaryKey = this._nameableImportEngine.RunCommand(schemaMap, command, procedure, state);
                }

                primaryKeys.Add(primaryKey);
                this.WriteItemMaps(state, schemaMap, primaryKey);
            }

            return primaryKeys;
        }

        /// <summary>
        /// Gets the CodeList status.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="codeListReference">The codeList reference.</param>
        /// <param name="itemScheme">The item scheme.</param>
        /// <returns>
        /// The <see cref="ItemSchemeFinalStatus"/>.
        /// </returns>
        protected ItemSchemeFinalStatus GetReferenceStatus(DbTransactionState state, IStructureReference codeListReference, StructureCache itemScheme)
        {
            return this._validateStatusEngine.GetReferenceStatus(state, codeListReference, itemScheme);
        }

        /// <summary>
        /// Writes the item maps.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="schemaMap">The schema map.</param>
        /// <param name="primaryKey">The primary key.</param>
        protected abstract void WriteItemMaps(DbTransactionState state, TSchemaMap schemaMap, long primaryKey);
    }
}