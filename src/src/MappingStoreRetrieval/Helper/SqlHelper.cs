// -----------------------------------------------------------------------
// <copyright file="SqlHelper.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;

    /// <summary>
    /// The SQL helper class.
    /// </summary>
    internal static class SqlHelper
    {
        /// <summary>
        /// Add where clause to <paramref name="sqlCommand"/>.
        /// </summary>
        /// <param name="sqlCommand">
        /// The SQL command buffer.
        /// </param>
        /// <param name="whereState">
        /// The WHERE clause state in <paramref name="sqlCommand"/>
        /// </param>
        /// <param name="format">
        /// The WHERE clause format.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void AddWhereClause(StringBuilder sqlCommand, WhereState whereState, string format, params object[] parameters)
        {
            switch (whereState)
            {
                case WhereState.Nothing:
                    sqlCommand.Append(" WHERE ");
                    break;
                case WhereState.Where:
                    break;
                case WhereState.And:
                    sqlCommand.Append(" AND ");
                    break;
            }

            sqlCommand.AppendFormat(CultureInfo.InvariantCulture, format, parameters);
        }
    }
}