using System;
using ProcyonSharp.Bindings;

namespace ProcyonSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class InputAttribute : Attribute
    {
        public InputAttribute(Key defaultKey, bool alt = false, bool ctrl = false, bool shift = false)
        {
            DefaultKey = defaultKey;
            DefaultModifier = new KeyMod(shift, ctrl, alt);
        }

        public Key DefaultKey { get; }

        public KeyMod DefaultModifier { get; }
    }
}