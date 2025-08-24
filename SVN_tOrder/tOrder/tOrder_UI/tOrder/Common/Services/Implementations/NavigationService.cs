//===================================================================
// $Workfile:: NavigationService.cs                                $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Centralized navigation handler for XAML Frame routing.
//===================================================================

namespace tOrder.Common;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using tOrder.UI;
using tOrder.Shell;
#endregion //Using directives

//===================================================================
// class NavigationService
//===================================================================

public class NavigationService : INavigationService
{
    //-----------------------------------------------------------
    #region Fields
    //-----------------------------------------------------------

    private Frame? m_frame; // Internal reference to UI Frame

    // Optional static page map – fallback při selhání Type.GetType()
    private static readonly Dictionary<string, Type> PageMap = new()
    {
        { "OverviewByIPC", typeof(OverviewByIPC) },
      //  { "OverviewByBetreiber", typeof(OverviewByBetreiber) },
      //  { "SchichtAnfang", typeof(SchichtAnfang) },
      //  { "SchichtEnde", typeof(SchichtEnde) },
       // { "Rusten", typeof(Rusten) },
       // { "Korektur", typeof(Korektur) },
       // { "InfoSystem", typeof(InfoSystem) },

        // Secondary navigation
        //{ "CapacityUnitDashboard", typeof(CapacityUnitDashboardPage) },

        // Float menu placeholder pages
       /* { "Einstellteile", typeof(DummyPage) },
        { "Muster", typeof(DummyPage) },
        { "ETF", typeof(DummyPage) },
        { "WZ-BlattWechsel", typeof(DummyPage) },
        { "WZ-Wechsel", typeof(DummyPage) },
        { "WZ-Historie", typeof(DummyPage) },
        { "Auftrags-verwaltung", typeof(DummyPage) },
        { "Sample", typeof(DummyPage) }*/
    };

    #endregion //Fields

    //-----------------------------------------------------------
    #region Initialization
    //-----------------------------------------------------------

    /// <summary>
    /// Initializes the navigation frame. Must be called from UI layer.
    /// </summary>
    /// <param name="frame">XAML frame control used for navigation.</param>
    public void Initialize(Frame frame)
    {
        m_frame = frame;
    }

    #endregion //Initialization

    //-----------------------------------------------------------
    #region Navigation API
    //-----------------------------------------------------------

    /// <summary>
    /// Navigates to the specified page type.
    /// </summary>
    /// <param name="pageType">Target page type.</param>
    /// <returns>True if navigation succeeded.</returns>
    public bool Navigate(Type pageType)
    {
        if (m_frame is null || pageType is null)
            return false;

        return m_frame.Navigate(pageType);
    }

    /// <summary>
    /// Navigates using page key (string identifier).
    /// </summary>
    /// <param name="pageKey">Page identifier (e.g. "OverviewByIPC").</param>
    /// <returns>True if navigation succeeded.</returns>
    public bool Navigate(string pageKey)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            return false;

        // Prefer externí registr (pokud existuje)
        var pageType = tOrder.Common.PageRegistry.GetByKey(pageKey)?.GetPageType();


        // Fallback: hardcoded mapa
        pageType ??= PageMap.TryGetValue(pageKey, out var fallbackType) ? fallbackType : null;

        if (pageType is null)
        {
            Console.WriteLine($"[NavigationService] ❌ Neznámý PageKey: {pageKey}");
            return false;
        }

        Console.WriteLine($"[NavigationService] Navigating to: {pageKey} ({pageType.Name})");
        return Navigate(pageType);
    }

    /// <summary>
    /// Navigates back in the navigation history.
    /// </summary>
    public void GoBack()
    {
        if (m_frame?.CanGoBack == true)
            m_frame.GoBack();
    }

    #endregion //Navigation API
}
