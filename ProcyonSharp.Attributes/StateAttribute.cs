using System;
using System.Diagnostics;

namespace ProcyonSharp.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class StateAttribute : Attribute
{
    public StateAttribute(object stateEnumValue)
    {
        Debug.Assert(stateEnumValue is Enum);
        StateEnumValue = stateEnumValue;
    }

    public object StateEnumValue { get; }
}