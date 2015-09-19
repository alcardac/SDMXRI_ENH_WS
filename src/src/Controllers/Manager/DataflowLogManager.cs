// -----------------------------------------------------------------------
// <copyright file="DataflowLogManager.cs" company="EUROSTAT">
//   Date Created : 2014-11-03
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
namespace Estat.Sri.Ws.Controllers.Manager
{
    using System;
    using System.Configuration;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// The dataflow log manager.
    /// </summary>
    public class DataflowLogManager : IDataflowLogManager
    {
        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log;

        /// <summary>
        /// The _data format
        /// </summary>
        private readonly BaseDataFormat _dataFormat;

        /// <summary>
        /// The _separator.
        /// </summary>
        private readonly string _separator;

        /// <summary>
        /// Initializes static members of the <see cref="DataflowLogManager"/> class.
        /// </summary>
        static DataflowLogManager()
        {
            // use the same logger as in Java.
            _log = LogManager.GetLogger("org.estat.nsiws.dataflowlogger");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowLogManager" /> class.
        /// </summary>
        /// <param name="dataFormat">The data format.</param>
        public DataflowLogManager(BaseDataFormat dataFormat)
        {
            if (dataFormat == null)
            {
                throw new ArgumentNullException("dataFormat");
            }

            this._dataFormat = dataFormat;
            this._separator = ConfigurationManager.AppSettings["log.df.file.separator"] ?? ";";
        }

        /// <summary>
        /// Logs the specified dataflow identifier.
        /// </summary>
        /// <param name="dataflow">The dataflow identifier.</param>
        public void Log(IDataflowObject dataflow)
        {
            if (dataflow == null)
            {
                throw new ArgumentNullException("dataflow");
            }

            _log.InfoFormat("{0}{1}{2}", dataflow.Id, this._separator, this._dataFormat);
        }
    }
}