using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace PonziWorld.MainTabs.TimeAdvancement;

public partial class AdvanceDialog : BaseMetroDialog
{
    public TextChangedEventHandler interestRateChangedEventHandler;

    public AdvanceDialog()
    {
        InitializeComponent();
        interestRateChangedEventHandler = new TextChangedEventHandler(InterestRateChanged);
    }

    private void InterestRateChanged(object sender, TextChangedEventArgs eventArgs)
    {
        if (DataContext is not AdvanceDialogViewModel viewModel)
            return;

        if (eventArgs.Source is not TextBox interestRateTextBox)
            return;

        if (!double.TryParse(interestRateTextBox.Text, out double interestRate))
            return;

        viewModel.InterestRate = interestRate;
    }

    public bool IsSubmitted() =>
        DataContext is AdvanceDialogViewModel viewModel && viewModel.IsSubmitted;

    public double InterestRate()
    {
        if (DataContext is not AdvanceDialogViewModel viewModel)
            return 0d;

        return viewModel.InterestRate;
    }
}
