// -----------------------------------------------------------------------
// <copyright file="ICrossRetrievalManagerFactory.cs" company="EUROSTAT">
//   Date Created : 2013-04-16
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
namespace Estat.Sri.MappingStoreRetrieval.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    /// The CrossRetrievalManagerFactory interface.
    /// </summary>
    public interface ICrossRetrievalManagerFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of <see cref="ICrossReferenceMutableRetrievalManager"/> created using the specified
        ///     <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval Manager.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        ICrossReferenceMutableRetrievalManager GetCrossRetrievalManager<T>(T settings, ISdmxMutableObjectRetrievalManager retrievalManager);

        #endregion
    }
}