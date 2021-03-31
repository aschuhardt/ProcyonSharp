using System;
using System.Text;
using ProcyonSharp.Attributes;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp.Sample.States
{
    [State(GameState.Gameplay)]
    public class Gameplay : GameState<GameState>
    {
        private readonly StringBuilder _enteredText = new("> press enter to toggle text entry");

        private readonly Color _playerGlyphColor = new(1.0f, 1.0f, 0.01f);
        private (int Width, int Height) _glyphSize;

        private (int X, int Y) _playerPosition;

        public override void Load()
        {
            _playerPosition = (100, 100);
            _glyphSize = Global.Window.GlyphSize;
            Global.Window.ClearColor = new Color(0.1f, 0.3f, 0.23f);
        }

        public override void Draw(DrawContext ctx)
        {
            ctx.DrawChar(_playerPosition.X, _playerPosition.Y, Convert.ToByte('@'), foreColor: _playerGlyphColor);

            if (Global.TextEntryActive)
                ctx.DrawString(0, 0, _enteredText, true);
            else
                ctx.DrawString(0, 0, _enteredText, false, Color.Black, Color.White);
        }

        [Input(Key.Escape)]
        public void ReturnToMenu()
        {
            Global.PopState();
        }

        [Input(Key.Up)]
        [Input(Key.Kp8)]
        public void MoveUp()
        {
            _playerPosition.Y -= _glyphSize.Height;
        }

        [Input(Key.Down)]
        [Input(Key.Kp2)]
        public void MoveDown()
        {
            _playerPosition.Y += _glyphSize.Height;
        }

        [Input(Key.Left)]
        [Input(Key.Kp4)]
        public void MoveLeft()
        {
            _playerPosition.X -= _glyphSize.Width;
        }

        [Input(Key.Right)]
        [Input(Key.Kp6)]
        public void MoveRight()
        {
            _playerPosition.X += _glyphSize.Width;
        }

        [Input(Key.Enter)]
        public void StartTextEntry()
        {
            Global.BeginTextEntry(_enteredText, false, exitKeys: Key.Enter);
        }
    }
}