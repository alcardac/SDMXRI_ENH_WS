// -----------------------------------------------------------------------
// <copyright file="MaintainableReferenceDictionary.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// The maintainable reference map.
    /// </summary>
    public class MaintainableReferenceDictionary : MaintainableDictionary<IStructureReference>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        public MaintainableReferenceDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public MaintainableReferenceDictionary(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        public MaintainableReferenceDictionary(IDictionaryOfSets<IMaintainableMutableObject, IStructureReference> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected MaintainableReferenceDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}