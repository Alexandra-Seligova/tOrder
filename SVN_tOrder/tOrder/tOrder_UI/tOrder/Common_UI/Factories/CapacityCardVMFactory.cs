//===================================================================
// $Workfile:: CapacityPositionCardVMFactory.cs                            $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Factory for creating CapacityPositionCardVM instances with DI services.
//===================================================================

namespace tOrder.UI
{
    using System;
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using tOrder.Common;

    #endregion //Using directives

    //===================================================================
    // class CapacityPositionCardVMFactory
    //===================================================================

    public sealed class CapacityCardVMFactory : ICapacityCardVMFactory
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private readonly IDataService m_dataService;
        private readonly INavigationService m_navigationService;
        private readonly INotificationService m_notificationService;
        private readonly IUserContextService m_userContext;

        #endregion //Fields

        //-----------------------------------------------------------
        #region Constructor
        //-----------------------------------------------------------

        public CapacityCardVMFactory(
            IDataService dataService,
            INavigationService navigationService,
            INotificationService notificationService,
            IUserContextService userContext)
        {
            m_dataService = dataService;
            m_navigationService = navigationService;
            m_notificationService = notificationService;
            m_userContext = userContext;
        }

        #endregion //Constructor

        //-----------------------------------------------------------
        #region Factory Method
        //-----------------------------------------------------------

        public CapacityPositionCardVM Create(CapacityUnitM machine, CapacityPositionM station)
        {
            Console.WriteLine($"[CapacityCardVMFactory] Create new");

            return new CapacityPositionCardVM(
                machine,
                station,
                m_dataService,
                m_navigationService,
                m_notificationService,
                m_userContext);
        }

        #endregion //Factory Method
    }

    //===================================================================
}
