// -----------------------------------------------------------------------
// <copyright file="ArtefactImportEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-09
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
    using System.Linq;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The artefact import engine.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the maintainable
    /// </typeparam>
    public abstract class ArtefactImportEngine<T> : ArtefactBaseEngine, IImportEngine<T>, IDeleteEngine<T>
        where T : IMaintainableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactImportEngine{T}"/> class.
        /// </summary>
        /// <param name="database">
        /// The mapping store database instance.
        /// </param>
        protected ArtefactImportEngine(Database database)
            : base(database)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Delete the specified <paramref name="objects"/> from Mapping Store if they exist.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        public void Delete(IEnumerable<T> objects)
        {
            this.DeleteObjects(objects);
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
        public abstract ArtefactImportStatus Insert(DbTransactionState state, T maintainable);

        /// <summary>
        /// Insert the specified <paramref name="maintainables"/> to the mapping store.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<ArtefactImportStatus> Insert(IEnumerable<T> maintainables)
        {
            return this.ReplaceOrInsert(maintainables).ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert the specified <paramref name="artefact"/> to MAPPING STORE
        /// </summary>
        /// <param name="state">
        /// The Mapping Store connection and transaction state
        /// </param>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected override ArtefactImportStatus InsertArtefact(DbTransactionState state, IMaintainableObject artefact)
        {
            return this.Insert(state, (T)artefact);
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
        /// <param name="artefactStoredProcedure">
        /// The artefact Stored Procedure.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected ArtefactImportStatus InsertArtefactInternal(DbTransactionState state, T maintainable, ArtefactProcedurebase artefactStoredProcedure)
        {
            return this.InsertArtefactInternal(state, maintainable, artefactStoredProcedure, null);
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
        /// <param name="artefactStoredProcedure">
        /// The artefact Stored Procedure.
        /// </param>
        /// <param name="setupCommand">
        /// The setup Command. Use it to make additional changes to the command
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected ArtefactImportStatus InsertArtefactInternal(DbTransactionState state, T maintainable, ArtefactProcedurebase artefactStoredProcedure, Action<DbCommand> setupCommand)
        {
            ArtefactImportStatus artefactImportStatus;
            using (DbCommand command = artefactStoredProcedure.CreateCommandWithDefaults(state))
            {
                if (setupCommand != null)
                {
                    setupCommand(command);
                }

                artefactImportStatus = this.RunArtefactCommand(maintainable, command, artefactStoredProcedure);
            }

            return artefactImportStatus;
        }

        #endregion
    }
}