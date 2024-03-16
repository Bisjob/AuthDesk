using System.Collections.ObjectModel;
using System.Windows.Input;

using AuthDesk.Contracts.Services;
using AuthDesk.Properties;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls;

namespace AuthDesk.ViewModels;

public class ShellViewModel : ObservableObject
{
    private readonly INavigationService navigationService;
    private HamburgerMenuItem selectedMenuItem;
    private HamburgerMenuItem selectedOptionsMenuItem;
    private RelayCommand goBackCommand;
    private ICommand menuItemInvokedCommand;
    private ICommand optionsMenuItemInvokedCommand;
    private ICommand loadedCommand;
    private ICommand unloadedCommand;

    public HamburgerMenuItem SelectedMenuItem
    {
        get { return selectedMenuItem; }
        set { SetProperty(ref selectedMenuItem, value); }
    }

    public HamburgerMenuItem SelectedOptionsMenuItem
    {
        get { return selectedOptionsMenuItem; }
        set { SetProperty(ref selectedOptionsMenuItem, value); }
    }

    // TODO: Change the icons and titles for all HamburgerMenuItems here.
    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellMainPage, Glyph = "\uE8A5", TargetPageType = typeof(MainViewModel) },
    };

    public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", TargetPageType = typeof(SettingsViewModel) }
    };

    public RelayCommand GoBackCommand => goBackCommand ?? (goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

    public ICommand MenuItemInvokedCommand => menuItemInvokedCommand ?? (menuItemInvokedCommand = new RelayCommand(OnMenuItemInvoked));

    public ICommand OptionsMenuItemInvokedCommand => optionsMenuItemInvokedCommand ?? (optionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked));

    public ICommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(OnLoaded));

    public ICommand UnloadedCommand => unloadedCommand ?? (unloadedCommand = new RelayCommand(OnUnloaded));

    public ShellViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    private void OnLoaded()
    {
        navigationService.Navigated += OnNavigated;
    }

    private void OnUnloaded()
    {
        navigationService.Navigated -= OnNavigated;
    }

    private bool CanGoBack()
        => navigationService.CanGoBack;

    private void OnGoBack()
        => navigationService.GoBack();

    private void OnMenuItemInvoked()
        => NavigateTo(SelectedMenuItem.TargetPageType);

    private void OnOptionsMenuItemInvoked()
        => NavigateTo(SelectedOptionsMenuItem.TargetPageType);

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            navigationService.NavigateTo(targetViewModel.FullName);
        }
    }

    private void OnNavigated(object sender, string viewModelName)
    {
        var item = MenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        if (item != null)
        {
            SelectedMenuItem = item;
        }
        else
        {
            SelectedOptionsMenuItem = OptionMenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        }

        GoBackCommand.NotifyCanExecuteChanged();
    }
}
