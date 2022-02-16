using ProcyonSharp.Attributes;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;

namespace ProcyonSharp.Sample.States;

[State(SampleState.Menu)]
public class Menu : GameState<SampleState>
{
    private string _text;

    public override void Load()
    {
        _text = $"Press {Engine.BuildInputDescription(nameof(BeginGameplay))} to continue!";
        Engine.Window.GlyphScale = 1.0f;
    }

    public override void Draw(DrawContext ctx)
    {
        var (glyphWidth, glyphHeight) = Engine!.Window.GlyphSize;
        var (windowWidth, windowHeight) = Engine!.Window.Size;

        if (_text != null)
            ctx.DrawString((short)(windowWidth / 2 - _text.Length * glyphWidth / 2),
                (short)(windowHeight / 2 - glyphHeight / 2), _text);
    }

    [Input(Key.Enter)]
    public void BeginGameplay()
    {
        Engine?.PushState<Gameplay>();
    }
}