// -----------------------------------------------------------------------
// <copyright file="StructureExtensions.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStore.Store.Properties;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// The structure type extensions.
    /// </summary>
    public static class StructureExtensions
    {
        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(StructureExtensions));

        /// <summary>
        /// The _from mutable builder
        /// </summary>
        private static readonly IBuilder<IStructureReference, IMaintainableMutableObject> _fromMutableBuilder;

        /// <summary>
        /// Initializes static members of the <see cref="StructureExtensions"/> class.
        /// </summary>
        static StructureExtensions()
        {
            _fromMutableBuilder = new StructureReferenceFromMutableBuilder();
        }

        #region Public Methods and Operators

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <param name="mutableObject">The mutable object.</param>
        /// <returns>
        /// The <see cref="IStructureReference"/>
        /// </returns>
        public static IStructureReference AsReference(this IMaintainableMutableObject mutableObject)
        {
            return _fromMutableBuilder.Build(mutableObject);
        }

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <param name="mutableObject">The mutable object.</param>
        /// <param name="identifiableMutableObject">The identifiable mutable object.</param>
        /// <returns>
        /// The <see cref="IStructureReference" />
        /// </returns>
        public static IStructureReference AsReference(this IMaintainableMutableObject mutableObject, IIdentifiableMutableObject identifiableMutableObject)
        {
            return new StructureReferenceImpl(mutableObject.AgencyId, mutableObject.Id, mutableObject.Version, identifiableMutableObject.StructureType.EnumType, identifiableMutableObject.Id);
        }

        /// <summary>
        /// Get the mapping store component type.
        /// </summary>
        /// <param name="component">
        /// The SDMX Component
        /// </param>
        /// <returns>
        /// The mapping store component type. 
        /// </returns>
        public static string GetMappingStoreType(this IComponent component)
        {
            SdmxComponentType componentType;
            switch (component.StructureType.EnumType)
            {
                case SdmxStructureEnumType.Dimension:
                case SdmxStructureEnumType.MeasureDimension:
                    componentType = SdmxComponentType.Dimension;
                    break;
                case SdmxStructureEnumType.TimeDimension:
                    componentType = SdmxComponentType.TimeDimension;
                    break;
                case SdmxStructureEnumType.DataAttribute:
                    componentType = SdmxComponentType.Attribute;
                    break;
                case SdmxStructureEnumType.PrimaryMeasure:
                    componentType = SdmxComponentType.PrimaryMeasure;
                    break;
                case SdmxStructureEnumType.CrossSectionalMeasure:
                    componentType = SdmxComponentType.CrossSectionalMeasure;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("component", component.StructureType.ToString(), Resources.ExceptionNotComponentType);
            }

            return componentType.ToString();
        }

        /// <summary>
        /// Returns the mapping store assignment level for the specified <paramref name="component"/>.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <returns>
        /// The mapping store assignment level for the specified <paramref name="component"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="component"/> has an unsupported assignment level.
        /// </exception>
        public static string GetMappingStoreAssignmentLevel(this IAttributeObject component)
        {
            string returnValue;
            switch (component.AttachmentLevel)
            {
                case AttributeAttachmentLevel.DataSet:
                    returnValue = AttachmentLevelConstants.DataSet;
                    break;
                case AttributeAttachmentLevel.Group:
                    returnValue = AttachmentLevelConstants.Group;
                    break;
                case AttributeAttachmentLevel.DimensionGroup:
                    returnValue = AttachmentLevelConstants.Series;
                    break;
                case AttributeAttachmentLevel.Observation:
                    returnValue = AttachmentLevelConstants.Observation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("component", component.AttachmentLevel, Resources.ExceptionUnsupportedAttachmentLevel_);
            }

            return returnValue;
        }

        /// <summary>
        /// Returns all components of the specified <paramref name="dataStructure"/>.
        /// It includes the <see cref="ICrossSectionalDataStructureObject.CrossSectionalMeasures"/> 
        /// </summary>
        /// <param name="dataStructure">
        /// The data structure.
        /// </param>
        /// <returns>
        /// The components of the specified <paramref name="dataStructure"/>.
        /// </returns>
        public static IEnumerable<IComponent> GetAllComponents(this IDataStructureObject dataStructure)
        {
            foreach (var dimension in dataStructure.GetDimensions().OrderBy(dimension => dimension.Position))
            {
                yield return dimension;
            }

            yield return dataStructure.PrimaryMeasure;

            var crossDsd = dataStructure as ICrossSectionalDataStructureObject;
            if (crossDsd != null)
            {
                foreach (var crossSectionalMeasure in crossDsd.CrossSectionalMeasures)
                {
                    yield return crossSectionalMeasure;
                }
            }

            foreach (var attributeObject in dataStructure.Attributes)
            {
                yield return attributeObject;
            }
        }

        #endregion

        /// <summary>
        /// Returns an error message.
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        /// <returns>
        /// The <see cref="ImportMessage"/>.
        /// </returns>
        public static ImportMessage GetErrorMessage(this IStructureReference structureReference, Exception ex)
        {
            var errorMessage = string.Format(
                CultureInfo.InvariantCulture, 
                "Failure: {0} cannot be inserted. REASON: {1}{2}", 
                structureReference.GetAsHumanReadableString(), 
                ex.Message, 
                Environment.NewLine);
            _log.Error(errorMessage, ex);
            return new ImportMessage(ImportMessageStatus.Error, structureReference, errorMessage);
        }

        /// <summary>
        /// Returns the identification of the specified <paramref name="structureReference"/> as a human readable string.
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <returns>
        /// The identification of the specified <paramref name="structureReference"/> as a human readable string.
        /// </returns>
        public static string GetAsHumanReadableString(this IStructureReference structureReference)
        {
            var artefact = structureReference.MaintainableReference;
            var errorMessage = string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}:{2} (v{3})",
                structureReference.MaintainableStructureEnumType,
                artefact.AgencyId,
                artefact.MaintainableId,
                artefact.Version);
            return errorMessage;
        }

        /// <summary>
        /// Creates the child reference.
        /// </summary>
        /// <param name="structureReference">The structure reference.</param>
        /// <param name="structureType">Type of the structure.</param>
        /// <param name="ids">The ids.</param>
        /// <returns>The <see cref="IStructureReference"/> for <paramref name="ids"/> of type <paramref name="structureReference"/></returns>
        public static IStructureReference CreateChildReference(this IStructureReference structureReference, SdmxStructureEnumType structureType ,params string[] ids)
        {
            return new StructureReferenceImpl(structureReference.AgencyId, structureReference.MaintainableId, structureReference.Version, structureType, ids);
        }

        /// <summary>
        /// Convert the specified <paramref name="conceptScheme"/> to CodeList.
        /// </summary>
        /// <param name="conceptScheme">The concept scheme.</param>
        /// <returns>The CodeList.</returns>
        public static ICodelistMutableObject ConvertToCodelist(this IConceptSchemeMutableObject conceptScheme)
        {
            ICodelistMutableObject codelist = new CodelistMutableCore()
            {
                Id = conceptScheme.Id,
                AgencyId = conceptScheme.AgencyId,
                Version = conceptScheme.Version,
                FinalStructure = conceptScheme.FinalStructure,
                IsPartial = conceptScheme.IsPartial,
            };
            codelist.Names.AddAll(conceptScheme.Names);
            codelist.Descriptions.AddAll(conceptScheme.Descriptions);
            foreach (var item in conceptScheme.Items)
            {
                ICodeMutableObject code = new CodeMutableCore { Id = item.Id, };

                code.Names.AddAll(item.Names);
                code.Descriptions.AddAll(item.Descriptions);

                codelist.AddItem(code);
            }

            return codelist;
        }

        /// <summary>
        /// Gets the component.
        /// </summary>
        /// <param name="dsd">The DSD.</param>
        /// <param name="id">The component unique identifier.</param>
        /// <returns>The matched <see cref="IComponentMutableObject"/>; otherwise <c>null</c></returns>
        public static IComponentMutableObject GetComponent(this IDataStructureMutableObject dsd, string id)
        {
            switch (id)
            {
                case DimensionObject.TimeDimensionFixedId:
                    return dsd.Dimensions.FirstOrDefault(o => o.TimeDimension);
                case PrimaryMeasure.FixedId:
                    return dsd.PrimaryMeasure;
            }

            Func<IComponentMutableObject, bool> func = o => id.Equals(o.Id) || id.Equals(o.ConceptRef.ChildReference.Id);
            var dimension = dsd.Dimensions.FirstOrDefault(func);
            if (dimension != null)
            {
                return dimension;
            }

            if (dsd.AttributeList != null)
            {
                var attribute = dsd.AttributeList.Attributes.FirstOrDefault(func);
                if (attribute != null)
                {
                    return attribute;
                }
            }

            var crossDsd = dsd as ICrossSectionalDataStructureMutableObject;
            if (crossDsd != null)
            {
                return crossDsd.CrossSectionalMeasures.FirstOrDefault(func);
            }

            return null;
        }
    }
}