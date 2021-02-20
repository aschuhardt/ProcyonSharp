using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp.Bindings
{
    [SuppressUnmanagedCodeSecurity]
    public class Window : IDisposable
    {
        internal readonly IntPtr NativePtr;
        private bool _highFpsMode;

        private float _scale;

        public Window(int width, int height, string title, State state, float scale = 1.0f)
        {
            NativePtr = CreateWindow(width, height, new StringBuilder(title), scale,
                state.NativePtr);
            _scale = 1.0f;
            _highFpsMode = false;
        }

        public (int, int) Size
        {
            get
            {
                int width = 0, height = 0;
                GetWindowSize(NativePtr, ref width, ref height);
                return (width, height);
            }
        }

        public Color ClearColor
        {
            set => SetClearColor(value);
        }

        public (int, int) GlyphSize
        {
            get
            {
                int width = 0, height = 0;
                GetGlyphSize(NativePtr, ref width, ref height);
                return (width, height);
            }
        }

        public float GlyphScale
        {
            get => _scale;
            set
            {
                _scale = value;
                SetGlyphScale(NativePtr, _scale);
            }
        }

        public bool HighFpsMode
        {
            get => _highFpsMode;
            set
            {
                _highFpsMode = value;
                SetHighFpsMode(NativePtr, _highFpsMode);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        public void Run()
        {
            BeginLoop(NativePtr);
        }

        public void Close()
        {
            CloseWindow(NativePtr);
        }

        [DllImport("procyon", EntryPoint = "procy_create_window")]
        private static extern IntPtr CreateWindow(int width, int height, StringBuilder title, float scale,
            IntPtr state);

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

        [DllImport("procyon", EntryPoint = "procy_set_glyph_scale")]
        private static extern void SetGlyphScale(IntPtr window, float scale);

        private void ReleaseUnmanagedResources()
        {
            DestroyWindow(NativePtr);
        }

        ~Window()
        {
            ReleaseUnmanagedResources();
        }
    }
}