//===================================================================
// $Workfile:: INavigationService.cs                               $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Abstracts page navigation for consistent UI flow.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using Microsoft.UI.Xaml.Controls;

    #endregion //Using directives

    //===================================================================
    // interface INavigationService
    //===================================================================

    public interface INavigationService
    {
        void Initialize(Frame frame);           // Binds to a Frame instance (e.g. ContentFrame)
        bool Navigate(Type pageType);           // Standard navigation by page Type
        bool Navigate(string pageKey);          // Navigation by symbolic key (e.g. "OverviewByIPC")
        void GoBack();                          // Navigates one step back in navigation stack
    }

    //===================================================================
}
