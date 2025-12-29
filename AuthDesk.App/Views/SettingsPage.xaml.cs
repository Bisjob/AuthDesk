using System.Windows.Controls;

using AuthDesk.ViewModels;

namespace AuthDesk.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
