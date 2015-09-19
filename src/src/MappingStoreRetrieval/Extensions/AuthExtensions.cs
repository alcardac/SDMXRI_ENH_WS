// -----------------------------------------------------------------------
// <copyright file="AuthExtensions.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Collections.Generic;

    using Estat.Sdmxsource.Extension.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The dataflow authorization extensions.
    /// </summary>
    public static class AuthExtensions
    {
        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthExtensions));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Validate the specified <paramref name="authManager"/>.
        /// </summary>
        /// <param name="authManager">
        /// The dataflow authorization manager.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="authManager"/> is null but <paramref name="allowedDataflows"/> is not
        /// </exception>
        public static void ValidateAuthManager(this IAuthSdmxMutableObjectRetrievalManager authManager, IList<IMaintainableRefObject> allowedDataflows)
        {
            if (allowedDataflows != null && authManager == null)
            {
                _log.Error(ErrorMessages.ExceptionISdmxMutableObjectAuthRetrievalManagerNotSet);
                throw new ArgumentException(ErrorMessages.ExceptionISdmxMutableObjectAuthRetrievalManagerNotSet, "allowedDataflows");
            }
        }

        /// <summary>
        /// Check if <paramref name="structureType"/> requires authentication
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NeedsAuth(this IStructureReference structureType)
        {
            return structureType != null && structureType.MaintainableStructureEnumType.NeedsAuth();
        }

        #endregion
    }
}