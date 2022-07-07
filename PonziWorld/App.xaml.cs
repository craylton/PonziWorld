using PonziWorld.MainWindow;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace PonziWorld;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterInstance(Container);

        containerRegistry.Register<Window, MainWindow.MainWindow>(WindowDefinitions.MainWindow);
        containerRegistry.Register<IMainWindowVM, MainWindowVM>();
    }

    protected override Window CreateShell() =>
        Container.Resolve<Window>(WindowDefinitions.MainWindow);
}
