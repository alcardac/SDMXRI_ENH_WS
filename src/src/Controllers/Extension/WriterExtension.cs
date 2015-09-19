// -----------------------------------------------------------------------
// <copyright file="WriterExtension.cs" company="EUROSTAT">
//   Date Created : 2013-11-18
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
namespace Estat.Sri.Ws.Controllers.Extension
{
    using System;
    using System.Collections.Generic;

    public static class WriterExtension
    {
        /// <summary>
        /// Runs all actions in the queue.
        /// </summary>
        /// <param name="actions">The actions.</param>
        public static void RunAll(this Queue<Action> actions)
        {
            while (actions.Count > 0)
            {
                actions.Dequeue()();
            }
        }
    }
}