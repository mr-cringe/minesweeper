using Minesweeper.Core.Data;
using Minesweeper.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace Minesweeper.Core
{
    public class GameViewHeader : MonoBehaviour, IInitializable
    {
        [Inject]
        private SignalBus _signalBus;
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private TMP_Text _flagsLeftText;
        [SerializeField]
        private TMP_Text _timerText;
        [SerializeField]
        private RestartButton _restartButton;

        private bool _isInitialized;
        private bool _isStartCompleted;

        public void Initialize()
        {
            _isInitialized = true;

            if (_isStartCompleted)
            {
                Setup();
            }
        }

        private void Start()
        {
            _isStartCompleted = true;

            if (_isInitialized)
            {
                Setup();
            }
        }

        private void Setup()
        {
            _gameState.FlagsLeft.Unsubscribe(SetFlagsLeftText);
            _gameState.FlagsLeft.Subscribe(SetFlagsLeftText);
            _gameState.Timer.Unsubscribe(SetTimerText);
            _gameState.Timer.Subscribe(SetTimerText);
            _gameState.IsDead.Unsubscribe(_restartButton.SetDead);
            _gameState.IsDead.Subscribe(_restartButton.SetDead);

            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(() => _signalBus.Fire<RestartGameSignal>());
        }

        private void SetFlagsLeftText(int value)
        {
            _flagsLeftText.text = value.ToString();
        }

        private void SetTimerText(float value)
        {
            _timerText.text = Mathf.FloorToInt(value).ToString();
        }
    }
}