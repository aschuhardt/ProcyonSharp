using System;
using System.Runtime.CompilerServices;
using ProcyonSharp.Attributes;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

[assembly: TypeForwardedTo(typeof(Key))]
[assembly: TypeForwardedTo(typeof(StateAttribute))]
[assembly: TypeForwardedTo(typeof(InputAttribute))]
[assembly: TypeForwardedTo(typeof(Color))]
[assembly: TypeForwardedTo(typeof(DrawContext))]
[assembly: TypeForwardedTo(typeof(Window))]

namespace ProcyonSharp
{
    public static class Game
    {
        public static Global<T> Create<T, U>()
            where T : struct, Enum
            where U : IGameState<T>, new()
        {
            var globalState = new Global<T>();
            globalState.BeginState<U>();
            return globalState;
        }
    }
}