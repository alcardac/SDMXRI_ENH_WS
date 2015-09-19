// -----------------------------------------------------------------------
// <copyright file="ValidationHelper.cs" company="EUROSTAT">
//   Date Created : 2013-04-10
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
namespace Estat.Sri.MappingStore.Store.Helper
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The validation helper.
    /// </summary>
    public class ValidationHelper
    {
        #region Static Fields

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ValidationHelper));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Validate that the number of inserted codes equals the number of codes to insert.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="codeIds">
        /// The code ids.
        /// </param>
        /// <typeparam name="T">
        /// The item type
        /// </typeparam>
        [Conditional("DEBUG")]
        public static void Validate<T>(IItemSchemeObject<T> maintainable, IEnumerable<long> codeIds) where T : IItemObject
        {
            var insertedCount = codeIds.Count();
            var toBeInserted = maintainable.Items.Count;
            if (insertedCount < toBeInserted)
            {
                _log.ErrorFormat("Tried to import a codelist but not all codes were imported into the Mapping Store. Excepted : {0}, Actual: {1}.", toBeInserted, insertedCount);
                Debug.Assert(insertedCount < toBeInserted, "Tried to import a codelist but not all codes were imported into the Mapping Store");
            }
        }

        #endregion
    }
}