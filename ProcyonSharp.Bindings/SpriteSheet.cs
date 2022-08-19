using System;
using System.IO;
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
                $"Failed to load spritesheet from {path}");
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    public SpriteSheet(Window window, Stream data)
    {
        data.Seek(0, SeekOrigin.Begin);
        using var reader = new BinaryReader(data);
        var buffer = reader.ReadBytes((int)data.Length);
        Pointer = LoadSpriteShaderMemory(window.Pointer, buffer, (ulong)buffer.Length);
        if (Pointer == IntPtr.Zero)
            throw new Exception("Failed to load spritesheet from an in-memory buffer");
    }

    public Sprite CreateSprite(int x, int y, int width, int height)
    {
        return new Sprite(this, x, y, width, height);
    }

    protected override void Cleanup()
    {
        // handled by the window implementations
    }

    [DllImport("procyon", EntryPoint = "procy_load_sprite_shader")]
    private static extern IntPtr LoadSpriteShader(IntPtr window, string path);

    [DllImport("procyon", EntryPoint = "procy_load_sprite_shader_mem")]
    private static extern IntPtr LoadSpriteShaderMemory(IntPtr window, byte[] data, ulong size);
}