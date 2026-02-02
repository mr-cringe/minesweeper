using Minesweeper.Utils;
using UnityEngine;

namespace Minesweeper.Core.Data
{
    public class GameState
    {
        public Vector2Int FieldSize;
        public int MinesCount;
        
        public ReactiveProperty<float> Timer;
        public ReactiveProperty<int> FlagsLeft;
        public ReactiveProperty<bool> IsPlaying;
        public ReactiveProperty<bool> IsDead;
        public ReactiveProperty<bool> IsWin;

        private GameState()
        {
            Timer = new ReactiveProperty<float>();
            FlagsLeft = new ReactiveProperty<int>();
            IsPlaying = new ReactiveProperty<bool>();
            IsDead = new ReactiveProperty<bool>();
            IsWin = new ReactiveProperty<bool>();
        }

        public static GameState Create(GameSettings settings)
        {
            var gameState = new GameState();
            gameState.Initialize(settings);

            return gameState;
        }

        public void Initialize(GameSettings settings)
        {
            FieldSize = settings.FieldSize;
            MinesCount = settings.MinesCount;
            
            Timer.Value = 0;
            FlagsLeft.Value = settings.MinesCount;
            IsPlaying.Value = false;
            IsDead.Value = false;
            IsWin.Value = false;
        }
    }
}