using ProcyonSharp.Sample.States;

namespace ProcyonSharp.Sample
{
    internal class Program
    {
        private static void Main()
        {
            using var state = Game.Create<GameState, Menu>();
            state.Start(800, 600, "SuruliaSharp");
        }
    }
}