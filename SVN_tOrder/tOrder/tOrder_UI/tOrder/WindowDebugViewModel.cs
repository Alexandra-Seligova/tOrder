/*==================================================================================
 * WindowDebugViewModel.cs + UIElementRegistry.cs
 * 
 * Ladicí ViewModel a registr prvků UI pro tOrder
 * -------------------------------------------------------------
 * Tento modul umožňuje:
 * - Spravovat hlavní okno aplikace (rozlišení, pozice, zámek poměru stran)
 * - Získávat zpětnou vazbu o stavu okna (aktuální rozlišení)
 * - Inspektovat UI prvky (název, aktuální šířka/výška)
 * - Dynamicky měnit velikost vybraného UI prvku (např. Grid, TopBar)
 * - Obousměrně komunikovat mezi hlavním oknem a ladicím (sekundárním) oknem
 * 
 * Architektura:
 * Program.cs (App entry)
 *        │
 *        ▼
 * WindowDebugViewModel  ←  ResolutionWindow (Debug GUI)
 *        ▲
 *        │
 * MainWindow (AppWindow + UI)
 * 
 * UIElementRegistry umožňuje dynamickou práci s UI prvky z různých částí aplikace.
 * 
 * Tento modul je základ pro budoucí živý inspektor prvků v tOrder.
 *=================================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace tOrder;

public class WindowDebugViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private AppWindow? _mainAppWindow;
    private double? _lockedAspectRatio = null;

    private string? _selectedElementName;
    private double _selectedElementWidth;
    private double _selectedElementHeight;

    public string CurrentResolution { get; private set; } = "—";

    /// <summary>Zamčený poměr stran okna (null = žádný).</summary>
    public double? LockedAspectRatio
    {
        get => _lockedAspectRatio;
        set
        {
            if (_lockedAspectRatio != value)
            {
                _lockedAspectRatio = value;
                OnPropertyChanged();
                ApplyAspectRatioLock();
            }
        }
    }

    public string? SelectedElementName
    {
        get => _selectedElementName;
        set => SetField(ref _selectedElementName, value);
    }

    public double SelectedElementWidth
    {
        get => _selectedElementWidth;
        set { if (SetField(ref _selectedElementWidth, value)) ApplyElementResize(); }
    }

    public double SelectedElementHeight
    {
        get => _selectedElementHeight;
        set { if (SetField(ref _selectedElementHeight, value)) ApplyElementResize(); }
    }

    public void UpdateResolution(int width, int height)
    {
        CurrentResolution = $"{width} x {height}";
        OnPropertyChanged(nameof(CurrentResolution));
    }

    public void AttachWindow(AppWindow window)
    {
        _mainAppWindow = window;
        UpdateCurrentResolution();
    }

    public void UpdateCurrentResolution()
    {
        if (_mainAppWindow != null)
        {
            var size = _mainAppWindow.Size;
            CurrentResolution = $"{size.Width} x {size.Height}";
            OnPropertyChanged(nameof(CurrentResolution));
        }
    }

    private void ApplyAspectRatioLock()
    {
        if (_mainAppWindow == null || LockedAspectRatio is null)
            return;

        var current = _mainAppWindow.Size;
        var width = current.Width;
        var height = (int)(width / LockedAspectRatio.Value);

        _mainAppWindow.Resize(new SizeInt32(width, height));
        UpdateCurrentResolution();
    }

    private void ApplyElementResize()
    {
        if (_selectedElementName == null)
            return;

        var target = UIElementRegistry.GetElementByName(_selectedElementName);
        if (target != null)
        {
            target.Width = SelectedElementWidth;
            target.Height = SelectedElementHeight;
        }
    }

    public void ResizeMainWindow(int width, int height)
    {
        if (_mainAppWindow != null)
        {
            _mainAppWindow.Resize(new SizeInt32(width, height));
            UpdateCurrentResolution();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (!Equals(field, value))
        {
            field = value;
            OnPropertyChanged(name);
            return true;
        }
        return false;
    }
}

public static class UIElementRegistry
{
    private static readonly Dictionary<string, FrameworkElement> Elements = new();

    public static void RegisterElement(string name, FrameworkElement element)
    {
        if (!Elements.ContainsKey(name))
            Elements[name] = element;
    }

    public static FrameworkElement? GetElementByName(string name)
    {
        Elements.TryGetValue(name, out var element);
        return element;
    }

    public static void RemoveElement(string name)
    {
        if (Elements.ContainsKey(name))
            Elements.Remove(name);
    }

    public static IEnumerable<string> ListRegisteredElements()
        => Elements.Keys;
}
