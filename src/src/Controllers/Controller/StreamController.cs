// -----------------------------------------------------------------------
// <copyright file="StreamController.cs" company="EUROSTAT">
//   Date Created : 2013-10-10
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
    /// The stream controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public class StreamController<TWriter> : IStreamController<TWriter>
    {
        #region Fields

        /// <summary>
        ///     The _action.
        /// </summary>
        private readonly Action<TWriter, Queue<Action>> _action;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamController{TWriter}"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        public StreamController(Action<TWriter, Queue<Action>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this._action = action;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Stream XML output to <paramref name="writer"/>
        /// </summary>
        /// <param name="writer">
        ///     The writer to write the output to
        /// </param>
        /// <param name="actions"></param>
        public void StreamTo(TWriter writer, Queue<Action> actions)
        {
            this._action(writer, actions);
        }

        #endregion
    }
}