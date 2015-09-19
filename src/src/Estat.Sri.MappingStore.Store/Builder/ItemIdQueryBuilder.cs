// -----------------------------------------------------------------------
// <copyright file="ItemIdQueryBuilder.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Builder
{
    using System;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;

    /// <summary>
    /// The item id query builder.
    /// </summary>
    public class ItemIdQueryBuilder : IBuilder<string, ItemTableInfo>
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemIdQueryBuilder"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        public ItemIdQueryBuilder(Database database)
        {
            this._database = database;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Builds an object of type <see cref="string"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from <see cref="ItemTableInfo"/>
        /// </param>
        /// <returns>
        /// Object of type <see cref="string"/>
        /// </returns>
        public string Build(ItemTableInfo buildFrom)
        {
            if (buildFrom == null)
            {
                throw new ArgumentNullException("buildFrom");
            }

            var id = this._database.BuildParameterName("id");

            return string.Format(
                CultureInfo.InvariantCulture,
                "select I.ID, I.ITEM_ID as SYSID from {0} T INNER JOIN ITEM I ON I.ITEM_ID = T.{1} WHERE T.{2} = {3} ",
                buildFrom.Table,
                buildFrom.PrimaryKey,
                buildFrom.ForeignKey,
                id);
        }

        #endregion
    }
}