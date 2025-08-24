//===================================================================
// $Workfile:: IDialogService.cs                                   $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Dialog interface for showing messages and confirmations.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;

    #endregion //Using directives

    //===================================================================
    // interface IDialogService
    //===================================================================

    public interface IDialogService
    {
        Task ShowMessageAsync(string title, string content);             // Basic modal message dialog
        Task<bool> ShowConfirmationAsync(string title, string content);  // Yes/No confirmation dialog
        void SetXamlRoot(XamlRoot xamlRoot);                             // Must be called from View to attach dialog context
    }

    //===================================================================
}

/*
 ✅ IDialogService
 🔹 Provides interactive modal dialogs for confirmation, messages, or form entry.

 Use cases:
 - Confirm destructive actions ("Do you really want to delete?")
 - Present selection options
 - Display input forms (e.g. custom dialog for editing)
*/
