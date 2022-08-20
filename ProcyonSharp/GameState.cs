using System;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp;

public abstract class GameState<T> : IGameState<T> where T : struct, Enum
{
    public Window Window => Engine?.Window;
    public Engine<T> Engine { get; set; } = null!;

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

    protected void PushState<U, V>(V parameter) where U : IParameterizedGameState<T, V>, new()
    {
        Engine.PushState<U, V>(parameter);
    }

    protected void ReplaceState<U, V>(V parameter) where U : IParameterizedGameState<T, V>, new()
    {
        Engine.ReplaceState<U, V>(parameter);
    }

    protected void PushState<U>() where U : IGameState<T>, new()
    {
        Engine.PushState<U>();
    }

    protected void ReplaceState<U>() where U : IGameState<T>, new()
    {
        Engine.ReplaceState<U>();
    }

    protected IGameState<T> PopState()
    {
        return Engine.PopState();
    }
}