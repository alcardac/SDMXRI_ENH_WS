// -----------------------------------------------------------------------
// <copyright file="MappingUtils.cs" company="EUROSTAT">
//   Date Created : 2013-12-04
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
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System.Collections.Generic;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    /// <summary>
    /// Helper class with methods that check if the mapping is complete and other helper methods
    /// </summary>
    public static class MappingUtils
    {
        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(MappingUtils));

        #region Public Methods

        /// <summary>
        /// This method checks whether a mapping set is complete, in order to be used for the Data Retriever.
        /// For example,one of the checks is whether all dimensions and mandatory attributes are mapped.
        /// </summary>
        /// <param name="mappingSet">
        /// The mapping set to be checked
        /// </param>
        /// <returns>
        /// True if the mapping is complete
        /// </returns>
        public static bool IsMappingSetComplete(MappingSetEntity mappingSet)
        {
            var componentMappings = new Dictionary<ComponentEntity, MappingEntity>();
            foreach (MappingEntity mapping in mappingSet.Mappings)
            {
                foreach (ComponentEntity component in mapping.Components)
                {
                    componentMappings.Add(component, mapping);
                }
            }

            return IsMappingSetComplete(mappingSet.Dataflow.Dsd, componentMappings);
        }

        /// <summary>
        /// This method checks whether a mapping set is complete, in order to be used for the Data Retriever.
        /// For example,one of the checks is whether all dimensions and mandatory attributes are mapped.
        /// </summary>
        /// <param name="dsd">The DSD to be checked</param>
        /// <param name="componentMapping">Map between components and their mapping</param>
        /// <returns>
        /// True if the mapping is complete
        /// </returns>
        public static bool IsMappingSetComplete(
            DsdEntity dsd, Dictionary<ComponentEntity, MappingEntity> componentMapping)
        {
            // _log.Info(String.Format(CultureInfo.InvariantCulture,"Checking if mapping for dataflow '{0}' DSD '{1}' is complete.",
            // mappingSet.Dataflow.Id, dsd.Id));
            bool returnValue = true;
            bool measureDimensionNotMapped = false;

            if (dsd.Dimensions != null)
            {
                int i = 0;

                // check if all dimensions are mapped
                while (i < dsd.Dimensions.Count)
                {
                    if (!dsd.Dimensions[i].MeasureDimension && !componentMapping.ContainsKey(dsd.Dimensions[i]))
                    {
                        _log.WarnFormat(
                                CultureInfo.InvariantCulture, 
                                "Dimension '{0}' is not mapped.", 
                                dsd.Dimensions[i].Concept.Id);
                        returnValue = false;
                    }
                    else if (dsd.Dimensions[i].MeasureDimension && !componentMapping.ContainsKey(dsd.Dimensions[i]))
                    {
                        measureDimensionNotMapped = true;
                    }

                    i++;
                }
            }
            else
            {
                string message = string.Format(
                    CultureInfo.InvariantCulture, "No dimensions defined in the DSD {0}", dsd.Id);

                _log.Info(message);

                // throw new DataRetrieverException(ErrorTypes.NO_MAPPING_SET, message);
                return false;
            }

            // check if time dimension is mapped, if one exists.
            if (dsd.TimeDimension != null)
            {
                if (!componentMapping.ContainsKey(dsd.TimeDimension))
                {
                    _log.WarnFormat(CultureInfo.InvariantCulture, "Time Dimension '{0}' is not mapped.", dsd.TimeDimension.Concept.Id);
                    returnValue = false;
                }
            }

            // check xs-measures
            if (dsd.CrossSectionalMeasures != null && dsd.CrossSectionalMeasures.Count > 0 && measureDimensionNotMapped)
            {
                foreach (ComponentEntity component in dsd.CrossSectionalMeasures)
                {
                    if (!componentMapping.ContainsKey(component))
                    {
                        _log.WarnFormat(CultureInfo.InvariantCulture, ErrorMessages.CrossSectionalMeasureNotMappedFormat1, component.Concept.Id);
                        returnValue = false;
                    }
                }
            }
            else if (dsd.PrimaryMeasure != null)
            {
                // primary measure
                if (!componentMapping.ContainsKey(dsd.PrimaryMeasure))
                {
                    {
                        _log.WarnFormat(CultureInfo.InvariantCulture, ErrorMessages.PrimaryMeasureNotMappedFormat1, dsd.PrimaryMeasure.Concept.Id);
                        returnValue = false;
                    }
                }
            }

            // atributes
            if (dsd.Attributes != null && dsd.Attributes.Count > 0)
            {
                foreach (ComponentEntity component in dsd.Attributes)
                {
                    if (component.AttStatus == AssignmentStatus.Mandatory && !componentMapping.ContainsKey(component))
                    {
                        _log.WarnFormat(CultureInfo.InvariantCulture, ErrorMessages.MandatoryAttributeNotMappedFormat1, component.Concept.Id);
                        returnValue = false;
                    }
                }
            }

            // test if 1-N or N-1 mappings have transcoding
            foreach (var kv in componentMapping)
            {
                if (kv.Value.Transcoding == null && (kv.Value.Columns.Count > 1 || kv.Value.Components.Count > 1))
                {
                    _log.WarnFormat(CultureInfo.CurrentCulture, ErrorMessages.ComponentMappingNoTranscodingFormat1, kv.Key.Concept.Id);
                    returnValue = false;
                }
            }

            _log.Info(
                string.Format(
                    CultureInfo.InvariantCulture, InformativeMessages.CheckMappingSetResultFormat1, returnValue));

            return returnValue;
        }

        #endregion
    }
}