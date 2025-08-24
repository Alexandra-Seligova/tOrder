//===================================================================
// $Workfile:: OverviewByIPCVM.cs                                  $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     ViewModel for OverviewByIPC page – builds capacity card list.
//===================================================================

namespace tOrder.UI
{
    using System;
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using tOrder.Common;

    #endregion

    //===================================================================
    // class OverviewByIPCVM
    //===================================================================

    public sealed partial class OverviewByIPCVM : BaseVM
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private readonly ICapacityCardVMFactory m_factory;
        private readonly IDataService m_dataService;
        private bool m_bIsInitialized;

        #endregion

        //-----------------------------------------------------------
        #region Observable Properties
        //-----------------------------------------------------------

        /// <summary>
        /// List of capacity cards (one card = one CapacityPosition)
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<CapacityPositionCardVM> capacityCards = [];

        #endregion

        //-----------------------------------------------------------
        #region Constructor
        //-----------------------------------------------------------

        public OverviewByIPCVM(
            INavigationService navigationService,
            INotificationService notificationService,
            IUserContextService userContext,
            ICapacityCardVMFactory factory,
            IDataService dataService)
            : base(navigationService, notificationService, userContext)
        {
            m_factory = factory;
            m_dataService = dataService;

            LoadedCommand = new AsyncRelayCommand(InitializeAsync);
        }

        #endregion

        //-----------------------------------------------------------
        #region Commands
        //-----------------------------------------------------------

        public IAsyncRelayCommand LoadedCommand { get; }

        #endregion

        //-----------------------------------------------------------
        #region Lifecycle
        //-----------------------------------------------------------

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            if (m_bIsInitialized)
                return;

            await LoadCapacityCardsAsync();
            m_bIsInitialized = true;

            Console.WriteLine("[OverviewByIPCVM] InitializeAsync");
        }

        #endregion

        //-----------------------------------------------------------
        #region Load Methods
        //-----------------------------------------------------------

        private async Task LoadCapacityCardsAsync()
        {
            var lstUnits = await m_dataService.GetCapacityUnitsAsync();

            var lstVMs = lstUnits
                .SelectMany(unit => unit.Positions.Select(position =>
                    m_factory.Create(unit, position))) // ← tady vzniká každý ViewModel
                .ToList();

            await Task.WhenAll(lstVMs.Select(async vm =>
            {
                await vm.InitializeAsync();// ← tady se načítají data do každé karty
            }));

            CapacityCards = new ObservableCollection<CapacityPositionCardVM>(lstVMs);


            Console.WriteLine("[OverviewByIPCVM] LoadCapacityCardsAsync");
        }

        #endregion
    }

    //===================================================================
}
