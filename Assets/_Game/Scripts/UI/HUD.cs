using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Minesweeper.UI
{
    public class HUD : MonoBehaviour, IInitializable
    {
        [Inject]
        private SettingsPanel _settingsPanel;

        [SerializeField]
        private Button _settingsButton;

        public void Initialize()
        {
            _settingsButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.AddListener(() => _settingsPanel.gameObject.SetActive(true));
        }
    }
}