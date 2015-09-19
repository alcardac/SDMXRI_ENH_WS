// -----------------------------------------------------------------------
// <copyright file="SetExtensions.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The set extensions.
    /// </summary>
    internal static class SetExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Get one or nothing.
        /// </summary>
        /// <param name="mutableObjects">
        /// The mutable objects.
        /// </param>
        /// <typeparam name="T">
        /// The type of the <paramref name="mutableObjects"/>
        /// </typeparam>
        /// <returns>
        /// The <typeparamref name="T"/>.
        /// </returns>
        public static T GetOneOrNothing<T>(this ISet<T> mutableObjects)
        {
            //// Note do not use SingleOrDefault
            switch (mutableObjects.Count)
            {
                case 0:
                    return default(T);
                case 1:
                    return mutableObjects.First();
                default:
                    throw new ArgumentException(ErrorMessages.MoreThanOneArtefact);
            }
        }

        /// <summary>
        /// Check if the specified <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/>. 
        /// </summary>
        /// <param name="sdmxStructureTypes">
        /// The SDMX structure types.
        /// </param>
        /// <param name="structureEnumType">
        /// The structure type as an ENUM.
        /// </param>
        /// <returns>
        /// True if <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/> or if <paramref name="sdmxStructureTypes"/> is empty; otherwise false.  
        /// </returns>
        public static bool HasStructure(this ISet<SdmxStructureType> sdmxStructureTypes, SdmxStructureEnumType structureEnumType)
        {
            return sdmxStructureTypes.Count == 0 || sdmxStructureTypes.Contains(SdmxStructureType.GetFromEnum(structureEnumType));
        }


        /// <summary>
        /// Check if the specified <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/>. 
        /// </summary>
        /// <param name="sdmxStructureTypes">
        /// The SDMX structure types.
        /// </param>
        /// <param name="structureEnumType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// True if <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/> or if <paramref name="sdmxStructureTypes"/> is empty; otherwise false.  
        /// </returns>
        public static bool HasStructure(this ISet<SdmxStructureType> sdmxStructureTypes, SdmxStructureType structureEnumType)
        {
            return sdmxStructureTypes.Count == 0 || sdmxStructureTypes.Contains(structureEnumType);
        }

        #endregion
    }
}