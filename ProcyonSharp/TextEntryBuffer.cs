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

        public TextEntryBuffer(StringBuilder buffer, Key[] exitKeys, bool multiLine)
        {
            _buffer = buffer;
            _exitKeys = exitKeys;
            _multiLine = multiLine;
            Finished = false;
        }

        public bool Finished { get; private set; }

        private void HandleBackspace(bool ctrl)
        {
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
            for (var i = 0; i < TabWidth; i++)
                _buffer.Append(' ');
        }

        public void HandleTextInput(char c)
        {
            _buffer.Append(c);
        }
    }
}