using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable UnusedMember.Global

namespace ProcyonSharp.Bindings.Drawing
{
    /// <summary>
    ///     Provides drawing routines to game state implementations
    /// </summary>
    public class DrawContext
    {
        private readonly Window _window;

        internal DrawContext(Window window)
        {
            _window = window;
        }

        /// <summary>
        ///     Draws a string at the specified location
        /// </summary>
        /// <param name="x">The X-coordinate in pixels</param>
        /// <param name="y">The Y-coordinate in pixels</param>
        /// <param name="contents">The text content to be printed</param>
        /// <param name="bold">Whether or not the string should be printed using a bold typeface (default = false)</param>
        /// <param name="foreColor">The color of the text glyphs (default = <see cref="Color.White" />)</param>
        /// <param name="backColor">The color of the text's background (default = <see cref="Color.Black" />)</param>
        public void DrawString(int x, int y, StringBuilder contents, bool bold = false, Color? foreColor = null,
            Color? backColor = null)
        {
            if (bold)
                DrawStringBold(_window.NativePtr, x, y, foreColor.GetValueOrDefault(Color.White),
                    backColor.GetValueOrDefault(Color.Black), contents);
            else
                DrawString(_window.NativePtr, x, y, foreColor.GetValueOrDefault(Color.White),
                    backColor.GetValueOrDefault(Color.Black), contents);
        }

        /// <summary>
        ///     Draws a string at the specified location
        /// </summary>
        /// <param name="x">The X-coordinate in pixels</param>
        /// <param name="y">The Y-coordinate in pixels</param>
        /// <param name="contents">The text content to be printed</param>
        /// <param name="bold">Whether or not the string should be printed using a bold typeface (default = false)</param>
        /// <param name="foreColor">The color of the text glyphs (default = <see cref="Color.White" />)</param>
        /// <param name="backColor">The color of the text's background (default = <see cref="Color.Black" />)</param>
        public void DrawString(int x, int y, string contents, bool bold = false, Color? foreColor = null,
            Color? backColor = null)
        {
            if (bold)
                DrawStringBold(_window.NativePtr, x, y, foreColor.GetValueOrDefault(Color.White),
                    backColor.GetValueOrDefault(Color.Black), contents);
            else
                DrawString(_window.NativePtr, x, y, foreColor.GetValueOrDefault(Color.White),
                    backColor.GetValueOrDefault(Color.Black), contents);
        }

        /// <summary>
        ///     Draw a character at the specified location
        /// </summary
        /// <param name="x">The X-coordinate in pixels</param>
        /// <param name="y">The Y-coordinate in pixels</param>
        /// <param name="c">The character to be printed</param>
        /// <param name="bold">Whether or not the string should be printed using a bold typeface (default = false)</param>
        /// <param name="foreColor">The color of the text glyphs (default = <see cref="Color.White" />)</param>
        /// <param name="backColor">The color of the text's background (default = <see cref="Color.Black" />)</param>
        public void DrawChar(int x, int y, byte c, bool bold = false, Color? foreColor = null, Color? backColor = null)
        {
            if (bold)
                DrawCharBold(_window.NativePtr, x, y, foreColor.GetValueOrDefault(Color.White),
                    backColor.GetValueOrDefault(Color.Black), c);
            else
                DrawChar(_window.NativePtr, x, y, foreColor.GetValueOrDefault(Color.White),
                    backColor.GetValueOrDefault(Color.Black), c);
        }

        /// <summary>
        ///     Draws a colored rectangle at the specified location
        /// </summary>
        /// <param name="x">The X-coordinate in pixels</param>
        /// <param name="y">The Y-coordinate in pixels</param>
        /// <param name="width">The width of the rectangle in pixels</param>
        /// <param name="height">The height of the rectangle in pixels</param>
        /// <param name="color">The color of the rectangle (default = <see cref="Color.White" />)</param>
        public void DrawRect(int x, int y, int width, int height, Color? color = null)
        {
            DrawRect(_window.NativePtr, x, y, width, height, color.GetValueOrDefault(Color.White));
        }

        /// <summary>
        ///     Draws a colored line between two points
        /// </summary>
        /// <param name="x1">The X-coordinate in pixels of the first point</param>
        /// <param name="y1">The Y-coordinate in pixels of the first point</param>
        /// <param name="x2">The X-coordinate in pixels of the second point</param>
        /// <param name="y2">The Y-coordinate in pixels of the second point</param>
        /// <param name="color">The color of the line (default = <see cref="Color.White" />)</param>
        public void DrawLine(int x1, int y1, int x2, int y2, Color? color = null)
        {
            DrawLine(_window.NativePtr, x1, y1, x2, y2, color.GetValueOrDefault(Color.White));
        }


        [DllImport("procyon", EntryPoint = "procy_draw_string")]
        private static extern void DrawString(IntPtr windowPtr, int x, int y, Color foreColor, Color backColor,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder contents);

        [DllImport("procyon", EntryPoint = "procy_draw_string")]
        private static extern void DrawString(IntPtr windowPtr, int x, int y, Color foreColor, Color backColor,
            [MarshalAs(UnmanagedType.LPStr)] string contents);

        [DllImport("procyon", EntryPoint = "procy_draw_string_bold")]
        private static extern void DrawStringBold(IntPtr windowPtr, int x, int y, Color foreColor, Color backColor,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder contents);


        [DllImport("procyon", EntryPoint = "procy_draw_string_bold")]
        private static extern void DrawStringBold(IntPtr windowPtr, int x, int y, Color foreColor, Color backColor,
            [MarshalAs(UnmanagedType.LPStr)] string contents);

        [DllImport("procyon", EntryPoint = "procy_draw_char")]
        private static extern void DrawChar(IntPtr windowPtr, int x, int y, Color foreColor, Color backColor,
            byte codepoint);

        [DllImport("procyon", EntryPoint = "procy_draw_char_bold")]
        private static extern void DrawCharBold(IntPtr windowPtr, int x, int y, Color foreColor, Color backColor,
            byte codepoint);

        [DllImport("procyon", EntryPoint = "procy_draw_rect")]
        private static extern void DrawRect(IntPtr windowPtr, int x, int y, int width, int height, Color color);

        [DllImport("procyon", EntryPoint = "procy_draw_line")]
        private static extern void DrawLine(IntPtr windowPtr, int x1, int y1, int x2, int y2, Color color);
    }
}