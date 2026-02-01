using Minesweeper.Utils;
using Zenject;

namespace Minesweeper.Bootstrap
{
    public class ApplicationEntryPoint : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            var projectContextContainer = ProjectContext.Instance.Container;
            SignalBusInstaller.Install(projectContextContainer);
            
            projectContextContainer.Bind<SceneService>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ApplicationEntryPoint>().FromInstance(this).AsSingle().NonLazy();
        }

        public void Initialize()
        {
            Container.UnbindInterfacesTo<ApplicationEntryPoint>();
            Container.Unbind<ApplicationEntryPoint>();

            ProjectContext.Instance.Container.Resolve<SceneService>().LoadGameScene(null);
        }
    }
}