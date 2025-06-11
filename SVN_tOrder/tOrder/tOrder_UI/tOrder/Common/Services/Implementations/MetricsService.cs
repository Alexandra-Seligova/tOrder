//===================================================================
// $Workfile:: MetricsService.cs                                   $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Developer-facing telemetry and usage tracking (local debug).
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
    // class MetricsService
    //===================================================================

    public class MetricsService : IMetricsService
    {
        //-----------------------------------------------------------
        #region Tracking API
        //-----------------------------------------------------------

        public void TrackEvent(string eventName, Dictionary<string, object>? properties = null)
        {
            Debug.WriteLine($"[Event] {eventName} - {Serialize(properties)}"); // Custom event log
        }

        public void TrackError(string errorMessage, Exception? ex = null)
        {
            Debug.WriteLine($"[Error] {errorMessage}: {ex?.Message}"); // Error and optional exception
        }

        public void TrackPageView(string pageName)
        {
            Debug.WriteLine($"[PageView] {pageName}"); // Page tracking
        }

        #endregion //Tracking API

        //-----------------------------------------------------------
        #region Helpers
        //-----------------------------------------------------------

        private string Serialize(Dictionary<string, object>? dict)
        {
            if (dict == null || dict.Count == 0) return "{}";
            return string.Join(", ", dict.Select(kv => $"{kv.Key}={kv.Value}")); // Simple key=value serialization
        }

        #endregion //Helpers
    }

    //===================================================================
}
