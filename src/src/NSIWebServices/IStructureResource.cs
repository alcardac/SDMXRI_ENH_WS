// -----------------------------------------------------------------------
// <copyright file="IStructureResource.cs" company="EUROSTAT">
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
    ///     TODO: Update summary.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IStructureResource
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get structure.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="resourceId">
        /// The resource id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/{agencyId}/{resourceId}/{version}/")]
        Message GetStructure(string structure, string agencyId, string resourceId, string version);

        /// <summary>
        /// The get structure all.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/")]
        Message GetStructureAll(string structure);

        /// <summary>
        /// The get structure all ids latest.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/{agencyId}/")]
        Message GetStructureAllIdsLatest(string structure, string agencyId);

        /// <summary>
        /// The get structure latest.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="resourceId">
        /// The resource id.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/{agencyId}/{resourceId}/")]
        Message GetStructureLatest(string structure, string agencyId, string resourceId);

        #endregion
    }
}