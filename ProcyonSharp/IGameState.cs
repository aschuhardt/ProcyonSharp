using System;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp
{
    public interface IGameState<T> where T : struct, Enum
    {
        Global<T> Global { set; }

        void Load();

        void Unload();

        void Draw(DrawContext ctx);

        void KeyPressed(Key key, KeyMod modifier);

        void KeyReleased(Key key, KeyMod modifier);

        void Resized(int width, int height);
    }
}