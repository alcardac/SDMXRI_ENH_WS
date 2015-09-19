// -----------------------------------------------------------------------
// <copyright file="ItemStatus.cs" company="EUROSTAT">
//   Date Created : 2013-04-22
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
namespace Estat.Sri.MappingStore.Store.Model
{
    /// <summary>
    /// The item status.
    /// </summary>
    public class ItemStatus
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStatus"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="sysID">
        /// The sys id.
        /// </param>
        public ItemStatus(string id, long sysID)
        {
            this.Id = id;
            this.SysID = sysID;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the sys id.
        /// </summary>
        public long SysID { get; private set; }

        #endregion
    }
}