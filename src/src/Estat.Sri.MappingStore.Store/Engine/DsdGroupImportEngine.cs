// -----------------------------------------------------------------------
// <copyright file="DsdGroupImportEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-24
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
    using System.Data.Common;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    ///     The DSD group import engine.
    /// </summary>
    public class DsdGroupImportEngine : IIdentifiableImportEngine<IGroup>
    {
        /// <summary>
        /// The _insert DSD group
        /// </summary>
        private static readonly InsertDsdGroup _insertDsdGroup;

        /// <summary>
        /// The _annotation insert engine
        /// </summary>
        private static readonly IAnnotationInsertEngine _annotationInsertEngine;

        /// <summary>
        /// The _insert group annotation
        /// </summary>
        private static readonly InsertGroupAnnotation _insertGroupAnnotation;

        /// <summary>
        /// Initializes static members of the <see cref="DsdGroupImportEngine"/> class.
        /// </summary>
        static DsdGroupImportEngine()
        {
            _insertDsdGroup = new StoredProcedures().InsertDsdGroup;
            _insertGroupAnnotation = new InsertGroupAnnotation();
            _annotationInsertEngine = new AnnotationInsertEngine();
        }

        #region Public Methods and Operators

        /// <summary>
        /// Insert the specified <paramref name="items"/> to the mapping store with <paramref name="state"/>
        /// </summary>
        /// <param name="state">
        ///     The MAPPING STORE connection and transaction state
        /// </param>
        /// <param name="items">
        ///     The items.
        /// </param>
        /// <param name="parentArtefact">
        ///     The primary key of the parent artefact.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{Long}"/>.
        /// </returns>
        public ItemStatusCollection Insert(DbTransactionState state, IEnumerable<IGroup> items, long parentArtefact)
        {
            var storedProcedure = _insertDsdGroup;
            var annotations = new List<Tuple<long, IGroup>>();
            var groupIds = new ItemStatusCollection();
            using (DbCommand command = storedProcedure.CreateCommand(state))
            {
                DbParameter dsdParameter = storedProcedure.CreateDsdIdParameter(command);

                DbParameter idParameter = storedProcedure.CreateIdParameter(command);

                DbParameter outputParameter = storedProcedure.CreateOutputParameter(command);

                foreach (var group in items)
                {
                    idParameter.Value = group.Id;
                    dsdParameter.Value = parentArtefact;

                    command.ExecuteNonQuery();

                    var id = (long)outputParameter.Value;
                    groupIds.Add(new ItemStatus(group.Id, id));
                    if (group.Annotations.Count > 0)
                    {
                        annotations.Add(new Tuple<long, IGroup>(id, group));
                    }
                }
            }

            foreach (var annotation in annotations)
            {
                IAnnotableObject annotableObject = annotation.Item2;
                _annotationInsertEngine.Insert(state, annotation.Item1, _insertGroupAnnotation, annotableObject.Annotations);
            }

            return groupIds;
        }

        #endregion
    }
}