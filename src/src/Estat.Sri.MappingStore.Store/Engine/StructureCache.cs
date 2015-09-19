// -----------------------------------------------------------------------
// <copyright file="StructureCache.cs" company="EUROSTAT">
//   Date Created : 2013-04-28
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
    using System.Globalization;
    using System.Linq;

    using Dapper;

    using Estat.Sri.MappingStore.Store.Builder;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStore.Store.Properties;
    using Estat.Sri.MappingStoreRetrieval.Builder;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The structure cache.
    /// </summary>
    internal class StructureCache
    {
        #region Fields

        /// <summary>
        ///     The table info builder.
        /// </summary>
        private static readonly ItemTableInfoBuilder _tableInfoBuilder = new ItemTableInfoBuilder();

        /// <summary>
        ///     The _dictionary.
        /// </summary>
        private readonly IDictionary<IStructureReference, ItemSchemeFinalStatus> _dictionary = new Dictionary<IStructureReference, ItemSchemeFinalStatus>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the <c>ITEM.ITEM_ID</c> value from Mapping Store for <paramref name="artefactId"/>
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="sdmxStructure">
        /// The SDMX Structure.
        /// </param>
        /// <param name="artefactId">
        /// The artefact primary key. <c>ARTEFACT.ART_ID</c>.
        /// </param>
        /// <returns>
        /// The <c>ITEM.ITEM_ID</c> value from Mapping Store for <paramref name="artefactId"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="state"/> is null 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// At <paramref name="sdmxStructure"/>, unsupported structure
        /// </exception>
        public static ItemStatusCollection GetId(DbTransactionState state, SdmxStructureEnumType sdmxStructure, long artefactId)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            var tableInfo = _tableInfoBuilder.Build(sdmxStructure);
            if (tableInfo == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.ExceptionUnsupportedStructureReferenceFormat1, sdmxStructure), "sdmxStructure");
            }

            var itemIdQueryBuilder = new ItemIdQueryBuilder(state.Database);

            var query = itemIdQueryBuilder.Build(tableInfo);
            return new ItemStatusCollection(state.Connection.Query<ItemStatus>(
                query,
                new { id = artefactId },
                state.Transaction,
                false));
        }

        /// <summary>
        /// Gets the component map ids.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="dsdReference">
        /// The DSD reference.
        /// </param>
        /// <returns>
        /// The component id to primary key value dictionary
        /// </returns>
        public static IDictionary<string, long> GetComponentMapIds(DbTransactionState state, IStructureReference dsdReference)
        {
            IDictionary<string, long> map = new Dictionary<string, long>(StringComparer.Ordinal);

            var idParameter = state.Database.CreateInParameter("p_id", DbType.AnsiString, dsdReference.MaintainableId);
            var agencyParameter = state.Database.CreateInParameter("p_agency", DbType.AnsiString, dsdReference.AgencyId);
            var versionParameter = state.Database.CreateInParameter("p_version", DbType.AnsiString, dsdReference.Version);

            var queryFormat = dsdReference.MaintainableStructureEnumType.EnumType == SdmxStructureEnumType.Dsd
                                  ? "select c.COMP_ID, c.ID from COMPONENT c inner join ARTEFACT_VIEW a on a.ART_ID = c.DSD_ID where a.ID = {0} and a.AGENCY = {1} and a.VERSION = {2}"
                                  : "select c.COMP_ID, c.ID from COMPONENT c inner join DATAFLOW d on d.DSD_ID = c.DSD_ID inner join ARTEFACT_VIEW a on a.ART_ID = d.DF_ID where a.ID = {0} and a.AGENCY = {1} and a.VERSION = {2}";
            using (var command = state.Database.GetSqlStringCommandFormat(queryFormat, idParameter, agencyParameter, versionParameter))
            using (var reader = state.Database.ExecuteReader(command))
            {
                var sysIdOrdinal = reader.GetOrdinal("COMP_ID");
                var idOrdinal = reader.GetOrdinal("ID");

                while (reader.Read())
                {
                    map.Add(reader.GetString(idOrdinal), reader.GetInt64(sysIdOrdinal));
                }
            }

            return map;
        }

        /// <summary>
        /// Gets the structure.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="reference">The reference.</param>
        /// <returns>The <see cref="ItemStatusCollection"/></returns>
        public ItemSchemeFinalStatus GetStructure(DbTransactionState state, IStructureReference reference)
        {
            ItemSchemeFinalStatus returnObjet;
            if (!this._dictionary.TryGetValue(reference, out returnObjet))
            {
                var artefactFinalStatus = ArtefactBaseEngine.GetFinalStatus(state, reference);

                ItemStatusCollection collection = null;
                if (artefactFinalStatus != null && artefactFinalStatus.IsFinal && reference.HasChildReference())
                {
                    switch (reference.TargetReference.EnumType)
                    {
                        case SdmxStructureEnumType.Component:
                        case SdmxStructureEnumType.Dimension:
                        case SdmxStructureEnumType.TimeDimension:
                        case SdmxStructureEnumType.MeasureDimension:
                        case SdmxStructureEnumType.DataAttribute:
                        case SdmxStructureEnumType.PrimaryMeasure:
                        case SdmxStructureEnumType.CrossSectionalMeasure:
                            var map = GetComponentMapIds(state, reference);
                            collection = new ItemStatusCollection(map.Select(pair => new ItemStatus(pair.Key, pair.Value)));
                            break;
                        default:
                            collection = GetId(state, reference.TargetReference.EnumType, artefactFinalStatus.PrimaryKey);
                            break;
                    }
                }

                returnObjet = new ItemSchemeFinalStatus(artefactFinalStatus ?? ArtefactFinalStatus.Empty, collection);
            }

            return returnObjet;
        }

        #endregion
    }
}