using System;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp;

public abstract class GameState<T> : IGameState<T> where T : struct, Enum
{
    public Engine<T> Engine { get; set; } = null!;

    protected Window Window => Engine?.Window;

    public virtual void Load()
    {
    }

    public virtual void Unload()
    {
    }

    public virtual void Draw(DrawContext ctx)
    {
    }

    public virtual void KeyPressed(Key key, KeyMod modifier)
    {
    }

    public virtual void KeyReleased(Key key, KeyMod modifier)
    {
    }

    public virtual void Resized(int width, int height)
    {
    }

    public virtual void MouseMoved(double x, double y)
    {
    }

    public virtual void MousePressed(MouseButton button, KeyMod modifier)
    {
    }

    public virtual void MouseReleased(MouseButton button, KeyMod modifier)
    {
    }
}