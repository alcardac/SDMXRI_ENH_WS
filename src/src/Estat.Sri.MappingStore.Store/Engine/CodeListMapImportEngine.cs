// -----------------------------------------------------------------------
// <copyright file="CodeListMapImportEngine.cs" company="EUROSTAT">
//   Date Created : 2014-10-10
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using Estat.Ma.Model.StoredProcedure;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    /// The CodeListMap import engine
    /// </summary>
    internal class CodeListMapImportEngine : ItemSchemeMapImportEngine<ICodelistMapObject, InsertCodeListMapProcedure, InsertCodeMapProcedure>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeListMapImportEngine"/> class.
        /// </summary>
        public CodeListMapImportEngine()
            : base(SdmxStructureEnumType.Code)
        {
        }
    }
}