using System;
using DRCOG.Domain;
using DRCOG.Domain.Models;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Common.Services.MemberShipServiceSupport.SSO.Domain;

namespace Trips4.Utilities.ApplicationState
{
    /// <summary>
    /// Single point container for everything that 
    /// needs to be persisted in session for the application
    /// </summary>
    public class ApplicationState
    {
        public ApplicationState()
        {
            CurrentUser = new Person();
        }

        /// <summary>
        /// Tracks the users current search settings
        /// </summary>
        /// <value>The current search settings.</value>
        public ProjectSearchModel ProjectSearchModel { get; set; }

        /// <summary>
        /// Tracks which TIP the user is currently working in
        /// </summary>
        /// <value>The current TIP.</value>
        public string CurrentRTP { get; set; }

        /// <summary>
        /// Tracks which Survey the user is currently working in
        /// </summary>
        /// <value>The current Survey.</value>
        public string CurrentSurvey { get; set; }

        /// <summary>
        /// Tracks which TIP the user is currently working in
        /// </summary>
        /// <value>The current TIP.</value>
        public string CurrentTIP { get; set; }

        /// <summary>
        /// Tracks which program the user is current using
        /// </summary>
        /// <value>The current uprogram.</value>
        public Enums.ApplicationState CurrentProgram { get; private set; }

        // TODO: rename to SetCurrentProgram
        public void SetStateType(Enums.ApplicationState stateType)
        {
            CurrentProgram = stateType;
        }

        /// <summary>
        /// Gets or sets the current User object.
        /// </summary>
        /// <value>The current user.</value>
        public Person CurrentUser { get; set; }

    }
}
