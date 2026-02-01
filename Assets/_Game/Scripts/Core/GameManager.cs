using Minesweeper.Core.Data;
using Minesweeper.UI;
using UnityEngine;
using Zenject;
using Random = System.Random;

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

        private Random _rnd = new();

        private void Update()
        {
            if (_gameState.IsPlaying.Value && !_gameState.IsDead.Value)
            {
                _gameState.Timer.Value += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
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

        public void GenerateCellsData(int invokedCellIndex)
        {
            // todo split into methods?
            var fieldSize = _gameState.FieldSize;
            var cellDatas = new CellData[fieldSize.x * fieldSize.y];
            var step = cellDatas.Length / _gameState.MinesCount;
            
            for (var i = 0; i < _gameState.MinesCount; i++)
            {
                var mineIndex = Mathf.Clamp(step * i + _rnd.Next(step), 0, cellDatas.Length - 1);

                if (mineIndex == invokedCellIndex)
                {
                    if (mineIndex == cellDatas.Length - 1)
                    {
                        mineIndex--;
                    }
                    else
                    {
                        mineIndex++;
                    }
                }

                cellDatas[mineIndex] = new CellData(-1);
            }
            
            // todo cell numbers
            for (var i = 0; i < cellDatas.Length; i++)
            {
                if (cellDatas[i] != null)
                {
                    continue;
                }
                cellDatas[i] = new CellData(i % 5);
            }

            cellDatas[invokedCellIndex].State = CellState.Opened;

            _field.InjectCellDatas(cellDatas);

            _gameState.IsPlaying.Value = true;
        }
    }
}