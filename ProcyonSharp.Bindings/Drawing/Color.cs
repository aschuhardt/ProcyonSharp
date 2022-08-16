using System;
using System.Runtime.InteropServices;

namespace ProcyonSharp.Bindings.Drawing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Color
{
    public static readonly Color White = FromRgb(0xFF, 0xFF, 0xFF);
    public static readonly Color Black = FromRgb(0x0, 0x0, 0x0);

    public readonly int Value;

    [DllImport("procyon", EntryPoint = "procy_create_color")]
    private static extern Color CreateColor(byte r, byte g, byte b);

    public static Color FromRgb(float r, float g, float b)
    {
        return CreateColor(
            (byte)MathF.Min(MathF.Abs(r) * 255.0f, 255.0f),
            (byte)MathF.Min(MathF.Abs(g) * 255.0f, 255.0f),
            (byte)MathF.Min(MathF.Abs(b) * 255.0f, 255.0f));
    }

    public static Color FromRgb(byte r, byte g, byte b)
    {
        return CreateColor(r, g, b);
    }

    public Color(int value)
    {
        Value = value;
    }
}