using Minesweeper.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Minesweeper.UI
{
    public class SettingsPanel : MonoBehaviour, IInitializable
    {
        [Inject]
        private GameSettings _gameSettings;

        [SerializeField]
        private TMP_InputField _columnsInput;
        [SerializeField]
        private TMP_InputField _rowsInput;
        [SerializeField]
        private TMP_InputField _minesInput;
        [SerializeField]
        private Button _closeButton;

        public void Initialize()
        {
            _columnsInput.onValueChanged.RemoveAllListeners();
            _rowsInput.onValueChanged.RemoveAllListeners();
            _minesInput.onValueChanged.RemoveAllListeners();

            _columnsInput.text = _gameSettings.FieldSize.x.ToString();
            _rowsInput.text = _gameSettings.FieldSize.y.ToString();
            _minesInput.text = _gameSettings.MinesCount.ToString();

            _columnsInput.onValueChanged.AddListener(value =>
                {
                    if (int.TryParse(value, out var columns))
                    {
                        _gameSettings.FieldSize.x = columns;
                    }
                    else
                    {
                        _columnsInput.SetTextWithoutNotify(_gameSettings.FieldSize.x.ToString());
                    }
                }
            );
            _rowsInput.onValueChanged.AddListener(value =>
                {
                    if (int.TryParse(value, out var rows))
                    {
                        _gameSettings.FieldSize.y = rows;
                    }
                    else
                    {
                        _columnsInput.SetTextWithoutNotify(_gameSettings.FieldSize.y.ToString());
                    }
                }
            );
            _minesInput.onValueChanged.AddListener(value =>
                {
                    if (int.TryParse(value, out var mines))
                    {
                        _gameSettings.MinesCount = mines;
                    }
                    else
                    {
                        _minesInput.SetTextWithoutNotify(_gameSettings.MinesCount.ToString());
                    }
                }
            );

            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }
}