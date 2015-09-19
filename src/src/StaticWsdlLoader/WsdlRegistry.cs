// -----------------------------------------------------------------------
// <copyright file="WsdlRegistry.cs" company="EUROSTAT">
//   Date Created : 2013-10-21
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
namespace Estat.Sri.Ws.Wsdl
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The WSDL registry.
    /// </summary>
    public sealed class WsdlRegistry
    {
        #region Static Fields

        /// <summary>
        ///     The singleton instance
        /// </summary>
        private static readonly WsdlRegistry _instance = new WsdlRegistry();

        #endregion

        #region Fields

        /// <summary>
        ///     The _WSDL map
        /// </summary>
        private readonly IDictionary<string, WsdlInfo> _wsdlMap = new Dictionary<string, WsdlInfo>(StringComparer.Ordinal);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="WsdlRegistry" /> class from being created.
        /// </summary>
        private WsdlRegistry()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the singleton instance
        /// </summary>
        public static WsdlRegistry Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        ///     Gets or sets the WSDL REST URL.
        /// </summary>
        /// <value>
        ///     The WSDL rest URL.
        /// </value>
        public string WsdlRestUrl { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="wsdlInfo">
        /// The WSDL information.
        /// </param>
        public void Add(WsdlInfo wsdlInfo)
        {
            this._wsdlMap[wsdlInfo.Name] = wsdlInfo;
        }

        /// <summary>
        /// Adds the specified WSDL info.
        /// </summary>
        /// <param name="wsdlInfos">The WSDL info.</param>
        public void Add(params WsdlInfo[] wsdlInfos)
        {
            foreach (var wsdlInfo in wsdlInfos)
            {
                this._wsdlMap [wsdlInfo.Name] = wsdlInfo;
            }
        }

        ////public string GetBaseDir(string name)
        ////{
        ////    string baseDir;
        ////    if (this._baseDirMap.TryGetValue(name, out baseDir))
        ////    {
        ////        return baseDir;
        ////    }

        ////    return null;
        ////}

        /// <summary>
        /// Gets the WSDL path.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The WSDL path
        /// </returns>
        public WsdlInfo GetWsdlInfo(string name)
        {
            WsdlInfo wsdlPath;
            if (this._wsdlMap.TryGetValue(name, out wsdlPath))
            {
                return wsdlPath;
            }

            return null;
        }

        #endregion
    }
}