using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace tOrder.Common;

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

