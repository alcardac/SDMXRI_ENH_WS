// -----------------------------------------------------------------------
// <copyright file="OuputStoredProcedureExtension.cs" company="EUROSTAT">
//   Date Created : 2013-04-09
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
namespace Estat.Sri.MappingStore.Store.Extension
{
    using System.Data.Common;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Model;

    /// <summary>
    /// The <see cref="OutputProcedureBase"/> extension.
    /// </summary>
    public static class OuputStoredProcedureExtension
    {
        /// <summary>
        /// Creates a new instance of a <see cref="DbCommand"/> for the stored procedure with the specified <paramref name="state"/> 
        /// </summary>
        /// <param name="stored">
        /// The stored.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// a new instance of a <see cref="DbCommand"/> for the stored procedure with the specified <paramref name="state"/> 
        /// </returns>
        /// <remarks>
        /// It sets the following <see cref="DbCommand"/> properties <see cref="DbCommand.Connection"/> , <see cref="DbCommand.Transaction"/> , <see cref="DbCommand.CommandType"/> and <see cref="DbCommand.CommandText"/>
        /// </remarks>
        public static DbCommand CreateCommandWithDefaults(this IProcedure stored, DbTransactionState state)
        {
            return stored.CreateCommandWithDefaults(state.Database);
        }

        /// <summary>
        /// Creates a new instance of a <see cref="DbCommand"/> for the stored procedure with the specified <paramref name="state"/> 
        /// </summary>
        /// <param name="stored">
        /// The stored.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// a new instance of a <see cref="DbCommand"/> for the stored procedure with the specified <paramref name="state"/> 
        /// </returns>
        /// <remarks>
        /// It sets the following <see cref="DbCommand"/> properties <see cref="DbCommand.Connection"/> , <see cref="DbCommand.Transaction"/> , <see cref="DbCommand.CommandType"/> and <see cref="DbCommand.CommandText"/>
        /// </remarks>
        public static DbCommand CreateCommand(this IProcedure stored, DbTransactionState state)
        {
            return stored.CreateCommand(state.Connection, state.Transaction);
        }
    }
}