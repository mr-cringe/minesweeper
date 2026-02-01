using System.Collections.Generic;
using Minesweeper.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Minesweeper.Core
{
    public class Field : MonoBehaviour, IInitializable
    {
        [Inject]
        private CellsPool _cellsPool;
        [Inject]
        private GameSettings  _gameSettings;
        
        [Header("Cells")]
        [SerializeField]
        private RectTransform _panelRect;
        [SerializeField]
        private GridLayoutGroup _gridLayout;
        [SerializeField]
        private RectTransform _cellsContainer;

        private bool _isInitialized;
        private bool _isStartCompleted;
        private HashSet<Cell> _cells = new();

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

        public void Setup()
        {
            DespawnAllCells();
            SetupPanelRectSize(_gameSettings.FieldSize);
            SpawnCells(_gameSettings.FieldSize);
        }

        private void SetupPanelRectSize(Vector2Int fieldSize)
        {
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = fieldSize.x;

            _panelRect.sizeDelta = new Vector2(
                _gridLayout.padding.left + _gridLayout.padding.right + _gridLayout.cellSize.x * fieldSize.x,
                _gridLayout.padding.top + _gridLayout.padding.bottom + _gridLayout.cellSize.y * fieldSize.y
            );
        }

        private void DespawnAllCells()
        {
            foreach (var cell in _cells)
            {
                _cellsPool.Despawn(cell);
            }

            _cells.Clear();
        }

        private void SpawnCells(Vector2Int fieldSize)
        {
            var totalCells = fieldSize.x * fieldSize.y;
            for (var i = 0; i < totalCells; i++)
            {
                var cell = _cellsPool.Spawn();
                _cells.Add(cell);

                cell.SetIndex(i);
                cell.transform.SetParent(_cellsContainer);
                cell.transform.localScale = Vector3.one;
            }
        }

        public void InjectCellDatas(CellData[] cellDatas)
        {
            foreach (var cell in _cells)
            {
                cell.InjectData(cellDatas[cell.Index]);
            }
        }
    }
}