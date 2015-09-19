// -----------------------------------------------------------------------
// <copyright file="MappingStoreHeaderRetrievalManager.cs" company="EUROSTAT">
//   Date Created : 2013-04-11
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
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Configuration;

    using Estat.Sri.MappingStoreRetrieval.Engine;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    /// <summary>
    /// The mapping store header retrieval manager.
    /// </summary>
    public class MappingStoreHeaderRetrievalManager : IHeaderRetrievalManager
    {
        /// <summary>
        /// The _get header.
        /// </summary>
        private readonly Func<HeaderRetrieverEngine, IHeader> _getHeader;

        /// <summary>
        /// The header retriever engine.
        /// </summary>
        private readonly HeaderRetrieverEngine _headerRetrieverEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreHeaderRetrievalManager"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings for Mapping Store database.
        /// </param>
        /// <param name="getHeader">
        /// The method that gets the Header.
        /// </param>
        public MappingStoreHeaderRetrievalManager(ConnectionStringSettings connectionStringSettings, Func<HeaderRetrieverEngine, IHeader> getHeader)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            if (getHeader == null)
            {
                throw new ArgumentNullException("getHeader");
            }

            this._getHeader = getHeader;
            this._headerRetrieverEngine = new HeaderRetrieverEngine(new Database(connectionStringSettings));
        }

        /// <summary>
        /// Gets a header object
        /// </summary>
        public IHeader Header
        {
            get
            {
                return this._getHeader(this._headerRetrieverEngine);
            }
        }
    }
}