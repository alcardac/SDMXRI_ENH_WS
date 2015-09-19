// -----------------------------------------------------------------------
// <copyright file="CodeCollection.cs" company="EUROSTAT">
//   Date Created : 2013-04-10
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A string based <see cref="Collection{T}"/>
    /// </summary>
    public class CodeCollection : Collection<string>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCollection"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.Collection`1"/> class that is empty.
        /// </summary>
        public CodeCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCollection"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.Collection`1"/> class as a wrapper for the specified list.
        /// </summary>
        /// <param name="list">
        /// The list that is wrapped by the new collection.
        ///                 </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="list"/> is null.
        /// </exception>
        public CodeCollection(IList<string> list)
            : base(list)
        {
        }

        #endregion
    }
}