//===================================================================
// $Workfile:: DialogService.cs                                    $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Handles modal user dialogs (messages and confirmations).
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    #endregion //Using directives

    //===================================================================
    // class DialogService
    //===================================================================

    public class DialogService : IDialogService
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private XamlRoot? m_xamlRoot; // Context used to bind dialogs to correct visual tree

        #endregion //Fields

        //-----------------------------------------------------------
        #region Initialization
        //-----------------------------------------------------------

        public void SetXamlRoot(XamlRoot xamlRoot)
        {
            m_xamlRoot = xamlRoot;
            Debug.WriteLine($"✅ SetXamlRoot on DialogService instance {GetHashCode()}");
        }

        #endregion //Initialization

        //-----------------------------------------------------------
        #region Message Dialog
        //-----------------------------------------------------------

        public async Task ShowMessageAsync(string title, string content)
        {
            Debug.WriteLine($"💬 ShowMessageAsync called on instance {GetHashCode()}");

            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = m_xamlRoot
            };

            await dialog.ShowAsync();
        }

        #endregion //Message Dialog

        //-----------------------------------------------------------
        #region Confirmation Dialog
        //-----------------------------------------------------------

        public async Task<bool> ShowConfirmationAsync(string title, string content)
        {
            if (m_xamlRoot == null)
                return false;

            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = m_xamlRoot
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        #endregion //Confirmation Dialog
    }

    //===================================================================
}
