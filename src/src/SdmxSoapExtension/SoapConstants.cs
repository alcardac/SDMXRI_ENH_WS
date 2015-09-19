// -----------------------------------------------------------------------
// <copyright file="SoapConstants.cs" company="EUROSTAT">
//   Date Created : 2011-09-04
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
namespace Estat.Nsi.SdmxSoapValidatorExtension
{
    /// <summary>
    /// SOAP XML Tags
    /// </summary>
    public static class SoapConstants
    {
        #region Constants and Fields

        /// <summary>
        /// SOAP Body tag
        /// </summary>
        public const string Body = "Body";

        /// <summary>
        /// The local name of the SOAP 1.2 Fault Actor/Node element
        /// </summary>
        public const string Soap12ActorLocalName = "Node";

        /// <summary>
        /// XPath of SOAP 1.2 Detail element
        /// </summary>
        public const string Soap12DetailPath = "//soap:Fault/soap:Detail";

        /// <summary>
        /// XPath of SOAP 1.2 Reason/Fault String element
        /// </summary>
        public const string Soap12ReasonPath = "//soap:Reason";

        /// <summary>
        /// XPath of SOAP 1.2 Reason text element
        /// </summary>
        public const string Soap12ReasonTextPath = "soap:Text";

        /// <summary>
        /// The local name of the SOAP Fault Actor/Node element
        /// </summary>
        public const string SoapActorLocalName = "faultactor";

        /// <summary>
        /// XPath of SOAP Fault Code text
        /// </summary>
        public const string SoapCodePath = "//faultcode/text()";

        /// <summary>
        /// XPath of SOAP Detail element
        /// </summary>
        public const string SoapDetailPath = "//soap:Fault/detail";

        /// <summary>
        /// XPath of SOAP Reason/Fault String element
        /// </summary>
        public const string SoapReasonPath = "//faultstring";

        #endregion
    }
}