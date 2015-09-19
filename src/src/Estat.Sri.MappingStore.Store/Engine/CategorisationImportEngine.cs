// -----------------------------------------------------------------------
// <copyright file="CategorisationImportEngine.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStore.Store.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    using Estat.Ma.Model.StoredProcedure;
    using Estat.Sri.MappingStore.Store.Builder;
    using Estat.Sri.MappingStore.Store.Extension;
    using Estat.Sri.MappingStore.Store.Factory;
    using Estat.Sri.MappingStore.Store.Model;
    using Estat.Sri.MappingStoreRetrieval;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;

    /// <summary>
    ///     The categorisation import engine.
    /// </summary>
    public class CategorisationImportEngine : ArtefactImportEngine<ICategorisationObject>
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ArtefactBaseEngine));

        #endregion

        #region Fields

        /// <summary>
        /// The _stored procedures
        /// </summary>
        private static readonly StoredProcedures _storedProcedures;

        /// <summary>
        ///     The _category builder.
        /// </summary>
        private readonly CategoryBuilder _categoryBuilder;

        /// <summary>
        ///     The _category import.
        /// </summary>
        private readonly IItemImportEngine<ICategoryObject> _categoryImport;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="CategorisationImportEngine"/> class.
        /// </summary>
        static CategorisationImportEngine()
        {
            _storedProcedures = new StoredProcedures();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationImportEngine"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        public CategorisationImportEngine(Database connectionStringSettings)
            : this(connectionStringSettings, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationImportEngine"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        /// <param name="categoryFactory">
        /// The category Factory.
        /// </param>
        public CategorisationImportEngine(Database connectionStringSettings, IItemImportFactory<ICategoryObject> categoryFactory)
            : base(connectionStringSettings)
        {
            var factory = categoryFactory ?? new ItemImportFactory<ICategoryObject>();
            this._categoryImport = factory.GetItemImport();
            this._categoryBuilder = new CategoryBuilder();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Insert the specified <paramref name="maintainable"/> to the mapping store with <paramref name="state"/>
        /// </summary>
        /// <param name="state">
        /// The MAPPING STORE connection and transaction state
        /// </param>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        public override ArtefactImportStatus Insert(DbTransactionState state, ICategorisationObject maintainable)
        {
            var cache = new StructureCache();
            return this.InsertInternal(state, maintainable, cache);
        }

        /// <summary>
        /// Insert the specified <paramref name="maintainables"/> to the mapping store.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{ArtefactImportStatus}"/>.
        /// </returns>
        public new IEnumerable<ArtefactImportStatus> Insert(IEnumerable<ICategorisationObject> maintainables)
        {
            var cache = new StructureCache();
            foreach (var artefact in maintainables)
            {
                using (DbTransactionState state = DbTransactionState.Create(this.Database))
                {
                    ArtefactImportStatus artefactImportStatus;
                    try
                    {
                        artefactImportStatus = this.InsertInternal(state, artefact, cache);
                        state.Commit();
                    }
                    catch (MappingStoreException e)
                    {
                        _log.Error(artefact.Urn.ToString(), e);
                        state.RollBack();
                        artefactImportStatus = new ArtefactImportStatus(-1, artefact.AsReference.GetErrorMessage(e));
                    }
                    catch (DbException e)
                    {
                        _log.Error(artefact.Urn.ToString(), e);
                        state.RollBack();
                        artefactImportStatus = new ArtefactImportStatus(-1, artefact.AsReference.GetErrorMessage(e));
                    }

                    yield return artefactImportStatus;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build warning message.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        private static ArtefactImportStatus BuildWarningMessage(ICategorisationObject maintainable, string text)
        {
            string message = string.Format(CultureInfo.InvariantCulture, text, maintainable.CategoryReference.GetAsHumanReadableString(), maintainable.StructureReference.GetAsHumanReadableString());
            return new ArtefactImportStatus(-1, new ImportMessage(ImportMessageStatus.Warning, maintainable.AsReference, message));
        }

        /// <summary>
        /// Check if it exists categorisation between artefact with <paramref name="artefactId"/> and category with
        ///     <paramref name="categoryId"/>
        ///     .
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="artefactId">
        /// The artefact id.
        /// </param>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool ExistsCategorisation(DbTransactionState state, long artefactId, long categoryId)
        {
            var artIdParam = state.Database.CreateInParameter("artId", DbType.Int64, artefactId);
            var catIdParam = state.Database.CreateInParameter("catId", DbType.Int64, categoryId);
            var value = state.ExecuteScalarFormat("select count(*) from categorisation where art_id = {0} and cat_id = {1}", artIdParam, catIdParam);

             // normally count(*) should always return a number. Checking just in case I missed something.
            if (value != null && !Convert.IsDBNull(value))
            {
                // in .net, oracle will return 128bit decimal, sql server 32bit int, while mysql & sqlite 64bit long.
                long count = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                return count > 0;
            }

            return false;
        }

        /// <summary>
        /// Returns the referenced category primary key. If the category does not exist it will be added.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="categoryScheme">
        /// The category scheme.
        /// </param>
        /// <returns>
        /// The <see cref="ItemStatus"/>.
        /// </returns>
        private ItemStatus GetCategoryPrimaryKey(DbTransactionState state, ICategorisationObject maintainable, ItemSchemeFinalStatus categoryScheme)
        {
            ItemStatus categoryPrimaryKey;
            if (!categoryScheme.ItemIdMap.TryGetValue(maintainable.CategoryReference.ChildReference.Id, out categoryPrimaryKey))
            {
                ICategoryObject categoryObject = this._categoryBuilder.Build(maintainable.CategoryReference);
                var primaryKey = this._categoryImport.Insert(state, new[] { categoryObject }, categoryScheme.FinalStatus.PrimaryKey).FirstOrDefault();
                categoryPrimaryKey = new ItemStatus(categoryObject.Id, primaryKey);
            }

            return categoryPrimaryKey;
        }

        /// <summary>
        /// Insert the specified <paramref name="maintainable"/> to mapping store <c>CATEGORISATION</c> table
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="cache">
        /// The cached Dataflow and Category Scheme
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactImportStatus"/>.
        /// </returns>
        private ArtefactImportStatus InsertInternal(DbTransactionState state, ICategorisationObject maintainable, StructureCache cache)
        {
            var dataflowStatus = cache.GetStructure(state, maintainable.StructureReference);
            ArtefactImportStatus returnValue;
            if (dataflowStatus.FinalStatus.PrimaryKey > 0)
            {
                var categoryScheme = cache.GetStructure(state, maintainable.CategoryReference);
                if (categoryScheme.FinalStatus.PrimaryKey > 0 && categoryScheme.FinalStatus.IsFinal)
                {
                    var categoryPrimaryKey = this.GetCategoryPrimaryKey(state, maintainable, categoryScheme);
                    if (!ExistsCategorisation(state, dataflowStatus.FinalStatus.PrimaryKey, categoryPrimaryKey.SysID))
                    {
                        var artefactStoredProcedure = _storedProcedures.InsertCategorisation;

                        returnValue = this.InsertArtefactInternal(
                            state, 
                            maintainable, 
                            artefactStoredProcedure, 
                            command =>
                                {
                                    artefactStoredProcedure.CreateArtIdParameter(command).Value = dataflowStatus.FinalStatus.PrimaryKey;
                                    artefactStoredProcedure.CreateCatIdParameter(command).Value = categoryPrimaryKey.SysID;
                                });

                        string message = string.Format(
                            CultureInfo.InvariantCulture, 
                            "Successfully categorized {0} with Category {1} of {2}\n", 
                            maintainable.StructureReference.GetAsHumanReadableString(), 
                            maintainable.CategoryReference.ChildReference.Id, 
                            maintainable.CategoryReference.GetAsHumanReadableString());
                        returnValue = new ArtefactImportStatus(returnValue.PrimaryKeyValue, new ImportMessage(ImportMessageStatus.Success, maintainable.AsReference, message));
                    }
                    else
                    {
                        returnValue = BuildWarningMessage(maintainable, "Warning: Ignoring duplicate categorisation of {0} with {1}\n");
                    }
                }
                else if (!categoryScheme.FinalStatus.IsFinal)
                {
                    returnValue = BuildWarningMessage(maintainable, "Failure: {0} is not Final so it cannot be referenced from {1}\n");
                }
                else
                {
                    returnValue = BuildWarningMessage(maintainable, "Failure: {0} does not exist so it cannot be referenced from {1}\n");
                }
            }
            else
            {
                string message = string.Format(CultureInfo.InvariantCulture, "Failure: Cannot categorize {0}, because it does not exist\n", maintainable.StructureReference.GetAsHumanReadableString());
                returnValue = new ArtefactImportStatus(-1, new ImportMessage(ImportMessageStatus.Warning, maintainable.AsReference, message));
            }

            return returnValue;
        }

        #endregion
    }
}