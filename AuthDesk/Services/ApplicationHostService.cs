using AuthDesk.Contracts.Activation;
using AuthDesk.Contracts.Services;
using AuthDesk.Contracts.Views;
using AuthDesk.ViewModels;

using Microsoft.Extensions.Hosting;

namespace AuthDesk.Services;

public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private readonly INavigationService navigationService;
    private readonly IPersistAndRestoreService persistAndRestoreService;
    private readonly IThemeSelectorService themeSelectorService;
    private readonly IEnumerable<IActivationHandler> activationHandlers;
    private IShellWindow shellWindow;
    private bool isInitialized;

    public ApplicationHostService(IServiceProvider serviceProvider, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService, IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService)
    {
        this.serviceProvider = serviceProvider;
        this.activationHandlers = activationHandlers;
        this.navigationService = navigationService;
        this.themeSelectorService = themeSelectorService;
        this.persistAndRestoreService = persistAndRestoreService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize services that you need before app activation
        await InitializeAsync();

        await HandleActivationAsync();

        // Tasks after activation
        await StartupAsync();
        isInitialized = true;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        persistAndRestoreService.PersistData();
        await Task.CompletedTask;
    }

    private async Task InitializeAsync()
    {
        if (!isInitialized)
        {
            persistAndRestoreService.RestoreData();
            themeSelectorService.InitializeTheme();
            await Task.CompletedTask;
        }
    }

    private async Task StartupAsync()
    {
        if (!isInitialized)
        {
            await Task.CompletedTask;
        }
    }

    private async Task HandleActivationAsync()
    {
        var activationHandler = activationHandlers.FirstOrDefault(h => h.CanHandle());

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync();
        }

        await Task.CompletedTask;

        if (App.Current.Windows.OfType<IShellWindow>().Count() == 0)
        {
            // Default activation that navigates to the apps default page
            shellWindow = serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
            navigationService.Initialize(shellWindow.GetNavigationFrame());
            shellWindow.ShowWindow();
            navigationService.NavigateTo(typeof(MainViewModel).FullName);
            await Task.CompletedTask;
        }
    }
}
