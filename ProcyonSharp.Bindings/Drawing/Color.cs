using System.Runtime.InteropServices;

namespace ProcyonSharp.Bindings.Drawing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Color
{
    public static readonly Color White = new(1.0f, 1.0f, 1.0f);
    public static readonly Color Black = new(0.0f, 0.0f, 0.0f);

    public readonly float Red;
    public readonly float Green;
    public readonly float Blue;

    public Color(float r, float g, float b)
    {
        Red = r;
        Green = g;
        Blue = b;
    }
}