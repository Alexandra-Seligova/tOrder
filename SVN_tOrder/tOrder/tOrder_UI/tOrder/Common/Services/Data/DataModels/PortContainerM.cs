// PortContainerM.cs
using System;

namespace tOrder.Common;

/// <summary>
/// Model kontejneru / kyblíku pro vstup nebo výstup
/// </summary>
public class PortContainerM
{
    /// <summary>
    /// True = vstup, False = výstup
    /// </summary>
    public bool IsInput { get; set; }

    /// <summary>
    /// Počet kusů aktuálně v kontejneru
    /// </summary>
    public uint PieceCount { get; set; }

    /// <summary>
    /// ID přiřazeného kontejneru (null = žádný)
    /// </summary>
    public int? ContainerId { get; set; }

    /// <summary>
    /// Nepovinný popis kontejneru (např. typ, obsah, označení)
    /// </summary>
    public string? Description { get; set; }
}
