// -----------------------------------------------------------------------
// <copyright file="INameableImportEngine.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using System.Data.Common;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The NameableImportEngine interface.
    /// </summary>
    /// <typeparam name="T">The <see cref="INameableObject" /> based type</typeparam>
    /// <typeparam name="TProc">The type of the procedure.</typeparam>
    public interface INameableImportEngine<in T, in TProc> where T : INameableObject where TProc : IIdentifiableProcedure
    {
        /// <summary>
        /// Inserts the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="command">The command.</param>
        /// <param name="itemProcedure">The item procedure.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The primary key value
        /// </returns>
        long RunCommand(T item, DbCommand command, TProc itemProcedure, DbTransactionState state);
    }
}