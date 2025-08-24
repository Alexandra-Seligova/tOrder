//===================================================================
// $Workfile:: IDataService.cs                                     $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Interface defining abstract data access for UI and capacity data.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using tOrder.Shell;
    using tOrder.UI;

    #endregion

    //===================================================================
    // interface IDataService
    //===================================================================

    /// <summary>
    /// Abstract contract for retrieving application data.
    /// This implementation is used by MockDataService via MockData.xml.
    /// </summary>
    public interface IDataService
    {
        //-----------------------------------------------------------
        #region Navigation Menu Data
        //-----------------------------------------------------------

        /// <summary>
        /// Gets navigation menu items for the side panel.
        /// </summary>
        Task<IList<NavigationItemModel>> GetNavigationItemsAsync();

        /// <summary>
        /// Gets menu items shown in the context menu of capacity cards.
        /// </summary>
        Task<IList<MenuItemM>> GetCardMenuItemsAsync();

        #endregion

        //-----------------------------------------------------------
        #region Capacity & Station Data
        //-----------------------------------------------------------

        /// <summary>
        /// Gets full list of simulated capacity units and positions.
        /// </summary>
        Task<IList<CapacityUnitM>> GetCapacityUnitsAsync();

        #endregion

    }

    //===================================================================
}
