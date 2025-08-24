# ✨ SPC / tOrder – Kodovací konvence

> 🧭 Dokument pro sjednocení vývoje, čitelnosti a kvality architektury v rámci projektu SPC `tOrder`  
> 📅 Verze: 1.0 | 📂 Kontext: `SPC2.Tools`, `tOrder`, `MVVM`, `WinUI3`

---

## 📁 Struktura souboru a hlavičky

Každý `.cs` soubor musí začínat hlavičkou v jednotném formátu:

```csharp
//===================================================================
// $Workfile:: NázevSouboru.cs                                     $
// $Author::                                                       $
// $Revision::                                                     $
// $Date::                                                         $
//===================================================================
// Description: Stručný popis souboru
// Revision history:
// xx.mm.rrrr, Jméno, Popis změny
//===================================================================
```

**Každá třída** je zřetelně označena:

```csharp
//===================================================================
// class NázevTřídy
//===================================================================
```

Pro členění používej `#region` sekce:
```csharp
//-----------------------------------------------------------
#region Fields
//-----------------------------------------------------------
```

---

## 🧠 Konvence pojmenování (Maďarská notace)

| 💡 Typ / Význam               | Prefix  | Příklad                   |
|-----------------------------|---------|----------------------------|
| `int` – čísla, indexy       | `n`     | `nCount`, `nRecord`       |
| `bool` – logická hodnota    | `b`     | `bResult`, `bIsRemote`    |
| `string`                    | `s`     | `sName`, `sKey`           |
| `object`, `var`             | `o`     | `oValue`, `oResult`       |
| `float`, `double`, `decimal`| `f`, `d`| `dTimestamp`, `fScale`    |
| `char`                      | `ch`    | `chSeparator`             |
| `DateTime`                  | `dt`    | `dtStart`, `dtEnd`        |
| `TimeSpan`                  | `ts`    | `tsTimeout`               |
| `List<T>`                   | `lst`   | `lstItems`, `lstPoints`   |
| `Dictionary<K,V>`           | `dic`   | `dicMappings`             |
| `delegate`, `Func`, `Action`| `fn`    | `fnCallback`, `m_fnDoX`   |
| `StringBuilder`             | `sb`    | `sbFormat`, `sbText`      |
| `Handle`, `HWND` apod.      | `h`     | `hWnd`, `hIcon`           |
| `Pointer`, `IntPtr`         | `p`     | `pData`, `pBuffer`        |

**Zvláštní pravidla:**  
- Privátní proměnné: `m_` prefix → `m_sName`, `m_nState`  
- Statické `readonly` proměnné: `s_` prefix → `s_fnGetDataRow`  
- Dvouznakové zkratky VELKÉ (`UI`, `ID`), tří a více znakové: PascalCase (`Xml`, `GpuMem`)  

---

## 🧰 Formátování a styl

- ✅ **Tabulátory** místo mezer (vynuceno `.editorconfig`)
- 📚 `using` direktivy jsou ve 3 skupinách:
  1. `System.*`
  2. `Microsoft.*`
  3. Projektové (`SPC2.*`)
  - uvnitř abecedně

---

## 🏗️ Konstrukční pravidla

| Fáze             | Popis                                                                 |
|------------------|------------------------------------------------------------------------|
| `Konstruktor`    | Pouze vytvoření instance, nikdy neprovádí logiku ani validaci         |
| `Init()`         | Spouští se jednou, nastavuje výchozí stavy                            |
| `Load()` / `Reload()` | Opakovatelné, plní data, ověřuje validitu, připravuje pro binding |

---

## 🎨 UI / Matematická pravidla

- Rozlišujeme **fyzický pixel** × **efektivní pixel**
- UI layouty mají sudý počet řad → **ideálně 4**
- Škálování: `4 → 5` (125%), `4 → 6` (150%)
- ❌ Mezery 3px NEPOVOLENY – minimálně 4px pro konzistenci

---

## 📜 Filozofie vývoje

- Nejjednodušší funkční kód = **nejlepší kód**
- Kvalitní vývojář:
  - ovládá své nástroje (IDE, Git, refactor, debug)
  - rozumí **architektuře** a **historickým principům**
  - respektuje vrstvy a odpovědnosti (MVVM, SRP, DI)
- 🔍 Držíme se **Microsoft best practices** jako výchozího rámce

---
1) Používej explicitní operátory místo .Value
Pokud pracuješ s XElement, můžeš bezpečně dělat:

csharp
Zkopírovat
Upravit
(string)element           // místo element?.Value
(DateTime)element         // místo DateTime.Parse(element.Value)
(int?)element             // místo Convert.ToInt32(...), zároveň bezpečné pro null
Tím pádem tvůj kód bude:

csharp
Zkopírovat
Upravit
var posts = from item in rssFeed.Descendants("item")
    select new 
    {   
        Title = (string)item.Element("title"),
        Published = (DateTime?)item.Element("pubDate"),
        NumComments = (int?)item.Element(slashNamespace + "comments") ?? 0,
        Url = (string)item.Element("link"),
        Tags = item.Elements("category")
                    .OrderBy(c => (string)c)
                    .Select(c => (string)c)
                    .ToList()
    };
