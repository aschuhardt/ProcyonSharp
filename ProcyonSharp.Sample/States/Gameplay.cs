using System;
using System.Text;
using ProcyonSharp.Attributes;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;
using ProcyonSharp.TextEntry;

namespace ProcyonSharp.Sample.States;

[State(SampleState.Gameplay)]
public class Gameplay : GameState<SampleState>
{
    private readonly StringBuilder _enteredText = new("> press enter to toggle text entry");

    private readonly Color _playerGlyphColor = Color.FromRgb(255, 255, 0);
    private Sprite _cobblestone;
    private (int Width, int Height) _glyphSize;
    private (int X, int Y) _playerPosition;
    private (double X, double Y) _mousePosition;
    private bool _mouseDown;

    public override void Load()
    {
        _cobblestone = new SpriteSheet(Engine.Window, "cobblestone.png").CreateSprite(0, 0, 16, 16);
        _playerPosition = (100, 100);
        _glyphSize = Engine.Window.GlyphSize;
        Engine.Window.ClearColor = Color.FromRgb(0.1f, 0.3f, 0.23f);
    }

    public override void Draw(DrawContext ctx)
    {
        var (windowWidth, windowHeight) = Engine.Window.Size;
        var (spriteWidth, spriteHeight) = _cobblestone.Size;
        for (var i = 0; i < windowWidth / spriteWidth; i++)
            ctx.DrawSprite(_cobblestone, (short)(i * spriteWidth), (short)(windowHeight - spriteHeight));

        ctx.DrawChar((short)_playerPosition.X, (short)_playerPosition.Y, Convert.ToByte('@'),
            foreColor: _playerGlyphColor);

        if (Engine.TextEntryActive)
            ctx.DrawString(0, 0, _enteredText, true);
        else
            ctx.DrawString(0, 0, _enteredText, false, Color.Black, Color.White);

        ctx.DrawString(0, 100, $"Mouse position: ({_mousePosition.X:F1}, {_mousePosition.Y:F1})");

        if (_mouseDown)
            ctx.DrawString(0, 120, "Mouse pressed!", bold: true);
    }

    [Input(Key.Escape)]
    public void ReturnToMenu()
    {
        Engine.PopState();
    }

    [Input(Key.Up)]
    [Input(Key.Kp8)]
    public void MoveUp()
    {
        _playerPosition.Y -= _glyphSize.Height;
    }

    [Input(Key.Down)]
    [Input(Key.Kp2)]
    public void MoveDown()
    {
        _playerPosition.Y += _glyphSize.Height;
    }

    [Input(Key.Left)]
    [Input(Key.Kp4)]
    public void MoveLeft()
    {
        _playerPosition.X -= _glyphSize.Width;
    }

    [Input(Key.Right)]
    [Input(Key.Kp6)]
    public void MoveRight()
    {
        _playerPosition.X += _glyphSize.Width;
    }

    [Input(Key.Enter)]
    public void StartTextEntry()
    {
        Engine.BeginTextEntry(_enteredText, new TextEntryOptions { ExitKeys = new[] { Key.Escape, Key.Enter } });
    }

    public override void MousePressed(MouseButton button, KeyMod modifier)
    {
        _mouseDown = true;
    }

    public override void MouseReleased(MouseButton button, KeyMod modifier)
    {
        _mouseDown = false;
    }

    public override void MouseMoved(double x, double y)
    {
        _mousePosition.X = x;
        _mousePosition.Y = y;
    }
}