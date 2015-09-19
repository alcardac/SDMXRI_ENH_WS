// -----------------------------------------------------------------------
// <copyright file="IStaticWsdlService.cs" company="EUROSTAT">
//   Date Created : 2013-10-10
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
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    ///     The static <c>WSDL</c> service
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IStaticWsdlService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the WSDL.
        /// </summary>
        /// <param name="name">
        /// The service name.
        /// </param>
        /// <returns>
        /// The stream to the WSDL.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{name}")]
        Stream GetWsdl(string name);

        #endregion
    }
}