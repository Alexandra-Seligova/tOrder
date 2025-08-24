//===================================================================
// $Workfile:: IUserContextService.cs                              $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Interface for accessing current user's identity and roles.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Interface
    //-----------------------------------------------------------

    public interface IUserContextService
    {
        string UserId { get; }                // Unique user identifier (e.g. login, GUID)
        string UserName { get; }              // Display name or login name
        string[] Roles { get; }               // List of assigned roles (e.g. Admin, Operator)
        bool IsInRole(string role);           // Returns true if user has the given role
        bool IsAuthenticated { get; }         // True if user is logged in and context is valid
    }

    #endregion //Interface
}

//===================================================================

/*
 👤 IUserContextService
 Provides runtime information about the currently logged-in user.

 Use cases:
 - Show user name and role in UI
 - Control access to secured actions (role-based visibility)
 - Personalize the experience (e.g. theme, language, defaults)
*/
