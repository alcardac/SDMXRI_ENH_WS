// -----------------------------------------------------------------------
// <copyright file="SqlQueryInfo.cs" company="EUROSTAT">
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
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;

    /// <summary>
    /// This class holds information about a SQL Query
    /// </summary>
    internal class SqlQueryInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryInfo"/> class. 
        /// </summary>
        public SqlQueryInfo()
        {
            this.WhereStatus = WhereState.And;
        }

        /// <summary>
        /// Gets or sets the SQL query format.
        /// </summary>
        public string QueryFormat { get; set; }

        /// <summary>
        /// Gets or sets the order by string if any. (Optional).
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it needs a WHERE clause.
        /// </summary>
        public WhereState WhereStatus { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="SqlQueryInfo"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="SqlQueryInfo"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", this.QueryFormat, this.OrderBy);
        }
    }
}