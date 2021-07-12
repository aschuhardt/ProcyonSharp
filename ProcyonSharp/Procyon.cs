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
    public static class Procyon
    {
        public static Engine<T> Create<T, U>()
            where T : struct, Enum
            where U : IGameState<T>, new()
        {
            var engine = new Engine<T>();
            engine.InitialState<U>();
            return engine;
        }
    }
}