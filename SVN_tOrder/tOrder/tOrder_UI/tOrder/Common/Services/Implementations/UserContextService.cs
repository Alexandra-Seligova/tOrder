//===================================================================
// $Workfile:: UserContextService.cs                               $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Stores identity and role info for the current user session.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Linq;

    #endregion //Using directives

    //===================================================================
    // class UserContextService
    //===================================================================

    public class UserContextService : IUserContextService
    {
        //-----------------------------------------------------------
        #region Properties
        //-----------------------------------------------------------

        public string UserId { get; private set; } = "anonymous";            // Internal user ID
        public string UserName { get; private set; } = "Nepřihlášen";        // Display name
        public string[] Roles { get; private set; } = [];                    // Assigned roles
        public bool IsAuthenticated => UserId != "anonymous";                // True if authenticated

        #endregion //Properties

        //-----------------------------------------------------------
        #region Public Methods
        //-----------------------------------------------------------

        public void SetUser(string id, string name, string[] roles)
        {
            UserId = id;
            UserName = name;
            Roles = roles;
        }

        public bool IsInRole(string role) => Roles.Contains(role);           // Role check

        #endregion //Public Methods
    }

    //===================================================================
}
