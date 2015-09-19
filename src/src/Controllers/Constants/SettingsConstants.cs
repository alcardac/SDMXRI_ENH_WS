// -----------------------------------------------------------------------
// <copyright file="SettingsConstants.cs" company="EUROSTAT">
//   Date Created : 2011-09-09
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
    using Estat.Sri.Ws.Controllers.Manager;

    /// <summary>
    ///     Constant values used in <see cref="SettingsManager" />
    /// </summary>
    internal static class SettingsConstants
    {
        #region Constants

        /// <summary>
        ///     The web.config appSettings variable name for overriding the 32bit binary folder.
        ///     Default is set at <see cref="DefaultBin32" />
        /// </summary>
        public const string Bin32 = "bin32";

        /// <summary>
        ///     The web.config appSettings variable name for overriding the 64bit binary folder.
        ///     Default is set at <see cref="DefaultBin64" />
        /// </summary>
        public const string Bin64 = "bin64";

        /// <summary>
        ///     Default 32bit directory name
        /// </summary>
        public const string DefaultBin32 = "win32";

        /// <summary>
        ///     Default 64bit directory name
        /// </summary>
        public const string DefaultBin64 = "x64";

        /// <summary>
        ///     The web.config appSettings variable name for the default DDB Oracle Provider
        /// </summary>
        public const string DefaultDdbOracleProvider = "defaultDDBOracleProvider";

        /// <summary>
        ///     The default prefix
        /// </summary>
        public const string DefaultPrefix = "defaultPrefix";

        /// <summary>
        ///     DLL extension with dot.
        /// </summary>
        public const string DllExtension = ".dll";

        /// <summary>
        ///     The web.config appSettings variable name for log level
        /// </summary>
        public const string LogLevel = "logLevel";

        /// <summary>
        ///     The web.config appSettings variable name for log template
        /// </summary>
        public const string LogTemplate = "logTemplateFormat";

        /// <summary>
        ///     The Name of the mapping store connection string. The default value is MappingStoreServer
        /// </summary>
        public const string MappingStoreConnectionName = "MappingStoreServer";

        /// <summary>
        ///     The path environment setting
        /// </summary>
        public const string PathEnvironment = "path";

        /// <summary>
        ///     The additional path setting
        /// </summary>
        public const string PathSetting = "path";

        /// <summary>
        ///     The web.config appSettings variable name for overriding the PlatformSpecificAssemblies separated by comma
        ///     Default is set at <see cref="SettingsManager._platformAssemblies" />
        /// </summary>
        public const string PlatformSpecificAssemblies = "PlatformSpecificAssemblies";

        /// <summary>
        ///     Sqlite data provider
        /// </summary>
        public const string SqlLiteDataProvider = "System.Data.SQLite";

        /// <summary>
        ///     Virtual bin folder in the app directory
        /// </summary>
        public const string VirtualBin = "~/bin";

        #endregion
    }
}