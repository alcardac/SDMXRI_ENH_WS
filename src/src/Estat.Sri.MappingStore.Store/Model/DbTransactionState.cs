// -----------------------------------------------------------------------
// <copyright file="DbTransactionState.cs" company="EUROSTAT">
//   Date Created : 2013-04-09
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
namespace Estat.Sri.MappingStore.Store.Model
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Common;

    using Estat.Ma.Helpers;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    /// <summary>
    /// The DB transaction state.
    /// </summary>
    public class DbTransactionState : IDisposable
    {
        #region Fields

        /// <summary>
        /// The _connection.
        /// </summary>
        private readonly DbConnection _connection;

        /// <summary>
        /// The _transaction.
        /// </summary>
        private readonly DbTransaction _transaction;

        /// <summary>
        /// The database
        /// </summary>
        private readonly Database _database;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionState"/> class. 
        /// </summary>
        /// <param name="transaction">
        ///     The transaction.
        /// </param>
        /// <param name="database">The database</param>
        public DbTransactionState(DbTransaction transaction, Database database)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            this._connection = transaction.Connection;
            this._transaction = transaction;

            this._database = new Database(database, transaction);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return this._transaction;
            }
        }

        /// <summary>
        /// Gets the database
        /// </summary>
        public Database Database
        {
            get
            {
                return this._database;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create a <see cref="DbTransactionState"/>
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        /// <returns>
        /// The <see cref="DbTransactionState"/>.
        /// </returns>
        public static DbTransactionState Create(ConnectionStringSettings connectionStringSettings)
        {
            var database = DatabasePool.GetDatabase(connectionStringSettings);
            return Create(database);
        }

        /// <summary>
        /// Create a <see cref="DbTransactionState"/>
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <returns>
        /// The <see cref="DbTransactionState"/>.
        /// </returns>
        public static DbTransactionState Create(Database database)
        {
            var connection = database.CreateConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            return new DbTransactionState(transaction, database);
        }

        /// <summary>
        /// Executes the <paramref name="query"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="query">
        /// The query format string.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The scalar <see cref="object"/>.
        /// </returns>
        public object ExecuteScalarFormat(string query, params DbParameter[] parameters)
        {
            return this._database.ExecuteScalarFormat(query, parameters);
        }

        /// <summary>
        /// Executes the specified <paramref name="query"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="query">
        /// The query format string.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The number of affected records.
        /// </returns>
        public int ExecuteNonQueryFormat(string query, params DbParameter[] parameters)
        {
            return this._database.ExecuteNonQueryFormat(query, parameters);
        }

        /// <summary>
        /// Executes the specified <paramref name="query"/> with the specified <paramref name="listParameters"/>
        /// </summary>
        /// <param name="query">
        /// The query format string.
        /// </param>
        /// <param name="listParameters">
        /// The list of Parameters.
        /// </param>
        /// <returns>
        /// The number of affected records.
        /// </returns>
        public int ExecuteNonQueryFormat(string query, IEnumerable<DbParameter[]> listParameters)
        {
            int count = 0;
            foreach (var parameters in listParameters)
            {
                count += this._database.ExecuteNonQueryFormat(query, parameters);
            }

            return count;
        }

        /// <summary>
        /// Executes the <paramref name="query"/> with the specified <paramref name="parameters"/> and runs the <paramref name="reader"/>
        /// </summary>
        /// <param name="query">
        /// The query format.
        /// </param>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void ExecuteReaderFormat(string query, Action<DbDataReader> reader, params DbParameter[] parameters)
        {
            using (var command = this._database.GetSqlStringCommandFormat(query, parameters))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    reader(dataReader);
                }
            }
        }

        /// <summary>
        ///   Rollback all pending changes and dispose the object.
        /// </summary>
        public void RollBack()
        {
            if (this._transaction != null)
            {
                this._transaction.Rollback();
            }

            this.Dispose();
        }

        /// <summary>
        ///     Commit all pending changes and dispose the object.
        /// </summary>
        public void Commit()
        {
            if (this._transaction != null)
            {
                this._transaction.Commit();
            }

            this.Dispose();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="dispose">
        /// If set to true dispose managed objects as well
        /// </param>
        protected void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (this._transaction != null)
                {
                    this._transaction.Dispose();
                }

                if (this._connection != null)
                {
                    this._connection.Dispose();
                }
            }
        }

        #endregion
    }
}