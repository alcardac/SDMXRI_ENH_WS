// -----------------------------------------------------------------------
// <copyright file="IAuthorizationProvider.cs" company="EUROSTAT">
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
    /// Interface for Authorization Providers
    /// </summary>
    public interface IAuthorizationProvider
    {
        #region Public Methods

        /// <summary>
        /// Check if there is at least one dataflow with the specified ID in list of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow ID
        /// </param>
        /// <returns>
        /// True if there is at least one dataflow with the specified ID in list of allowed dataflows. Else false
        /// </returns>
        bool AccessControl(IUser user, string dataflowId);

        /// <summary>
        /// Check if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowRef">
        /// The <see cref="IMaintainableRefObject"/> to check
        /// </param>
        /// <returns>
        /// True if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </returns>
        bool AccessControl(IUser user, IMaintainableRefObject dataflowRef);

        /// <summary>
        /// Get the collection of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <returns>
        /// The list of dataflows for the <see cref="IUser"/> 
        /// </returns>
        ICollection<IMaintainableRefObject> GetDataflows(IUser user);

        /// <summary>
        /// Get the collection of allowed dataflows with the specific dataflow id
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow id
        /// </param>
        /// <returns>
        /// The list of dataflows for the <see cref="IUser"/> 
        /// </returns>
        IEnumerable<IMaintainableRefObject> GetDataflows(IUser user, string dataflowId);

        #endregion
    }
}