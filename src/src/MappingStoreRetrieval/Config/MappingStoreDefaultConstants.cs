// -----------------------------------------------------------------------
// <copyright file="MappingStoreDefaultConstants.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Config
{
    /// <summary>
    /// The default Mapping store DB DB_TYPE to DB provider values 
    /// </summary>
    public static class MappingStoreDefaultConstants
    {
        #region Constants and Fields

        /// <summary>
        /// The default name used for MySQL DDB at Mapping Store database
        /// </summary>
        public const string MySqlName = "MySQL";

        /// <summary>
        /// The default provider for MySQL
        /// </summary>
        public const string MySqlProvider = "MySql.Data.MySqlClient";

        /// <summary>
        /// The default name used for Oracle DDB at Mapping Store database
        /// </summary>
        public const string OracleName = "Oracle";

        /// <summary>
        /// The default provider for Oracle
        /// </summary>
        public const string OracleProvider = "System.Data.OracleClient";

        /// <summary>
        /// The alternative provider for Oracle
        /// </summary>
        public const string OracleProviderOdp = "Oracle.DataAccess.Client";

        /// <summary>
        /// The default name used for PCAxis DDB at Mapping Store database
        /// </summary>
        public const string PCAxisName = "PCAxis";

        /// <summary>
        /// The default provider for PCAxis
        /// </summary>
        public const string PCAxisProvider = "org.estat.PcAxis.PcAxisProvider";

        /// <summary>
        /// The default name used for SQL Server DDB at Mapping Store database
        /// </summary>
        public const string SqlServerName = "SqlServer";

        /// <summary>
        /// The default provider for SQL Server
        /// </summary>
        public const string SqlServerProvider = "System.Data.SqlClient";

        #endregion
    }
}