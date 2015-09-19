// -----------------------------------------------------------------------
// <copyright file="InitializerValueObject.cs" company="EUROSTAT">
//   Date Created : 2011-09-04
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
    /// <summary>
    /// A VO class that holds the data needed to be passed from <see cref="SdmxSoapValidator.GetInitializer(System.Type)"/> to <see cref="SdmxSoapValidator.Initialize"/>
    /// </summary>
    public sealed class InitializerValueObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets name of the called Web Method
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets name space used by the current WSDL
        /// </summary>
        public string WsdlNamespace { get; set; }

        #endregion

        ///// <summary>
        ///// Whether to validation soap body
        ///// </summary>
        // public bool ValidateSoapBody { get; set; }
    }
}