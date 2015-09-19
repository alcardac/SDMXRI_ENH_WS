// -----------------------------------------------------------------------
// <copyright file="AbstractFactory.cs" company="EUROSTAT">
//   Date Created : 2011-06-19
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
namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Abstract class for building Factories containing the common code
    /// </summary>
    public abstract class AbstractFactory
    {
        #region Methods

        /// <summary>
        /// Create an instance of the specified type
        /// </summary>
        /// <typeparam name="T">
        /// The base type of the interface to create
        /// </typeparam>
        /// <param name="typeName">
        /// The type name of the implementation to create
        /// </param>
        /// <returns>
        /// The instance of the specified type or null if it fails
        /// </returns>
        protected static T Create<T>(string typeName) where T : class
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            T instance = null;
            try
            {
                Type type = Type.GetType(typeName);
                if (type != null)
                {
                    instance = Activator.CreateInstance(type) as T;
                }
            }
            catch (TargetInvocationException ex)
            {
                // TODO add proper logging
                Trace.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is AuthConfigurationException)
                    {
                        throw ex.InnerException;
                    }

                    throw new AuthConfigurationException(ex.InnerException.Message, ex.InnerException);
                }

                throw;
            }
            catch (Exception ex)
            {
                // TODO add proper logging
                Trace.WriteLine(ex.ToString());
                throw;
            }

            return instance;
        }

        #endregion
    }
}