// -----------------------------------------------------------------------
// <copyright file="ItemSqlQuery.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
    /// <summary>
    ///     The item SQL query.
    /// </summary>
    internal class ItemSqlQuery : SqlQueryBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSqlQuery"/> class.
        /// </summary>
        /// <param name="queryInfo">
        /// The query Info.
        /// </param>
        /// <param name="parentSysId">
        /// The parent ItemScheme parent Id.
        /// </param>
        public ItemSqlQuery(SqlQueryInfo queryInfo, long parentSysId)
            : base(queryInfo)
        {
            this.ParentSysId = parentSysId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent item scheme primary key value.
        /// </summary>
        public long ParentSysId { get; set; }

        #endregion
    }
}