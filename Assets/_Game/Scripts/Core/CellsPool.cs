using Zenject;

namespace Minesweeper.Core
{
    public class CellsPool : MonoPoolableMemoryPool<Cell>
    {
        protected override void OnCreated(Cell item)
        {
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(Cell item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Cell item)
        {
            item.gameObject.SetActive(false);
        }
    }
}