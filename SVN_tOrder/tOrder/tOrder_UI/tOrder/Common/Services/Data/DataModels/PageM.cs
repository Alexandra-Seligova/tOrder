using System;

namespace tOrder.Common;
public class PageM
{
    public string Key { get; set; } = string.Empty;
    public string PageTypeName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string ParentKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Type? GetPageType()
    {
        return Type.GetType(PageTypeName);
    }

    public override string ToString()
    {
        return $"{Key} → {Title} (Active: {IsActive})";
    }
}

