namespace tOrder.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using tOrder.Common;

public static class PageRegistry
{
    private static readonly List<PageM> s_pages = new()
    {
        new PageM { Key = "OverviewByIPC", PageTypeName = "tOrder.UI.OverviewByIPC", Title = "Overview by IPC" },
        new PageM { Key = "OverviewByBetreiber", PageTypeName = "tOrder.UI.OverviewByBetreiber", Title = "Overview by User" },
        new PageM { Key = "SchichtAnfang", PageTypeName = "tOrder.UI.SchichtAnfang", Title = "Start Shift" },
        new PageM { Key = "SchichtEnde", PageTypeName = "tOrder.UI.SchichtEnde", Title = "End Shift" },
        new PageM { Key = "Rusten", PageTypeName = "tOrder.UI.Rusten", Title = "Machine Setup" },
        new PageM { Key = "InfoSystem", PageTypeName = "tOrder.UI.InfoSystem", Title = "Info System" },
        new PageM { Key = "CapacityUnitDashboard", PageTypeName = "tOrder.UI.CapacityUnitDashboardPage", Title = "CU Dashboard" },
    };

    public static PageM? GetByKey(string key)
    {
        return s_pages.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
    }
}
