// -----------------------------------------------------------------------
// <copyright file="SdmxMessageSoap.cs" company="EUROSTAT">
//   Date Created : 2013-11-18
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
namespace Estat.Sri.Ws.Controllers.Model
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Web;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Controller;

    /// <summary>
    /// The SDMX message
    /// </summary>
    public class SdmxMessageSoap : SdmxMessageBase
    {
        #region Constants and Fields

        /// <summary>
        /// The _XML qualified name
        /// </summary>
        private readonly XmlQualifiedName _xmlQualifiedName;

        /// <summary>
        /// A value indicating whether the message is a fault.
        /// </summary>
        private bool _isFaulted;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxMessageSoap"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <param name="xmlQualifiedName">Name of the XML qualified.</param>
        public SdmxMessageSoap(IStreamController<XmlWriter> controller, Func<Exception, FaultException> exceptionHandler, XmlQualifiedName xmlQualifiedName)
            : base(controller, exceptionHandler, OperationContext.Current.IncomingMessageVersion)
        {
            if (xmlQualifiedName == null)
            {
                throw new ArgumentNullException("xmlQualifiedName");
            }

            this._xmlQualifiedName = xmlQualifiedName;
        }

        #endregion

        /// <summary>
        /// Gets a value that indicates whether this message generates any SOAP faults.
        /// </summary>
        /// <returns>
        /// true if this message generates any SOAP faults; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
        public override bool IsFault
        {
            get
            {
                return this._isFaulted;
            }
        }

        /// <summary>
        /// Called when the message body is written to an XML file.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to write this message body to an XML file.</param>
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            try
            {
                var prefix = writer.LookupPrefix(this._xmlQualifiedName.Namespace) ?? "web";
                this.ActionQueue.Enqueue(() => writer.WriteStartElement(prefix, this._xmlQualifiedName.Name, this._xmlQualifiedName.Namespace));

                base.OnWriteBodyContents(writer);
                writer.WriteEndElement();
            }
            catch (FaultException e)
            {
                var messageFault = new SdmxMessageFault(e.CreateMessageFault(), this._xmlQualifiedName.Namespace);
                messageFault.WriteTo(writer, this.Version.Envelope);
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                }

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Response.StatusCode = 500;
                }

                this._isFaulted = true;
            }
            finally
            {
                writer.Flush();
            }
        }
    }
}