﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Estat.Sri.MappingStoreRetrieval.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported mapping. N-N mappings are not supported. Component {0}.
        /// </summary>
        internal static string ComponentMappingNNMapping {
            get {
                return ResourceManager.GetString("ComponentMappingNNMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incomplete mapping. Neither constant value or columns mapped for component {0}..
        /// </summary>
        internal static string ComponentMappingNoMappingFormat1 {
            get {
                return ResourceManager.GetString("ComponentMappingNoMappingFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incomplete mapping. Transcoding must be enabled for 1-N or N-1 mappings. Component {0}..
        /// </summary>
        internal static string ComponentMappingNoTranscodingFormat1 {
            get {
                return ResourceManager.GetString("ComponentMappingNoTranscodingFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incomplete mapping. No trancoding rules defined for component {0}..
        /// </summary>
        internal static string ComponentMappingNoTranscodingRulesFormat1 {
            get {
                return ResourceManager.GetString("ComponentMappingNoTranscodingRulesFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not implemented getting references from non-maintainable types : .
        /// </summary>
        internal static string CrossReferenceIdentifiable {
            get {
                return ResourceManager.GetString("CrossReferenceIdentifiable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CrossReference not set. Set the CrossReference property to a ICrossReferenceMutableRetrievalManager instance before calling this method..
        /// </summary>
        internal static string CrossReferenceNotSet {
            get {
                return ResourceManager.GetString("CrossReferenceNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cross Sectional Measure &apos;{0}&apos; is not mapped..
        /// </summary>
        internal static string CrossSectionalMeasureNotMappedFormat1 {
            get {
                return ResourceManager.GetString("CrossSectionalMeasureNotMappedFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IMaintainableRefObject.MaintainableId cannot be null.
        /// </summary>
        internal static string DataflowIdIsNull {
            get {
                return ResourceManager.GetString("DataflowIdIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IAuthSdmxMutableObjectRetrievalManager not set and allowed dataflows is not null. Blocking output...
        /// </summary>
        internal static string ExceptionISdmxMutableObjectAuthRetrievalManagerNotSet {
            get {
                return ResourceManager.GetString("ExceptionISdmxMutableObjectAuthRetrievalManagerNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tried to add TimeTranscodingEntity with TranscodingId different that then one specified in the constructor..
        /// </summary>
        internal static string InvalidTimeTranscoding {
            get {
                return ResourceManager.GetString("InvalidTimeTranscoding", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mandatory attribute &apos;{0}&apos; is not mapped..
        /// </summary>
        internal static string MandatoryAttributeNotMappedFormat1 {
            get {
                return ResourceManager.GetString("MandatoryAttributeNotMappedFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was no {0} for Id:{1}.
        /// </summary>
        internal static string MappingStoreNoEntityFormat2 {
            get {
                return ResourceManager.GetString("MappingStoreNoEntityFormat2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one artefact returned.
        /// </summary>
        internal static string MoreThanOneArtefact {
            get {
                return ResourceManager.GetString("MoreThanOneArtefact", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Primary Measure &apos;{0}&apos; is not mapped..
        /// </summary>
        internal static string PrimaryMeasureNotMappedFormat1 {
            get {
                return ResourceManager.GetString("PrimaryMeasureNotMappedFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not parse SDMX Query Time element : {0}.Please check if it is in Time Period format..
        /// </summary>
        internal static string TimeDimensionInvalidTimeInSDMXQueryFormat {
            get {
                return ResourceManager.GetString("TimeDimensionInvalidTimeInSDMXQueryFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TimeDimension transcoding with 3 or more columns is not yet supported.
        /// </summary>
        internal static string TimeDimensionTranscodingOver2Columns {
            get {
                return ResourceManager.GetString("TimeDimensionTranscodingOver2Columns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incomplete Mapping. {0} column Time Dimension mapping without transcoding is not supported.
        /// </summary>
        internal static string TimeDimensionUnsupportedMappingFormat1 {
            get {
                return ResourceManager.GetString("TimeDimensionUnsupportedMappingFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Variable length period is used but period is first.
        /// </summary>
        internal static string TimeDimensionVariableLenPeriodNotFirst {
            get {
                return ResourceManager.GetString("TimeDimensionVariableLenPeriodNotFirst", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The year selection must be exactly 4 digits..
        /// </summary>
        internal static string TimeDimensionYearNo4Digits {
            get {
                return ResourceManager.GetString("TimeDimensionYearNo4Digits", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Year and Period selections overlap.
        /// </summary>
        internal static string TimeDimensionYearPeriodOverlap {
            get {
                return ResourceManager.GetString("TimeDimensionYearPeriodOverlap", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown Database type: &apos;{0}&apos;.
        /// </summary>
        internal static string UnknownDatabaseTypeFormat1 {
            get {
                return ResourceManager.GetString("UnknownDatabaseTypeFormat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown StructureQueryDetail.
        /// </summary>
        internal static string UnknownStructureQueryDetail {
            get {
                return ResourceManager.GetString("UnknownStructureQueryDetail", resourceCulture);
            }
        }
    }
}
