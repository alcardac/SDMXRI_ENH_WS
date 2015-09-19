// -----------------------------------------------------------------------
// <copyright file="ItemCommandBuilder.cs" company="EUROSTAT">
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
    using System.Data;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    /// <summary>
    /// The item command builder.
    /// </summary>
    internal class ItemCommandBuilder : ICommandBuilder<ItemSqlQuery>
    {
        #region Fields

        /// <summary>
        ///     The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        public ItemCommandBuilder(Database mappingStoreDb)
        {
            this._mappingStoreDb = mappingStoreDb;
        }

        /// <summary>
        ///     Gets the mapping store DB.
        /// </summary>
        protected Database MappingStoreDb
        {
            get
            {
                return this._mappingStoreDb;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public virtual DbCommand Build(ItemSqlQuery buildFrom)
        {
            var inParameter = this.MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, buildFrom.ParentSysId);
            return this.MappingStoreDb.GetSqlStringCommandParam(buildFrom.QueryInfo.ToString(), inParameter);
        }

        #endregion
    }
}