// -----------------------------------------------------------------------
// <copyright file="DataflowCompare.cs" company="EUROSTAT">
//   Date Created : 2011-06-19
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
namespace Estat.Nsi.AuthModule
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// Comparer for <see cref="IMaintainableRefObject"/>
    /// </summary>
    internal class DataflowCompare : EqualityComparer<IMaintainableRefObject>
    {
        #region Public Methods

        /// <summary>
        /// Determines whether two objects of type DataflowRefBean are equal.
        /// </summary>
        /// <returns>
        /// true if the specified DataflowRefBean are equal; otherwise, false.
        /// </returns>
        /// <param name="x">
        /// The first object to compare.
        ///                 </param>
        /// <param name="y">
        /// The second object to compare.
        /// </param>
        public override bool Equals(IMaintainableRefObject x, IMaintainableRefObject y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null)
            {
                return false;
            }

            if (y == null)
            {
                return false;
            }

            return string.Equals(x.MaintainableId, y.MaintainableId) && string.Equals(x.AgencyId, y.AgencyId)
                   && string.Equals(x.Version, y.Version);
        }

        /// <summary>
        /// A hash function for the specified DataflowRefBean for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the specified DataflowRefBean object.
        /// </returns>
        /// <param name="obj">
        /// The DataflowRefBean object for which to get a hash code. if null it returns 0
        /// </param>
        public override int GetHashCode(IMaintainableRefObject obj)
        {
            if (obj == null)
            {
                return 0;
            }

            int hash = (obj.MaintainableId ?? string.Empty).GetHashCode() ^ (obj.AgencyId ?? string.Empty).GetHashCode()
                       ^ (obj.Version ?? string.Empty).GetHashCode();
            return hash;
        }

        #endregion
    }
}