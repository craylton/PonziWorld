using MahApps.Metro.Controls;

namespace PonziWorld.MainWindow;

public partial class MainWindow : MetroWindow
{
    public MainWindow(IMainWindowVM viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
