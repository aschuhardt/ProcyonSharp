using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable UnusedMember.Global

namespace ProcyonSharp.Bindings.Drawing;

/// <summary>
///     Provides drawing routines to game state implementations
/// </summary>
public class DrawContext
{
    private readonly Window _window;

    private short _layer;

    internal DrawContext(Window window)
    {
        Layer = 1;
        _window = window;
    }

    public int Layer
    {
        get => _layer;
        set => _layer = (short)(value % short.MaxValue);
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
    public void DrawString(short x, short y, StringBuilder contents, bool bold = false, in Color? foreColor = null,
        in Color? backColor = null)
    {
        if (bold)
            DrawStringBold(_window.Pointer, x, y, _layer, foreColor.GetValueOrDefault(Color.White),
                backColor.GetValueOrDefault(Color.Black), contents);
        else
            DrawString(_window.Pointer, x, y, _layer, foreColor.GetValueOrDefault(Color.White),
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
    public void DrawString(short x, short y, string contents, bool bold = false, in Color? foreColor = null,
        in Color? backColor = null)
    {
        if (bold)
            DrawStringBold(_window.Pointer, x, y, _layer, foreColor.GetValueOrDefault(Color.White),
                backColor.GetValueOrDefault(Color.Black), contents);
        else
            DrawString(_window.Pointer, x, y, _layer, foreColor.GetValueOrDefault(Color.White),
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
    public void DrawChar(short x, short y, byte c, bool bold = false, in Color? foreColor = null,
        in Color? backColor = null)
    {
        if (bold)
            DrawCharBold(_window.Pointer, x, y, _layer, foreColor.GetValueOrDefault(Color.White),
                backColor.GetValueOrDefault(Color.Black), c);
        else
            DrawChar(_window.Pointer, x, y, _layer, foreColor.GetValueOrDefault(Color.White),
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
    public void DrawRect(short x, short y, short width, short height, in Color? color = null)
    {
        DrawRect(_window.Pointer, x, y, _layer, width, height, color.GetValueOrDefault(Color.White));
    }

    /// <summary>
    ///     Draws a colored line between two points
    /// </summary>
    /// <param name="x1">The X-coordinate in pixels of the first point</param>
    /// <param name="y1">The Y-coordinate in pixels of the first point</param>
    /// <param name="x2">The X-coordinate in pixels of the second point</param>
    /// <param name="y2">The Y-coordinate in pixels of the second point</param>
    /// <param name="color">The color of the line (default = <see cref="Color.White" />)</param>
    public void DrawLine(short x1, short y1, short x2, short y2, in Color? color = null)
    {
        DrawLine(_window.Pointer, x1, y1, x2, y2, _layer, color.GetValueOrDefault(Color.White));
    }

    /// <summary>
    ///     Draws a sprite
    /// </summary>
    /// <param name="sprite">The sprite to be drawn</param>
    /// <param name="x">The X-coordinate in pixels of the top-left position of the sprite</param>
    /// <param name="y">The Y-coordinate in pixels of the top-left position of the sprite</param>
    /// <param name="foreColor">The foreground color of the sprite (default = <see cref="Color.White"/></param>
    /// <param name="backColor">The background color of the sprite (default = <see cref="Color.Black"/></param>
    public void DrawSprite(Sprite sprite, short x, short y, in Color? foreColor = null, in Color? backColor = null)
    {
        DrawSprite(_window.Pointer, x, y, _layer, foreColor.GetValueOrDefault(Color.White), backColor.GetValueOrDefault(Color.Black), sprite.Pointer);
    }

    [DllImport("procyon", EntryPoint = "procy_draw_sprite")]
    private static extern void DrawSprite(IntPtr windowPtr, short x, short y, short z, Color foreColor, Color backColor,
        IntPtr sprite);

    [DllImport("procyon", EntryPoint = "procy_draw_string")]
    private static extern void DrawString(IntPtr windowPtr, short x, short y, short z, Color foreColor, Color backColor,
        StringBuilder contents);

    [DllImport("procyon", EntryPoint = "procy_draw_string")]
    private static extern void DrawString(IntPtr windowPtr, short x, short y, short z, Color foreColor, Color backColor,
        string contents);

    [DllImport("procyon", EntryPoint = "procy_draw_string_bold")]
    private static extern void DrawStringBold(IntPtr windowPtr, short x, short y, short z, Color foreColor,
        Color backColor,
        StringBuilder contents);


    [DllImport("procyon", EntryPoint = "procy_draw_string_bold")]
    private static extern void DrawStringBold(IntPtr windowPtr, short x, short y, short z, Color foreColor,
        Color backColor,
        string contents);

    [DllImport("procyon", EntryPoint = "procy_draw_char")]
    private static extern void DrawChar(IntPtr windowPtr, short x, short y, short z, Color foreColor, Color backColor,
        byte codepoint);

    [DllImport("procyon", EntryPoint = "procy_draw_char_bold")]
    private static extern void DrawCharBold(IntPtr windowPtr, short x, short y, short z, Color foreColor,
        Color backColor,
        byte codepoint);

    [DllImport("procyon", EntryPoint = "procy_draw_rect")]
    private static extern void DrawRect(IntPtr windowPtr, short x, short y, short z, short width, short height,
        Color color);

    [DllImport("procyon", EntryPoint = "procy_draw_line")]
    private static extern void DrawLine(IntPtr windowPtr, short x1, short y1, short x2, short y2, short z, Color color);
}