// -----------------------------------------------------------------------
// <copyright file="LocalisedStringInsertEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-05
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
    using System.Data;
    using System.Data.Common;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The class responsible for adding records to <c>LOCALISED_STRING</c>.
    /// </summary>
    public class LocalisedStringInsertEngine
    {
        /// <summary>
        /// The _insert localized string
        /// </summary>
        private static readonly InsertLocalisedString _insertLocalisedString;

        /// <summary>
        /// Initializes static members of the <see cref="LocalisedStringInsertEngine"/> class.
        /// </summary>
        static LocalisedStringInsertEngine()
        {
            _insertLocalisedString = new StoredProcedures().InsertLocalisedString;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Insert a record with the values from <paramref name="maintainable" /> to <c>LOCALISED_STRING</c> for an artifact with the specified
        /// <paramref name="artefactPrimaryKey" />
        /// </summary>
        /// <param name="artefactPrimaryKey">The artifact primary key.</param>
        /// <param name="maintainable">The maintainable.</param>
        /// <param name="database">The database.</param>
        public void InsertForArtefact(long artefactPrimaryKey, INameableObject maintainable, Database database)
        {
            using (var dbCommand = _insertLocalisedString.CreateCommand(database))
            {
                _insertLocalisedString.CreateArtIdParameter(dbCommand).Value = artefactPrimaryKey;
                _insertLocalisedString.CreateItemIdParameter(dbCommand);
                InsertCommon(maintainable, dbCommand);
            }
        }

        /// <summary>
        /// Insert a record with the values from <paramref name="nameableObject" /> to <c>LOCALISED_STRING</c> for an item with the specified
        /// <paramref name="itemPrimaryKey" />
        /// </summary>
        /// <param name="itemPrimaryKey">The item primary key.</param>
        /// <param name="nameableObject">The nameable Object.</param>
        /// <param name="database">The database.</param>
        public void InsertForItem(long itemPrimaryKey, INameableObject nameableObject, Database database)
        {
            using (var dbCommand = _insertLocalisedString.CreateCommand(database))
            {
                _insertLocalisedString.CreateItemIdParameter(dbCommand).Value = itemPrimaryKey;
                _insertLocalisedString.CreateArtIdParameter(dbCommand);
                InsertCommon(nameableObject, dbCommand);
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

        /// <summary>
        /// Insert a record with the values from <paramref name="nameableObject"/> to <c>LOCALISED_STRING</c>
        /// </summary>
        /// <param name="nameableObject">
        /// The nameable object.
        /// </param>
        /// <param name="dbCommand">
        /// The DB command.
        /// </param>
        private static void InsertCommon(INameableObject nameableObject, DbCommand dbCommand)
        {
            _insertLocalisedString.CreateOutputParameter(dbCommand);
            var typeParameter = _insertLocalisedString.CreateTypeParameter(dbCommand);
            var languageParameter = _insertLocalisedString.CreateLanguageParameter(dbCommand);

            var textParameter = _insertLocalisedString.CreateTextParameter(dbCommand);
            typeParameter.Value = LocalisedStringType.Name;
            foreach (var name in nameableObject.Names)
            {
                InsertCommon(name, dbCommand, languageParameter, textParameter);
            }

            typeParameter.Value = LocalisedStringType.Desc;
            foreach (var description in nameableObject.Descriptions)
            {
                InsertCommon(description, dbCommand, languageParameter, textParameter);
            }
        }

        /// <summary>
        /// Insert a record with the values from <paramref name="text"/> to <c>LOCALISED_STRING</c>
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="dbCommand">
        /// The DB command.
        /// </param>
        /// <param name="languageParameter">
        /// The language Parameter.
        /// </param>
        /// <param name="textParameter">
        /// The text Parameter.
        /// </param>
        private static void InsertCommon(ITextTypeWrapper text, IDbCommand dbCommand, IDataParameter languageParameter, IDataParameter textParameter)
        {
            languageParameter.Value = GetLanguage(text);
            textParameter.Value = text.Value;

            dbCommand.ExecuteNonQuery();
        }

        #endregion
    }
}