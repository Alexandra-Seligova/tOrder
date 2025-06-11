//===================================================================
// $Workfile:: AlertType.cs                                        $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Enum representing types of alert messages in the system.
//===================================================================

namespace tOrder.Common
{
    /// <summary>
    /// Typy výstražných stavů používaných v systému notifikací.
    /// </summary>
    public enum AlertType
    {
        Info,
        Warning,
        Error,
        Critical,
        None
    }
}
