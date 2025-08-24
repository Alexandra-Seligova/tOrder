//===================================================================
// $Workfile:: INotificationService.cs                             $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Notification API for user feedback and debug output.
//===================================================================



using System.Collections.Generic;

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Notification Types
    //-----------------------------------------------------------

    public enum NotificationType
    {
        None,       // No active notification
        Info,       // Informational message
        Warning,    // Non-blocking warning
        Upcoming,   // Scheduled event or reminder
        Debug       // Developer/debug message
    }

    #endregion //Notification Types

    //-----------------------------------------------------------
    #region Notification Service Interface
    //-----------------------------------------------------------

    public interface INotificationService
    {
        void ShowInfo(string message);                  // Show blue info message
        void ShowWarning(string message);               // Show orange warning
        void ShowError(string message);                 // Show red error toast
        void ShowSuccess(string message);               // Show green success toast

        void Debug(string message);                     // Developer/debug output
        IReadOnlyCollection<string> GetRecentLog(int count = 100); // Recent message log
    }

    #endregion //Notification Service Interface
}

//===================================================================

/*
 ✅ INotificationService
 🔹 Handles instant system messages (toasts, snackbars, developer info)

 Use cases:
 - Top-right system toast ("Saved", "Connection lost", "Access denied")
 - Inline snackbar in page (non-blocking)
 - Push messages triggered remotely
 - Developer debug stream
*/