U tebe při práci s XML (třeba pro načítání barev, CapacityUnits...) můžeš taky dělat:

csharp
Zkopírovat
Upravit
var color = (string)e.Attribute("Hex");
a pro nullable hodnoty:

csharp
Zkopírovat
Upravit
var counter = (uint?)e.Attribute("Counter");
2) Pokud používáš LINQ to XML, nikdy nepiš .Value bez kontroly null
Místo:

csharp
Zkopírovat
Upravit
item.Element("foo").Value   // hodí NRE, pokud foo není
Piš:

csharp
Zkopírovat
Upravit
(string)item.Element("foo") // vrátí null, pokud foo není
3) Null-coalescing operator ??
Pokud chceš náhradu pro chybějící hodnoty, použij ??:

csharp
Zkopírovat
Upravit
NumComments = (int?)item.Element("comments") ?? 0
Totéž pro barvy:

csharp
Zkopírovat
Upravit
string hex = (string)e.Attribute("Hex") ?? "#FF0000";
4) Vytvoř si případně vlastní extension metody na Element a Attribute
Pokud chceš vlastní styl ala .AsInt(), .AsString(), můžeš:

csharp
Zkopírovat
Upravit
public static class XElementExtensions
{
    public static int? AsNullableInt(this XElement element)
        => element == null ? (int?)null : (int?)element;

    public static string AsString(this XElement element)
        => (string)element;
}
Použití:

csharp
Zkopírovat
Upravit
var val = item.Element("foo").AsNullableInt() ?? 42;
Jak to aplikovat na tvůj MockDataService?
Příklad refaktoringu:
Místo:

csharp
Zkopírovat
Upravit
var key = GetAttr(e, "Key");
var hex = GetAttr(e, "Hex");
byte r = byte.TryParse(GetAttr(e, "R"), out var rv) ? rv : (byte)0;
Můžeš (když bys šel přímo přes XElement):

csharp
Zkopírovat
Upravit
var key = (string)e.Attribute("Key");
var hex = (string)e.Attribute("Hex");
byte r = (byte?)e.Attribute("R") ?? 0;
Pokud použiješ LINQ to XML idiomaticky, nemusíš psát helpery typu GetAttr.

Ukázka refaktorovaného úseku podle článku:
csharp
Zkopírovat
Upravit
private void LoadColorPalette(XElement root)
{
    var colorPalette = root.Element("ColorPalette")?.Elements("ColorM") ?? [];
    foreach (var e in colorPalette)
    {
        var key = (string)e.Attribute("Key");
        var hex = (string)e.Attribute("Hex");
        byte r = (byte?)e.Attribute("R") ?? 0;
        byte g = (byte?)e.Attribute("G") ?? 0;
        byte b = (byte?)e.Attribute("B") ?? 0;
        byte a = (byte?)e.Attribute("A") ?? 255;

        m_colors.Add(new ColorM(key, hex, r, g, b, a));
    }
    Console.WriteLine($"[MockDataService] Color palette loaded: {m_colors.Count}");
}
Totéž platí pro LoadCapacityUnits, LoadPositionAssociations atd.

Shrnutí pravidel:
Vždy používej (string), (int?), (DateTime?) na atribut/element, ne .Value, pokud nechceš řešit NRE.

Používej ?? na defaultní hodnoty – typicky když očekáváš, že něco může chybět.

Uvažuj o extension metodách, pokud se ti některé převody opakují.

Helper GetAttr(e, "X") je užitečný pro fallback na "", ale pro hodnoty, které opravdu můžou být null, použij raději (typ)e.Attribute("X").

Závěrečná ukázka:
Místo:

csharp
Zkopírovat
Upravit
private static string GetAttr(XElement elem, string attrName, string? fallback = "")
    => elem.Attribute(attrName)?.Value ?? fallback;
Raději piš:

csharp
Zkopírovat
Upravit
var key = (string)e.Attribute("Key") ?? "";
var val = (int?)e.Attribute("Num") ?? 0;
Nebo si napiš extension:

csharp
Zkopírovat
Upravit
public static int AsInt(this XElement el, string attr) =>
    (int?)el.Attribute(attr) ?? 0;
Takhle budeš psát idiomaticky LINQ to XML v moderním stylu, přesně jak radí Scott Gu a Hanselman.
📎 Dokument slouží jako referenční i výukový rámec pro tým vývoje SPC/tOrder. V případě nejasností nebo návrhů na rozšíření kontaktujte správce architektury projektu.
Shrnutí pravidel:
Vždy používej (string), (int?), (DateTime?) na atribut/element, ne .Value, pokud nechceš řešit NRE.

Používej ?? na defaultní hodnoty – typicky když očekáváš, že něco může chybět.

Uvažuj o extension metodách, pokud se ti některé převody opakují.

Helper GetAttr(e, "X") je užitečný pro fallback na "", ale pro hodnoty, které opravdu můžou být null, použij raději (typ)e.Attribute("X").

