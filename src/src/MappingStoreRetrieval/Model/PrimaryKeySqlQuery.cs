// -----------------------------------------------------------------------
// <copyright file="PrimaryKeySqlQuery.cs" company="EUROSTAT">
//   Date Created : 2014-11-06
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
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    internal class PrimaryKeySqlQuery : SqlQueryBase
    {
        private readonly long _primaryKeyValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryBase" /> class.
        /// </summary>
        /// <param name="queryInfo">The query Info.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        public PrimaryKeySqlQuery(SqlQueryInfo queryInfo, long primaryKeyValue)
            : base(queryInfo)
        {
            this._primaryKeyValue = primaryKeyValue;
        }

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>
        /// The primary key value.
        /// </value>
        public long PrimaryKeyValue
        {
            get
            {
                return this._primaryKeyValue;
            }
        }
    }
}