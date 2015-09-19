// -----------------------------------------------------------------------
// <copyright file="CrossReferenceChildBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The cross reference child builder.
    /// </summary>
    public class CrossReferenceChildBuilder : ICrossReferenceSetBuilder
    {
        /// <summary>
        /// Builds an <see cref="ISet{IStructureReference}"/> from the specified <paramref name="identifiable"/>
        /// </summary>
        /// <param name="identifiable">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IStructureReference}"/> from the specified <paramref name="identifiable"/>
        /// </returns>
        public ISet<IStructureReference> Build(IIdentifiableMutableObject identifiable)
        {
            var structureReferences = new HashSet<IStructureReference>();
            switch (identifiable.StructureType.EnumType)
            {
                case SdmxStructureEnumType.Dataflow:
                    {
                        // TODO categorisations
                        AddReferences(identifiable as IDataflowMutableObject, structureReferences);
                    }

                    break;
                case SdmxStructureEnumType.Categorisation:
                    {
                        AddReferences(identifiable as ICategorisationMutableObject, structureReferences);
                    }

                    break;
                case SdmxStructureEnumType.CategoryScheme:

                    // TODO v2.0 DataflowRef
                    break;
                case SdmxStructureEnumType.Category:

                    // TODO v2.0 DataflowRef
                    break;
                case SdmxStructureEnumType.Component:
                case SdmxStructureEnumType.Dimension:
                case SdmxStructureEnumType.PrimaryMeasure:
                case SdmxStructureEnumType.MeasureDimension:
                case SdmxStructureEnumType.TimeDimension:
                case SdmxStructureEnumType.CrossSectionalMeasure:
                case SdmxStructureEnumType.DataAttribute:
                    {
                        AddReferences(identifiable as IComponentMutableObject, structureReferences);
                    }

                    break;

                case SdmxStructureEnumType.Dsd:
                    {
                        AddReferences(identifiable as IDataStructureMutableObject, structureReferences);
                    }

                    break;

                case SdmxStructureEnumType.HierarchicalCodelist:
                    {
                        AddReferences(identifiable as IHierarchicalCodelistMutableObject, structureReferences);
                    }

                    break;
            }

            return structureReferences;
        }

        /// <summary>
        /// Add references to <paramref name="structureReferences"/>
        /// </summary>
        /// <param name="hcl">
        /// The HCL.
        /// </param>
        /// <param name="structureReferences">
        /// The structure references.
        /// </param>
        private static void AddReferences(IHierarchicalCodelistMutableObject hcl, ISet<IStructureReference> structureReferences)
        {
            if (hcl != null && !hcl.Stub)
            {
                foreach (ICodelistRefMutableObject codelistRefMutableObject in hcl.CodelistRef)
                {
                    structureReferences.Add(codelistRefMutableObject.CodelistReference);
                }
            }
        }

        /// <summary>
        /// Add references to <paramref name="structureReferences"/>
        /// </summary>
        /// <param name="dsd">
        /// The DSD.
        /// </param>
        /// <param name="structureReferences">
        /// The structure references.
        /// </param>
        private static void AddReferences(IDataStructureMutableObject dsd, ISet<IStructureReference> structureReferences)
        {
            if (dsd != null && !dsd.Stub)
            {
                AddReferences(dsd.Dimensions, structureReferences);

                if (dsd.AttributeList != null)
                {
                    AddReferences(dsd.AttributeList.Attributes, structureReferences);
                }

                AddReferences(dsd.PrimaryMeasure, structureReferences);

                var crossDsd = dsd as ICrossSectionalDataStructureMutableObject;
                if (crossDsd != null)
                {
                    AddReferences(crossDsd.CrossSectionalMeasures, structureReferences);
                    structureReferences.UnionWith(crossDsd.MeasureDimensionCodelistMapping.Values);
                }
            }
        }

        /// <summary>
        /// Add references to <paramref name="structureReferences"/>
        /// </summary>
        /// <param name="categorisation">
        /// The categorisation.
        /// </param>
        /// <param name="structureReferences">
        /// The structure references.
        /// </param>
        private static void AddReferences(ICategorisationMutableObject categorisation, ISet<IStructureReference> structureReferences)
        {
            if (categorisation != null && !categorisation.Stub)
            {
                structureReferences.Add(categorisation.CategoryReference);
                structureReferences.Add(categorisation.StructureReference);
            }
        }

        /// <summary>
        /// Add references to <paramref name="structureReferences"/>
        /// </summary>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <param name="structureReferences">
        /// The structure references.
        /// </param>
        private static void AddReferences(IDataflowMutableObject dataflow, ISet<IStructureReference> structureReferences)
        {
            if (dataflow != null && !dataflow.Stub)
            {
                structureReferences.Add(dataflow.DataStructureRef);
            }
        }

        /// <summary>
        /// Add references to <paramref name="structureReferences"/>
        /// </summary>
        /// <param name="components">
        /// The components.
        /// </param>
        /// <param name="structureReferences">
        /// The structure references.
        /// </param>
        /// <typeparam name="T">
        /// The item type of <paramref name="components"/>
        /// </typeparam>
        private static void AddReferences<T>(IEnumerable<T> components, ICollection<IStructureReference> structureReferences) where T : IComponentMutableObject
        {
            foreach (T component in components)
            {
                AddReferences(component, structureReferences);
            }
        }

        /// <summary>
        /// Add references to <paramref name="structureReferences"/>
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="structureReferences">
        /// The structure references.
        /// </param>
        private static void AddReferences(IComponentMutableObject component, ICollection<IStructureReference> structureReferences)
        {
            if (component != null)
            {
                structureReferences.Add(component.ConceptRef);
                if (component.Representation != null && component.Representation.Representation != null)
                {
                    structureReferences.Add(component.Representation.Representation);
                }
            }
        }
    }
}