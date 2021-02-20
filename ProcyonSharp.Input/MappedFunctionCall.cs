using System;
using ProcyonSharp.Bindings;
using YamlDotNet.Serialization;

namespace ProcyonSharp.Input
{
    public class MappedFunctionCall<T>
    {
        public MappedFunctionCall()
        {
            FunctionCall = new Action(() => { });
        }

        [YamlMember(Alias = "name")] public string? FunctionName { get; set; }

        public Key Key { get; set; }
        public bool Ctrl { get; set; }
        public bool Shift { get; set; }
        public bool Alt { get; set; }

        [YamlIgnore] public Delegate FunctionCall { get; set; }
    }
}