using Minesweeper.Configs;
using Minesweeper.Core;
using Minesweeper.Core.Data;
using Minesweeper.Signals;
using Minesweeper.UI;
using UnityEngine;
using Zenject;

namespace Minesweeper.Bootstrap
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private ColorsConfig _colorsConfig;
        [SerializeField]
        private HUD _hud;
        [SerializeField]
        private SettingsPanel _settingsPanel;
        [SerializeField]
        private GameManager _gameManager;
        [SerializeField]
        private GameViewHeader _gameViewHeader;
        [SerializeField]
        private Field _field;
        [SerializeField]
        private Cell _cellPrefab;

        public override void InstallBindings()
        {
            Container.DeclareSignal<RestartGameSignal>();
            Container.BindSignal<RestartGameSignal>().ToMethod(() => _gameManager.RestartGame());

            Container.Bind<ColorsConfig>().FromInstance(_colorsConfig).AsSingle().NonLazy();
            
            // todo move gamesettings into project context??
            var gameSettings = GameSettings.CreateDefault();
            Container.Bind<GameSettings>().FromInstance(gameSettings).AsSingle().NonLazy();
            Container.Bind<GameState>().FromMethod(() => GameState.Create(gameSettings)).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<HUD>().FromInstance(_hud).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SettingsPanel>().FromInstance(_settingsPanel).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameViewHeader>().FromInstance(_gameViewHeader).AsSingle().NonLazy();
            Container
                .BindMemoryPool<Cell, CellsPool>()
                .WithInitialSize(100)
                .ExpandByDoubling()
                .FromComponentInNewPrefab(_cellPrefab)
                .UnderTransformGroup("Cells Pool")
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<Field>().FromInstance(_field).AsSingle().NonLazy();
        }
    }
}