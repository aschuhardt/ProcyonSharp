using System;
using ProcyonSharp.Bindings;

namespace ProcyonSharp.TextEntry;

public class TextEntryOptions
{
    public TextEntryOptions()
    {
        ExitKeys = Array.Empty<Key>();
        MaxLength = 1024;
    }

    public Action<string> OnCompletion { get; set; }
    public int MaxLength { get; set; }
    public Key[] ExitKeys { get; set; }
    public bool AllowTab { get; set; }
    public bool MultiLine { get; set; }
}