using System.Collections.Generic;
using ProcyonSharp.Bindings;

namespace ProcyonSharp.Input
{
    public class GameStateInputFunctionKeyMap<T>
    {
        public GameStateInputFunctionKeyMap()
        {
            Functions = new Dictionary<Key, ICollection<MappedFunctionCall<T>>>();
        }

        public IDictionary<Key, ICollection<MappedFunctionCall<T>>> Functions { get; }
    }
}