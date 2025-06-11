

namespace tOrder;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

//===================================================================
// class ConsoleManager
//===================================================================


public static class ConsoleManager
{
    // === Nastavení bufferu, okna, barev ===
    public static void Init(int width = 120, int bufferHeight = 1000, int windowHeight = 30)
    {
        try
        {
            NativeMethods.AllocConsole();
            IntPtr hConsole = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE);

            NativeMethods.SetConsoleScreenBufferSize(hConsole, new NativeMethods.COORD((short)width, (short)bufferHeight));

            try
            {
                Console.SetBufferSize(width, bufferHeight);
                Console.SetWindowSize(width, windowHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[WARNING] Console buffer/window set failed: " + ex.Message);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[Console] Initialized: {width}x{bufferHeight} (window {windowHeight} lines)");
        }
        catch (Exception ex)
        {
            Debug.WriteLine("[FATAL] Console init failed: " + ex.Message);
        }
    }

    // === Barvy textu ===
    public static void SetTextColorWhite() => Console.ForegroundColor = ConsoleColor.White;
    public static void SetTextColorGreen() => Console.ForegroundColor = ConsoleColor.Green;
    public static void SetTextColorGray() => Console.ForegroundColor = ConsoleColor.Gray;

    // === Změna velikosti písma (windows only) ===
    // POZOR: Lze změnit jen programově přes WinAPI, většinou pomocí SetCurrentConsoleFontEx, zde příklad:
    public static void SetFont(string fontName = "Consolas", short fontSize = 16)
    {
        var cfi = new NativeMethods.CONSOLE_FONT_INFO_EX();
        cfi.cbSize = (uint)Marshal.SizeOf(cfi);
        cfi.FaceNameString = fontName;
        cfi.dwFontSize.Y = fontSize;
        NativeMethods.SetCurrentConsoleFontEx(NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE), false, ref cfi);
    }

    // === Utility: Výpis registrovaných služeb ===
    public static void DumpServices(IServiceCollection services)
    {
        SetTextColorGreen();
        Console.WriteLine("====================================================");
        Console.WriteLine("  Registered Services in tOrder DI Container");
        Console.WriteLine("====================================================");
        foreach (var s in services)
            Console.WriteLine($"{s.Lifetime,-10} | {s.ServiceType.Name,-35} => {s.ImplementationType?.Name}");
        Console.WriteLine("====================================================");
        SetTextColorWhite();
    }

    // === Příklad utility: Vypiš stránku ===
    public static void PrintCurrentPage(string page)
    {
        SetTextColorGray();
        Console.WriteLine($"[PAGE] Aktuální stránka: {page}");
        SetTextColorWhite();
    }

    // === Toolbar (TLAČÍTKA) v konzoli ===
    // Nelze udělat standardní toolbar s tlačítky jako v GUI. Console umí jen text, barvy, znakové grafické prvky.
    // Můžeš ale nabídnout uživatelské "menu" (prompt) a reagovat na stisk kláves:
    public static void ShowToolbar()
    {
        SetTextColorGreen();
        Console.WriteLine("---- TOOLBAR ---- [N]avigate | [R]efresh | [D]ump DI | [P]age | [Q]uit");
        SetTextColorWhite();
        Console.Write(">> ");
        var key = Console.ReadKey(intercept: true).Key;
        Console.WriteLine();

        switch (key)
        {
            case ConsoleKey.N:
                Console.Write("Zadej stránku pro navigaci: ");
                var pg = Console.ReadLine();
                PrintCurrentPage(pg);
                break;
            case ConsoleKey.R:
                Console.WriteLine("Force refresh triggered!");
                break;
            case ConsoleKey.D:
                // Potřebuješ předat IServiceCollection services
                // DumpServices(services);
                Console.WriteLine("Dump DI called (implement as needed)");
                break;
            case ConsoleKey.P:
                PrintCurrentPage("MockPage");
                break;
            case ConsoleKey.Q:
                Console.WriteLine("Exiting toolbar...");
                break;
            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }
}

