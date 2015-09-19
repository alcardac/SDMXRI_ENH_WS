// -----------------------------------------------------------------------
// <copyright file="ArtefactBaseEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-05
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
    using System.Data.Common;
    using System.Globalization;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Builder;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The artefact base.
    /// </summary>
    public abstract class ArtefactBaseEngine
    {
        #region Static Fields

        /// <summary>
        ///     The localized string insert engine.
        /// </summary>
        private static readonly LocalisedStringInsertEngine _localisedStringInsertEngine;

        /// <summary>
        /// The _annotation insert engine
        /// </summary>
        private static readonly IAnnotationInsertEngine _annotationInsertEngine;

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log;

        /// <summary>
        ///     The table info builder.
        /// </summary>
        private static readonly TableInfoBuilder _tableInfoBuilder;

        /// <summary>
        /// The _insert artefact annotation
        /// </summary>
        private static readonly InsertArtefactAnnotation _insertArtefactAnnotation;

        #endregion

        #region Fields

        /// <summary>
        /// The _database instance for Mapping Store.
        /// </summary>
        private readonly Database _database;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ArtefactBaseEngine"/> class.
        /// </summary>
        static ArtefactBaseEngine()
        {
            _log = LogManager.GetLogger(typeof(ArtefactBaseEngine));
           _localisedStringInsertEngine = new LocalisedStringInsertEngine();
           _annotationInsertEngine = new AnnotationInsertEngine();
           _tableInfoBuilder = new TableInfoBuilder();
            _insertArtefactAnnotation = new InsertArtefactAnnotation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactBaseEngine"/> class.
        /// </summary>
        /// <param name="database">The database instance for Mapping Store</param>
        protected ArtefactBaseEngine(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            this._database = database;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the database instance for Mapping Store.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        protected Database Database
        {
            get
            {
                return this._database;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Delete an ARTEFACT with the specified <paramref name="primaryKey"/> value
        /// </summary>
        /// <param name="state">
        /// The Mapping Store <see cref="DbTransactionState"/>
        /// </param>
        /// <param name="primaryKey">
        /// The primary key value.
        /// </param>
        /// <returns>
        /// The number of records deleted.
        /// </returns>
        public int Delete(DbTransactionState state, long primaryKey)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            this.DeleteChildStructures(state, primaryKey);
            _log.DebugFormat(CultureInfo.InvariantCulture, "Deleting artefact record with primary key (ART_ID) = {0}", primaryKey);
            state.ExecuteNonQueryFormat("DELETE FROM ANNOTATION WHERE ANN_ID in (select ANN_ID from ARTEFACT_ANNOTATION WHERE ART_ID = {0})", state.Database.CreateInParameter("artId", DbType.Int64, primaryKey));
            return state.ExecuteNonQueryFormat("DELETE FROM ARTEFACT WHERE ART_ID = {0}", state.Database.CreateInParameter("artId", DbType.Int64, primaryKey));
        }

        /// <summary>
        /// Returns the final status (id and is final value) of the specified <paramref name="reference"/>; otherwise it returns null
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="reference">
        /// The structure reference.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="reference"/> unsupported structure
        /// </exception>
        /// <returns>
        /// The <see cref="ArtefactFinalStatus"/> of the specified <paramref name="reference"/>; otherwise it returns null.
        /// </returns>
        public static ArtefactFinalStatus GetFinalStatus(DbTransactionState state, IStructureReference reference)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }

            var tableInfo = _tableInfoBuilder.Build(reference.MaintainableStructureEnumType.EnumType);
            if (tableInfo == null)
            {
                _log.WarnFormat("Unsupported structure type {0}", reference.MaintainableStructureEnumType.EnumType);
                return ArtefactFinalStatus.Empty;
                ////throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.ExceptionUnsupportedStructureReferenceFormat1, reference), "reference");
            }

            var finalQueryBuilder = new IsFinalQueryBuilder(state.Database);

            var query = finalQueryBuilder.Build(tableInfo);

            var maintainableRefObject = reference.MaintainableReference;
            var version = maintainableRefObject.SplitVersion(3);
            ArtefactFinalStatus artefactFinalStatus = ArtefactFinalStatus.Empty;
            state.ExecuteReaderFormat(
                query,
                reader =>
                {
                    if (reader.Read())
                    {
                        var primaryKey = DataReaderHelper.GetInt64(reader, "primaryKey");
                        var finalStatus = DataReaderHelper.GetBoolean(reader, "isFinal");
                        artefactFinalStatus = new ArtefactFinalStatus(primaryKey, finalStatus);
                    }
                },
                state.Database.CreateInParameter("id", DbType.AnsiString, maintainableRefObject.MaintainableId),
                state.Database.CreateInParameter("agency", DbType.AnsiString, maintainableRefObject.AgencyId),
                state.Database.CreateInParameter("version1", DbType.AnsiString, version[0].ToDbValue(0)),
                state.Database.CreateInParameter("version2", DbType.AnsiString, version[1].ToDbValue(0)),
                state.Database.CreateInParameter("version3", DbType.AnsiString, version[2].ToDbValue()));
           
            return artefactFinalStatus;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete the specified <paramref name="objects"/> from Mapping Store if they exist.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <typeparam name="T"> The type of maintainable object</typeparam>
        protected void DeleteObjects<T>(IEnumerable<T> objects) where T : IMaintainableObject
        {
            foreach (var maintainableObject in objects)
            {
                using (DbTransactionState state = DbTransactionState.Create(this._database))
                {
                    try
                    {
                        IStructureReference structureReference = maintainableObject.AsReference;
                        var status = GetFinalStatus(state, structureReference);
                        if (status != null && status.PrimaryKey > 0)
                        {
                            _log.DebugFormat(CultureInfo.InvariantCulture, "Deleting artefact record {0}.", structureReference.GetAsHumanReadableString());
                            this.Delete(state, status.PrimaryKey);
                        }
                        else
                        {
                            _log.WarnFormat(CultureInfo.InvariantCulture, "Failed to delete artefact record {0}.", structureReference.GetAsHumanReadableString());
                        }

                        state.Commit();
                    }
                    catch (Exception e)
                    {
                        state.RollBack();
                        _log.Error(maintainableObject.Urn, e);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the message for update failure when an artefact with the specified <paramref name="structureReference"/> already exists in MAPPING STORE and it is FINAL
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <returns>
        /// The <see cref="ImportMessage"/>.
        /// </returns>
        protected ImportMessage GetCannotReplaceMessage(IStructureReference structureReference)
        {
            // "Artefact with ID:" + artefact.GetID() + " VERSION:" + artefact.version + " AGENCY:" + artefact.agencyID + " is FINAL, so no update occurred" + Environment.NewLine;
            var artefact = structureReference.MaintainableReference;
            var message = string.Format(
                CultureInfo.InvariantCulture, 
                "Failure: {0} {1}:{2} (v{3}) cannot be updated, it is final.{4}", 
                structureReference.TargetReference, 
                artefact.AgencyId, 
                artefact.MaintainableId, 
                artefact.Version, 
                Environment.NewLine);
            _log.WarnFormat(CultureInfo.InvariantCulture, message);
            var importMessage = new ImportMessage(ImportMessageStatus.Success, structureReference, message);
            return importMessage;
        }

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
        protected abstract ArtefactImportStatus InsertArtefact(DbTransactionState state, IMaintainableObject artefact);

        /// <summary>
        /// Deletes the child items.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="primaryKey">The primary key.</param>
        protected virtual void DeleteChildStructures(DbTransactionState state, long primaryKey)
        {
        }

        /// <summary>
        /// Replace or insert the specified <paramref name="artefact"/> to MAPPING STORE
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="state"/> is null
        ///     -or-
        ///     <paramref name="artefact"/> is null
        /// </exception>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected ArtefactImportStatus ReplaceOrInsert(DbTransactionState state, IMaintainableObject artefact)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            if (artefact == null)
            {
                throw new ArgumentNullException("artefact");
            }

            var structureReference = artefact.AsReference;
            _log.DebugFormat(CultureInfo.InvariantCulture, "Replacing or Insert artefact = {0}", structureReference);
            var finalStatus = GetFinalStatus(state, structureReference);
            ArtefactImportStatus status;
            if (finalStatus == null || finalStatus.PrimaryKey < 0)
            {
                status = this.InsertArtefact(state, artefact);
            }
            else if (!finalStatus.IsFinal)
            {
                this.Delete(state, finalStatus.PrimaryKey);
                status = this.InsertArtefact(state, artefact);
            }
            else
            {
                status = new ArtefactImportStatus(finalStatus.PrimaryKey, this.GetCannotReplaceMessage(structureReference));
            }

            return status;
        }

        /// <summary>
        /// Insert or replace artefacts.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainables.
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="IMaintainableObject"/> based interface
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable{ArtefactImportStatus}"/>.
        /// </returns>
        protected IEnumerable<ArtefactImportStatus> ReplaceOrInsert<T>(IEnumerable<T> maintainables) where T : IMaintainableObject
        {
            foreach (var artefact in maintainables)
            {
                using (DbTransactionState state = DbTransactionState.Create(this._database))
                {
                    ArtefactImportStatus artefactImportStatus;
                    try
                    {
                        artefactImportStatus = this.ReplaceOrInsert(state, artefact);
                        state.Commit();
                    }
                    catch (MappingStoreException e)
                    {
                        _log.Error(artefact.Urn.ToString(), e);
                        state.RollBack();
                        artefactImportStatus = new ArtefactImportStatus(-1, artefact.AsReference.GetErrorMessage(e));
                    }
                    catch (DbException e)
                    {
                        _log.Error(artefact.Urn.ToString(), e);
                        state.RollBack();
                        artefactImportStatus = new ArtefactImportStatus(-1, artefact.AsReference.GetErrorMessage(e));
                    }

                    yield return artefactImportStatus;
                }
            }
        }

        /// <summary>
        /// Run common artefact import command.
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="artefactStoredProcedure">
        /// The artefact stored procedure.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected ArtefactImportStatus RunArtefactCommand(IMaintainableObject artefact, DbCommand command, ArtefactProcedurebase artefactStoredProcedure)
        {
            DbParameter versionParameter = artefactStoredProcedure.CreateVersionParameter(command);
            versionParameter.Value = artefact.Version ?? (object)DBNull.Value;

            DbParameter agencyParameter = artefactStoredProcedure.CreateAgencyParameter(command);
            agencyParameter.Value = artefact.AgencyId ?? (object)DBNull.Value;

            DbParameter validFromParameter = artefactStoredProcedure.CreateValidFromParameter(command);
            if (artefact.StartDate != null)
            {
                validFromParameter.Value = artefact.StartDate.Date;
            }

            DbParameter validToParameter = artefactStoredProcedure.CreateValidToParameter(command);
            if (artefact.EndDate != null)
            {
                validToParameter.Value = artefact.EndDate.Date;
            }

            DbParameter uriParameter = artefactStoredProcedure.CreateUriParameter(command);
            if (artefact.Uri != null)
            {
                uriParameter.Value = artefact.Uri.ToString();
            }

            DbParameter isFinalParameter = artefactStoredProcedure.CreateIsFinalParameter(command);
            isFinalParameter.Value = artefact.IsFinal.IsTrue ? 1 : 0;

            var artID = this.RunNameableArtefactCommand(artefact, command, artefactStoredProcedure);

            // "Inserted artefact with ID:" + artefactGetID + " VERSION:" + artefact.version + " AGENCY:" + artefact.agencyID + Environment.NewLine;
            var structureReference = artefact.AsReference;
            var importMessage = new ImportMessage(
                ImportMessageStatus.Success,
                structureReference,
                string.Format("Success: {0} was inserted.{1}", structureReference.GetAsHumanReadableString(), Environment.NewLine));

            return new ArtefactImportStatus(artID, importMessage);
        }

        /// <summary>
        /// Run common artefact import command.
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="artefactStoredProcedure">
        /// The artefact stored procedure.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        protected long RunIdentifiableArterfactCommand(IIdentifiableObject artefact, DbCommand command, ArtefactProcedurebase artefactStoredProcedure)
        {
            DbParameter idParameter = artefactStoredProcedure.CreateIdParameter(command);
            idParameter.Value = artefact.Id ?? (object)DBNull.Value;

            DbParameter outputParameter = artefactStoredProcedure.CreateOutputParameter(command);

            command.ExecuteNonQuery();

            var artID = (long)outputParameter.Value;

            _annotationInsertEngine.Insert(new DbTransactionState(command.Transaction, this._database), artID, _insertArtefactAnnotation, artefact.Annotations);

            return artID;
        }

        /// <summary>
        /// Run common artefact import command.
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="artefactStoredProcedure">
        /// The artefact stored procedure.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        protected long RunNameableArtefactCommand(INameableObject artefact, DbCommand command, ArtefactProcedurebase artefactStoredProcedure)
        {
            var artID = this.RunIdentifiableArterfactCommand(artefact, command, artefactStoredProcedure);

            DbTransactionState state = new DbTransactionState(command.Transaction, this._database);
            _localisedStringInsertEngine.InsertForArtefact(artID, artefact, state.Database);

            return artID;
        }

        #endregion
    }
}