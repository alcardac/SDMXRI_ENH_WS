// -----------------------------------------------------------------------
// <copyright file="CategoryBuilder.cs" company="EUROSTAT">
//   Date Created : 2013-04-29
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
namespace Estat.Sri.MappingStore.Store.Builder
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The category builder.
    /// </summary>
    public class CategoryBuilder : IBuilder<ICategoryObject, ICrossReference>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a <see cref="ICategoryObject"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type  <see cref="ICategoryObject"/>
        /// </returns>
        public ICategoryObject Build(ICrossReference buildFrom)
        {
            return new CategoryMock(buildFrom);
        }

        #endregion

        /// <summary>
        ///     The category mock.
        /// </summary>
        private class CategoryMock : ICategoryObject
        {
            #region Fields

            /// <summary>
            /// The _categorisation.
            /// </summary>
            private readonly ICategorisationObject _categorisation;

            /// <summary>
            ///     The _cross reference.
            /// </summary>
            private readonly ICrossReference _crossReference;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="CategoryMock"/> class.
            /// </summary>
            /// <param name="crossReference">
            /// The cross reference.
            /// </param>
            public CategoryMock(ICrossReference crossReference)
            {
                this._crossReference = crossReference;
                this._categorisation = this._crossReference.ReferencedFrom as ICategorisationObject;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets a list of all the underlying text types for this identifiable (does not recurse down to children).
            ///     <p />
            ///     Gets an empty list if there are no text types for this identifiable
            /// </summary>
            /// <value> </value>
            public IList<ITextTypeWrapper> AllTextTypes
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets the list of annotations
            ///     <p />
            ///     <b>NOTE</b>The list is a copy so modify the returned list will not
            ///     be reflected in the AnnotableObject instance
            /// </summary>
            /// <value> </value>
            public IList<IAnnotation> Annotations
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Builds a IStructureReference that is a representation of this IIdentifiableObject as a reference.  The
            ///     returned IStructureReference can be used to uniquely identify this identifiable @object.
            /// </summary>
            public IStructureReference AsReference
            {
                get
                {
                    return this._crossReference;
                }
            }

            /// <summary>
            ///     Gets a set of composite Objects to this sdmxObject
            /// </summary>
            public ISet<ISdmxObject> Composites
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets a set of cross references that are made by this sdmxObject, or by any composite sdmxObject of this sdmxObject
            /// </summary>
            public ISet<ICrossReference> CrossReferences
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets the description in the default locale
            /// </summary>
            /// <value> first locale value it finds or null if there are none </value>
            public string Description
            {
                get
                {
                    return this._categorisation.Description;
                }
            }

            /// <summary>
            ///     Gets a list of descriptions for this component
            ///     <p />
            ///     <b>NOTE:</b>The list is a copy so modifying the returned list will not
            ///     be reflected in the IIdentifiableObject instance
            /// </summary>
            /// <value> </value>
            public IList<ITextTypeWrapper> Descriptions
            {
                get
                {
                    return this._categorisation.Descriptions;
                }
            }

            /// <summary>
            ///     Gets the id for this component, this is a mandatory field and will never be null
            /// </summary>
            /// <value> </value>
            public string Id
            {
                get
                {
                    return this._crossReference.ChildReference.Id;
                }
            }

            /// <summary>
            ///     Gets a set of identifiable that are contained within this identifiable
            /// </summary>
            /// <value> </value>
            public ISet<IIdentifiableObject> IdentifiableComposites
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets the first identifiable parent of this SDMXObject
            ///     <p />
            ///     If this is a MaintainableObject, then there will be no parent to return, so will return a value of null
            /// </summary>
            /// <value> </value>
            public IIdentifiableObject IdentifiableParent
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets any child items, if no children exist then an empty list is returned
            ///     <p />
            ///     <b>NOTE</b>The list is a copy so modify the returned set will not
            ///     be reflected in this instance
            /// </summary>
            //// TODO Use ReadOnlyCollection<T> to avoid copying the list.
            public IList<ICategoryObject> Items
            {
                get
                {
                    return new ICategoryObject[0];
                }
            }

            /// <summary>
            ///     Gets the maintainable parent, by recurring up the parent tree to find
            ///     If this is a maintainable then it will return a reference to itself.
            /// </summary>
            /// <value> </value>
            public IMaintainableObject MaintainableParent
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets the name in the default locale
            /// </summary>
            /// <value> </value>
            public string Name
            {
                get
                {
                    return this._categorisation.Name;
                }
            }

            /// <summary>
            ///     Gets a list of names for this component - will return an empty list if no Names exist.
            ///     <p />
            ///     <b>NOTE:</b>The list is a copy so modifying the returned list will not
            ///     be reflected in the IIdentifiableObject instance
            /// </summary>
            /// <value> first locale value it finds or null if there are none </value>
            public IList<ITextTypeWrapper> Names
            {
                get
                {
                    return this._categorisation.Names;
                }
            }

            /// <summary>
            ///     Gets the parent that this SdmxObject belongs to.
            ///     If this is a Maintainable Object, then there will be no parent to return, so will return a value of null
            /// </summary>
            /// <value> </value>
            public ISdmxStructure Parent
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets the structure type of this component.
            /// </summary>
            /// <value> </value>
            public SdmxStructureType StructureType
            {
                get
                {
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category);
                }
            }

            /// <summary>
            ///     Gets the URI for this component, returns null if there is no URI.
            ///     <p />
            ///     URI describes where additional information can be found for this component, this is guaranteed to return
            ///     a value if the structure is a <c>IMaintainableObject</c> and isExternalReference is true
            /// </summary>
            /// <value> </value>
            public Uri Uri
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            ///     Gets the URN for this component.  The URN is unique to this instance and is a computable generated value based on
            ///     other attributes set within the component.
            /// </summary>
            /// <value> </value>
            public Uri Urn
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            #region Explicit Interface Properties

            /// <summary>
            ///     Gets the parent that this SdmxObject belongs to
            ///     <p />
            ///     If this is a Maintainable Object, then there will be no parent to return, so will return a value of null
            /// </summary>
            ISdmxObject ISdmxObject.Parent
            {
                get
                {
                    return this.Parent;
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// Gets a value indicating whether the SdmxObject equals the given sdmxObject in every respect (except for the validTo property of a maintainable artefact, this is not taken into consideration)
            ///     <p/>
            ///     This method calls deepEquals on any SdmxObject composites.
            /// </summary>
            /// <param name="sdmxObject">
            /// The sdmxObject.
            /// </param>
            /// <param name="includeFinalProperties">
            /// Set to true to check final properties.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/> .
            /// </returns>
            public bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns the annotations with the given title, returns an empty Set is no annotations exist that have a type which
            ///     matches the given string
            /// </summary>
            /// <param name="title">
            /// The annotation title.
            /// </param>
            /// <returns>
            /// The <see cref="ISet{IAnnotation}"/>.
            /// </returns>
            public ISet<IAnnotation> GetAnnotationsByTitle(string title)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns the annotations with the given type, returns an empty Set is no annotations exist that have a type which
            ///     matches the given string
            /// </summary>
            /// <param name="type">
            /// The Annotation type
            /// </param>
            /// <returns>
            /// The <see cref="ISet{IAnnotation}"/>.
            /// </returns>
            public ISet<IAnnotation> GetAnnotationsByType(string type)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets any composites of this SdmxObject of the given type
            /// </summary>
            /// <param name="type">
            /// The type.
            /// </param>
            /// <typeparam name="T">
            /// Generic type parameter
            /// </typeparam>
            /// <returns>
            /// The <see cref="ISet{T}"/>.
            /// </returns>
            public ISet<T> GetComposites<T>(Type type)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets a period separated id of this identifiable, starting from the non-maintainable top level ancestor to this identifiable.
            ///     <p/>
            ///     For example, if this is Category A living as a child of Category AA, then this method will return AA.A (not the category scheme id is not present in this identifier)
            ///     <p/>
            ///     Gets null if this is a maintainable Object
            /// </summary>
            /// <param name="includeDifferentTypes">
            /// Include different types.
            /// </param>
            /// <returns>
            /// The <see cref="string"/> .
            /// </returns>
            public string GetFullIdPath(bool includeDifferentTypes)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Visits all items up the parent hierarchy to return the first occurrence of parent of the given type that this SdmxObject belongs to
            ///     <p/>
            ///     If a parent of the given type does not exist in the hierarchy, null will be returned
            /// </summary>
            /// <typeparam name="T">
            /// Generic type parameter.
            /// </typeparam>
            /// <param name="includeThisInSearch">
            /// if true then this type will be first checked to see if it is of the given type
            /// </param>
            /// <returns>
            /// The <see cref="T"/>.
            /// </returns>
            public T GetParent<T>(bool includeThisInSearch) where T : class
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets a value indicating whether the @object has an annotation with the given type
            /// </summary>
            /// <param name="annoationType">
            /// Annotation type
            /// </param>
            /// <returns>
            /// The <see cref="bool"/> .
            /// </returns>
            public bool HasAnnotationType(string annoationType)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}