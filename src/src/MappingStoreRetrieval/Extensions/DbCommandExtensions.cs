// -----------------------------------------------------------------------
// <copyright file="DbCommandExtensions.cs" company="EUROSTAT">
//   Date Created : 2015-06-26
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
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System.Data.Common;

    using log4net;

    /// <summary>
    /// Extensions for <see cref="DbCommand"/>
    /// </summary>
    public static class DbCommandExtension
    {
        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(DbCommandExtension));

        /// <summary>
        /// An exception free <see cref="DbCommand.Cancel()"/> to workaround issues with drivers, notably MySQL.
        /// </summary>
        /// <param name="command">The command.</param>
        public static void SafeCancel(this DbCommand command)
        {
            try
            {
                command.Cancel();
            }
            catch (DbException e)
            {
                _log.Warn("Error while trying to cancel the command. In some cases, e.g. MySQL, it is safe to ignore the error.", e);
            }
        }
    }
}