// -----------------------------------------------------------------------
// <copyright file="IAuthCommandBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-04-15
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
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The Authorization aware <see cref="DbCommand"/> Builder interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input
    /// </typeparam>
    internal interface IAuthCommandBuilder<in T> where T : SqlQueryBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="SqlQueryBase"/> based class to build from.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        DbCommand Build(T buildFrom, IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}