using System;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp;

public interface IGameState<T> where T : struct, Enum
{
    Engine<T> Engine { set; }

    void Load();

    void Unload();

    void Draw(DrawContext ctx);

    void KeyPressed(Key key, KeyMod modifier);

    void KeyReleased(Key key, KeyMod modifier);

    void Resized(int width, int height);

    void MouseMoved(double x, double y);

    void MousePressed(MouseButton button, KeyMod modifier);

    void MouseReleased(MouseButton button, KeyMod modifier);
}