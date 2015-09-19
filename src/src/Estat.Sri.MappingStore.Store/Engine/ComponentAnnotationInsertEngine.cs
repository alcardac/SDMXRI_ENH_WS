// -----------------------------------------------------------------------
// <copyright file="ComponentAnnotationInsertEngine.cs" company="EUROSTAT">
//   Date Created : 2015-02-23
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using System.Collections.Generic;
    using System.Linq;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Annotation;

    /// <summary>
    ///     Implementation that removes certain auto-generated annotations for TIME.
    /// </summary>
    public class ComponentAnnotationInsertEngine : IAnnotationInsertEngine
    {
        #region Fields

        /// <summary>
        ///     The _decorated
        /// </summary>
        private readonly IAnnotationInsertEngine _decorated;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentAnnotationInsertEngine"/> class.
        /// </summary>
        /// <param name="decorated">
        /// The decorated.
        /// </param>
        public ComponentAnnotationInsertEngine(IAnnotationInsertEngine decorated)
        {
            this._decorated = decorated;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Insert a record with the values from <paramref name="annotations"/> to <paramref name="annotationProcedureBase"/>
        ///     for an artifact with the specified
        ///     <paramref name="annotatablePrimaryKey"/>
        /// </summary>
        /// <param name="state">
        /// The mapping store connection and transaction state
        /// </param>
        /// <param name="annotatablePrimaryKey">
        /// The artifact primary key.
        /// </param>
        /// <param name="annotationProcedureBase">
        /// The annotation procedure base.
        /// </param>
        /// <param name="annotations">
        /// The annotations.
        /// </param>
        public void Insert(DbTransactionState state, long annotatablePrimaryKey, AnnotationProcedureBase annotationProcedureBase, IList<IAnnotation> annotations)
        {
            this._decorated.Insert(
                state, 
                annotatablePrimaryKey, 
                annotationProcedureBase, 
                annotations.Where(annotation => !string.Equals(annotation.Title, AnnotationConstant.CodeTimeDimensionTitle)).ToArray());
        }

        #endregion
    }
}