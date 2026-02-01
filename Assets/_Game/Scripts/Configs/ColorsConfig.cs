using UnityEngine;

namespace Minesweeper.Configs
{
    [CreateAssetMenu(fileName = nameof(ColorsConfig), menuName = "Minesweeper/Configs/" + nameof(ColorsConfig))]
    public class ColorsConfig : ScriptableObject
    {
        public Color[] ColorsPerNumber;

        public Color GetColor(int number)
        {
            return ColorsPerNumber[Mathf.Clamp(number, 0, ColorsPerNumber.Length -1)];
        }
    }
}