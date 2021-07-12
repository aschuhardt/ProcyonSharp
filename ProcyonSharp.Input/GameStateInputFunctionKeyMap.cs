using System.Collections.Generic;
using ProcyonSharp.Bindings;

namespace ProcyonSharp.Input
{
    public class GameStateInputFunctionKeyMap
    {
        public GameStateInputFunctionKeyMap()
        {
            Functions = new Dictionary<Key, ICollection<MappedFunctionCall>>();
        }

        public IDictionary<Key, ICollection<MappedFunctionCall>> Functions { get; }
    }
}