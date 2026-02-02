using System.Collections.Generic;
using Minesweeper.Core.Data;
using Minesweeper.Signals;
using Minesweeper.UI;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Minesweeper.Core
{
    public class GameManager : MonoBehaviour, IInitializable
    {
        [Inject]
        private SignalBus _signalBus;
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

        private Random _rnd;
        private CellData[] _cellDatas;

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

            _rnd = new Random();
        }

        public void RestartGame()
        {
            _cellDatas = null;
            _gameState.Initialize(_gameSettings);
            _field.Setup();
        }
        
        public void WinGame()
        {
            _gameState.IsPlaying.Value = false;
            _gameState.IsWin.Value = true;
        }

        public void GenerateCellsData(int invokedCellIndex)
        {
            _cellDatas = new CellData[_gameState.FieldSize.x * _gameState.FieldSize.y];

            CalculateMines(invokedCellIndex);
            CalculateMinesAroundCount();

            _field.InjectCellDatas(_cellDatas);
            _gameState.IsPlaying.Value = true;
        }

        private void CalculateMines(int invokedCellIndex)
        {
            if (_gameState.MinesCount >= _cellDatas.Length)
            {
                for (var i = 0; i < _cellDatas.Length; i++)
                {
                    _cellDatas[i] = new CellData(i == invokedCellIndex ? 0 : -1);
                }

                return;
            }
            
            var step = _cellDatas.Length / _gameState.MinesCount;

            for (var i = 0; i < _gameState.MinesCount; i++)
            {
                var mineIndex = Mathf.Clamp(step * i + _rnd.Next(step), 0, _cellDatas.Length - 1);

                if (mineIndex == invokedCellIndex)
                {
                    if (mineIndex == _cellDatas.Length - 1)
                    {
                        mineIndex--;
                    }
                    else
                    {
                        mineIndex++;
                    }
                }

                _cellDatas[mineIndex] = new CellData(-1);
            }
        }

        private void CalculateMinesAroundCount()
        {
            var fieldSize = _gameState.FieldSize;

            for (var i = 0; i < _cellDatas.Length; i++)
            {
                var row = i / fieldSize.x;
                var column = i % fieldSize.x;

                if (_cellDatas[i] == null)
                {
                    _cellDatas[i] = new CellData(0);
                    continue;
                }

                if (!_cellDatas[i].IsMined)
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
                        var cellData = _cellDatas[index] ??= new CellData(0);

                        if (cellData.IsMined)
                        {
                            continue;
                        }

                        cellData.IncMinesCountAround();
                    }
                }
            }
        }

        private void GameOver(int cellIndex)
        {
            _gameState.IsDead.Value = true;
            _field.ForEachCell(cell => cell.RevealCell(cellIndex));
        }

        public void OpenCellsFrom(int cellIndex, bool updateView = true)
        {
            var openedCellData = _cellDatas[cellIndex];
            
            if (openedCellData.IsMined)
            {
                GameOver(cellIndex);
                return;
            }

            if (openedCellData.MinesCountAround > 0)
            {
                _cellDatas[cellIndex].State = CellState.Opened;
                _field.UpdateCellsView();
                return;
            }

            var fieldSize = _gameState.FieldSize;
            var row = cellIndex / fieldSize.x;
            var column = cellIndex % fieldSize.x;
            var chainOpenCells = new HashSet<int>();

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
                    var cellData = _cellDatas[index] ??= new CellData(0);

                    if (cellData.IsMined)
                    {
                        // just in case
                        continue;
                    }

                    if (cellData.State == CellState.Flagged)
                    {
                        continue;
                    }
                    
                    if (cellData.State != CellState.Opened)
                    {
                        cellData.State = CellState.Opened;
                        
                        if (cellData.MinesCountAround == 0)
                        {
                            chainOpenCells.Add(index);
                        }
                    }
                }
            }

            foreach (var chainOpenCell in chainOpenCells)
            {
                OpenCellsFrom(chainOpenCell, false);
            }

            if (updateView)
            {
                _field.UpdateCellsView();
            }
        }

        public void CheckWinCondition()
        {
            var hasClosedCells = false;
            
            foreach (var cellData in _cellDatas)
            {
                if (!cellData.IsMined && cellData.State != CellState.Opened)
                {
                    hasClosedCells = true;
                    break;
                }
            }

            if (!hasClosedCells)
            {
                _signalBus.Fire<WinSignal>();
            }
        }
    }
}