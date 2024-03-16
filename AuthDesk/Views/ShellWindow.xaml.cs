using System.Windows.Controls;

using AuthDesk.Contracts.Views;
using AuthDesk.ViewModels;

using MahApps.Metro.Controls;

namespace AuthDesk.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame()
        => shellFrame;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
