// -----------------------------------------------------------------------
// <copyright file="ConnectionEntity.cs" company="EUROSTAT">
//   Date Created : 2013-04-10
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
namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This is the class representation of a dissemination database connection
    /// </summary>
    public class ConnectionEntity : PersistentEntityBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionEntity"/> class. 
        /// Initializes a new instance of the <see cref="PersistentEntityBase"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public ConnectionEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the database owner.
        /// </summary>
        /// <value>
        /// The database owner.
        /// </value>
        public string DBOwner { get; set; }
        
        /// <summary>
        /// Gets or sets the database ADO.NET connection string
        /// </summary>
        public string AdoConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database name
        /// </summary>
        public string DBName { get; set; }

        /// <summary>
        /// Gets or sets the database user password
        /// </summary>
        public string DBPassword { get; set; }

        /// <summary>
        /// Gets or sets the database server port
        /// </summary>
        public int DBPort { get; set; }

        /// <summary>
        /// Gets or sets the database server
        /// </summary>
        public string DBServer { get; set; }

        /// <summary>
        /// Gets or sets the database type
        /// </summary>
        public string DBType { get; set; }

        /// <summary>
        /// Gets or sets the database user
        /// </summary>
        public string DBUser { get; set; }

        /// <summary>
        /// Gets or sets the database JDBC connection string
        /// </summary>
        public string JdbcConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the connection name
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}