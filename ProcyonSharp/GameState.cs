using System;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp
{
    public abstract class GameState<T> : IGameState<T> where T: struct, Enum
    {
        public Global<T> Global { get; set; } = null!;
        
        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
        }

        public virtual void Draw(DrawContext ctx)
        {
        }
    }
}