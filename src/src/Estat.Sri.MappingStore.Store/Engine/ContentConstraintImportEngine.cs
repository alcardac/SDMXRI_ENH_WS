// -----------------------------------------------------------------------
// <copyright file="ContentConstraintImportEngine.cs" company="EUROSTAT">
//   Date Created : 2014-10-13
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

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    /// The content constraint import engine.
    /// </summary>
    public class ContentConstraintImportEngine : ArtefactImportEngine<IContentConstraintObject>
    {
        /// <summary>
        /// The validate status engine
        /// </summary>
        private readonly ValidateStatusEngine _validateStatusEngine = new ValidateStatusEngine();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentConstraintImportEngine"/> class. 
        /// </summary>
        /// <param name="database">
        /// The mapping store database instance.
        /// </param>
        public ContentConstraintImportEngine(Database database)
            : base(database)
        {
        }

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
        public override ArtefactImportStatus Insert(DbTransactionState state, IContentConstraintObject maintainable)
        {
            var insertContentConstraint = new InsertContentConstraintProcedure();

            var artefactStatus = this.InsertArtefactInternal(
                state,
                maintainable,
                insertContentConstraint,
                command =>
                    {
                        insertContentConstraint.CreateActualDataParameter(command).Value = maintainable.IsDefiningActualDataPresent;
                        if (maintainable.ReferencePeriod != null)
                        {
                            if (!string.IsNullOrWhiteSpace(maintainable.ReleaseCalendar.Offset))
                            {
                                insertContentConstraint.CreateOffsetParameter(command).Value = maintainable.ReleaseCalendar.Offset;
                            }

                            if (!string.IsNullOrWhiteSpace(maintainable.ReleaseCalendar.Periodicity))
                            {
                                insertContentConstraint.CreatePeriodicityParameter(command).Value = maintainable.ReleaseCalendar.Periodicity;
                            }

                            if (!string.IsNullOrWhiteSpace(maintainable.ReleaseCalendar.Tolerance))
                            {
                                insertContentConstraint.CreateToleranceParameter(command).Value = maintainable.ReleaseCalendar.Tolerance;
                            }
                        }
                    });
           this.InsertContentConstraintAttachment(state, maintainable, artefactStatus);

            if (maintainable.IncludedCubeRegion != null)
            {
                InsertCubeRegion(state, maintainable.IncludedCubeRegion, artefactStatus, true);
            }

            if (maintainable.ExcludedCubeRegion != null)
            {
                InsertCubeRegion(state, maintainable.ExcludedCubeRegion, artefactStatus, false);
            }

            return artefactStatus;
        }

        /// <summary>
        /// Inserts the cube region.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="cubeRegion">The cube region.</param>
        /// <param name="artefactStatus">The artefact status.</param>
        /// <param name="isInclude">if set to <c>true</c> [is include].</param>
        private static void InsertCubeRegion(DbTransactionState state, ICubeRegion cubeRegion, ArtefactImportStatus artefactStatus, bool isInclude)
        {
            var procedure = new InsertCubeRegionProcedure();
            long cubeRegionPrimaryKey;
            using (var command = procedure.CreateCommand(state))
            {
                procedure.CreateContentConstraintIdParameter(command, artefactStatus.PrimaryKeyValue);
                procedure.CreateIncludeParameter(command, isInclude);
                var outputParameter = procedure.CreateOutputParameter(command);
                command.ExecuteNonQuery();
                cubeRegionPrimaryKey = (long)outputParameter.Value;
            }

            // handle dimensions
            var keyValuesWithId = InsertKeyValues(state, isInclude, cubeRegionPrimaryKey, cubeRegion.KeyValues, SdmxComponentType.Dimension);

            InsertValues(state, isInclude, keyValuesWithId);

            // handle attributes
            var attrValuesWithId = InsertKeyValues(state, isInclude, cubeRegionPrimaryKey, cubeRegion.AttributeValues, SdmxComponentType.Attribute);

            InsertValues(state, isInclude, attrValuesWithId);
        }

        /// <summary>
        /// Inserts the values.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="isInclude">if set to <c>true</c> [is include].</param>
        /// <param name="keyValuesWithId">The key values with identifier.</param>
        private static void InsertValues(DbTransactionState state, bool isInclude, IEnumerable<Tuple<long, IKeyValues>> keyValuesWithId)
        {
            var valueProc = new InsertCubeRegionValueProcedure();
            using (var command = valueProc.CreateCommand(state))
            {
                valueProc.CreateIncludeParameter(command, isInclude);
                foreach (var tuple in keyValuesWithId)
                {
                    var cubeRegionKeyValueId = tuple.Item1;
                    var cubeRegionKeyValue = tuple.Item2;
                    valueProc.CreateCubeRegionKeyValueIdParameter(command, cubeRegionKeyValueId);
                    foreach (var value in cubeRegionKeyValue.Values)
                    {
                        valueProc.CreateMemberValueParameter(command, value);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the key values.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="isInclude">if set to <c>true</c> [is include].</param>
        /// <param name="cubeRegionPrimaryKey">The cube region primary key.</param>
        /// <param name="keyValuesCollection">The key values collection.</param>
        /// <param name="sdmxComponentType">Type of the SDMX component.</param>
        /// <returns>
        /// The list of primary key, <see cref="IKeyValues" /> and <see cref="IComponent" />
        /// </returns>
        private static IEnumerable<Tuple<long, IKeyValues>> InsertKeyValues(DbTransactionState state, bool isInclude, long cubeRegionPrimaryKey, ICollection<IKeyValues> keyValuesCollection, SdmxComponentType sdmxComponentType)
        {
            var dimensionType = sdmxComponentType.ToString();
            var keyValuesWithId = new List<Tuple<long, IKeyValues>>(keyValuesCollection.Count);
            var keyValueProc = new InsertCubeRegionKeyValueProcedure();
            using (var command = keyValueProc.CreateCommand(state))
            {
                keyValueProc.CreateIncludeParameter(command, isInclude);
                keyValueProc.CreateCubeRegionIdParameter(command, cubeRegionPrimaryKey);
                foreach (var keyValues in keyValuesCollection)
                {
                    keyValueProc.CreateMemberIdParameter(command, keyValues.Id);
                    keyValueProc.CreateComponentTypeParameter(command, dimensionType);
                    var outputParameter = keyValueProc.CreateOutputParameter(command);
                    command.ExecuteNonQuery();
                    keyValuesWithId.Add(new Tuple<long, IKeyValues>((long)outputParameter.Value, keyValues));
                }
            }

            return keyValuesWithId;
        }

        /// <summary>
        /// Inserts the content constraint attachment.
        /// </summary>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <param name="maintainable">
        ///     The maintainable.
        /// </param>
        /// <param name="artefactStatus">
        ///     The artefact status.
        /// </param>
        private void InsertContentConstraintAttachment(DbTransactionState state, IContentConstraintObject maintainable, ArtefactImportStatus artefactStatus)
        {
            if (maintainable.ConstraintAttachment == null)
            {
                return;
            }

            var procedure = new InsertContentConstraintAttachmentProcedure();
            var structureCache = new StructureCache();
            using (var command = procedure.CreateCommand(state))
            {
                procedure.CreateContentConstraintIdParameter(command, artefactStatus.PrimaryKeyValue);
                foreach (var crossReference in maintainable.ConstraintAttachment.StructureReference)
                {
                    var itemSchemeFinalStatus = structureCache.GetStructure(state, crossReference);
                    this._validateStatusEngine.ValidateFinalStatus(itemSchemeFinalStatus.FinalStatus, crossReference);
                    procedure.CreateArtefactIdParameter(command, itemSchemeFinalStatus.FinalStatus.PrimaryKey);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}