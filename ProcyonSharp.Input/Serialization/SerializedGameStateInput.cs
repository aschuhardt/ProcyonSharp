using System;

namespace ProcyonSharp.Input.Serialization
{
    public class SerializedGameStateInput<T, U> where T : Enum
    {
        public T? State { get; set; }
        public MappedFunctionCall<U>[]? Functions { get; set; }
    }
}