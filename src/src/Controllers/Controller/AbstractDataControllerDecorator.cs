// -----------------------------------------------------------------------
// <copyright file="AbstractDataControllerDecorator.cs" company="EUROSTAT">
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
namespace Estat.Sri.Ws.Controllers.Controller
{
    /// <summary>
    /// The abstract data controller decorator.
    /// </summary>
    /// <typeparam name="TQuery">
    /// The query format type.
    /// </typeparam>
    /// <typeparam name="TWriter">
    /// The writer type.
    /// </typeparam>
    public abstract class AbstractDataControllerDecorator<TQuery, TWriter> : IController<TQuery, TWriter>
    {
        /// <summary>
        /// The _controller
        /// </summary>
        private readonly IController<TQuery, TWriter> _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDataControllerDecorator{TQuery, TWriter}"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        protected AbstractDataControllerDecorator(IController<TQuery, TWriter> controller)
        {
            this._controller = controller;
        }

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public virtual IStreamController<TWriter> ParseRequest(TQuery input)
        {
            return this._controller.ParseRequest(input);
        }
    }
}