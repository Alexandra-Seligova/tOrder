namespace tOrder.Common;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices; // nahoře pro CallerMemberName

using System.Xml.Linq;

using Microsoft.UI.Xaml;

using tOrder.Shell;
using tOrder.UI;
using tOrder.Common;
using tOrder.Common;

#endregion

//===================================================================
// class MockDataService
//===================================================================

public sealed class MockDataService : IDataService
{
    private const string DataFileName = "MockData.xml";

    private readonly List<CapacityUnitM> m_units = [];
    private readonly List<NavigationItemModel> m_navItems = [];
    private readonly List<MenuItemM> m_cardItems = [];
    private readonly List<string> m_notifications = [];
    private int m_nNotificationStep = 0;
    private readonly List<ColorM> m_colors = new();

    public MockDataService() => LoadXmlData();

    /// <summary>
    /// Loads mock application data from an embedded XML file (MockData.xml).
    /// Parses navigation, card menu, capacity unit structures, and optional notification history.
    /// </summary>

    private void LoadXmlData([CallerMemberName] string? caller = null)
    {
        Console.WriteLine($"[MockDataService] LoadXmlData triggered by: {caller}");

        string filePath = Path.Combine(AppContext.BaseDirectory, "Common", "Services", "Data", DataFileName);
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"[MockDataService] File not found: {filePath}");
            return;
        }

        var doc = XDocument.Load(filePath);
        var root = doc.Root;
        if (root is null)
        {
            Console.WriteLine("[MockDataService] XML root is null.");
            return;
        }

        LoadColorPalette(root);
        LoadNavigationItems(root);
        LoadCardMenuItems(root);
        LoadCapacityUnits(root);
        LoadNotificationHistory(root);

        Console.WriteLine("[MockDataService] XML document fully loaded.");
    }

    // -----------------------------------------------------------
    // Metoda pro paletu barev
    private void LoadColorPalette(XElement root)
    {
        var colorPalette = root.Element("ColorPalette")?.Elements("ColorM") ?? [];
        foreach (var e in colorPalette)
        {
            var key = e.AsStringAttr("Key");
            var hex = e.AsStringAttr("Hex");
            byte r = (byte)e.AsIntAttr("R");
            byte g = (byte)e.AsIntAttr("G");
            byte b = (byte)e.AsIntAttr("B");
            byte a = (byte)e.AsIntAttr("A", 255);

            m_colors.Add(new ColorM(key, hex, r, g, b, a));
        }

        Console.WriteLine($"[MockDataService] Color palette loaded: {m_colors.Count}");
    }

    // -----------------------------------------------------------
    // Metoda pro navigaci
    private void LoadNavigationItems(XElement root)
    {
        var navItems = root.Element("NavigationItems")?.Elements("Item") ?? [];
        foreach (var e in navItems)
        {
            m_navItems.Add(new NavigationItemModel(
                GetAttr(e, "Key"),
                GetAttr(e, "Icon"),
                GetAttr(e, "Title"),
                GetAttr(e, "Page")));
        }
        Console.WriteLine($"[MockDataService] Navigation items loaded: {m_navItems.Count}");
    }

    // -----------------------------------------------------------
    // Metoda pro card menu položky
    private void LoadCardMenuItems(XElement root)
    {
        var cardItems = root.Element("CardMenuItems")?.Elements("Item") ?? [];
        foreach (var e in cardItems)
        {
            m_cardItems.Add(new MenuItemM(GetAttr(e, "Label"), string.Empty));
        }
        Console.WriteLine($"[MockDataService] Card menu items loaded: {m_cardItems.Count}");
    }

    // -----------------------------------------------------------
    // Metoda pro capacity units a pozice
    private void LoadCapacityUnits(XElement root)
    {
        Console.WriteLine("[MockDataService] Výpis všech načtených jednotek a pozic:");
        var unitElems = root.Element("CapacityUnits")?.Elements("Unit") ?? [];
        foreach (var unitElem in unitElems)
        {
            var unit = new CapacityUnitM(unitElem.AsStringAttr("Id"))
            {
                State = unitElem.AsEnumAttr("State", CapacityState.Off),
                UnitColorHex = unitElem.AsStringAttr("UnitColor")
            };
            Console.WriteLine($"Jednotka: {unit.UnitId} (hlavička barva: {unit.UnitColorHex ?? "N/A"})");


            foreach (var posElem in unitElem.Elements("Position"))
            {
                var position = new CapacityPositionM(
      posElem.AsStringAttr("Code"),
      posElem.AsIntAttr("Index"))
                {
                    OrderNumber = posElem.AsStringAttr("OrderNumber"),
                    ArticleCode = posElem.AsStringAttr("ArticleCode"),
                    HaeringCharge = posElem.AsStringAttr("HaeringCharge"),
                    MaterialBatch = posElem.AsStringAttr("MaterialBatch"),
                    ProductionType = posElem.AsStringAttr("ProductionType"),
                    LocalState = posElem.AsEnumAttr("LocalState", CapacityState.Off),
                    OEE = posElem.AsDoubleAttr("OEE"),
                    CapacityUtilization = posElem.AsDoubleAttr("CapacityUtilization"),
                    Efficiency = posElem.AsDoubleAttr("Efficiency"),
                    Quality = posElem.AsDoubleAttr("Quality"),
                    LastUpdateDate = posElem.AsStringAttr("LastUpdateDate"),
                    LastUpdateTime = posElem.AsStringAttr("LastUpdateTime"),
                    RestPiecesToWZK = posElem.AsIntAttr("RestPiecesToWZK"),
                    RestPiecesToQA = posElem.AsIntAttr("RestPiecesToQA"),
                    RestTimeToQA = posElem.AsStringAttr("RestTimeToQA"),
                    Counter = (uint?)posElem.Attribute("Counter"),
                    StatusColorHex = posElem.AsStringAttr("StatusColor"),
                    IndexColorHex = posElem.AsStringAttr("IndexColor")
                };


                LoadPositionAssociations(posElem, position);
                unit.Positions.Add(position);
                Console.WriteLine($" - Pozice: {position.Code} (Index: {position.Index}, Stav: {position.LocalState}, Barva: {position.StatusColorHex ?? "default"})");

            }
            m_units.Add(unit);
        }
        Console.WriteLine($"[MockDataService] Capacity units loaded: {m_units.Count}");
    }

    // -----------------------------------------------------------
    // Pomocná metoda pro načtení asociací do pozice
    private void LoadPositionAssociations(XElement posElem, CapacityPositionM position)
    {
        foreach (var person in posElem.Element("LoggedInPersons")?.Elements("Person") ?? [])
        {
            position.LoggedInPersons.Add(new UserM(
                GetAttr(person, "Name"),
                UIntParseNullable(person, "Id"),
                GetAttr(person, "Surname"),
                GetAttr(person, "MiddleName", null),
                UIntParseNullable(person, "AssociatedPersonId")));
        }

        foreach (var bin in posElem.Element("InputBins")?.Elements("Bin") ?? [])
        {
            position.InputBins.Add(new PortM
            {
                Index = IntParse(bin, "Index"),
                IsActive = BoolParse(bin, "IsActive"),
                OutputValue = IntParse(bin, "OutputValue"),
                Content = DoubleParse(bin, "Content"),
                Unit = GetAttr(bin, "Unit", "Stk.")
            });
        }

        foreach (var bin in posElem.Element("OutputBins")?.Elements("Bin") ?? [])
        {
            position.OutputBins.Add(new PortM
            {
                Index = IntParse(bin, "Index"),
                IsActive = BoolParse(bin, "IsActive"),
                Content = DoubleParse(bin, "Content"),
                Unit = GetAttr(bin, "Unit", "Stk.")
            });
        }

        foreach (var action in posElem.Element("Actions")?.Elements("Action") ?? [])
        {
            position.Actions.Add(new ActionM
            {
                Key = GetAttr(action, "Key"),
                IsEnabled = BoolParse(action, "IsEnabled")
            });
        }
    }

    // -----------------------------------------------------------
    // Metoda pro historii notifikací
    private void LoadNotificationHistory(XElement root)
    {
        m_notifications.AddRange(
            root.Element("Notification")?.Element("History")?.Elements("Item")?.Select(e => e.Value) ?? []);
        Console.WriteLine($"[MockDataService] Notification history items: {m_notifications.Count}");
    }


    #region Helpers
    private static string GetAttr(XElement elem, string attrName, string? fallback = "")
        => elem.Attribute(attrName)?.Value ?? fallback;

    private static int IntParse(XElement elem, string attrName)
        => int.TryParse(GetAttr(elem, attrName), out var result) ? result : 0;

    private static uint? UIntParseNullable(XElement elem, string attrName)
        => uint.TryParse(GetAttr(elem, attrName), out var result) ? result : null;

    private static double DoubleParse(XElement elem, string attrName)
        => double.TryParse(GetAttr(elem, attrName), out var result) ? result : 0;

    private static bool BoolParse(XElement elem, string attrName)
        => bool.TryParse(GetAttr(elem, attrName), out var result) && result;

    private static T EnumParse<T>(XElement elem, string attrName, T fallback) where T : struct
        => Enum.TryParse(GetAttr(elem, attrName), out T result) ? result : fallback;
    #endregion
    public Task<IList<ColorM>> GetColorPaletteAsync()
    => Task.FromResult<IList<ColorM>>(m_colors);

    public Task<IList<NavigationItemModel>> GetNavigationItemsAsync()
        => Task.FromResult<IList<NavigationItemModel>>(m_navItems);

    public Task<IList<MenuItemM>> GetCardMenuItemsAsync()
        => Task.FromResult<IList<MenuItemM>>(m_cardItems);

    public Task<IList<CapacityUnitM>> GetCapacityUnitsAsync()
        => Task.FromResult<IList<CapacityUnitM>>(m_units);

    public Task<NotificationType> GetCurrentNotificationStatusAsync()
    {
        m_nNotificationStep++;
        var status = (m_nNotificationStep % 5) switch
        {
            0 => NotificationType.None,
            1 => NotificationType.Info,
            2 => NotificationType.Upcoming,
            3 => NotificationType.Warning,
            4 => NotificationType.Debug,
            _ => NotificationType.None
        };
        return Task.FromResult(status);
    }

    public Task<IEnumerable<string>> GetNotificationHistoryAsync()
        => Task.FromResult<IEnumerable<string>>(m_notifications);
}

