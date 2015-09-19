// -----------------------------------------------------------------------
// <copyright file="HeaderSettings.cs" company="EUROSTAT">
//   Date Created : 2013-10-14
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
namespace Estat.Sri.Ws.Controllers.Constants
{
    using System.ComponentModel;
    using System.Configuration;

    // This class allows you to handle specific events on the settings class:
    // The SettingChanging event is raised before a setting's value is changed.
    // The PropertyChanged event is raised after a setting's value is changed.
    // The SettingsLoaded event is raised after the setting values are loaded.
    // The SettingsSaving event is raised before the setting values are saved.
    /// <summary>
    /// The header settings.
    /// </summary>
    internal sealed partial class HeaderSettings
    {
        #region Methods

        /// <summary>
        /// The setting changing event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }

        /// <summary>
        /// The settings saving event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }

        #endregion
    }
}