// -----------------------------------------------------------------------
// <copyright file="SoapOperationResponse.cs" company="EUROSTAT">
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
namespace Estat.Sri.Ws.Controllers.Constants
{
    /// <summary>
    ///     The soap operation response.
    /// </summary>
    public enum SoapOperationResponse
    {
        /// <summary>
        ///     The default value.
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The get compact data response.
        /// </summary>
        GetCompactDataResponse, 

        /// <summary>
        ///     The get utility data response.
        /// </summary>
        GetUtilityDataResponse, 

        /// <summary>
        ///     The get cross sectional data response.
        /// </summary>
        GetCrossSectionalDataResponse, 

        /// <summary>
        ///     The query structure response.
        /// </summary>
        QueryStructureResponse, 

        /// <summary>
        ///     The get generic data response.
        /// </summary>
        GetGenericDataResponse, 

        /// <summary>
        ///     The get generic time series data response.
        /// </summary>
        GetGenericTimeSeriesDataResponse, 

        /// <summary>
        ///     The get structure specific data response.
        /// </summary>
        GetStructureSpecificDataResponse, 

        /// <summary>
        ///     The get structure specific time series data response.
        /// </summary>
        GetStructureSpecificTimeSeriesDataResponse, 

        /// <summary>
        ///     The get generic metadata response.
        /// </summary>
        GetGenericMetadataResponse, 

        /// <summary>
        ///     The get structure specific metadata response.
        /// </summary>
        GetStructureSpecificMetadataResponse, 

        /// <summary>
        ///     The get structures response.
        /// </summary>
        GetStructuresResponse, 

        /// <summary>
        ///     The get dataflow response.
        /// </summary>
        GetDataflowResponse, 

        /// <summary>
        ///     The get metadataflow response.
        /// </summary>
        GetMetadataflowResponse, 

        /// <summary>
        ///     The get data structure response.
        /// </summary>
        GetDataStructureResponse, 

        /// <summary>
        ///     The get metadata structure response.
        /// </summary>
        GetMetadataStructureResponse, 

        /// <summary>
        ///     The get category scheme response.
        /// </summary>
        GetCategorySchemeResponse, 

        /// <summary>
        ///     The get concept scheme response.
        /// </summary>
        GetConceptSchemeResponse, 

        /// <summary>
        ///     The get codelist response.
        /// </summary>
        GetCodelistResponse, 

        /// <summary>
        ///     The get hierarchical codelist response.
        /// </summary>
        GetHierarchicalCodelistResponse, 

        /// <summary>
        ///     The get organisation scheme response.
        /// </summary>
        GetOrganisationSchemeResponse, 

        /// <summary>
        ///     The get reporting taxonomy response.
        /// </summary>
        GetReportingTaxonomyResponse, 

        /// <summary>
        ///     The get structure set response.
        /// </summary>
        GetStructureSetResponse, 

        /// <summary>
        ///     The get process response.
        /// </summary>
        GetProcessResponse, 

        /// <summary>
        ///     The get categorisation response.
        /// </summary>
        GetCategorisationResponse, 

        /// <summary>
        ///     The get provision agreement response.
        /// </summary>
        GetProvisionAgreementResponse, 

        /// <summary>
        ///     The get constraint response.
        /// </summary>
        GetConstraintResponse, 

        /// <summary>
        ///     The get data schema response.
        /// </summary>
        GetDataSchemaResponse, 

        /// <summary>
        ///     The get metadata schema response.
        /// </summary>
        GetMetadataSchemaResponse,

        /// <summary>
        /// The submit structure response
        /// </summary>
        SubmitStructureResponse
    }
}