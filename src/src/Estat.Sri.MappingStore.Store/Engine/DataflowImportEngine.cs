// -----------------------------------------------------------------------
// <copyright file="DataflowImportEngine.cs" company="EUROSTAT">
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
    using System.Globalization;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// The dataflow import engine.
    /// </summary>
    public class DataflowImportEngine : ArtefactImportEngine<IDataflowObject>
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(DataflowImportEngine));

        /// <summary>
        /// The artefact stored procedure
        /// </summary>
        private static readonly InsertDataflow _artefactStoredProcedure;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="DataflowImportEngine"/> class.
        /// </summary>
        static DataflowImportEngine()
        {
            _artefactStoredProcedure = new StoredProcedures().InsertDataflow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowImportEngine"/> class. 
        /// </summary>
        /// <param name="database">
        /// The mapping store database instance.
        /// </param>
        public DataflowImportEngine(Database database)
            : base(database)
        {
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
        public override ArtefactImportStatus Insert(DbTransactionState state, IDataflowObject maintainable)
        {
            var dsdStatus = GetFinalStatus(state, maintainable.DataStructureRef);
            if (dsdStatus != null && (dsdStatus.PrimaryKey > 0 && dsdStatus.IsFinal))
            {
                _log.DebugFormat(CultureInfo.InvariantCulture, "Importing artefact {0}", maintainable.Urn);

                var artefactStoredProcedure = _artefactStoredProcedure;

                return this.InsertArtefactInternal(state, maintainable, artefactStoredProcedure, command => { artefactStoredProcedure.CreateDsdIdParameter(command).Value = dsdStatus.PrimaryKey; artefactStoredProcedure.CreateMapSetIdParameter(command); });
            }

            //// DSD issues

            var importMessage = BuildErrorMessage(maintainable, dsdStatus);

            return new ArtefactImportStatus(-1, importMessage);
        }

        /// <summary>
        /// Build the error message based on the specified <paramref name="dsdStatus"/>.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="dsdStatus">
        /// The <c>DSD</c> status
        /// </param>
        /// <returns>
        /// The <see cref="ImportMessage"/>.
        /// </returns>
        private static ImportMessage BuildErrorMessage(IDataflowObject maintainable, ArtefactFinalStatus dsdStatus)
        {
            var maintainableRefObject = maintainable.DataStructureRef.MaintainableReference;
            var message = string.Format(
                CultureInfo.InvariantCulture,
                "Dataflow {0} uses the DSD:\r\n ID: {1}\r\n VERSION:{2}\r\n AGENCY: {3}\r\n which doesn't exist in the Mapping Store",
                maintainable.Id,
                maintainableRefObject.MaintainableId,
                maintainableRefObject.Version,
                maintainableRefObject.AgencyId);
            ImportMessage importMessage;
            var structureReference = maintainable.AsReference;

            if (dsdStatus == null || dsdStatus.PrimaryKey < 1)
            {
                var doesntExistMessage = string.Format(CultureInfo.InvariantCulture, "{0} which doesn't exist in the Mapping Store", message);
                _log.Error(doesntExistMessage);
                importMessage = new ImportMessage(ImportMessageStatus.Error, structureReference, doesntExistMessage);
            }
            else
            {
                var notFinalMessage = string.Format(CultureInfo.InvariantCulture, "{0} which is not Final", message);
                _log.Error(notFinalMessage);
                importMessage = new ImportMessage(ImportMessageStatus.Error, structureReference, notFinalMessage);
            }

            return importMessage;
        }

        #endregion
    }
}