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
            var cellDatas = new CellData[_gameState.FieldSize.x * _gameState.FieldSize.y];
            
            CalculateMines(invokedCellIndex, cellDatas);
            CalculateMinesAroundCount(cellDatas);

            cellDatas[invokedCellIndex].State = CellState.Opened;

            _field.InjectCellDatas(cellDatas);

            _gameState.IsPlaying.Value = true;
        }

        private void CalculateMines(int invokedCellIndex, CellData[] cellDatas)
        {
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
        }

        private void CalculateMinesAroundCount(CellData[] cellDatas)
        {
            var fieldSize = _gameState.FieldSize;
            
            for (var i = 0; i < cellDatas.Length; i++)
            {
                var row = i / fieldSize.x;
                var column = i % fieldSize.x;

                if (cellDatas[i] == null)
                {
                    cellDatas[i] = new CellData(0);
                    continue;
                }
                
                if (!cellDatas[i].IsMined)
                {
                    continue;
                }

                for (var r = row - 1; r < row + 2; r++)
                {
                    if (r < 0 || r >= fieldSize.y)
                    {
                        continue;
                    }
                    
                    for (var c = column - 1; c < column + 2; c++)
                    {
                        if (c < 0 || c >= fieldSize.x)
                        {
                            continue;
                        }

                        var index = r * fieldSize.x + c;
                        var cellData = cellDatas[index] ??= new CellData(0);

                        if (cellData.IsMined)
                        {
                            continue;
                        }
                        
                        cellData.IncMinesCountAround();
                    }
                }
            }
        }
    }
}