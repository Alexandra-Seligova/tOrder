//===================================================================
// $Workfile:: ProductionOrderView.xaml.cs                           $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-08-15 23:50:00 +0200 (čt, 15 srp 2025)              $
//===================================================================
// Description: SPC - tOrder
//     View for tab "Production Order" in InfoContentC.
//     Displays core production order details with checkbox flags.
//===================================================================

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using Microsoft.UI.Xaml.Controls;
using tOrder.Common;

#endregion // Using directives

//===================================================================
// class ProductionOrderView
//===================================================================

public sealed partial class ProductionOrderView : UserControl
{
    //-----------------------------------------------------------
    #region Public Properties (for Binding)
    //-----------------------------------------------------------

    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();


    // --- Main info ---
    public string Artikel { get; set; } = "F 00V H28 042";
    public string Beschreibung { get; set; } = "Ventilhülse aus 1.4418 nach BN 5 899 601 519\nFertigungsteil";
    public string Index { get; set; } = "AL";
    public string RueckmeldeNr { get; set; } = "38253";
    public string Arbeitsplan { get; set; } = "F 00V H28 042/2";
    public string AFO { get; set; } = "10";
    public string TZVon { get; set; } = "0";
    public string TZBis { get; set; } = "10";
    public string Meister { get; set; } = "Grünwald, Alexander";
    public string TBKLager { get; set; } = "- 1 - QS";
    public string Stueckzeit { get; set; } = "6,00 s";


    // --- Quantities ---
    public string Auftragsmenge { get; set; } = "400.000";
    public string Fertigmenge { get; set; } = "0";
    public string Schwebend { get; set; } = "0";
    public string Beanstandet { get; set; } = "0";
    public string Gesamtmenge { get; set; } = "0";
    public string SchwebendBEH { get; set; } = "0";


    // --- Cumulative (split) values ---
    public string GSKummLink { get; set; } = "0";
    public string GSKummRechts { get; set; } = "0";

    public string PKummLink { get; set; } = "0";
    public string PKummRechts { get; set; } = "0";

    public string AusschussLink { get; set; } = "0";
    public string AusschussRechts { get; set; } = "0";

    public string EinstellteileLink { get; set; } = "0";
    public string EinstellteileRechts { get; set; } = "0";


    // --- Additional ---
    public string Zusatztara { get; set; } = "CPAL010 Clipspalette Ventilhülse 027/032";
    public string Warenbewegung { get; set; } = "63 US / Hubbewegung + Schaukeln 0,2 sec.";
    public string WarenbewegungAbg { get; set; } = "0 n.a.";


    // --- Flags ---
    public bool IsQSPruefung { get; set; } = false;
    public bool IsQSFPruefung { get; set; } = true;
    public bool IsHSPflicht { get; set; } = false;
    public bool IsHSMachine { get; set; } = false;
    public bool IsHSScheibe { get; set; } = false;

    #endregion // Public Properties


    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public ProductionOrderView()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    #endregion // Constructor
}
//===================================================================
