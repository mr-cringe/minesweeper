using Minesweeper.Core.Data;
using Minesweeper.UI;
using UnityEngine;
using Zenject;

namespace Minesweeper.Core
{
    public class GameManager : MonoBehaviour, IInitializable
    {
        [Inject]
        private GameSettings _gameSettings;
        [Inject]
        private GameState _gameState;
        [Inject]
        private HUD _hud;
        [Inject]
        private SettingsPanel _settingsPanel;
        [Inject]
        private Field _field;

        private void Update()
        {
            if (_gameState.IsPlaying.Value && !_gameState.IsDead.Value)
            {
                _gameState.Timer.Value += Time.deltaTime;
            }
        }

        public void Initialize()
        {
            _hud.gameObject.SetActive(true);
            _settingsPanel.gameObject.SetActive(false);
        }

        public void RestartGame()
        {
            _gameState.Initialize(_gameSettings);
            _field.Setup();
        }
    }
}