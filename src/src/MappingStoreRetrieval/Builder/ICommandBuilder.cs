// -----------------------------------------------------------------------
// <copyright file="ICommandBuilder.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;

    /// <summary>
    /// The CommandBuilder interface.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="SqlQueryBase"/> based type
    /// </typeparam>
    internal interface ICommandBuilder<in T> : IBuilder<DbCommand, T>
        where T : SqlQueryBase
    {
    }
}