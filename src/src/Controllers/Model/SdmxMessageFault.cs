// -----------------------------------------------------------------------
// <copyright file="SdmxMessageFault.cs" company="EUROSTAT">
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
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;

    public class SdmxMessageFault : MessageFault
    {

        private readonly MessageFault _messageFault;

        private readonly string _nameSpace;

        public SdmxMessageFault(MessageFault messageFault, string nameSpace)
        {
            if (messageFault == null)
            {
                throw new ArgumentNullException("messageFault");
            }

            this._messageFault = messageFault;
            this._nameSpace = nameSpace;
        }

        /// <summary>
        /// When overridden in a non-abstract derived class, writes the contents of the detail element. 
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the detail element.</param>
        protected override void OnWriteDetailContents(XmlDictionaryWriter writer)
        {
            var prefix = writer.LookupPrefix(this._nameSpace) ?? "web";
            using (var reader = this._messageFault.GetReaderAtDetailContents())
            {
                if ("Error".Equals(reader.LocalName) && string.IsNullOrWhiteSpace(reader.NamespaceURI))
                {
                    writer.WriteStartElement(prefix, "Error", this._nameSpace);
                    reader.Read();

                    // ErrorMessage
                    writer.WriteNode(reader, false);

                    // ErrorNumber
                    writer.WriteNode(reader, false);

                    // 
                    writer.WriteNode(reader, false);
                }
            }
        }

        /// <summary>
        /// Gets the SOAP fault code. 
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.FaultCode"/> that contains the SOAP fault code.
        /// </returns>
        public override FaultCode Code
        {
            get
            {
                return this._messageFault.Code;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="T:System.ServiceModel.Channels.MessageFault"/> has a detail object.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.ServiceModel.Channels.MessageFault"/> has a detail object; otherwise, false.
        /// </returns>
        public override bool HasDetail
        {
            get
            {
                return this._messageFault.HasDetail;
            }
        }

        /// <summary>
        /// Gets a textual description of a SOAP fault. 
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.FaultReason"/> that contains a textual description of a SOAP fault.
        /// </returns>
        public override FaultReason Reason
        {
            get
            {
                return this._messageFault.Reason;
            }
        }
    }
}