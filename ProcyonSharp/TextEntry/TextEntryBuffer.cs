using System.Linq;
using System.Text;
using ProcyonSharp.Bindings;

namespace ProcyonSharp.TextEntry
{
    internal class TextEntryBuffer
    {
        private const int TabWidth = 4;
        private readonly StringBuilder _buffer;
        private readonly TextEntryOptions _options;

        public TextEntryBuffer(StringBuilder buffer, TextEntryOptions options)
        {
            _buffer = buffer;
            _options = options;
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
            if (!_options.MultiLine)
                return;

            _buffer.AppendLine();
        }

        public void HandleKeyPressed(Key key, KeyMod mod)
        {
            if (_options.ExitKeys.Contains(key))
            {
                Finished = true;
                _options.OnCompletion(_buffer.ToString());
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
            if (!_options.AllowTab || _buffer.Length + TabWidth > _options.MaxLength)
                return;

            for (var i = 0; i < TabWidth; i++)
                _buffer.Append(' ');
        }

        public void HandleTextInput(char c)
        {
            if (_buffer.Length >= _options.MaxLength)
                return;

            _buffer.Append(c);
        }
    }
}