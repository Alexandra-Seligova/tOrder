//===================================================================
// $Workfile:: ConsoleManager.cs                                    $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-07-25 01:10:00 +0200 (pá, 25 čvc 2025)              $
//===================================================================
// Description: SPC - tOrder
//     Console utility class for debug output, font control,
//     service dumping and interactive toolbar.
//===================================================================

namespace tOrder;

#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
#endregion

//===================================================================
// class ConsoleManager
//===================================================================

/// <summary>
/// Provides console-related utilities for debugging, diagnostics,
/// visual output formatting and DI inspection.
/// </summary>
public static class ConsoleManager
{
    //-----------------------------------------------------------
    // Initialization
    //-----------------------------------------------------------

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

    //-----------------------------------------------------------
    // Colors
    //-----------------------------------------------------------

    public static void SetTextColorWhite() => Console.ForegroundColor = ConsoleColor.White;
    public static void SetTextColorGreen() => Console.ForegroundColor = ConsoleColor.Green;
    public static void SetTextColorGray() => Console.ForegroundColor = ConsoleColor.Gray;

    //-----------------------------------------------------------
    // Font configuration (Windows only)
    //-----------------------------------------------------------

    public static void SetFont(string fontName = "Consolas", short fontSize = 16)
    {
        var cfi = new NativeMethods.CONSOLE_FONT_INFO_EX();
        cfi.cbSize = (uint)Marshal.SizeOf(cfi);
        cfi.FaceNameString = fontName;
        cfi.dwFontSize.Y = fontSize;
        NativeMethods.SetCurrentConsoleFontEx(
            NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE),
            false,
            ref cfi
        );
    }

    //-----------------------------------------------------------
    // Diagnostics
    //-----------------------------------------------------------

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

    public static void PrintCurrentPage(string page)
    {
        SetTextColorGray();
        Console.WriteLine($"[PAGE] Aktuální stránka: {page}");
        SetTextColorWhite();
    }

    //-----------------------------------------------------------
    // Interactive Debug Toolbar
    //-----------------------------------------------------------

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
/*
===============================================================================
🖥️ ConsoleManager – Debug Console Utilities for tOrder
===============================================================================

This static class provides diagnostic, visual, and interactive utilities
for launching and working with a Windows console attached to the tOrder app.

Included features:

- 📦 Console initialization with buffer, font and size
- 🎨 Foreground color switching (white, gray, green)
- 🔠 Font override (via SetCurrentConsoleFontEx)
- 🧩 DI container service dump (IServiceCollection)
- 🧭 Current page indicator (used during shell nav)
- 🔧 Interactive debug toolbar with basic triggers

Usage is optional and primarily intended for development/testing.

Call `ConsoleManager.Init()` during startup to attach console.
===============================================================================
*/
