using AuthDesk.Contracts.Services;
using AuthDesk.Contracts.ViewModels;
using AuthDesk.Core.Contracts.Services;
using AuthDesk.Models;
using AuthDesk.Properties;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace AuthDesk.ViewModels;

public class MainViewModel : ObservableObject, INavigationAware, IDisposable
{
    private readonly IDialogCoordinator dialogCoordinator;
    private readonly INavigationService navigationService;
	private readonly IJsonImporter jsonImporter;
	private readonly ICodeEntriesService codeEntriesService;
	private ICommand navigateToDetailCommand;
	private ICommand navigateToAddEntryCommand;
	private ICommand deleteEntryCommand;

	public ICommand NavigateToDetailCommand 
        => navigateToDetailCommand ??= new RelayCommand<CodeEntryContext>(NavigateToDetail);
    public ICommand NavigateToAddEntryCommand
        => navigateToAddEntryCommand ??= new RelayCommand(NavigateToAddEntry);
	public ICommand DeleteEntryCommand
		=> deleteEntryCommand ??= new RelayCommand<CodeEntryContext>(DeleteEntry);

    public ObservableCollection<CodeEntryContext> Source { get; } = [];

	public ObservableCollection<SplitButtonCommandItem> ImportOptions { get; }

    private readonly DispatcherTimer timer;

	public MainViewModel(
        INavigationService navigationService,
        IJsonImporter jsonImporter,
        ICodeEntriesService codeEntriesService)
    {
        this.dialogCoordinator = DialogCoordinator.Instance;
        this.navigationService = navigationService;
		this.jsonImporter = jsonImporter;
		this.codeEntriesService = codeEntriesService;

        this.jsonImporter.OnAskForPassword += AskForPassword;

        ImportOptions =
        [
            new SplitButtonCommandItem(Resources.MainPageImportAegisJson, new RelayCommand(ImportAegisJson))
        ];

        timer = new DispatcherTimer
		{
			Interval = TimeSpan.FromSeconds(30)
		};
		timer.Tick += Timer_Tick;
		timer.Start();
	}

    public void OnNavigatedTo(object parameter)
    {
        RefreshSource();
    }

    public void OnNavigatedFrom()
    {
	}
	private void Timer_Tick(object sender, EventArgs e)
	{
		foreach (var entry in Source)
		{
			entry.GenerateNewCode();
		}
	}

	private void RefreshSource()
    {
        Source.Clear();
		foreach (var item in codeEntriesService.Entries.Entries)
			Source.Add(new CodeEntryContext(item));
	}

    private void NavigateToDetail(CodeEntryContext entry)
    {
        navigationService.NavigateTo(typeof(MainDetailViewModel).FullName, entry.Id);
	}
	private void NavigateToAddEntry()
	{
		navigationService.NavigateTo(typeof(AddEntryViewModel).FullName);
	}

    private async void ImportAegisJson()
    {
        var openFileDialog = new OpenFileDialog()
        {
			Filter = "json Files|*.json"
	    };
		if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var items = await jsonImporter.OpenJsonAegis(openFileDialog.FileName);

                foreach (var item in items)
                    codeEntriesService.Entries.Entries.Add(item);
                codeEntriesService.SaveData();
                RefreshSource();
            }
            catch (Exception ex)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Exception", ex.ToString());
            }
		}
	}

	private void DeleteEntry(CodeEntryContext entry)
	{
		codeEntriesService.Entries.Entries.Remove(entry.Entry);
		codeEntriesService.SaveData();
		RefreshSource();
	}

    private async Task AskForPassword(object sender, AsyncPasswordEventArgs e)
    {
        e.Password = await dialogCoordinator.ShowInputAsync(this, "Password required", "The file is protected by a password");
    }

    public void Dispose()
    {
        timer.Tick -= Timer_Tick;
        timer.Stop();

        jsonImporter.OnAskForPassword -= AskForPassword;
    }
}
