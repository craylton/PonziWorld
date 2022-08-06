using MahApps.Metro.Controls.Dialogs;
using PonziWorld.Company;
using PonziWorld.DataRegion.PerformanceHistoryTab;
using PonziWorld.DataRegion.TimeAdvancement;
using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using Prism.Ioc;
using Prism.Unity;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace PonziWorld;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<Window, MainWindow.MainWindow>();
        containerRegistry.Register<IInvestorsRepository, MongoDbInvestorsRepository>();
        containerRegistry.Register<ICompanyRepository, MongoDbCompanyRepository>();
        containerRegistry.Register<IInvestmentsRepository, MongoDbInvestmentsRepository>();
        containerRegistry.Register<IPerformanceHistoryRepository, MongoDbPerformanceHistoryRepository>();
        containerRegistry.Register<IDialogCoordinator, DialogCoordinator>();
        containerRegistry.Register<ITimeAdvancementCoordinator, TimeAdvancementCoordinator>();
    }

    protected override void Initialize()
    {
        // Make it use local currency instead of default $
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));

        base.Initialize();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();
        eventAggregator.GetEvent<MainWindowInitialisedEvent>().Publish();
    }

    protected override Window CreateShell() =>
        Container.Resolve<Window>();
}
