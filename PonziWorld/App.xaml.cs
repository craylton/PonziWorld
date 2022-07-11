using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace PonziWorld;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry) =>
        containerRegistry.Register<Window, MainWindow.MainWindow>();

    protected override Window CreateShell() =>
        Container.Resolve<Window>();
}
