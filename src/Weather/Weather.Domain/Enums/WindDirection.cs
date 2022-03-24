namespace Weather.Domain.Enums;

[Flags]
public enum WindDirection
{
    Calm = 0x0000,
    Northern = 0x0001,
    Northwest = 0x0002,
    Western = 0x0004,
    Southwest = 0x0008,
    Southern = 0x0016,
    SouthEast = 0x0032,
    Eastern = 0x0064,
    Northeast = 0x0128
}