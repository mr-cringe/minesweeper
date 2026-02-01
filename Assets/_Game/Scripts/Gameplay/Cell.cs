using Minesweeper.Configs;
using Minesweeper.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Minesweeper.Core
{
    public class Cell : MonoBehaviour, IPoolable
    {
        [Inject]
        private GameState _gameState;
        [Inject]
        private GameManager _gameManager;
        [Inject]
        private ColorsConfig _colorsConfig;

        [SerializeField]
        private GameObject _closedView;
        [SerializeField]
        private GameObject _openedView;
        [SerializeField]
        private TMP_Text _countText;
        [SerializeField]
        private GameObject _flaggedView;
        [SerializeField]
        private GameObject _flaggedWrongView;
        [SerializeField]
        private GameObject _mineView;
        [SerializeField]
        private GameObject _mineRedView;
        [SerializeField]
        private Button _button;

        public CellData CellData { get; private set; }
        public int Index { get; private set; }

        public void OnSpawned()
        {
            CellData = null;
            UpdateView();
            _button.onClick.AddListener(OpenCell);
        }

        public void OnDespawned()
        {
            _button.onClick.RemoveAllListeners();
            CellData = null;
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void InjectData(CellData cellData)
        {
            CellData = cellData;
            _countText.text = cellData.MinesCountAround < 1 ? "" : cellData.MinesCountAround.ToString();
            _countText.color = _colorsConfig.GetColor(cellData.MinesCountAround);
            
            UpdateView();
        }

        private void UpdateView()
        {
            var targetView = (CellData?.State ?? CellState.Closed) switch
            {
                CellState.Closed => _closedView,
                CellState.Opened => _openedView,
                CellState.Flagged => _flaggedView,
                CellState.FlaggedWrong => _flaggedWrongView,
                CellState.Mine => _mineView,
                CellState.MineRed => _mineRedView,
                _ => null,
            };

            if (targetView == null)
            {
                // wtf?
                return;
            }

            SetViewActiveIfMatch(_closedView, targetView);
            SetViewActiveIfMatch(_openedView, targetView);
            SetViewActiveIfMatch(_flaggedView, targetView);
            SetViewActiveIfMatch(_flaggedWrongView, targetView);
            SetViewActiveIfMatch(_mineView, targetView);
            SetViewActiveIfMatch(_mineRedView, targetView);
        }

        private void SetViewActiveIfMatch(GameObject view, GameObject targetView)
        {
            view.SetActive(view == targetView);
        }

        private void OpenCell()
        {
            if (!_gameState.IsPlaying.Value)
            {
                _gameManager.GenerateCellsData(Index);
            }
            
            // todo
        }
    }
}