using UnityEngine;
using Zenject;

namespace Minesweeper.Core
{
    public class Cell : MonoBehaviour, IPoolable
    {
        public void OnSpawned()
        {
        }
        
        public void OnDespawned()
        {
        }
    }
}