// -----------------------------------------------------------------------
// <copyright file="ItemTableInfo.cs" company="EUROSTAT">
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
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The item table info.
    /// </summary>
    public class ItemTableInfo : TableInfo
    {
        #region Public Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTableInfo"/> class.
        /// </summary>
        /// <param name="structureType">Type of the structure.</param>
        public ItemTableInfo(SdmxStructureEnumType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        ///     Gets or sets the foreign key.
        /// </summary>
        public string ForeignKey { get; set; }

        /// <summary>
        ///     Gets or sets the parent item. If any.
        /// </summary>
        public string ParentItem { get; set; }

        #endregion
    }
}