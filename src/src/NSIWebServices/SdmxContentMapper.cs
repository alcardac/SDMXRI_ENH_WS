// -----------------------------------------------------------------------
// <copyright file="SdmxContentMapper.cs" company="EUROSTAT">
//   Date Created : 2014-05-30
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
namespace Estat.Sri.Ws.Rest
{
    using System.ServiceModel.Channels;

    using Estat.Sri.Ws.Rest.Utils;

    public class SdmxContentMapper  : WebContentTypeMapper
    {
        /// <summary>
        /// When overridden in a derived class, returns the message format used for a specified content type.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ServiceModel.Channels.WebContentFormat"/> that specifies the format to which the message content type is mapped. 
        /// </returns>
        /// <param name="contentType">The content type that indicates the MIME type of data to be interpreted.</param>
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            if (contentType.Contains("application/vnd.sdmx") || contentType.Contains("text/*") || contentType.Contains("application/*"))
            {
                return WebContentFormat.Raw;
            }
            if (contentType.Contains(SdmxMedia.ApplicationXml) || contentType.Contains(SdmxMedia.TextXml))
            {
                return WebContentFormat.Xml;
            }

            return WebContentFormat.Default;
        }
    }
}