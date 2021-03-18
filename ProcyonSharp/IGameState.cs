using System;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp
{
    public interface IGameState<T> where T : struct, Enum
    {
        Global<T> Global { set; }

        void Load();

        void Unload();

        void Draw(DrawContext ctx);
    }
}