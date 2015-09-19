// -----------------------------------------------------------------------
// <copyright file="IDataResource.cs" company="EUROSTAT">
//   Date Created : 2013-10-07
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
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;

    /// <summary>
    /// The DataResource interface.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IDataResource
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get generic data.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{flowRef}/{key}/{providerRef}/")]
        Message GetGenericData(string flowRef, string key, string providerRef);

        /// <summary>
        /// The get generic data all keys.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{flowRef}/")]
        Message GetGenericDataAllKeys(string flowRef);

        /// <summary>
        /// The get generic data all providers.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{flowRef}/{key}/")]
        Message GetGenericDataAllProviders(string flowRef, string key);

        #endregion
    }
}