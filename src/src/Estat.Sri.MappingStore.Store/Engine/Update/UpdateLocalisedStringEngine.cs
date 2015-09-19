// -----------------------------------------------------------------------
// <copyright file="UpdateLocalisedStringEngine.cs" company="EUROSTAT">
//   Date Created : 2014-04-10
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
namespace Estat.Sri.MappingStore.Store.Engine.Update
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;

    using Estat.Ma.Helpers;
    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Properties;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///     Class that provides methods to update the localized string of a nameable or maintainable.
    /// </summary>
    public class UpdateLocalisedStringEngine
    {
        #region Constants

        /// <summary>
        ///     The select localized string by foreign key
        /// </summary>
        private const string SelectLocalisedStringByForeignKey =
            "select LS_ID, TEXT, LANGUAGE from LOCALISED_STRING where TYPE={0} and (({1} is null) or (ART_ID = {1})) and (({2} is null) or (ITEM_ID = {2}))";

        /// <summary>
        ///     The update localized string statement. It take 2 parameters, 1. the text, 2. The LS_ID value.
        /// </summary>
        private const string UpdateLocalisedStringQuery = "UPDATE LOCALISED_STRING SET TEXT={0} where LS_ID = {1}";

        #endregion

        #region Fields

        /// <summary>
        /// The _insert localised string
        /// </summary>
        private static readonly InsertLocalisedString _insertLocalisedString;

        /// <summary>
        ///     The mapping store database
        /// </summary>
        private readonly Database _mappingStoreDatabase;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="UpdateLocalisedStringEngine"/> class.
        /// </summary>
        static UpdateLocalisedStringEngine()
        {
            _insertLocalisedString = new StoredProcedures().InsertLocalisedString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLocalisedStringEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDatabase">
        /// The mapping store database.
        /// </param>
        public UpdateLocalisedStringEngine(Database mappingStoreDatabase)
        {
            this._mappingStoreDatabase = mappingStoreDatabase;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Updates the specified <paramref name="mutable"/>.
        /// </summary>
        /// <param name="mutable">
        /// The nameable mutable object.
        /// </param>
        /// <param name="primaryKeyValue">
        /// The primary key value.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="mutable"/> is null
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="primaryKeyValue"/> is not valid
        /// </exception>
        public void Update(INameableMutableObject mutable, long primaryKeyValue)
        {
            bool isArtefact = IsArtefact(mutable);
            this.Update(primaryKeyValue, LocalisedStringType.Name, isArtefact, mutable.Names);
            this.Update(primaryKeyValue, LocalisedStringType.Desc, isArtefact, mutable.Descriptions);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified mutable is an <c>artefact</c>.
        /// </summary>
        /// <param name="mutable">
        /// The mutable.
        /// </param>
        /// <returns>
        /// true is the <c>LOCALISED_STRING.ART_ID</c> should be used; otherwise false.
        /// </returns>
        private static bool IsArtefact(IMutableObject mutable)
        {
            return mutable.StructureType.IsMaintainable || mutable.StructureType.IsOneOf(SdmxStructureEnumType.Hierarchy);
        }

        /// <summary>
        /// Executes the update statement.
        /// </summary>
        /// <param name="inMutableUpdateNames">
        /// The in mutable update names.
        /// </param>
        /// <param name="statement">
        /// The statement.
        /// </param>
        private void ExecuteUpdateStatement(IEnumerable<LocalisedStringVO> inMutableUpdateNames, string statement)
        {
            DbParameter primaryKeyParameter = this._mappingStoreDatabase.CreateInParameter("p_lsid", DbType.Int64);

            var textParameter = this._mappingStoreDatabase.CreateInParameter("p_text", DbType.String);
            using (var cmd = this._mappingStoreDatabase.GetSqlStringCommandFormat(statement, textParameter, primaryKeyParameter))
            {
                foreach (var name in inMutableUpdateNames)
                {
                    primaryKeyParameter.Value = name.PrimaryKeyValue;
                    textParameter.Value = name.TextTypeWrapper.Value;
                    var count = cmd.ExecuteNonQuery();
                    Debug.Assert(count <= 1, "Possible bug. More than 1 row affected at LOCALISED_STRING. Number of rows affected:" + count);
                }
            }
        }

        /// <summary>
        /// Inserts the specified text mutable object.
        /// </summary>
        /// <param name="textObject">
        /// The text mutable object.
        /// </param>
        /// <param name="isArtefact">
        /// The SDMX structure.
        /// </param>
        /// <param name="primaryKeyValue">
        /// The primary key value.
        /// </param>
        /// <param name="localisedType">
        /// A name or a description.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="textObject"/>
        ///     or
        ///     <paramref name="isArtefact"/>
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="isArtefact"/>
        ///     or <paramref name="primaryKeyValue"/>
        /// </exception>
        private void Insert(IEnumerable<ITextTypeWrapperMutableObject> textObject, bool isArtefact, long primaryKeyValue, string localisedType)
        {
            if (textObject == null)
            {
                throw new ArgumentNullException("textObject");
            }

            if (primaryKeyValue < 1)
            {
                throw new ArgumentException(Resources.ExceptionInvalidPrimaryKey, "primaryKeyValue");
            }

            var insertLocalisedString = _insertLocalisedString;
            using (var dbCommand = insertLocalisedString.CreateCommand(this._mappingStoreDatabase))
            {
                if (isArtefact)
                {
                    insertLocalisedString.CreateArtIdParameter(dbCommand).Value = primaryKeyValue;
                    insertLocalisedString.CreateItemIdParameter(dbCommand);
                }
                else
                {
                    insertLocalisedString.CreateArtIdParameter(dbCommand);
                    insertLocalisedString.CreateItemIdParameter(dbCommand).Value = primaryKeyValue;
                }

                insertLocalisedString.CreateOutputParameter(dbCommand);
                var typeParameter = insertLocalisedString.CreateTypeParameter(dbCommand);
                var languageParameter = insertLocalisedString.CreateLanguageParameter(dbCommand);

                var textParameter = insertLocalisedString.CreateTextParameter(dbCommand);
                typeParameter.Value = localisedType;
                foreach (var mutableObject in textObject)
                {
                    if (!string.IsNullOrWhiteSpace(mutableObject.Value))
                    {
                        languageParameter.Value = mutableObject.Locale;
                        textParameter.Value = mutableObject.Value;
                        dbCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the list of <see cref="LocalisedStringVO"/> for the specified nameable primary key value.
        /// </summary>
        /// <param name="nameablePrimaryKeyValue">
        /// The nameable primary key value.
        /// </param>
        /// <param name="localisedType">
        /// Type of the localized.
        /// </param>
        /// <param name="isArtefact">
        /// Set to <c>true</c> if the foreign key is <c>ART_ID</c>.
        /// </param>
        /// <returns>
        /// The list of <see cref="LocalisedStringVO"/>
        /// </returns>
        private IEnumerable<LocalisedStringVO> Retrieve(long nameablePrimaryKeyValue, string localisedType, bool isArtefact)
        {
            var typeParameter = this._mappingStoreDatabase.CreateInParameter("p_type", DbType.AnsiString, localisedType);
            DbParameter artIdParameter;
            DbParameter itemIdParameter;
            if (isArtefact)
            {
                artIdParameter = this._mappingStoreDatabase.CreateInParameter("p_artid", DbType.Int64, nameablePrimaryKeyValue);
                itemIdParameter = this._mappingStoreDatabase.CreateInParameter("p_itemId", DbType.Int64, DBNull.Value);
            }
            else
            {
                artIdParameter = this._mappingStoreDatabase.CreateInParameter("p_artid", DbType.Int64, DBNull.Value);
                itemIdParameter = this._mappingStoreDatabase.CreateInParameter("p_itemId", DbType.Int64, nameablePrimaryKeyValue);
            }

            using (var command = this._mappingStoreDatabase.GetSqlStringCommandFormat(SelectLocalisedStringByForeignKey, typeParameter, artIdParameter, itemIdParameter))
            using (var reader = command.ExecuteReader())
            {
                int lsdIdIdx = reader.GetOrdinal("LS_ID");
                int textIdx = reader.GetOrdinal("TEXT");
                int langIdx = reader.GetOrdinal("LANGUAGE");
                while (reader.Read())
                {
                    var lsdId = DataReaderHelper.GetInt64(reader, lsdIdIdx);
                    var text = DataReaderHelper.GetString(reader, textIdx);
                    var lang = DataReaderHelper.GetString(reader, langIdx);
                    yield return new LocalisedStringVO { PrimaryKeyValue = lsdId, TextTypeWrapper = new TextTypeWrapperMutableCore(lang, text) };
                }
            }
        }

        /// <summary>
        /// Updates the specified primary key value.
        /// </summary>
        /// <param name="primaryKeyValue">
        /// The primary key value.
        /// </param>
        /// <param name="localisedType">
        /// Type of the localized.
        /// </param>
        /// <param name="isArtefact">
        /// Set to <c>true</c> if the foreign key is <c>ART_ID</c>.
        /// </param>
        /// <param name="names">
        /// The names.
        /// </param>
        private void Update(long primaryKeyValue, string localisedType, bool isArtefact, IEnumerable<ITextTypeWrapperMutableObject> names)
        {
            var inDatabaseMap = this.Retrieve(primaryKeyValue, localisedType, isArtefact).ToDictionary(vo => vo.TextTypeWrapper.Locale);
            var inMutableMap = names.ToDictionary(vo => vo.Locale);
            var inMutableUpdateNames = from d in inDatabaseMap
                                       join m in inMutableMap on d.Key equals m.Key
                                       where !d.Value.TextTypeWrapper.Value.Equals(m.Value.Value)
                                       select new LocalisedStringVO { PrimaryKeyValue = d.Value.PrimaryKeyValue, TextTypeWrapper = m.Value };
            var inMutableNewNames = from m in inMutableMap where !inDatabaseMap.ContainsKey(m.Key) select m.Value;
            var toDeleteInDatabase = from localisedStringVo in inDatabaseMap where !inMutableMap.ContainsKey(localisedStringVo.Key) select localisedStringVo.Value.PrimaryKeyValue;

            this.ExecuteUpdateStatement(inMutableUpdateNames, UpdateLocalisedStringQuery);
            DbHelper.BulkDelete(this._mappingStoreDatabase, "LOCALISED_STRING", "LS_ID", new Stack<long>(toDeleteInDatabase));
            this.Insert(inMutableNewNames, isArtefact, primaryKeyValue, localisedType);
        }

        #endregion

        /// <summary>
        ///     The value object for the <c>LOCALISED_STRING</c>
        /// </summary>
        private class LocalisedStringVO
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the primary key value.
            /// </summary>
            /// <value>
            ///     The primary key value.
            /// </value>
            public long PrimaryKeyValue { get; set; }

            /// <summary>
            ///     Gets or sets the text type wrapper.
            /// </summary>
            /// <value>
            ///     The text type wrapper.
            /// </value>
            public ITextTypeWrapperMutableObject TextTypeWrapper { get; set; }

            #endregion
        }
    }
}