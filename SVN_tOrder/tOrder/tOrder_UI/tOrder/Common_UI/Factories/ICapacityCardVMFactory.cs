//===================================================================
// $Workfile:: ICapacityPositionCardVMFactory.cs                            $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Factory interface for creating CapacityPositionCardVM instances.
//===================================================================

namespace tOrder.UI
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using tOrder.Common;

    #endregion //Using directives

    //===================================================================
    // interface ICapacityPositionCardVMFactory
    //===================================================================

    public interface ICapacityCardVMFactory
    {
        CapacityPositionCardVM Create(CapacityUnitM machine, CapacityPositionM station); // Creates a new ViewModel instance
    }

    //===================================================================
}
