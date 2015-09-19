// -----------------------------------------------------------------------
// <copyright file="INSIStdV21Service.cs" company="EUROSTAT">
//   Date Created : 2013-10-22
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
    ///     The SDMX v2.1 SOAP interface.
    /// </summary>
    [ServiceContract(Namespace = SoapNamespaces.SdmxV21, ConfigurationName = "SdmxServiceService", SessionMode = SessionMode.NotAllowed, Name = "SdmxService")]
    [DispatchByBodyElementBehavior]
    [Description("Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.1 Schema files and the standard WSDL.")]
    public interface INSIStdV21Service
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
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetConstraint, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetConstraint(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetOrganisationScheme, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetOrganisationScheme(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetStructureSet, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetStructureSet(Message request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.SubmitStructure, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message SubmitStructure(Message request);

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
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message DefaultHandler(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCategorisation, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetCategorisation(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCategoryScheme, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetCategoryScheme(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCodelist, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetCodelist(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetConceptScheme, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetConceptScheme(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetDataStructure, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetDataStructure(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetDataflow, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetDataflow(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in generic format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> generic format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetGenericData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetGenericData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in generic format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> generic format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetGenericTimeSeriesData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetGenericTimeSeriesData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetHierarchicalCodelist, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetHierarchicalCodelist(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in structure specific format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> compact format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetStructureSpecificData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetStructureSpecificData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in structure specific format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> compact format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetStructureSpecificTimeSeriesData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetStructureSpecificTimeSeriesData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetStructures, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetStructures(Message request);

        #endregion
    }
}