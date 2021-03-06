﻿// -----------------------------------------------------------------------
// <copyright file="NameableBaseEngine.cs" company="EUROSTAT">
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
    using System.Data.Common;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The nameable base engine.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="INameableObject"/></typeparam>
    /// <typeparam name="TProc">The type of the procedure.</typeparam>
    public class NameableBaseEngine<T, TProc> : INameableImportEngine<T, TProc>
        where T : INameableObject where TProc : IIdentifiableProcedure
    {
        #region Fields

        /// <summary>
        ///     The localized string insert engine.
        /// </summary>
        private readonly LocalisedStringInsertEngine _localisedStringInsertEngine = new LocalisedStringInsertEngine();

        /// <summary>
        /// The _annotation insert engine
        /// </summary>
        private readonly IAnnotationInsertEngine _annotationInsertEngine = new AnnotationInsertEngine();

        /// <summary>
        /// The _annotation procedure base
        /// </summary>
        private readonly InsertItemAnnotation _annotationProcedureBase = new InsertItemAnnotation();

        #endregion

        #region Methods

        /// <summary>
        /// The run common <see cref="INameableObject" /> command for the specified <paramref name="item" />
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="command">The command.</param>
        /// <param name="itemProcedure">The item procedure.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The primary key of the item added.
        /// </returns>
        public long RunCommand(T item, DbCommand command, TProc itemProcedure, DbTransactionState state)
        {
            DbParameter idParameter = itemProcedure.CreateIdParameter(command);
            idParameter.Value = item.Id;

            DbParameter outputParameter = itemProcedure.CreateOutputParameter(command);

            command.ExecuteNonQuery();

            var itemID = (long)outputParameter.Value;

            this._localisedStringInsertEngine.InsertForItem(itemID, item, state.Database);

            this._annotationInsertEngine.Insert(state, itemID, this._annotationProcedureBase, ((IAnnotableObject)item).Annotations);
            return itemID;
        }

        #endregion
    }
}