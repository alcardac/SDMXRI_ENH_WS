// -----------------------------------------------------------------------
// <copyright file="IdentifiableImportFactory.cs" company="EUROSTAT">
//   Date Created : 2013-04-20
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

    using Estat.Sri.MappingStore.Store.Engine;
    using Estat.Sri.MappingStore.Store.Helper;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The nameable import factory.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="INameableObject"/> type
    /// </typeparam>
    public class IdentifiableImportFactory<T> : IIdentifiableImportFactory<T>
        where T : IIdentifiableObject
    {
        #region Fields

        /// <summary>
        ///     The _custom factory.
        /// </summary>
        private readonly Func<IIdentifiableImportEngine<T>> _customFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableImportFactory{T}"/> class.
        /// </summary>
        /// <param name="customFactory">
        /// The custom factory.
        /// </param>
        public IdentifiableImportFactory(Func<IIdentifiableImportEngine<T>> customFactory)
        {
            this._customFactory = customFactory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IdentifiableImportFactory{T}" /> class.
        /// </summary>
        public IdentifiableImportFactory()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns the <see cref="IItemImportEngine{T}" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="IItemImportEngine{T}" />.
        /// </returns>
        public IIdentifiableImportEngine<T> GetIdentifiableImport()
        {
            IIdentifiableImportEngine<T> nameableImportEngine = null;
            if (this._customFactory != null)
            {
                nameableImportEngine = this._customFactory();
            }

            return nameableImportEngine ?? DefaultEngineHelper.GetIdentifiableEngine<T>();
        }

        #endregion
    }
}