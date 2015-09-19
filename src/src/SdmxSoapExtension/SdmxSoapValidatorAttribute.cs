// -----------------------------------------------------------------------
// <copyright file="SdmxSoapValidatorAttribute.cs" company="EUROSTAT">
//   Date Created : 2010-06-15
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
namespace Estat.Nsi.SdmxSoapValidatorExtension
{
    using System;
    using System.Web.Services.Protocols;

    /// <summary>
    /// Attribute that enables validation of Soap Messages and optionally the soap body contents using the current service WSDL and SDMX schema
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SdmxSoapValidatorAttribute : SoapExtensionAttribute
    {
        /*/// <summary>
        /// Whether to require the parameter container element under operation in SOAP message
        /// Defaults to : true
        /// </summary>
        bool _requireParameterContainer = true;*/
        #region Public Properties

        /// <summary>
        /// Getter for the SOAP Extension type. Which is in all cases <see cref="SdmxSoapValidator"/>
        /// </summary>
        public override Type ExtensionType
        {
            get
            {
                /*       if (_requireParameterContainer)
                {*/
                return typeof(SdmxSoapValidator);

                /*}
                else
                {
                    return typeof(SdmxSoapValidatorNoParameter);
                }*/
            }
        }

        /// <summary>
        /// Gets or sets the priority of the SOAP Extension
        /// Defaults to : 0
        /// </summary>
        public override int Priority { get; set; }

        #endregion

     
    }
}