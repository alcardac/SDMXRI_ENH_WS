// -----------------------------------------------------------------------
// <copyright file="DbConstants.cs" company="EUROSTAT">
//   Date Created : 2011-08-13
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
namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// A list of constants used by the <see cref="DbAuthenticationProvider"/> and <see cref="DbAuthorizationProvider"/>
    /// </summary>
    internal static class DbConstants
    {
        #region Constants and Fields

        /// <summary>
        /// The dataflow agency id field. This string will replace the <see cref="DataflowAgencyIdMacro"/> from  <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowAgencyIdField = "dataflowAgencyIdField";

        /// <summary>
        /// The dataflow agency id macro variable used in the <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowAgencyIdMacro = "${agencyId}";

        /// <summary>
        /// The dataflow id field. This string will replace the <see cref="DataflowIdMacro"/> from  <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowIdField = "dataflowIdField";

        /// <summary>
        /// The dataflow id macro variable used in the <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowIdMacro = "${id}";

        /// <summary>
        /// The dataflow version field. This string will replace the <see cref="DataflowVersionMacro"/> from  <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowVersionField = "dataflowVersionField";

        /// <summary>
        /// The dataflow version macro variable used in the <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowVersionMacro = "${version}";

        /// <summary>
        /// The user macro variable used in the user provided queries
        /// </summary>
        public const string UserMacro = "${user}";

        /// <summary>
        /// The prepared statement parameter name
        /// </summary>
        public const string UserParamName = "auserid";

        #endregion
    }
}