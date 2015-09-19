// -----------------------------------------------------------------------
// <copyright file="TextFormatTypesQueryEngine.cs" company="EUROSTAT">
//   Date Created : 2013-04-22
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
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Xml;

    using Dapper;

    using Estat.Ma.Helpers;
    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The text format types query engine.
    /// </summary>
    public class TextFormatTypesQueryEngine
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(TextFormatTypesQueryEngine));

        /// <summary>
        /// The _insert text format
        /// </summary>
        private static readonly InsertTextFormat _insertTextFormat;

        #endregion

        #region Fields

        /// <summary>
        ///     The _data type map.
        /// </summary>
        private readonly Dictionary<string, long> _dataTypeMap = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     The _facet map.
        /// </summary>
        private readonly Dictionary<string, long> _facetMap = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="TextFormatTypesQueryEngine"/> class.
        /// </summary>
        static TextFormatTypesQueryEngine()
        {
            _insertTextFormat = new StoredProcedures().InsertTextFormat;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatTypesQueryEngine"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public TextFormatTypesQueryEngine(ConnectionStringSettings connectionStringSettings)
            : this(DatabasePool.GetDatabase(connectionStringSettings))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatTypesQueryEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDB">
        /// The mapping store DB.
        /// </param>
        public TextFormatTypesQueryEngine(Database mappingStoreDB)
        {
            this.Init(mappingStoreDB);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatTypesQueryEngine" /> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public TextFormatTypesQueryEngine(DbTransactionState state)
        {
            this.Init(state.Database, state.Transaction);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Return the data type id for the specified <paramref name="value"/>
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The data type id; otherwise null
        /// </returns>
        public long? GetDataTypeId(string value)
        {
            long id;
            if (this._dataTypeMap.TryGetValue(value, out id))
            {
                return id;
            }

            return null;
        }

        /// <summary>
        /// Return the facet type id for the specified <paramref name="value"/>
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The facet type id; otherwise null
        /// </returns>
        public long? GetFacetTypeId(string value)
        {
            long id;
            if (this._facetMap.TryGetValue(value, out id))
            {
                return id;
            }

            return null;
        }

        /// <summary>
        /// Insert text formats.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="formats">
        /// The formats.
        /// </param>
        public void InsertTextFormats(DbTransactionState state, IEnumerable<KeyValuePair<long, ITextFormat>> formats)
        {
            var insertTextFormat = _insertTextFormat;
            using (DbCommand command = insertTextFormat.CreateCommand(state))
            {
                var compidParameter = insertTextFormat.CreateCompidParameter(command);
                var facetTypeEnumParameter = insertTextFormat.CreateFacetTypeEnumParameter(command);
                var facetValueParameter = insertTextFormat.CreateFacetValueParameter(command);
                insertTextFormat.CreateOutputParameter(command);

                var facets = new Stack<KeyValuePair<long, string>>();

                foreach (var format in formats)
                {
                    compidParameter.Value = format.Key;
                    var textFormat = format.Value;
                    if (textFormat.TextType != null)
                    {
                        var textType = textFormat.TextType.EnumType.ToString();
                        long id;
                        if (this._dataTypeMap.TryGetValue(textType, out id))
                        {
                            facetTypeEnumParameter.Value = id;
                            facetValueParameter.Value = DBNull.Value;
                            command.ExecuteNonQuery();
                        }
                    }

                    this.AddFacet(textFormat.Decimals, "decimals", facets);
                    this.AddFacet(textFormat.MaxLength, "maxLength", facets);
                    this.AddFacet(textFormat.MinLength, "minLength", facets);
                    this.AddFacet(textFormat.Pattern, "pattern", facets);
                    this.AddFacet(textFormat.TimeInterval, "timeInterval", facets);

                    this.AddFacet(textFormat.EndValue, "endValue", facets);
                    this.AddFacet(textFormat.Interval, "interval", facets);
                    this.AddFacet(textFormat.Sequence, "isSequence", facets);
                    this.AddFacet(textFormat.StartValue, "startValue", facets);

                    while (facets.Count > 0)
                    {
                        var keyValuePair = facets.Pop();
                        if (keyValuePair.Value.Length > 51)
                        {
                            _log.Error("ERROR: facet value over 51 characters.");
                        }

                        facetTypeEnumParameter.Value = keyValuePair.Key;
                        facetValueParameter.Value = keyValuePair.Value.Length > 51 ? keyValuePair.Value.Substring(0, 51) : keyValuePair.Value;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add facet to the specified <paramref name="facets"/>
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="facets">
        /// The facets stack.
        /// </param>
        /// <typeparam name="T">
        /// The non-null-able type. Such as <c>structs</c>.
        /// </typeparam>
        private void AddFacet<T>(T? value, string name, Stack<KeyValuePair<long, string>> facets) where T : struct
        {
            long id;
            if (value.HasValue && this._facetMap.TryGetValue(name, out id))
            {
                facets.Push(new KeyValuePair<long, string>(id, Convert.ToString(value.Value, CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        ///  Add facet to the specified <paramref name="facets"/>
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="facets">
        /// The facets.
        /// </param>
        private void AddFacet(TertiaryBool value, string name, Stack<KeyValuePair<long, string>> facets)
        {
            long id;
            if (value.IsSet() && this._facetMap.TryGetValue(name, out id))
            {
                facets.Push(new KeyValuePair<long, string>(id, XmlConvert.ToString(value.IsTrue)));
            }
        }

        /// <summary>
        ///  Add facet to the specified <paramref name="facets"/>
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="facets">
        /// The facets.
        /// </param>
        private void AddFacet(string value, string name, Stack<KeyValuePair<long, string>> facets)
        {
            long id;
            if (!string.IsNullOrEmpty(value) && this._facetMap.TryGetValue(name, out id))
            {
                facets.Push(new KeyValuePair<long, string>(id, value));
            }
        }

        /// <summary>
        /// The build text format maps.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        private void BuildTextFormatMaps(IDbConnection connection, IDbTransaction transaction = null)
        {
            foreach (var enumValue in connection.Query<EnumerationValue>("select ENUM_ID AS ID, ENUM_NAME AS NAME, ENUM_VALUE AS VALUE from ENUMERATIONS", null, transaction, buffered: false))
            {
                switch (enumValue.Name)
                {
                    case "DataType":
                        this._dataTypeMap.Add(enumValue.Value, enumValue.ID);
                        break;
                    case "FacetType":
                        this._facetMap.Add(enumValue.Value, enumValue.ID);
                        break;
                    default:
                        _log.WarnFormat(CultureInfo.InvariantCulture, "Unsupported enum_name {0} with value {1}", enumValue.Name, enumValue.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Initialize this object
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="transaction">The transaction.</param>
        private void Init(Database database, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                this.BuildTextFormatMaps(transaction.Connection, transaction);
            }
            else
            {
                using (var connection = database.CreateConnection())
                {
                    connection.Open();
                    this.BuildTextFormatMaps(connection);
                }
            }
        }

        #endregion
    }
}