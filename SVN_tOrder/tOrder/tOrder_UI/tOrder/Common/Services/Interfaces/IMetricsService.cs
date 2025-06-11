//===================================================================
// $Workfile:: IMetricsService.cs                                  $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Interface for telemetry, diagnostics, and user behavior tracking.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Collections.Generic;

    #endregion //Using directives

    //===================================================================
    // interface IMetricsService
    //===================================================================

    public interface IMetricsService
    {
        void TrackEvent(string eventName, Dictionary<string, object>? properties = null); // Custom event tracking
        void TrackError(string errorMessage, Exception? ex = null);                        // Error reporting
        void TrackPageView(string pageName);                                               // Tracks page access
    }

    //===================================================================
}

/*
 📈 IMetricsService
 Collects runtime information for development, monitoring, or audit purposes.

 Includes:
 - Error counts and exceptions
 - Telemetry for usage (e.g. which pages are used most)
 - Internal action logging for diagnostics or business analytics
*/
