﻿// -----------------------------------------------------------------------
// <copyright file="BaseRetrievalEngine.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Data;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    /// The base retrieval engine from Mapping store.
    /// </summary>
    internal abstract class BaseRetrievalEngine
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(BaseRetrievalEngine));

        /// <summary>
        /// The default URI.
        /// </summary>
        private static readonly Uri _defaultUri = new Uri("http://need/to/changeit");

        /// <summary>
        /// Gets the default URI.
        /// </summary>
        protected static Uri DefaultUri
        {
            get
            {
                return _defaultUri;
            }
        }

        /// <summary>
        /// Read the localized string from <paramref name="dataReader"/>
        /// </summary>
        /// <param name="item">
        ///     The <see cref="INameableMutableObject"/> . 
        /// </param>
        /// <param name="typeIdx">
        ///     The <c>LOCALISED_STRING.TYPE</c> ordinal 
        /// </param>
        /// <param name="txtIdx">
        ///     The <c>LOCALISED_STRING.TEXT</c> ordinal 
        /// </param>
        /// <param name="langIdx">
        ///     The <c>LOCALISED_STRING.LANGUAGE</c> ordinal 
        /// </param>
        /// <param name="dataReader">
        ///     The MASTORE DB <see cref="IDataReader"/> 
        /// </param>
        /// <param name="detail">The Structure Query Detail</param>
        protected static void ReadLocalisedString(INameableMutableObject item, int typeIdx, int txtIdx, int langIdx, IDataRecord dataReader, ComplexStructureQueryDetailEnumType detail = ComplexStructureQueryDetailEnumType.Full)
        {
            //// TODO support SDMX-ML Query detail CompleteStub versus Stub when Common API supports it. 
            //// When it is stub then only name should be returned. 
            //// When it is complete stub both name and description.
            //// Now we use StructureQueryDetail which is for REST queries only.
            //// According to the http://sdmx.org/wp-content/uploads/2012/05/SDMX_2_1-SECTION_07_WebServicesGuidelines_May2012.pdf
            //// page 10, footnotes 10-12 REST AllStubs == SDMX-ML Query Stub so in that case we skip description

            var value = DataReaderHelper.GetString(dataReader, txtIdx);
            var locale = DataReaderHelper.GetString(dataReader, langIdx);
            string type = DataReaderHelper.GetString(dataReader, typeIdx);
            var textType = new TextTypeWrapperMutableCore { Locale = locale, Value = value };
            
            switch (type)
            {
                case LocalisedStringType.Name:
                    item.Names.Add(textType);
                    break;
                case LocalisedStringType.Desc:
                    if (detail != ComplexStructureQueryDetailEnumType.Stub)
                    {
                        item.Descriptions.Add(textType);
                    }

                    break;
                default:
                    _log.WarnFormat(CultureInfo.InvariantCulture, "Unknown type at LOCALISATION.TYPE : '{0}', Locale: '{1}', Text:'{2}'", type, locale, value);
                    if (item.Names.Count == 0)
                    {
                        item.AddName(null, !string.IsNullOrWhiteSpace(value) ? value : item.Id);
                    }

                    break;
            }
        } 
    }
}