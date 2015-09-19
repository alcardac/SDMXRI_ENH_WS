// -----------------------------------------------------------------------
// <copyright file="DeleteEngineFactory.cs" company="EUROSTAT">
//   Date Created : 2013-04-29
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
namespace Estat.Sri.MappingStore.Store.Factory
{
    using System;
    using System.Configuration;

    using Estat.Sri.MappingStore.Store.Engine;
    using Estat.Sri.MappingStore.Store.Helper;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The delete engine factory.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="IMaintainableObject"/> based type.
    /// </typeparam>
    public class DeleteEngineFactory<T> : IDeleteEngineFactory<T>
        where T : IMaintainableObject
    {
        #region Fields

        /// <summary>
        /// The _custom method.
        /// </summary>
        private readonly Func<ConnectionStringSettings, IDeleteEngine<T>> _customMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteEngineFactory{T}"/> class.
        /// </summary>
        public DeleteEngineFactory()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteEngineFactory{T}"/> class.
        /// </summary>
        /// <param name="customMethod">
        /// The custom method.
        /// </param>
        public DeleteEngineFactory(Func<ConnectionStringSettings, IDeleteEngine<T>> customMethod)
        {
            this._customMethod = customMethod;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the delete engine.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings
        /// </param>
        /// <returns>
        /// The <see cref="IDeleteEngine{T}"/>.
        /// </returns>
        public IDeleteEngine<T> GetDeleteEngine(ConnectionStringSettings connectionStringSettings)
        {
            IDeleteEngine<T> engine = null;
            if (this._customMethod != null)
            {
                engine = this._customMethod(connectionStringSettings);
            }

            return engine ?? DefaultEngineHelper.GetArtefactDeleteEngine<T>(connectionStringSettings);
        }

        #endregion
    }
}