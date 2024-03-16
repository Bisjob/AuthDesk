using System.Windows.Input;

using AuthDesk.Contracts.Services;
using AuthDesk.Contracts.ViewModels;
using AuthDesk.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Options;

namespace AuthDesk.ViewModels;

// TODO: Change the URL for your privacy policy in the appsettings.json file, currently set to https://YourPrivacyUrlGoesHere
public class SettingsViewModel : ObservableObject, INavigationAware
{
    private readonly AppConfig appConfig;
    private readonly IThemeSelectorService themeSelectorService;
    private readonly ISystemService systemService;
    private readonly IApplicationInfoService applicationInfoService;
    private AppTheme theme;
    private string versionDescription;
    private ICommand setThemeCommand;
    private ICommand privacyStatementCommand;

    public AppTheme Theme
    {
        get { return theme; }
        set { SetProperty(ref theme, value); }
    }

    public string VersionDescription
    {
        get { return versionDescription; }
        set { SetProperty(ref versionDescription, value); }
    }

    public ICommand SetThemeCommand => setThemeCommand ??= new RelayCommand<string>(OnSetTheme);

	public ICommand PrivacyStatementCommand => privacyStatementCommand ??= new RelayCommand(OnPrivacyStatement);

    public SettingsViewModel(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService)
    {
        this.appConfig = appConfig.Value;
        this.themeSelectorService = themeSelectorService;
        this.systemService = systemService;
        this.applicationInfoService = applicationInfoService;
    }

    public void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Properties.Resources.AppDisplayName} - {applicationInfoService.GetVersion()}";
        Theme = themeSelectorService.GetCurrentTheme();
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnSetTheme(string themeName)
    {
        var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
        themeSelectorService.SetTheme(theme);
    }

    private void OnPrivacyStatement()
        => systemService.OpenInWebBrowser(appConfig.PrivacyStatement);
}
