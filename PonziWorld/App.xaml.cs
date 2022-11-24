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
using Serilog;
using System;
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
        // Set up global unhandled exceptions
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleGlobalException);

        // Make it use local currency instead of default $
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));

        // Set up logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        base.Initialize();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Log.Logger.Information("=======================");
        Log.Logger.Information("Application initialised");
        Log.Logger.Information("=======================");

        IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();
        eventAggregator.GetEvent<MainWindowInitialisedEvent>().Publish();
    }

    protected override Window CreateShell() =>
        Container.Resolve<Window>();

    static void HandleGlobalException(object sender, UnhandledExceptionEventArgs args)
    {
        Exception ex = (Exception)args.ExceptionObject;
        Log.Logger.Error(ex.ToString());
    }
}
