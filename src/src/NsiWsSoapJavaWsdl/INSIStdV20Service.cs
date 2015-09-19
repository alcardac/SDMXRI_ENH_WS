// -----------------------------------------------------------------------
// <copyright file="INSIStdV20Service.cs" company="EUROSTAT">
//   Date Created : 2013-10-19
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
    using System.ComponentModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Model;
    using Estat.Sri.Ws.Wsdl;

    /// <summary>
    ///     The SDMX v2.0 interface.
    /// </summary>
    [ServiceContract(Namespace = SoapNamespaces.SdmxV20JavaStd, ConfigurationName = "NSIStdV20Service", SessionMode = SessionMode.NotAllowed, Name = "NSIStdV20Service")]
    [DispatchByBodyElementBehavior]
    [Description("Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the standard SDMX 2.0 Schema files and no extra element.")]
    public interface INSIStdV20Service
    {
        #region Public Methods and Operators

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "*", ReplyAction = "*")]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20JavaStd)]
        Message DefaultHandler(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in compact format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx compact format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCompactData, SoapNamespaces.SdmxV20JavaStd)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20JavaStd)]
        Message GetCompactData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in cross sectional format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx cross sectional format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCrossSectionalData, SoapNamespaces.SdmxV20JavaStd)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20JavaStd)]
        Message GetCrossSectionalData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in generic format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx generic format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetGenericData, SoapNamespaces.SdmxV20JavaStd)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20JavaStd)]
        Message GetGenericData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in Utility format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx cross sectional format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetUtilityData, SoapNamespaces.SdmxV20JavaStd)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20JavaStd)]
        Message GetUtilityData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx structural metadata based on a sdmx query structure request
        /// </summary>
        /// <param name="request">
        /// The sdmx query structure request
        /// </param>
        /// <returns>
        /// The sdmx structural metadata inside a RegistryInterface QueryStructureResponse
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.QueryStructure, SoapNamespaces.SdmxV20JavaStd)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20JavaStd)]
        Message QueryStructure(Message request);

        #endregion
    }
}