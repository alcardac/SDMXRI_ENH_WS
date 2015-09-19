﻿// -----------------------------------------------------------------------
// <copyright file="StructureMediaType.cs" company="EUROSTAT">
//   Date Created : 2013-10-11
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
namespace Estat.Sri.Ws.Rest.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The structure media type.
    /// </summary>
    public enum StructureMediaEnumType
    {
        /// <summary>
        /// The application XML.
        /// </summary>
        ApplicationXml, 

        /// <summary>
        /// Mime type <c>text/xml</c>.
        /// </summary>
        TextXml,

        /// <summary>
        /// The structure.
        /// </summary>
        Structure, 

        /// <summary>
        /// The edi structure.
        /// </summary>
        EdiStructure
    }

    /// <summary>
    /// The structure media type.
    /// </summary>
    public class StructureMediaType : BaseConstantType<StructureMediaEnumType>
    {
        #region Static Fields

        /// <summary>
        /// The instances.
        /// </summary>
        private static readonly IDictionary<StructureMediaEnumType, StructureMediaType> _instances = new Dictionary<StructureMediaEnumType, StructureMediaType>
                                                                                                        {
                                                                                                            {
                                                                                                                StructureMediaEnumType
                                                                                                                .ApplicationXml, 
                                                                                                                new StructureMediaType(
                                                                                                                StructureMediaEnumType
                                                                                                                .ApplicationXml, 
                                                                                                                SdmxMedia.ApplicationXml)
                                                                                                            }, 
                                                                                                            {
                                                                                                                StructureMediaEnumType
                                                                                                                .TextXml, 
                                                                                                                new StructureMediaType(
                                                                                                                StructureMediaEnumType
                                                                                                                .TextXml, 
                                                                                                                SdmxMedia.TextXml)
                                                                                                            }, 
                                                                                                            {
                                                                                                                StructureMediaEnumType.Structure, 
                                                                                                                new StructureMediaType(
                                                                                                                StructureMediaEnumType.Structure, 
                                                                                                                SdmxMedia.Structure)
                                                                                                            }, 
                                                                                                            {
                                                                                                                StructureMediaEnumType.EdiStructure, 
                                                                                                                new StructureMediaType(
                                                                                                                StructureMediaEnumType
                                                                                                                .EdiStructure, 
                                                                                                                SdmxMedia.EdiData)
                                                                                                            }
                                                                                                        };

        #endregion

        // private readonly StructureMediaEnumType _enumType;
        #region Fields

        /// <summary>
        /// The _sdmx media.
        /// </summary>
        private readonly string _sdmxMedia;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMediaType"/> class.
        /// </summary>
        /// <param name="mediaType">
        /// The Media type.
        /// </param>
        /// <param name="sdmxMedia">
        /// The SDMX media.
        /// </param>
        public StructureMediaType(StructureMediaEnumType mediaType, string sdmxMedia)
            : base(mediaType)
        {
            this._sdmxMedia = sdmxMedia;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the values.
        /// </summary>
        public static IEnumerable<StructureMediaType> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        /// Gets the media type name.
        /// </summary>
        public ContentType MediaTypeName
        {
            get
            {
                var contentType = new ContentType();
                contentType.MediaType = this._sdmxMedia;
                return contentType;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the value from <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// Type of the enumeration.
        /// </param>
        /// <returns>
        /// The <see cref="StructureMediaType"/>.
        /// </returns>
        public static StructureMediaType GetFromEnum(StructureMediaEnumType enumType)
        {
            StructureMediaType output;
            if (_instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// The get type from name.
        /// </summary>
        /// <param name="mediaTypeName">
        /// The media type name.
        /// </param>
        /// <returns>
        /// The <see cref="StructureMediaType"/>.
        /// </returns>
        public static StructureMediaType GetTypeFromName(string mediaTypeName)
        {
            if (string.IsNullOrEmpty(mediaTypeName) || new ContentType(mediaTypeName).MediaType.EndsWith("/*", StringComparison.Ordinal))
            {
                mediaTypeName = GetFromEnum(StructureMediaEnumType.Structure).MediaTypeName.MediaType;
            }

            return Values.FirstOrDefault(m => m.MediaTypeName.MediaType.Equals(mediaTypeName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// The get media type version.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The <see cref="ContentType"/>.
        /// </returns>
        public ContentType GetMediaTypeVersion(string version)
        {
            string outVersion = version;

            if (this.EnumType == StructureMediaEnumType.EdiStructure)
            {
                // ignore version. Are independent of version
                return this.MediaTypeName;
            }

            if (this.EnumType == StructureMediaEnumType.ApplicationXml || this.EnumType == StructureMediaEnumType.TextXml)
            {
                // according to SDMX WS guidelines return most recent version when application/xml
                return GetFromEnum(StructureMediaEnumType.Structure).GetMediaTypeVersion("2.1");
            }

            if (string.IsNullOrEmpty(version))
            {
                // if not given version, provide latest supported version for the STRUCTURE media type
                outVersion = "2.1";
            }
            else if (!version.Equals("2.0") && !version.Equals("2.1"))
            {
                // not supported version
                return null;
            }

            var contentType = new ContentType(this._sdmxMedia + ";version=" + outVersion);

            return contentType;
        }

        #endregion
    }
}