using System.Windows.Controls;

using AuthDesk.ViewModels;

namespace AuthDesk.Views;

public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
