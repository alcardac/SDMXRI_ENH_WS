// -----------------------------------------------------------------------
// <copyright file="StructureReferenceFromMutableBuilder.cs" company="EUROSTAT">
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
    using System;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///  Builds a <see cref="IStructureReference"/> from a <see cref="IMaintainableMutableObject"/>.
    /// </summary>
    public class StructureReferenceFromMutableBuilder : IBuilder<IStructureReference, IMaintainableMutableObject>
    {
        /// <summary>
        /// Builds an object of type <see cref="IStructureReference"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="IStructureReference"/>
        /// </returns>
        public IStructureReference Build(IMaintainableMutableObject buildFrom)
        {
            if (buildFrom == null)
            {
                throw new ArgumentNullException("buildFrom");
            }

            return new StructureReferenceImpl(buildFrom.AgencyId, buildFrom.Id, buildFrom.Version, buildFrom.StructureType);
        }
    }
}