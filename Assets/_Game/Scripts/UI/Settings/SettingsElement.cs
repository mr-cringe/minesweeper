using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.UI.Settings
{
    public class SettingsElement : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _input;
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private TMP_Text _sliderMinValue;
        [SerializeField]
        private TMP_Text _sliderMaxValue;

        private int _min;
        private int _max;
        private int _current;
        
        public void Setup(int min, int max, int current, Action<int> onValueChanged)
        {
            _min = min;
            _max = max;
            _current = current;
            
            _input.onValueChanged.RemoveAllListeners();
            _slider.onValueChanged.RemoveAllListeners();
            
            _sliderMinValue.text = _min.ToString();
            _sliderMaxValue.text = _max.ToString();
            _input.SetTextWithoutNotify(_current.ToString());
            SetSliderFillByValue(_current);
            
            _input.onValueChanged.AddListener(text =>
            {
                if (int.TryParse(text, out var value))
                {
                    _current = Clamp(value);
                    _input.SetTextWithoutNotify(_current.ToString());
                    SetSliderFillByValue(_current);
                    onValueChanged?.Invoke(_current);
                }
                else
                {
                    _input.SetTextWithoutNotify(_current.ToString());
                }
            });
            _slider.onValueChanged.AddListener(progress =>
            {
                _current = Clamp(Mathf.RoundToInt(_min + (_max - _min) * progress));
                _input.SetTextWithoutNotify(_current.ToString());
                onValueChanged?.Invoke(_current);
            });
        }

        private void SetSliderFillByValue(int value)
        {
            _slider.SetValueWithoutNotify((float) (value - _min) / (_max - _min));
        }

        private int Clamp(int value)
        {
            return Mathf.Clamp(value, _min, _max);
        }
    }
}