// -----------------------------------------------------------------------
// <copyright file="SdmxDispatchMessageInspector.cs" company="EUROSTAT">
//   Date Created : 2013-10-25
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
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// The SDMX dispatch message inspector.
    /// </summary>
    public class SdmxDispatchMessageInspector : IDispatchMessageInspector
    {
        #region Fields

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the
        ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/>
        ///     method.
        /// </returns>
        /// <param name="request">
        /// The request message.
        /// </param>
        /// <param name="channel">
        /// The incoming channel.
        /// </param>
        /// <param name="instanceContext">
        /// The current service instance.
        /// </param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">
        /// The reply message. This value is null if the operation is one way.
        /// </param>
        /// <param name="correlationState">
        /// The correlation object returned from the
        ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/>
        ///     method.
        /// </param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply == null)
            {
                return;
            }

            if (reply.IsEmpty)
            {
            }
        }

        #endregion
    }
}