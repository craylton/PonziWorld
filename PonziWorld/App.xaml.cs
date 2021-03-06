using MahApps.Metro.Controls.Dialogs;
using PonziWorld.Company;
using PonziWorld.DataRegion.TimeAdvancement;
using PonziWorld.Events;
using PonziWorld.Investments.Investors;
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
        containerRegistry.Register<IInvestorsRepository, MongoDbInvestorsRepository>();
        containerRegistry.Register<ICompanyRepository, MongoDbCompanyRepository>();
        containerRegistry.Register<IDialogCoordinator, DialogCoordinator>();
        containerRegistry.Register<ITimeAdvancementCoordinator, TimeAdvancementCoordinator>();
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
