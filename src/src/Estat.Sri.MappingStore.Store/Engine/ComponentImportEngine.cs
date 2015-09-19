// -----------------------------------------------------------------------
// <copyright file="ComponentImportEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-22
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
    using System.Linq;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Helper;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStore.Store.Properties;
    using Estat.Sri.MappingStoreRetrieval;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The component import engine.
    /// </summary>
    public class ComponentImportEngine : IIdentifiableImportEngine<IComponent>
    {
        #region Constants

        /// <summary>
        ///     The true value in Mapping Store
        /// </summary>
        private const int TrueValue = 1;

        #endregion

        /// <summary>
        /// The _stored procedures
        /// </summary>
        private static readonly StoredProcedures _storedProcedures;

        /// <summary>
        /// The _annotation insert engine
        /// </summary>
        private static readonly IAnnotationInsertEngine _annotationInsertEngine;

        /// <summary>
        /// The _insert component annotation
        /// </summary>
        private static readonly InsertComponentAnnotation _insertComponentAnnotation;

        /// <summary>
        /// Initializes static members of the <see cref="ComponentImportEngine"/> class.
        /// </summary>
        static ComponentImportEngine()
        {
            _storedProcedures = new StoredProcedures();
            _annotationInsertEngine = new ComponentAnnotationInsertEngine(new AnnotationInsertEngine());
            _insertComponentAnnotation = _storedProcedures.InsertComponentAnnotation;
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
        public ItemStatusCollection Insert(DbTransactionState state, IEnumerable<IComponent> items, long parentArtefact)
        {
            var components = items as IComponent[] ?? items.ToArray();
            if (components.Length == 0)
            {
                return new ItemStatusCollection();
            }

            var cache = new StructureCache();
            var returnValues = new ItemStatusCollection();

            var dsd = components[0].MaintainableParent as IDataStructureObject;
            if (dsd == null)
            {
                throw new ArgumentException(Resources.ExceptionCannotDetermineParent, "items");
            }

            foreach (var component in components)
            {
                returnValues.Add(new ItemStatus(component.Id, Insert(state, component, cache, parentArtefact)));
            }

            return returnValues;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the CodeList status.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="component">The component.</param>
        /// <param name="itemScheme">The item scheme.</param>
        /// <returns>
        /// The <see cref="ItemSchemeFinalStatus" />.
        /// </returns>
        private static ItemSchemeFinalStatus GetCodelistStatus(DbTransactionState state, IComponent component, StructureCache itemScheme)
        {
            ItemSchemeFinalStatus codelistStatus = null;
            if (component.HasCodedRepresentation())
            {
                var dimension = component as IDimension;
                var crossDsd = component.MaintainableParent as ICrossSectionalDataStructureObject;
                IStructureReference reference;
                if (dimension != null && dimension.MeasureDimension && crossDsd != null)
                {
                    reference = crossDsd.GetCodelistForMeasureDimension(dimension.Id);
                }
                else
                {
                    reference = component.Representation.Representation;
                }

                codelistStatus = itemScheme.GetStructure(state, reference);
                ValidateCodelist(codelistStatus.FinalStatus, reference);
            }

            return codelistStatus;
        }

        /// <summary>
        /// Returns an error message with the specified <paramref name="format"/> for <paramref name="reference"/>
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="reference">
        /// The component.
        /// </param>
        /// <returns>
        /// The error message.
        /// </returns>
        private static string GetError(string format, IStructureReference reference)
        {
            IMaintainableRefObject maintainableRefObject = reference.MaintainableReference;
            return reference.HasChildReference()
                       ? string.Format(format, maintainableRefObject.MaintainableId, maintainableRefObject.AgencyId, maintainableRefObject.Version, reference.ChildReference.Id)
                       : string.Format(format, maintainableRefObject.MaintainableId, maintainableRefObject.AgencyId, maintainableRefObject.Version);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="itemScheme">
        /// The item scheme.
        /// </param>
        /// <param name="parentArtefact">
        /// The parent artefact.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        /// <exception cref="MappingStoreException">
        /// THere was a problem with the <paramref name="component"/> references.
        /// </exception>
        private static long Insert(
            DbTransactionState state, IComponent component, StructureCache itemScheme, long parentArtefact)
        {
            var conceptStatus = itemScheme.GetStructure(state, component.ConceptRef);
            ItemStatus conceptID = ValidateConceptScheme(conceptStatus, component.ConceptRef);
            var codelistStatus = GetCodelistStatus(state, component, itemScheme);

            var formats = new List<KeyValuePair<long, ITextFormat>>();
            long compID;
            var attribute = component as IAttributeObject;
            var dimension = component as IDimension;
            var storedProcedure = _storedProcedures.InsertComponent;
            using (DbCommand command = storedProcedure.CreateCommand(state))
            {
                DbParameter idParameter = storedProcedure.CreateIdParameter(command);
                idParameter.Value = component.Id;

                SetDsd(parentArtefact, storedProcedure, command);

                SetConcept(storedProcedure, command, conceptID);

                SetComponentType(component, storedProcedure, command);

                SetCodelist(storedProcedure, command, codelistStatus, component);

                DbParameter isFreqDimParameter = storedProcedure.CreateIsFreqDimParameter(command);

                DbParameter isMeasureDimParameter = storedProcedure.CreateIsMeasureDimParameter(command);

                DbParameter attAssLevelParameter = storedProcedure.CreateAttAssLevelParameter(command);

                DbParameter attStatusParameter = storedProcedure.CreateAttStatusParameter(command);

                DbParameter attIsTimeFormatParameter = storedProcedure.CreateAttIsTimeFormatParameter(command);

                DbParameter xsMeasureCodeParameter = storedProcedure.CreateXsMeasureCodeParameter(command);

                DbParameter outputParameter = storedProcedure.CreateOutputParameter(command);

                switch (component.StructureType.EnumType)
                {
                    case SdmxStructureEnumType.Dimension:
                    case SdmxStructureEnumType.MeasureDimension:
                        {
                            SetDimensionParameters(dimension, isFreqDimParameter, isMeasureDimParameter);
                        }

                        break;
                    case SdmxStructureEnumType.TimeDimension:
                        break;
                    case SdmxStructureEnumType.DataAttribute:
                        {
                            SetAttributeParameters(attribute, attAssLevelParameter, attStatusParameter, attIsTimeFormatParameter);
                        }

                        break;
                    case SdmxStructureEnumType.PrimaryMeasure:
                        break;
                    case SdmxStructureEnumType.CrossSectionalMeasure:
                        {
                            SetCrossSectionalMeasureParameters(component, xsMeasureCodeParameter);
                        }

                        break;
                }

                SetCrossSectionalLevels(component, storedProcedure, command);

                command.ExecuteNonQuery();

                compID = (long)outputParameter.Value;

                if (component.Representation != null && component.Representation.TextFormat != null)
                {
                    formats.Add(new KeyValuePair<long, ITextFormat>(compID, component.Representation.TextFormat));
                }
            }

            InsertTextFormats(state, formats);
            _annotationInsertEngine.Insert(state, compID, _insertComponentAnnotation, component.Annotations);
            return compID;
        }

        /// <summary>
        /// Insert text formats.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="formats">
        /// The formats.
        /// </param>
        private static void InsertTextFormats(DbTransactionState state, IEnumerable<KeyValuePair<long, ITextFormat>> formats)
        {
            var textFormatEngine = TextFormatTypesPool.GetTextFormatQuery(state);
            textFormatEngine.InsertTextFormats(state, formats);
        }

        /// <summary>
        /// The insert attribute.
        /// </summary>
        /// <param name="attribute">
        /// The attribute.
        /// </param>
        /// <param name="attAssLevelParameter">
        /// The Attribute attachment level parameter.
        /// </param>
        /// <param name="attStatusParameter">
        /// The Attribute assignment status parameter.
        /// </param>
        /// <param name="attIsTimeFormatParameter">
        /// The Attribute is time format parameter.
        /// </param>
        private static void SetAttributeParameters(IAttributeObject attribute, IDataParameter attAssLevelParameter, IDataParameter attStatusParameter, IDataParameter attIsTimeFormatParameter)
        {
            if (attribute != null)
            {
                attAssLevelParameter.Value = attribute.GetMappingStoreAssignmentLevel();
                attStatusParameter.Value = attribute.AssignmentStatus;
                if (attribute.TimeFormat)
                {
                    attIsTimeFormatParameter.Value = TrueValue;
                }
            }
        }

        /// <summary>
        /// Set codelist parameters.
        /// </summary>
        /// <param name="storedProcedure">
        /// The stored procedure.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="codelistStatus">
        /// The codelist status.
        /// </param>
        /// <param name="component">
        /// The component.
        /// </param>
        private static void SetCodelist(InsertComponent storedProcedure, DbCommand command, ItemSchemeFinalStatus codelistStatus, IComponent component)
        {
            DbParameter clidParameter = storedProcedure.CreateClIdParameter(command);
            DbParameter conSchIdParameter = storedProcedure.CreateConSchIdParameter(command);
            if (codelistStatus != null && component.HasCodedRepresentation())
            {
                switch (component.Representation.Representation.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.CodeList:
                        clidParameter.Value = codelistStatus.FinalStatus.PrimaryKey;
                        break;
                    case SdmxStructureEnumType.ConceptScheme:
                        {
                             var crossDsd = component.MaintainableParent as ICrossSectionalDataStructureObject;
                            if (crossDsd != null)
                            {
                                clidParameter.Value = codelistStatus.FinalStatus.PrimaryKey;
                            }
                            else
                            {
                                conSchIdParameter.Value = codelistStatus.FinalStatus.PrimaryKey;
                            }
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Set component type parameter.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="storedProcedure">
        /// The stored procedure.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        private static void SetComponentType(IComponent component, InsertComponent storedProcedure, DbCommand command)
        {
            DbParameter typeParameter = storedProcedure.CreateTypeParameter(command);
            typeParameter.Value = component.GetMappingStoreType();
        }

        /// <summary>
        /// Set concept parameter.
        /// </summary>
        /// <param name="storedProcedure">
        /// The stored procedure.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="conceptID">
        /// The concept id.
        /// </param>
        private static void SetConcept(InsertComponent storedProcedure, DbCommand command, ItemStatus conceptID)
        {
            DbParameter conIdParameter = storedProcedure.CreateConIdParameter(command);

            conIdParameter.Value = conceptID.SysID;
        }

        /// <summary>
        /// Fill cross sectional levels.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="storedProcedure">
        /// The stored Procedure.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        private static void SetCrossSectionalLevels(IComponent component, InsertComponent storedProcedure, DbCommand command)
        {
            DbParameter xsAttlevelDsParameter = storedProcedure.CreateXsAttlevelDsParameter(command);

            DbParameter xsAttlevelGroupParameter = storedProcedure.CreateXsAttlevelGroupParameter(command);

            DbParameter xsAttlevelSectionParameter = storedProcedure.CreateXsAttlevelSectionParameter(command);

            DbParameter xsAttlevelObsParameter = storedProcedure.CreateXsAttlevelObsParameter(command);
            var crossDsd = component.MaintainableParent as ICrossSectionalDataStructureObject;
            if (crossDsd != null)
            {
                if (crossDsd.GetCrossSectionalAttachDataSet(false, component.StructureType).Contains(component))
                {
                    xsAttlevelDsParameter.Value = TrueValue;
                }

                if (crossDsd.GetCrossSectionalAttachGroup(false, component.StructureType).Contains(component))
                {
                    xsAttlevelGroupParameter.Value = TrueValue;
                }

                if (crossDsd.GetCrossSectionalAttachSection(false, component.StructureType).Contains(component))
                {
                    xsAttlevelSectionParameter.Value = TrueValue;
                }

                if (crossDsd.GetCrossSectionalAttachObservation(component.StructureType).Contains(component))
                {
                    xsAttlevelObsParameter.Value = TrueValue;
                }
            }
        }

        /// <summary>
        /// The insert cross sectional measure.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="xsMeasureCodeParameter">
        /// The CrossSectional measure code parameter.
        /// </param>
        private static void SetCrossSectionalMeasureParameters(IComponent component, IDataParameter xsMeasureCodeParameter)
        {
            var crossSectionalMeasureBean = component as ICrossSectionalMeasure;
            if (crossSectionalMeasureBean != null)
            {
                xsMeasureCodeParameter.Value = crossSectionalMeasureBean.Code;
            }
        }

        /// <summary>
        /// Insert dimension.
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        /// <param name="isFreqDimParameter">
        /// The is frequency dimension parameter.
        /// </param>
        /// <param name="isMeasureDimParameter">
        /// The is measure dimension parameter.
        /// </param>
        private static void SetDimensionParameters(IDimension dimension, IDataParameter isFreqDimParameter, IDataParameter isMeasureDimParameter)
        {
            if (dimension != null)
            {
                if (dimension.FrequencyDimension)
                {
                    isFreqDimParameter.Value = TrueValue;
                }

                if (dimension.MeasureDimension)
                {
                    isMeasureDimParameter.Value = TrueValue;
                }
            }
        }

        /// <summary>
        /// Set parent DataStructure definition.
        /// </summary>
        /// <param name="parentArtefact">
        /// The parent artefact.
        /// </param>
        /// <param name="storedProcedure">
        /// The stored procedure.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        private static void SetDsd(long parentArtefact, InsertComponent storedProcedure, DbCommand command)
        {
            DbParameter dsdIdParameter = storedProcedure.CreateDsdIdParameter(command);
            dsdIdParameter.Value = parentArtefact;
        }

        /// <summary>
        /// Validate codelist.
        /// </summary>
        /// <param name="codelistStatus">
        /// The codelist status.
        /// </param>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <exception cref="MappingStoreException">
        /// The specified <paramref name="codelistStatus"/> is not valid.
        /// </exception>
        private static void ValidateCodelist(ArtefactFinalStatus codelistStatus, IStructureReference component)
        {
            if (codelistStatus == null)
            {
                throw new ArgumentNullException("codelistStatus");
            }

            if (codelistStatus.PrimaryKey < 1)
            {
                throw new MappingStoreException(GetError("Codelist {0}, Agency {1}, Version {2} is not available. Cannot import DSD.", component));
            }

            if (!codelistStatus.IsFinal)
            {
                throw new MappingStoreException(GetError("Referenced Codelist {0}, Agency {1}, Version {2} is not Final. Cannot import DSD.", component));
            }
        }

        /// <summary>
        /// The validate concept scheme.
        /// </summary>
        /// <param name="conceptSchemeID">
        /// The concept scheme id.
        /// </param>
        /// <param name="reference">
        /// The reference.
        /// </param>
        /// <returns>
        /// The <see cref="ItemStatus"/>.
        /// </returns>
        /// <exception cref="MappingStoreException">
        /// The specified <paramref name="conceptSchemeID"/> is not valid.
        /// </exception>
        private static ItemStatus ValidateConceptScheme(ItemSchemeFinalStatus conceptSchemeID, IStructureReference reference)
        {
            if (conceptSchemeID.FinalStatus.PrimaryKey < 1)
            {
                throw new MappingStoreException(GetError("Concept Scheme {0}, Agency {1}, Version {2} for concept {3} is not available. Cannot import DSD.", reference));
            }

            if (!conceptSchemeID.FinalStatus.IsFinal)
            {
                throw new MappingStoreException(GetError("Referenced Concept Scheme {0}, Agency {1}, Version {2} for concept {3} is not Final. Cannot import DSD.", reference));
            }

            ItemStatus conceptId;
            if (!conceptSchemeID.ItemIdMap.TryGetValue(reference.ChildReference.Id, out conceptId))
            {
                throw new MappingStoreException(GetError("Concept {3} from concept scheme {0}, Agency {1}, Version {2} is not available. Cannot import DSD.", reference));
            }

            return conceptId;
        }

        #endregion
    }
}