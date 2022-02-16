using System;
using System.Runtime.InteropServices;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp.Bindings;

public class SpriteSheet : NativeObject
{
    // ReSharper disable once SuggestBaseTypeForParameter
    public SpriteSheet(Window window, string path)
    {
        Pointer = LoadSpriteShader(window.Pointer, path);
        if (Pointer == IntPtr.Zero)
            throw new Exception(
                $"Failed to load spritesheet from {path}; most likely the maximum number of spritesheets was exceeded");
    }

    public Sprite CreateSprite(short x, short y, short width, short height, in Color? foreColor = null,
        in Color? backColor = null)
    {
        return new Sprite(this, x, y, width, height)
        {
            ForeColor = foreColor.GetValueOrDefault(Color.White),
            BackColor = backColor.GetValueOrDefault(Color.Black)
        };
    }

    protected override void Cleanup()
    {
        // handled by the window implementations
    }

    [DllImport("procyon", EntryPoint = "procy_load_sprite_shader")]
    private static extern IntPtr LoadSpriteShader(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string path);
}