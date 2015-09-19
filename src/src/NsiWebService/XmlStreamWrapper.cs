// -----------------------------------------------------------------------
// <copyright file="XmlStreamWrapper.cs" company="EUROSTAT">
//   Date Created : 2011-12-05
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
namespace Estat.Nsi.DataDisseminationWS
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using Estat.Sri.Ws.Controllers.Builder;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Controller;

    using log4net;

    /// <summary>
    /// This class is a wrapper for streaming response instead of using <see cref="XmlDocument"/>
    /// </summary>
    [XmlRoot]
    [XmlSchemaProvider("GetAnySchema")]
    public class XmlStreamWrapper : IXmlSerializable
    {
        #region Constants and Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(XmlStreamWrapper));

        /// <summary>
        ///   The controller
        /// </summary>
        private readonly IStreamController<XmlWriter> _controller;

        /// <summary>
        /// The _soap operation
        /// </summary>
        private readonly SoapOperation _soapOperation;

        /// <summary>
        /// The _soap fault exception builder
        /// </summary>
        private readonly SoapFaultExceptionBuilder _soapFaultExceptionBuilder;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="XmlStreamWrapper" /> class.
        /// </summary>
        public XmlStreamWrapper()
        {
            this._soapFaultExceptionBuilder = new SoapFaultExceptionBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStreamWrapper"/> class.
        /// </summary>
        /// <param name="controller">
        /// The <see cref="IStreamController{XmlWriter}"/> which will be used at <see cref="WriteXml"/> 
        /// </param>
        /// <param name="soapOperation">
        /// The soap Operation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="controller"/>
        ///   is null
        /// </exception>
        internal XmlStreamWrapper(IStreamController<XmlWriter> controller, SoapOperation soapOperation) : this()
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            this._controller = controller;
            this._soapOperation = soapOperation;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get an empty <see cref="XmlQualifiedName"/>
        /// </summary>
        /// <param name="xs">
        /// Parameter not used in this method 
        /// </param>
        /// <returns>
        /// The <see cref="XmlQualifiedName.Empty"/> 
        /// </returns>
        public static XmlQualifiedName GetAnySchema(XmlSchemaSet xs)
        {
            // <complexType>
            var ct = new XmlSchemaComplexType { Name = "anyResultType" };

            // <sequence>
            var sequence = new XmlSchemaSequence();

            var any = new XmlSchemaAny { MinOccurs = 1, MaxOccurs = 1 };
            sequence.Items.Add(any);

            ct.Particle = sequence;

            var schema = new XmlSchema { TargetNamespace = "http://anyElement" };
            schema.Items.Add(ct);
            xs.Add(schema);
            return ct.QualifiedName;
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method. 
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. 
        /// </param>
        public void ReadXml(XmlReader reader)
        {
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. 
        /// </param>
        public void WriteXml(XmlWriter writer)
        {
            try
            {
                this._controller.StreamTo(writer, new Queue<Action>());
                writer.Flush();
            }
            catch (Exception e)
            {
                throw this._soapFaultExceptionBuilder.Build(e, this._soapOperation.ToString());
            }
        }

        #endregion
    }
}