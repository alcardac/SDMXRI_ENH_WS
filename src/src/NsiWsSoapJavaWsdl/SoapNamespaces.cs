// -----------------------------------------------------------------------
// <copyright file="SoapNamespaces.cs" company="EUROSTAT">
//   Date Created : 2013-10-22
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
namespace Estat.Sri.Ws.Soap
{
    using System;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Constants;

    /// <summary>
    ///     The soap namespaces.
    /// </summary>
    public static class SoapNamespaces
    {
        #region Constants

        /// <summary>
        ///     The SDMX v2.0 with Eurostat extensions.
        /// </summary>
        public const string SdmxV20Estat = "http://ec.europa.eu/eurostat/sri/service/2.0/extended";

        /// <summary>
        ///     The SDMX v2.0 Java. (note the '/' on the end and those are NAMESPACES not URL.)
        /// </summary>
        public const string SdmxV20JavaStd = "http://ec.europa.eu/eurostat/sri/service/2.0/";

        /// <summary>
        ///     The SDMX v2.0 .NET. (note the missing '/' on the end and those are NAMESPACES not URL.)
        /// </summary>
        public const string SdmxV20NetStd = "http://ec.europa.eu/eurostat/sri/service/2.0";

        /// <summary>
        ///     The SDMX v2.1.
        /// </summary>
        public const string SdmxV21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/webservices";

        #endregion

        /// <summary>
        /// Gets the SOAP operation.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="SoapOperation"/>.
        /// </returns>
        /// <remarks>This method will the Message body contents.</remarks>
        public static SoapOperation GetSoapOperation(this Message message)
        {
            using (XmlDictionaryReader reader = message.GetReaderAtBodyContents())
            {
                do
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.NamespaceURI.Equals(SdmxV21) || reader.NamespaceURI.Equals(SdmxV20Estat) || reader.NamespaceURI.Equals(SdmxV20JavaStd))
                        {
                            SoapOperation soapOperation;
                            if (Enum.TryParse(reader.LocalName, out soapOperation))
                            {
                                return soapOperation;
                            }
                        }
                    }
                }
                while (reader.Read());
            }

            return SoapOperation.Null;
        }
    }
}