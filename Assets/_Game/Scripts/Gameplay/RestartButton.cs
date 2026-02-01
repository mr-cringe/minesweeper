using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.Core
{
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private GameObject _faceSmileState;
        [SerializeField]
        private GameObject _faceDeadState;

        public Button.ButtonClickedEvent onClick => _button.onClick;

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        public void SetDead(bool isDead)
        {
            _faceSmileState.SetActive(!isDead);
            _faceDeadState.SetActive(isDead);
        }
    }
}