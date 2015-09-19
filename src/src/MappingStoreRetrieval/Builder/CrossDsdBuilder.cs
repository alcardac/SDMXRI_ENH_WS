// -----------------------------------------------------------------------
// <copyright file="CrossDsdBuilder.cs" company="EUROSTAT">
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
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// The cross DSD builder.
    /// </summary>
    internal class CrossDsdBuilder : IBuilder<ICrossSectionalDataStructureMutableObject, IDataStructureMutableObject>
    {
        /// <summary>
        /// Builds an object of type <see cref="ICrossSectionalDataStructureMutableObject"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="IDataStructureMutableObject"/>
        /// </returns>
        public ICrossSectionalDataStructureMutableObject Build(IDataStructureMutableObject buildFrom)
        {
            // TODO look for an object mapper or expressions. There used to be Emit but it hasn't been updated since 2010. Automapper is way too slow.
            ICrossSectionalDataStructureMutableObject crossDsd = new CrossSectionalDataStructureMutableCore();
            crossDsd.AgencyId = buildFrom.AgencyId;
            crossDsd.Id = buildFrom.Id;
            crossDsd.Version = buildFrom.Version;
            crossDsd.Names.AddAll(buildFrom.Names);
            crossDsd.Descriptions.AddAll(buildFrom.Descriptions);
            crossDsd.Annotations.AddAll(buildFrom.Annotations);
            crossDsd.DimensionList = buildFrom.DimensionList;
            crossDsd.AttributeList = buildFrom.AttributeList;
            crossDsd.StartDate = buildFrom.StartDate;
            crossDsd.EndDate = buildFrom.EndDate;
            crossDsd.FinalStructure = buildFrom.FinalStructure;
            crossDsd.ExternalReference = buildFrom.ExternalReference;
            crossDsd.Groups.AddAll(buildFrom.Groups);
            crossDsd.MeasureList = buildFrom.MeasureList;
            crossDsd.ServiceURL = buildFrom.ServiceURL;
            crossDsd.StructureURL = buildFrom.StructureURL;
            crossDsd.Uri = buildFrom.Uri;

            return crossDsd;
        }
    }
}