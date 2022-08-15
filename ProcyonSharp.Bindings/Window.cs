using System;
using System.Runtime.InteropServices;
using System.Text;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp.Bindings;

public class Window : NativeObject
{
    private bool _highFpsMode;

    private float _scale;

    public Window(int width, int height, string title, NativeEventHandler nativeEventHandler)
    {
        Pointer = CreateWindow(width, height, new StringBuilder(title), nativeEventHandler.Pointer);
        _scale = 1.0f;
        _highFpsMode = false;
    }

    public (int, int) Size
    {
        get
        {
            int width = 0, height = 0;
            GetWindowSize(Pointer, ref width, ref height);
            return (width, height);
        }
    }

#pragma warning disable CA1822 // Mark members as static
    public Color ClearColor
#pragma warning restore CA1822 // Mark members as static
    {
        set => SetClearColor(value);
    }

    public (int, int) GlyphSize
    {
        get
        {
            int width = 0, height = 0;
            GetGlyphSize(Pointer, ref width, ref height);
            return (width, height);
        }
    }

    public float GlyphScale
    {
        get => _scale;
        set
        {
            if (Math.Abs(_scale - value) <= float.Epsilon)
                return;

            _scale = value;
            SetGlyphScale(Pointer, _scale, _scale);
        }
    }

    public bool HighFpsMode
    {
        get => _highFpsMode;
        set
        {
            if (_highFpsMode == value) 
                return;

            _highFpsMode = value;
            SetHighFpsMode(Pointer, _highFpsMode);
        }
    }

    public void ResetScale()
    {
        ResetScale(Pointer);
    }

    public void Run()
    {
        BeginLoop(Pointer);
    }

    public void Close()
    {
        CloseWindow(Pointer);
    }

    [DllImport("procyon", EntryPoint = "procy_create_window")]
    private static extern IntPtr CreateWindow(int width, int height, StringBuilder title, IntPtr state);

    [DllImport("procyon", EntryPoint = "procy_destroy_window")]
    private static extern void DestroyWindow(IntPtr window);

    [DllImport("procyon", EntryPoint = "procy_begin_loop")]
    private static extern void BeginLoop(IntPtr window);

    [DllImport("procyon", EntryPoint = "procy_close_window")]
    private static extern void CloseWindow(IntPtr window);

    [DllImport("procyon", EntryPoint = "procy_get_window_size")]
    private static extern void GetWindowSize(IntPtr window, ref int width, ref int height);

    [DllImport("procyon", EntryPoint = "procy_set_clear_color")]
    private static extern void SetClearColor(Color color);

    [DllImport("procyon", EntryPoint = "procy_set_high_fps_mode")]
    private static extern void SetHighFpsMode(IntPtr window, bool isHighFpsMode);

    [DllImport("procyon", EntryPoint = "procy_get_glyph_size")]
    private static extern void GetGlyphSize(IntPtr window, ref int width, ref int height);

    [DllImport("procyon", EntryPoint = "procy_set_scale")]
    private static extern void SetGlyphScale(IntPtr window, float x, float y);

    [DllImport("procyon", EntryPoint = "procy_reset_scale")]
    private static extern void ResetScale(IntPtr window);

    protected override void Cleanup()
    {
        DestroyWindow(Pointer);
    }
}