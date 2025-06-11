using System;
using System.Linq;

namespace tOrder.Common;
public struct UserM
{
    public static readonly UserM Null = new("—", null, "—", null, null);

    public string Name { get; set; }
    public string? MiddleName { get; set; }
    public string Surname { get; set; }
    public uint? Id { get; set; }
    public uint? AssociatedPersonId { get; set; }

    public string FullName
    {
        get
        {
            var parts = new[] { Name, Surname }
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();

            string full = string.Join(" ", parts);

            if (full.Length > 50)
                Console.WriteLine($"[Warning] Full name '{full}' exceeds 50 characters.");

            return full;
        }
    }

    public UserM(string name, uint? id, string surname, string? middleName = null, uint? associatedPersonId = null)
    {
        Name = name ?? "—";
        Surname = surname ?? "—";
        MiddleName = middleName;
        Id = id;
        AssociatedPersonId = associatedPersonId;
    }

    public override string ToString()
    {
        var idText = Id.HasValue ? Id.Value.ToString() : "—";
        return $"{FullName} ({idText})";
    }
}
