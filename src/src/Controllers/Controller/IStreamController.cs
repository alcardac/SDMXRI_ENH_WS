// -----------------------------------------------------------------------
// <copyright file="IStreamController.cs" company="EUROSTAT">
//   Date Created : 2013-10-08
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Stream controller
    /// </summary>
    /// <typeparam name="T">
    /// The type of the output writer
    /// </typeparam>
    public interface IStreamController<in T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Stream XML output to <paramref name="writer"/>
        /// </summary>
        /// <param name="writer">
        ///     The writer to write the output to
        /// </param>
        /// <param name="actions"></param>
        void StreamTo(T writer, Queue<Action> actions);

        #endregion
    }
}