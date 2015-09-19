// -----------------------------------------------------------------------
// <copyright file="TableInfo.cs" company="EUROSTAT">
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
    /// The table info.
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// The _structure type
        /// </summary>
        private readonly SdmxStructureEnumType _structureType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableInfo"/> class.
        /// </summary>
        /// <param name="structureType">Type of the structure.</param>
        public TableInfo(SdmxStructureEnumType structureType)
        {
            this._structureType = structureType;
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the extra fields to include in the SELECT statement (should start with a comma)
        /// </summary>
        public string ExtraFields { get; set; }

        /// <summary>
        /// Gets the _structure type
        /// </summary>
        public SdmxStructureEnumType StructureType
        {
            get
            {
                return this._structureType;
            }
        }

        #endregion
    }
}