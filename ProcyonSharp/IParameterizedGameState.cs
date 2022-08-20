using System;

namespace ProcyonSharp;

public interface IParameterizedGameState<T, U> : IGameState<T> where T : struct, Enum
{
    public U Parameter { get; set; }
}