using System;
using System.Runtime.InteropServices;

namespace ProcyonSharp.Bindings.Drawing;

public class Sprite : NativeObject
{
    // ReSharper disable once SuggestBaseTypeForParameter
    internal Sprite(SpriteSheet spriteSheet, int x, int y, int width, int height)
    {
        Size = (width, height);
        Offset = (x, y);
        Pointer = CreateSprite(spriteSheet.Pointer, x, y, width, height);
    }

    public (int, int) Size { get; }
    public (int, int) Offset { get; }

    protected override void Cleanup()
    {
        DestroySprite(Pointer);
    }

    [DllImport("procyon", EntryPoint = "procy_create_sprite")]
    private static extern IntPtr CreateSprite(IntPtr spriteSheet, int x, int y, int width, int height);

    [DllImport("procyon", EntryPoint = "procy_destroy_sprite")]
    private static extern IntPtr DestroySprite(IntPtr ptr);
}