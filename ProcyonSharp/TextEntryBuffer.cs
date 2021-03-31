using System.Linq;
using System.Text;
using ProcyonSharp.Bindings;

namespace ProcyonSharp
{
    internal class TextEntryBuffer
    {
        private const int TabWidth = 4;

        private readonly StringBuilder _buffer;
        private readonly Key[] _exitKeys;
        private readonly bool _multiLine;
        private readonly int _maxLength;
        private readonly bool _allowTab;

        public TextEntryBuffer(StringBuilder buffer, Key[] exitKeys, bool multiLine, int maxLength, bool allowTab)
        {
            _buffer = buffer;
            _exitKeys = exitKeys;
            _multiLine = multiLine;
            _maxLength = maxLength;
            _allowTab = allowTab;
            Finished = false;
        }

        public bool Finished { get; private set; }

        private void HandleBackspace(bool ctrl)
        {
            if (_buffer.Length <= 0)
                return;
                
            const string wordSeparatorChars = " .,!?/'\"\\[]{}+=()`|<>\n";
            // if CTRL is depressed, remove from the end of the buffer until it's empty or until a separator is reached
            do
            {
                _buffer.Remove(_buffer.Length - 1, 1);
            } while (ctrl && _buffer.Length > 0 && !wordSeparatorChars.Contains(_buffer[^1]));
        }

        private void HandleEnter()
        {
            if (!_multiLine)
                return;

            _buffer.AppendLine();
        }

        public void HandleKeyPressed(Key key, KeyMod mod)
        {
            if (_exitKeys.Contains(key))
            {
                Finished = true;
                return;
            }

            switch (key)
            {
                case Key.Backspace:
                    HandleBackspace(mod.Control);
                    break;
                case Key.Enter:
                    HandleEnter();
                    break;
                case Key.Tab:
                    HandleTab();
                    break;
            }
        }

        private void HandleTab()
        {
            if (!_allowTab)
                return;
            
            for (var i = 0; i < TabWidth; i++)
                _buffer.Append(' ');
        }

        public void HandleTextInput(char c)
        {
            if (_buffer.Length >= _maxLength)
                return;
                
            _buffer.Append(c);
        }
    }
}