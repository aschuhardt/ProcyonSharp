using System;
using System.Runtime.InteropServices;

namespace ProcyonSharp.Bindings.Drawing;

public class Sprite : NativeObject
{
    // ReSharper disable once SuggestBaseTypeForParameter
    internal Sprite(SpriteSheet spriteSheet, short x, short y, short width, short height)
    {
        Size = (width, height);
        Offset = (x, y);
        Pointer = CreateSprite(spriteSheet.Pointer, x, y, width, height);
    }

    public (short, short) Size { get; }
    public (short, short) Offset { get; }
    public Color ForeColor { get; set; }
    public Color BackColor { get; set; }

    protected override void Cleanup()
    {
        DestroySprite(Pointer);
    }

    [DllImport("procyon", EntryPoint = "procy_create_sprite")]
    private static extern IntPtr CreateSprite(IntPtr spriteSheet, short x, short y, short width, short height);

    [DllImport("procyon", EntryPoint = "procy_destroy_sprite")]
    private static extern IntPtr DestroySprite(IntPtr ptr);
}