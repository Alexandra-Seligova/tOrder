namespace tOrder.Common;
public class ColorM
{
    public string Key { get; set; }
    public string Hex { get; set; }
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; } = 255;

    public ColorM(string key, string hex, byte r, byte g, byte b, byte a)
    {
        Key = key;
        Hex = hex;
        R = r;
        G = g;
        B = b;
        A = a;
    }
}
