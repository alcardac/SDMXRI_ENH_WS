// -----------------------------------------------------------------------
// <copyright file="SoapFaultFactory.cs" company="EUROSTAT">
//   Date Created : 2009-10-06
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
    using System.Web.Services.Protocols;
    using System.Xml;

    /// <summary>
    /// This class handle the creation of a SoapException
    /// in a standard format accepted by NSI Clients
    /// </summary>
    public static class SoapFaultFactory
    {
        #region Constants and Fields

        /// <summary>
        /// Value for the Error element of the SoapException inner xml. 
        /// The actual value is Error
        /// </summary>
        private const string Error = "Error";

        /// <summary>
        /// Value for the Error message element of the SoapException inner xml. 
        /// The actual value is ErrorMessage
        /// </summary>
        private const string ErrorMessage = "ErrorMessage";

        /// <summary>
        /// Value for the Error number element of the SoapException inner xml.
        /// The actual value is ErrorNumber
        /// </summary>
        private const string ErrorNumber = "ErrorNumber";

        /// <summary>
        /// Value for the Error source element of the SoapException inner xml. 
        /// The actual value is ErrorSource
        /// </summary>
        private const string ErrorSource = "ErrorSource";

        #endregion

        #region Public Methods

        /// <summary>
        /// This method initializes and populates an SoapException according to the input arguments
        /// </summary>
        /// <param name="source">
        /// The Uri that identify the piece of code that generated the exception
        /// </param>
        /// <param name="webServiceNamespace">
        /// The nsi namespace
        /// </param>
        /// <param name="errorMessage">
        /// The fault detail error message
        /// </param>
        /// <param name="errorNumber">
        /// The fault detail Error Number
        /// </param>
        /// <param name="errorSource">
        /// The fault detail error source
        /// </param>
        /// <param name="isClient">
        /// Identify if the error is caused on server or client side
        /// </param>
        /// <param name="faultValue">
        /// The fault message
        /// </param>
        /// <returns>
        /// The newly built SoapException
        /// </returns>
        public static SoapException CreateSoapException(
            string source, 
            string webServiceNamespace, 
            string errorMessage, 
            string errorNumber, 
            string errorSource, 
            bool isClient, 
            string faultValue)
        {
            var dom = new XmlDocument();

            // Create the root node
            XmlNode rootNode = dom.CreateNode(
                XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);

            // Create the error node and add it to the root node
            XmlNode errorNode = dom.CreateNode(XmlNodeType.Element, Error, webServiceNamespace);
            rootNode.AppendChild(errorNode);

            // Create the error number and add it to the error node
            XmlNode errorNumberNode = dom.CreateNode(XmlNodeType.Element, ErrorNumber, webServiceNamespace);
            errorNumberNode.InnerText = errorNumber;
            errorNode.AppendChild(errorNumberNode);

            // Create the error message and add it to the error node
            XmlNode errorMessageNode = dom.CreateNode(XmlNodeType.Element, ErrorMessage, webServiceNamespace);
            errorMessageNode.InnerText = errorMessage;
            errorNode.AppendChild(errorMessageNode);

            // Create the error source and add it to the error node
            XmlNode errorSourceNode = dom.CreateNode(XmlNodeType.Element, ErrorSource, webServiceNamespace);
            errorSourceNode.InnerText = errorSource;
            errorNode.AppendChild(errorSourceNode);

            // Create the location of the FaultCode
            XmlQualifiedName faultCode = isClient ? SoapException.ClientFaultCode : SoapException.ServerFaultCode;

            // Create the new exception
            var ret = new SoapException(faultValue, faultCode, source, rootNode);

            return ret;
        }

        #endregion
    }
}