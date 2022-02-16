using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcyonSharp.Bindings;

namespace ProcyonSharp.TextEntry
{
    public class TextEntryOptions
    {
        public Action<string> OnCompletion { get; set; }
        public int MaxLength { get; set; }
        public Key[] ExitKeys { get; set; }
        public bool AllowTab { get; set; }
        public bool MultiLine { get; set; }

        public TextEntryOptions()
        {
            ExitKeys = Array.Empty<Key>();
            MaxLength = 1024;
        }
    }
}
