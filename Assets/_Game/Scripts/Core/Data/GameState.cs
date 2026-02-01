using Minesweeper.Utils;

namespace Minesweeper.Core.Data
{
    public class GameState
    {
        public ReactiveProperty<float> Timer;
        public ReactiveProperty<int> FlagsLeft;
        public ReactiveProperty<bool> IsPlaying;
        public ReactiveProperty<bool> IsDead;

        public static GameState Create(GameSettings settings)
        {
            return new GameState()
            {
                Timer = new ReactiveProperty<float>()
                {
                    Value = 0,
                },
                FlagsLeft = new ReactiveProperty<int>()
                {
                    Value = settings.MinesCount,
                },
                IsPlaying = new ReactiveProperty<bool>()
                {
                    Value = false,
                },
                IsDead = new ReactiveProperty<bool>()
                {
                    Value = false,
                },
            };
        }

        public void Initialize(GameSettings settings)
        {
            Timer.Value = 0;
            FlagsLeft.Value = settings.MinesCount;
            IsPlaying.Value = false;
            IsDead.Value = false;
        }
    }
}