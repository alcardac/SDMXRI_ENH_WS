// -----------------------------------------------------------------------
// <copyright file="SdmxV20Errors.cs" company="EUROSTAT">
//   Date Created : 2013-10-24
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
namespace Estat.Sri.Ws.Controllers.Constants
{
    /// <summary>
    /// The sdmx v 20 errors.
    /// </summary>
    public static class SdmxV20Errors
    {
        #region Constants

        /// <summary>
        ///     The error code for client related errors in soap fault messages
        /// </summary>
        public const string ErrorNumberClient = "2000";

        /// <summary>
        ///     The error code for server related errors in soap fault messages
        /// </summary>
        public const string ErrorNumberServer = "1000";

        #endregion
    }
}