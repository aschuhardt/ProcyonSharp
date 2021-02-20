using ProcyonSharp.Attributes;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp.Sample.States
{
    [State(GameState.Menu)]
    public class Menu : IGameState<GameState>
    {
        private string? _text;
        public Global<GameState>? Global { get; set; }

        public void Load()

        {
            _text = $"Press {Global!.BuildInputDescription(nameof(BeginGameplay))} to continue!";
            Global!.Window.GlyphScale = 1.0f;
        }

        public void Unload()
        {
        }

        public void Draw(DrawContext ctx)
        {
            var (glyphWidth, glyphHeight) = Global!.Window.GlyphSize;
            var (windowWidth, windowHeight) = Global!.Window.Size;

            ctx.DrawString(windowWidth / 2 - _text.Length * glyphWidth / 2, windowHeight / 2 - glyphHeight / 2, _text);
        }

        [Input(Key.Enter)]
        public void BeginGameplay()
        {
            Global?.PushState<Gameplay>();
        }
    }
}