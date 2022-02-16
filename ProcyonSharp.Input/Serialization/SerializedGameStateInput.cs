using System;

namespace ProcyonSharp.Input.Serialization;

public class SerializedGameStateInput<T> where T : Enum
{
    public T State { get; set; }
    public MappedFunctionCall[] Functions { get; set; }
}