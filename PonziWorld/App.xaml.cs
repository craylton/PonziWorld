using PonziWorld.Bootstrapping;
using PonziWorld.ExistingInvestors;
using Prism.Events;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace PonziWorld;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<Window, MainWindow.MainWindow>();
        containerRegistry.Register<IInvestorsRepository, InvestorsRepository>();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        var eventAggregator = Container.Resolve<IEventAggregator>();
        eventAggregator.GetEvent<MainWindowInitialisedEvent>().Publish();
    }

    protected override Window CreateShell() =>
        Container.Resolve<Window>();
}
