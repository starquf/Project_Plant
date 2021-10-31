using System;

[Flags]
public enum NeedType
{
    None = 0,
    WATER = 1 << 0,
    SUN = 1 << 1
}
