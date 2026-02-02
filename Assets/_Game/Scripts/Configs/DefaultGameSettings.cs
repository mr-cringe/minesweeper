using Minesweeper.Core.Data;
using UnityEngine;

namespace Minesweeper.Configs
{
    [CreateAssetMenu(fileName = nameof(DefaultGameSettings), menuName = "Minesweeper/Configs/" + nameof(DefaultGameSettings))]
    public class DefaultGameSettings : ScriptableObject
    {
        public GameSettings DefaultSettings;
        public MinMaxValues MinMaxValues;
    }
}