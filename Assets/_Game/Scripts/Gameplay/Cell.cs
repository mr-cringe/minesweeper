using Minesweeper.Configs;
using Minesweeper.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Minesweeper.Core
{
    public class Cell : MonoBehaviour, IPoolable, IPointerClickHandler
    {
        [Inject]
        private GameSettings _gameSettings;
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

        private CellState _tempState;
        private CellData _cellData;

        public int Index { get; private set; }

        public void OnSpawned()
        {
            _cellData = null;
            _tempState = CellState.Closed;
            UpdateView();
            _button.onClick.AddListener(OnCellClicked);
        }

        public void OnDespawned()
        {
            _button.onClick.RemoveAllListeners();
            _cellData = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_gameState.IsPlaying.Value || _gameState.IsDead.Value)
            {
                return;
            }

            if (eventData.button != PointerEventData.InputButton.Right)
            {
                return;
            }

            var currState = _cellData?.State ?? _tempState;

            switch (currState)
            {
                case CellState.Closed:
                    if (_gameState.FlagsLeft.Value == 0)
                    {
                        return;
                    }

                    _gameState.FlagsLeft.Value--;

                    if (_cellData == null)
                    {
                        _tempState = CellState.Flagged;
                    }
                    else
                    {
                        _cellData.State = CellState.Flagged;
                    }

                    break;
                case CellState.Flagged:
                    _gameState.FlagsLeft.Value++;

                    if (_cellData == null)
                    {
                        _tempState = CellState.Closed;
                    }
                    else
                    {
                        _cellData.State = CellState.Closed;
                    }

                    break;
                default:
                    return;
            }

            UpdateView();
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void InjectData(CellData cellData)
        {
            _cellData = cellData;

            if (_tempState != CellState.Closed)
            {
                _cellData.State = _tempState;
            }

            _countText.text = cellData.MinesCountAround < 1 ? "" : cellData.MinesCountAround.ToString();
            _countText.color = _colorsConfig.GetColor(cellData.MinesCountAround);

            UpdateView();
        }

        public void UpdateView()
        {
            var targetView = (_cellData?.State ?? _tempState) switch
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

            SetViewActive(targetView);

            _button.interactable = !_openedView.activeSelf;
        }

        private void SetViewActive(GameObject targetView)
        {
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

        private void OnCellClicked()
        {
            if (_gameState.IsDead.Value || _gameState.IsWin.Value)
            {
                return;
            }

            if (!_gameState.IsPlaying.Value)
            {
                _gameManager.GenerateCellsData(Index);
                _gameManager.OpenCellsFrom(Index);
                _gameManager.CheckWinCondition();
                return;
            }

            if (_cellData == null || _cellData.State != CellState.Closed)
            {
                return;
            }

            _gameManager.OpenCellsFrom(Index);
            _gameManager.CheckWinCondition();
        }

        public void RevealCell(int detonatedIndex)
        {
            if (_cellData.IsMined)
            {
                if (_cellData.State == CellState.Flagged)
                {
                    return;
                }

                var mineView = Index == detonatedIndex ? _mineRedView : _mineView;
                SetViewActive(mineView);
                return;
            }

            if (_cellData.State == CellState.Flagged)
            {
                SetViewActive(_flaggedWrongView);
                return;
            }

            if (!_gameSettings.RevealNumbersOnGameOver)
            {
                return;
            }

            if (_cellData.State != CellState.Opened)
            {
                SetViewActive(_openedView);
            }
        }
    }
}