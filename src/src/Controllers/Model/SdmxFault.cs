// -----------------------------------------------------------------------
// <copyright file="SdmxFault.cs" company="EUROSTAT">
//   Date Created : 2013-10-24
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
namespace Estat.Sri.Ws.Controllers.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    ///     The SDMX Fault.
    /// </summary>
    [DataContract(Name = "Error", Namespace = "")]
    public class SdmxFault
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxFault"/> class.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <param name="errorNumber">
        /// The error number.
        /// </param>
        /// <param name="errorSource">
        /// The error source.
        /// </param>
        public SdmxFault(string errorMessage, int errorNumber, string errorSource)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorNumber = errorNumber;
            this.ErrorSource = errorSource;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        /// <value>
        ///     The error message.
        /// </value>
        [DataMember]
        public string ErrorMessage { get; private set; }

        /// <summary>
        ///     Gets the error number.
        /// </summary>
        /// <value>
        ///     The error number.
        /// </value>
        [DataMember]
        public int ErrorNumber { get; private set; }

        /// <summary>
        ///     Gets the error source.
        /// </summary>
        /// <value>
        ///     The error source.
        /// </value>
        [DataMember]
        public string ErrorSource { get; private set; }

        #endregion
    }
}