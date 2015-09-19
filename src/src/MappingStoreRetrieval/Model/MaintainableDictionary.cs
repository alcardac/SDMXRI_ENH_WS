// -----------------------------------------------------------------------
// <copyright file="MaintainableDictionary.cs" company="EUROSTAT">
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
    using System.Runtime.Serialization;

    using Estat.Sri.MappingStoreRetrieval.Helper;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    /// The maintainable dictionary.
    /// </summary>
    /// <typeparam name="TValue">
    /// The value type
    /// </typeparam>
    public class MaintainableDictionary<TValue> : DictionaryOfSets<IMaintainableMutableObject, TValue>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableDictionary{TValue}"/> class. 
        /// </summary>
        public MaintainableDictionary()
            : base(MaintainableMutableComparer.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableDictionary{TValue}"/> class. 
        /// </summary>
        /// <param name="capacity">
        /// The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2"/> can contain.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> is less than 0.
        /// </exception>
        public MaintainableDictionary(int capacity)
            : base(capacity, MaintainableMutableComparer.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableDictionary{TValue}"/> class. 
        /// </summary>
        /// <param name="dictionary">
        /// The <see cref="T:System.Collections.Generic.IDictionary`2"/> whose elements are copied to the new
        ///     <see cref="T:System.Collections.Generic.Dictionary`2"/>
        ///     .
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="dictionary"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="dictionary"/> contains one or more duplicate keys.
        /// </exception>
        public MaintainableDictionary(IDictionaryOfSets<IMaintainableMutableObject, TValue> dictionary)
            : base(dictionary, MaintainableMutableComparer.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableDictionary{TValue}"/> class. 
        /// </summary>
        /// <param name="info">
        /// A <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object containing the information required to serialize the
        ///     <see cref="T:System.Collections.Generic.Dictionary`2"/>
        ///     .
        /// </param>
        /// <param name="context">
        /// A <see cref="T:System.Runtime.Serialization.StreamingContext"/> structure containing the source and destination of the serialized stream associated with the
        ///     <see cref="T:System.Collections.Generic.Dictionary`2"/>
        ///     .
        /// </param>
        protected MaintainableDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}