public static class XElementExtensions
{
    public static string AsString(this XElement el)
        => (string)el ?? "";

    public static string AsStringAttr(this XElement el, string attrName)
        => (string)el.Attribute(attrName) ?? "";

    public static int AsIntAttr(this XElement el, string attrName, int fallback = 0)
    {
        var attr = el.Attribute(attrName);
        return attr != null && int.TryParse(attr.Value, out var val) ? val : fallback;
    }

    public static byte AsByteAttr(this XElement el, string attrName, byte fallback = 0)
    {
        var attr = el.Attribute(attrName);
        return attr != null && byte.TryParse(attr.Value, out var val) ? val : fallback;
    }

    public static double AsDoubleAttr(this XElement el, string attrName, double fallback = 0.0)
    {
        var attr = el.Attribute(attrName);
        return attr != null && double.TryParse(attr.Value, out var val) ? val : fallback;
    }

    public static bool AsBoolAttr(this XElement el, string attrName, bool fallback = false)
    {
        var attr = el.Attribute(attrName);
        return attr != null && bool.TryParse(attr.Value, out var val) ? val : fallback;
    }

    public static T AsEnumAttr<T>(this XElement el, string attrName, T fallback = default) where T : struct
        => Enum.TryParse((string)el.Attribute(attrName), out T result) ? result : fallback;

    public static string AsStringElem(this XElement parent, string elemName)
        => (string)parent.Element(elemName) ?? "";

    public static int AsIntElem(this XElement parent, string elemName, int fallback = 0)
    {
        var el = parent.Element(elemName);
        return el != null && int.TryParse(el.Value, out var val) ? val : fallback;
    }

    public static double AsDoubleElem(this XElement parent, string elemName, double fallback = 0.0)
    {
        var el = parent.Element(elemName);
        return el != null && double.TryParse(el.Value, out var val) ? val : fallback;
    }
}
