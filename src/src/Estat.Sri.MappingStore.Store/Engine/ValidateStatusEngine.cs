// -----------------------------------------------------------------------
// <copyright file="ValidateStatusEngine.cs" company="EUROSTAT">
//   Date Created : 2014-10-13
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using System;

    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The validate status engine.
    /// </summary>
    internal class ValidateStatusEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an error message with the specified <paramref name="format"/> for <paramref name="reference"/>
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="reference">
        /// The component.
        /// </param>
        /// <returns>
        /// The error message.
        /// </returns>
        public string GetError(string format, IStructureReference reference)
        {
            IMaintainableRefObject maintainableRefObject = reference.MaintainableReference;
            return reference.HasChildReference()
                       ? string.Format(
                           format, 
                           maintainableRefObject.MaintainableId, 
                           maintainableRefObject.AgencyId, 
                           maintainableRefObject.Version, 
                           reference.ChildReference.Id, 
                           reference.TargetReference)
                       : string.Format(format, maintainableRefObject.MaintainableId, maintainableRefObject.AgencyId, maintainableRefObject.Version, reference.TargetReference);
        }

        /// <summary>
        /// Gets the CodeList status.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="reference">
        /// The reference.
        /// </param>
        /// <param name="itemScheme">
        /// The item scheme.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeFinalStatus"/>.
        /// </returns>
        public ItemSchemeFinalStatus GetReferenceStatus(DbTransactionState state, IStructureReference reference, StructureCache itemScheme)
        {
            ItemSchemeFinalStatus codelistStatus = itemScheme.GetStructure(state, reference);
            this.ValidateFinalStatus(codelistStatus.FinalStatus, reference);
            return codelistStatus;
        }

        /// <summary>
        /// Validate the specified CodeList reference.
        /// </summary>
        /// <param name="refereceStatus">
        /// The codeList status.
        /// </param>
        /// <param name="codelistRef">
        /// The CodeList reference.
        /// </param>
        /// <exception cref="MappingStoreException">
        /// The specified <paramref name="refereceStatus"/> is not valid.
        /// </exception>
        public void ValidateFinalStatus(ArtefactFinalStatus refereceStatus, IStructureReference codelistRef)
        {
            if (refereceStatus == null)
            {
                throw new ArgumentNullException("refereceStatus");
            }

            if (refereceStatus.PrimaryKey < 1)
            {
                throw new MappingStoreException(this.GetError("Reference {3} {0}, Agency {1}, Version {2} is not available. Cannot import StructureSet.", codelistRef));
            }

            if (!refereceStatus.IsFinal)
            {
                throw new MappingStoreException(this.GetError("Referenced {3} {0}, Agency {1}, Version {2} is not Final. Cannot import StructureSet.", codelistRef));
            }
        }

        #endregion
    }
}