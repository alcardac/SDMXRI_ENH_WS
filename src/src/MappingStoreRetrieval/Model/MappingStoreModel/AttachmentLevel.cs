// -----------------------------------------------------------------------
// <copyright file="AttachmentLevel.cs" company="EUROSTAT">
//   Date Created : 2013-03-20
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
namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// Attribute attachment level
    /// </summary>
    public enum AttachmentLevel
    {
        /// <summary>
        /// For non attribute types. Default.
        /// </summary>
        None = 0, 

        /// <summary>
        /// Dataset level
        /// </summary>
        DataSet, 

        /// <summary>
        /// Group level.
        /// </summary>
        /// <remarks>Requires to define which groups</remarks>
        Group, 

        /// <summary>
        /// Series level
        /// </summary>
        Series, 

        /// <summary>
        /// Observation level
        /// </summary>
        Observation
    }
}