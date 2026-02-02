using Minesweeper.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Minesweeper.UI
{
    public class WinGamePanel : MonoBehaviour, IInitializable
    {
        [Inject]
        private GameManager _gameManager;
        
        [SerializeField]
        private Button _restartGameButton;
        
        public void Initialize()
        {
            _restartGameButton.onClick.RemoveAllListeners();
            _restartGameButton.onClick.AddListener(() =>
                {
                    gameObject.SetActive(false);
                    _gameManager.RestartGame();
                }
            );
            gameObject.SetActive(false);
        }
    }
}