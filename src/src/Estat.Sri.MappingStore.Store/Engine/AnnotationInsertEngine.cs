// -----------------------------------------------------------------------
// <copyright file="AnnotationInsertEngine.cs" company="EUROSTAT">
//   Date Created : 2014-10-13
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

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Extensions;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The class responsible for adding records to <c>ANNOTATION</c> and related tables.
    /// </summary>
    public class AnnotationInsertEngine : IAnnotationInsertEngine
    {
        /// <summary>
        /// The _insert annotation for artefacts
        /// </summary>
        private readonly InsertAnnotationText _annotationText;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationInsertEngine"/> class.
        /// </summary>
        public AnnotationInsertEngine()
        {
            this._annotationText = new StoredProcedures().InsertAnnotationText;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Insert a record with the values from <paramref name="annotations" /> to <paramref name="annotationProcedureBase" /> for an artifact with the specified
        /// <paramref name="annotatablePrimaryKey" />
        /// </summary>
        /// <param name="state">The mapping store connection and transaction state</param>
        /// <param name="annotatablePrimaryKey">The artifact primary key.</param>
        /// <param name="annotationProcedureBase">The annotation procedure base.</param>
        /// <param name="annotations">The annotations.</param>
        public void Insert(DbTransactionState state, long annotatablePrimaryKey, AnnotationProcedureBase annotationProcedureBase, IList<IAnnotation> annotations)
        {
            var count = annotations.Count;
            if (count == 0)
            {
                return;
            }

            var sysIdToAnnotation = new KeyValuePair<long, IAnnotation>[count];
            using (var command = annotationProcedureBase.CreateCommandWithDefaults(state))
            {
                annotationProcedureBase.CreateParentIdParameter(command, annotatablePrimaryKey);
                var outputParameter = annotationProcedureBase.CreateOutputParameter(command);
                for (int i = 0; i < count; i++)
                {
                    var annotation = annotations[i];
                    annotationProcedureBase.CreateIdParameter(command, annotation.Id);
                    annotationProcedureBase.CreateTitleParameter(command, annotation.Title);
                    annotationProcedureBase.CreateTypeParameter(command, annotation.Type);
                    annotationProcedureBase.CreateUriParameter(command, annotation.Uri != null ? annotation.Uri.ToString() : null);
                    command.ExecuteNonQuery();
                    if (annotation.Text.Count > 0)
                    {
                        sysIdToAnnotation[i] = new KeyValuePair<long, IAnnotation>((long)outputParameter.Value, annotation);
                    }
                }
            }

            using (var command = this._annotationText.CreateCommandWithDefaults(state))
            {
                for (int i = 0; i < sysIdToAnnotation.Length; i++)
                {
                    var keyValuePair = sysIdToAnnotation[i];
                    if (!keyValuePair.IsDefault())
                    {
                        this._annotationText.CreateAnnIdParameter(command).Value = keyValuePair.Key;
                        foreach (var textTypeWrapper in keyValuePair.Value.Text)
                        {
                            this._annotationText.CreateLanguageParameter(command).Value = GetLanguage(textTypeWrapper);
                            this._annotationText.CreateTextParameter(command).Value = textTypeWrapper.Value;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the normalized language.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The normalized language.
        /// </returns>
        private static string GetLanguage(ITextTypeWrapper text)
        {
            return string.IsNullOrEmpty(text.Locale) ? "en" : text.Locale.ToLowerInvariant();
        }

        #endregion
    }
}