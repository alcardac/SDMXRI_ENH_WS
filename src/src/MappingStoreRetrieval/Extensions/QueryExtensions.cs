// -----------------------------------------------------------------------
// <copyright file="QueryExtensions.cs" company="EUROSTAT">
//   Date Created : 2013-06-13
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
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using Estat.Sri.MappingStoreRetrieval.Constants;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// Various Structure query related extensions
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Returns the version constraints.
        /// </summary>
        /// <param name="complexStructureReferenceObject">
        /// The complex structure reference object.
        /// </param>
        /// <returns>
        /// The <see cref="VersionQueryType"/>.
        /// </returns>
        public static VersionQueryType GetVersionConstraints(this IComplexStructureReferenceObject complexStructureReferenceObject)
        {
            return complexStructureReferenceObject.VersionReference != null && complexStructureReferenceObject.VersionReference.IsReturnLatest.IsTrue
                       ? VersionQueryType.Latest
                       : VersionQueryType.All;
        }

        /// <summary>
        /// Returns the version constraints.
        /// </summary>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <returns>
        /// The <see cref="VersionQueryType"/>.
        /// </returns>
        public static VersionQueryType GetVersionConstraints(this bool returnLatest)
        {
            return returnLatest
                       ? VersionQueryType.Latest
                       : VersionQueryType.All;
        }
    }
}