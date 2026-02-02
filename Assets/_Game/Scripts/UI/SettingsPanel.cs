using Minesweeper.Core.Data;
using Minesweeper.UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Minesweeper.UI
{
    public class SettingsPanel : MonoBehaviour, IInitializable
    {
        [Inject]
        private GameSettings _gameSettings;
        [Inject]
        private MinMaxValues _minMaxValues;

        [SerializeField]
        private SettingsElement _columns;
        [SerializeField]
        private SettingsElement _rows;
        [SerializeField]
        private SettingsElement _mines;
        [SerializeField]
        private Toggle _revealNumbersOnGameOver;
        [SerializeField]
        private Button _closeButton;

        public void Initialize()
        {
            _columns.Setup(
                _minMaxValues.MinMaxColumns.x,
                _minMaxValues.MinMaxColumns.y,
                _gameSettings.FieldSize.x,
                x => _gameSettings.FieldSize.x = x
            );
            _rows.Setup(
                _minMaxValues.MinMaxRows.x,
                _minMaxValues.MinMaxRows.y,
                _gameSettings.FieldSize.y,
                y => _gameSettings.FieldSize.y = y
            );
            _mines.Setup(
                _minMaxValues.MinMaxMinesCount.x,
                _minMaxValues.MinMaxMinesCount.y,
                _gameSettings.MinesCount,
                x => _gameSettings.MinesCount = x
            );
            
            _revealNumbersOnGameOver.onValueChanged.RemoveAllListeners();
            _revealNumbersOnGameOver.isOn = _gameSettings.RevealNumbersOnGameOver;
            _revealNumbersOnGameOver.onValueChanged.AddListener(value => _gameSettings.RevealNumbersOnGameOver = value);

            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }
}