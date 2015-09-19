namespace Estat.Sri.Ws.SubmitStructure
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    using Estat.Sri.MappingStore.Store;
    using Estat.Sri.MappingStore.Store.Manager;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Persist;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;

    using Xml.Schema.Linq;

    /// <summary>
    /// The submit structure controller.
    /// </summary>
    public class SubmitStructureController
    {
        /// <summary>
        /// The parsing manager
        /// </summary>
        private readonly IStructureParsingManager _parsingManager = new StructureParsingManager();

        /// <summary>
        /// The mapping store connection string settings
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitStructureController"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">The connection string settings.</param>
        public SubmitStructureController(ConnectionStringSettings connectionStringSettings)
        {
            this._connectionStringSettings = connectionStringSettings;
        }

        /// <summary>
        /// Submits the specified structural meta-data .
        /// </summary>
        /// <param name="dataLocation">The data location pointing to the structural meta-data.</param>
        /// <returns>The imported objects</returns>
        /// <exception cref="Estat.Sri.Ws.SubmitStructure.SubmitStructureException">An error occurred while importing structural meta-data.</exception>
        public ISdmxObjects Submit(IReadableDataLocation dataLocation, SubmitStructureConstant.ActionType actionType = SubmitStructureConstant.ActionType.Replace)
        {
            // Parse structures IStructureParsingManager is an instance field.
            IStructureWorkspace structureWorkspace = this._parsingManager.ParseStructures(dataLocation);

            // Get immutable objects from workspace
            ISdmxObjects objects = structureWorkspace.GetStructureObjects(false);

            // create a new instance of the MappingStoreManager class which implements the IStructurePersistenceManager
            IList<ArtefactImportStatus> importStatus = new List<ArtefactImportStatus>();
            IStructurePersistenceManager persistenceManager = new MappingStoreManager(this._connectionStringSettings, importStatus);

            switch (actionType)
            {
                case SubmitStructureConstant.ActionType.Append:
                    break;
                case SubmitStructureConstant.ActionType.Replace:
                    // Save the structure to the mapping store database.
                    persistenceManager.SaveStructures(objects);

                    // Validate objects.
                    ValidateImport(importStatus);
                
                    break;
                case SubmitStructureConstant.ActionType.Delete:
                    // Delete the structure to the mapping store database.
                    persistenceManager.DeleteStructures(objects);
                    break;
                default:
                    break;
            }

            // Return the immutable object container.
            return objects;
        }

        /// <summary>
        /// Write the <paramref name="message"/> using <paramref name="xmlWriter"/>
        /// </summary>
        /// <param name="xmlWriter">
        /// The XML writer.
        /// </param>
        /// <param name="message">
        /// The SDMX message
        /// </param>
        public void Write(XmlWriter xmlWriter, XTypedElement message)
        {
            if (xmlWriter.WriteState == WriteState.Start)
            {
                message.Untyped.Save(xmlWriter);
            }
            else
            {
                //// this is needed for NSI WS & SOAP. We get a XmlWriter where the document has already started with SOAP envelope
                message.Untyped.WriteTo(xmlWriter);
            }

            xmlWriter.Flush();
        }

        /// <summary>
        /// Validates the import status.
        /// </summary>
        /// <param name="importStatus">The import status.</param>
        /// <exception cref="Estat.Sri.Ws.SubmitStructure.SubmitStructureException">An error occurred while importing structural meta-data.</exception>
        private static void ValidateImport(IEnumerable<ArtefactImportStatus> importStatus)
        {
            foreach (var artefactImportStatuse in importStatus)
            {
                if (artefactImportStatuse.ImportMessage.Status == ImportMessageStatus.Error || artefactImportStatuse.ImportMessage.Status == ImportMessageStatus.Warning)
                {
                    throw new SubmitStructureException(artefactImportStatuse.ImportMessage.Message, artefactImportStatuse.ImportMessage.StructureReference);
                }

                // TODO Add a ImportMessageStatus.CannotReplace to detect cases where it cannot replace artefacts.
            }
        }
    }
}
