// -----------------------------------------------------------------------
// <copyright file="HeaderExtensions.cs" company="EUROSTAT">
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
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    /// The header extensions.
    /// </summary>
    public static class HeaderExtensions
    {
         /// <summary>
         /// Add coordinate type of type <paramref name="coordinateType"/> with <paramref name="coordinateValue"/> to <paramref name="contact"/>
         /// </summary>
         /// <param name="contact">The contact</param>
         /// <param name="coordinateType">The type of communication, e.g. <c>Email</c></param>
         /// <param name="coordinateValue">The value, e.g. <c>foo@bar.gr</c></param>
         public static void AddCoordinateType(this IContactMutableObject contact, string coordinateType, string coordinateValue)
         {
             switch (coordinateType)
             {
                 case "Email":
                     contact.AddEmail(coordinateValue);
                     break;
                 case "Telephone":
                     contact.AddTelephone(coordinateValue);
                     break;
                 case "X400":
                     contact.AddX400(coordinateValue);
                     break;
                 case "Fax":
                     contact.AddFax(coordinateValue);
                     break;
                 case "URI":
                     contact.AddUri(coordinateValue);
                     break;
             }
         }
    }
}