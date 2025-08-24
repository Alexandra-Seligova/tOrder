//===================================================================
// $Workfile:: NotificationService.cs                              $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Handles local message log and system debug notifications.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    #endregion //Using directives

    //===================================================================
    // class NotificationService
    //===================================================================

    public class NotificationService : INotificationService
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private readonly List<string> m_log = new();   // Internal message log buffer

        #endregion //Fields

        //-----------------------------------------------------------
        #region Public API
        //-----------------------------------------------------------

        public void ShowInfo(string message) => Show(message, "Info");       // Blue info message
        public void ShowWarning(string message) => Show(message, "Warning"); // Orange warning
        public void ShowError(string message) => Show(message, "Error");     // Red error toast
        public void ShowSuccess(string message) => Show(message, "Success"); // Green success

        public void Debug(string message) => Show(message, "Debug");         // Developer debug log

        public IReadOnlyCollection<string> GetRecentLog(int count = 100) =>
            m_log.TakeLast(count).ToList().AsReadOnly();                     // Return last N entries

        #endregion //Public API

        //-----------------------------------------------------------
        #region Internals
        //-----------------------------------------------------------

        private void Show(string message, string level)
        {
            string full = $"[{DateTime.Now:HH:mm:ss}] [{level}] {message}";
            m_log.Add(full);

#if DEBUG
            //Debug.WriteLine(full); // Output in debug window
#endif
        }

        #endregion //Internals
    }

    //===================================================================
}
