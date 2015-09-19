// -----------------------------------------------------------------------
// <copyright file="IController.cs" company="EUROSTAT">
//   Date Created : 2011-12-07
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
namespace Estat.Sri.Ws.Controllers.Controller
{
    /// <summary>
    /// Web Service Controller interface for streaming output
    /// </summary>
    /// <typeparam name="T">
    /// The type of the request.
    /// </typeparam>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public interface IController<in T, in TWriter>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        IStreamController<TWriter> ParseRequest(T input);

        #endregion
    }
}