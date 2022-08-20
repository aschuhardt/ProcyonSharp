using System;

namespace ProcyonSharp;

public class ParameterizedGameState<T, U> : GameState<T>, IParameterizedGameState<T, U> where T : struct, Enum
{
    public U Parameter { get; set; }
}