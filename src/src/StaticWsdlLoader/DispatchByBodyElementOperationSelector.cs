// -----------------------------------------------------------------------
// <copyright file="DispatchByBodyElementOperationSelector.cs" company="EUROSTAT">
//   Date Created : 2013-10-21
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
namespace Estat.Sri.Ws.Wsdl
{
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.ServiceModel.Web;
    using System.Xml;

    /// <summary>
    ///     The dispatch by body element operation selector.
    /// </summary>
    /// <remarks>Based on <see href="http://msdn.microsoft.com/en-us/library/ms750531(v=vs.100).aspx" /> </remarks>
    public class DispatchByBodyElementOperationSelector : IDispatchOperationSelector
    {
        #region Fields

        /// <summary>
        ///     The _default operation name.
        /// </summary>
        private readonly string _defaultOperationName;

        /// <summary>
        ///     The _dispatch dictionary.
        /// </summary>
        private readonly IDictionary<XmlQualifiedName, string> _dispatchDictionary;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchByBodyElementOperationSelector"/> class.
        /// </summary>
        /// <param name="defaultOperationName">
        /// The default operation name.
        /// </param>
        /// <param name="dispatchDictionary">
        /// The dispatch dictionary.
        /// </param>
        public DispatchByBodyElementOperationSelector(string defaultOperationName, IDictionary<XmlQualifiedName, string> dispatchDictionary)
        {
            this._defaultOperationName = defaultOperationName;
            this._dispatchDictionary = dispatchDictionary;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Associates a local operation with the incoming method.
        /// </summary>
        /// <returns>
        /// The name of the operation to be associated with the <paramref name="message"/>.
        /// </returns>
        /// <param name="message">
        /// The incoming <see cref="T:System.ServiceModel.Channels.Message"/> to be associated with an
        ///     operation.
        /// </param>
        public string SelectOperation(ref Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            var lookupQName = new XmlQualifiedName(bodyReader.LocalName, bodyReader.NamespaceURI);
            message = CreateMessageCopy(message, bodyReader);
            if (this._dispatchDictionary.ContainsKey(lookupQName))
            {
                return this._dispatchDictionary[lookupQName];
            }

            if (this._defaultOperationName == null)
            {
                throw new WebFaultException<string>(lookupQName.ToString(), HttpStatusCode.BadRequest);
            }

            return this._defaultOperationName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create message copy.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        private static Message CreateMessageCopy(Message message, XmlDictionaryReader body)
        {
            Message copy = Message.CreateMessage(message.Version, message.Headers.Action, body);
            copy.Headers.CopyHeaderFrom(message, 0);
            copy.Properties.CopyProperties(message.Properties);
            return copy;
        }

        #endregion
    }
}