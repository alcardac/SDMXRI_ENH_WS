// -----------------------------------------------------------------------
// <copyright file="CategorisationBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-07-22
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
namespace Estat.Sri.MappingStore.Store.Builder
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;

    /// <summary>
    /// The categorisation builder.
    /// </summary>
    public class CategorisationBuilder
    {
        /// <summary>
        /// Return a <see cref="ICategorisationObject"/>  with the specified <paramref name="categoryReference"/> and <paramref name="structureReference"/>
        /// </summary>
        /// <param name="categoryReference">
        /// The category reference.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <returns>
        /// The <see cref="ICategorisationObject"/>.
        /// </returns>
        public ICategorisationObject Build(IStructureReference categoryReference, IStructureReference structureReference)
        {
            ICategorisationMutableObject mutable = new CategorisationMutableCore();
            mutable.CategoryReference = categoryReference;
            mutable.StructureReference = structureReference;

            var structureRef = structureReference.MaintainableReference;
            var categorySchemeRef = categoryReference.MaintainableReference;
            string name =
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}@{1}@{2}@{3}@{4}@{5}",
                    structureRef.MaintainableId,
                    structureRef.AgencyId,
                    structureRef.Version,
                    categorySchemeRef.MaintainableId,
                    categorySchemeRef.Version,
                    categoryReference.ChildReference.Id).Replace(".", string.Empty);
            mutable.Id = name.Length > 50 ? BuildHash(name) : name;

            mutable.Version = "1.0";
            mutable.AgencyId = categorySchemeRef.AgencyId;
            mutable.AddName("en", name);
            return mutable.ImmutableInstance;
        }

        /// <summary>
        /// Builds the hash.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The hash of the input </returns>
        private static string BuildHash(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            using (SHA1 sha1Manager = new SHA1Managed())
            {
                var computeHash = sha1Manager.ComputeHash(buffer);

                return BitConverter.ToString(computeHash).Replace("-", string.Empty);
            }
        }
    }
}
