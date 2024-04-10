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
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace AuthDesk.ViewModels;

public class MainViewModel : ObservableObject, INavigationAware, IDisposable
{
    private readonly IDialogCoordinator dialogCoordinator;
    private readonly INavigationService navigationService;
	private readonly IJsonImporter jsonImporter;
	private readonly ICodeEntriesService codeEntriesService;
	private ICommand navigateToAddEntryCommand;
	private ICommand deleteEntryCommand;
    private ICommand setGroupFilterCommand;
    private ICommand unselectEntryCommand;

    public ICommand NavigateToAddEntryCommand
        => navigateToAddEntryCommand ??= new RelayCommand(NavigateToAddEntry);
	public ICommand DeleteEntryCommand
		=> deleteEntryCommand ??= new RelayCommand<CodeEntryContext>(DeleteEntry);
    public ICommand SetGroupFilterCommand
        => setGroupFilterCommand ??= new RelayCommand<string>(SetGroupFilter);
    public ICommand UnselectEntryCommand
        => unselectEntryCommand ??= new RelayCommand(UnSelectEntry);

    public ObservableCollection<CodeEntryContext> Entries { get; } = [];
    public ICollectionView AllEntries => allEntries.View;

    private CollectionViewSource allEntries { get; set; }

    public ObservableCollection<SplitButtonCommandItem> ImportOptions { get; }

    public CodeEntryContext SelectedEntryCtx
    {
        get { return selectedEntryCtx; }
        set { SetProperty(ref selectedEntryCtx, value); }
    }

    public ObservableCollection<string> Groups { get; } = [];

    public string FilterName
    {
        get => filterName;
        set
        {
            if (value == filterName) return;
            SetProperty(ref filterName, value);
            AllEntries.Refresh();
        }
    }

    public string SelectedGroup
    {
        get => selectedGroup;
        set
        {
            if (value == selectedGroup) return;
            SetProperty(ref selectedGroup, value);
            AllEntries.Refresh();
        }
    }

    private CodeEntryContext selectedEntryCtx;
    private string filterName;
    private string selectedGroup = SelectedGroupEmpty;
    private const string SelectedGroupEmpty = "Filter by group..";

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

        allEntries = new CollectionViewSource
        {
            Source = Entries
        };
        allEntries.Filter += ApplyFilter;
        
        ImportOptions =
        [
            new SplitButtonCommandItem(Resources.MainPageImportAegisJson, new RelayCommand(ImportAegisJson))
        ];
	}

    public void OnNavigatedTo(object parameter)
    {
        RefreshSource();
    }

    public void OnNavigatedFrom()
    {
	}

	private void RefreshSource()
    {
        Entries.Clear();
		foreach (var item in codeEntriesService.Entries.Entries)
            Entries.Add(new CodeEntryContext(item));

        Groups.Clear();
        Groups.Add(SelectedGroupEmpty);
        var groups = Entries.Select(ctx => ctx.Entry.Group).Distinct().ToList();
        foreach (var group in groups)
            Groups.Add(group);
    }
    void ApplyFilter(object sender, FilterEventArgs e)
    {
        if (e.Item is not CodeEntryContext ctx)
        {
            e.Accepted = false;
            return;
        }

        if (!string.IsNullOrEmpty(FilterName) && !ctx.Entry.Name.Contains(FilterName, StringComparison.OrdinalIgnoreCase))
        {
            e.Accepted = false;
            return;
        }

        if (!string.IsNullOrEmpty(SelectedGroup) && SelectedGroup != SelectedGroupEmpty && ctx.Entry.Group != SelectedGroup)
        {
            e.Accepted = false;
            return;
        }

        e.Accepted = true;
    }

    private void SetGroupFilter(string group)
    {
        SelectedGroup = group;
    }
    private void UnSelectEntry()
    {
        SelectedEntryCtx = null;
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

	private async void DeleteEntry(CodeEntryContext ctx)
	{
        var result = await dialogCoordinator.ShowMessageAsync(
            this, 
            "Confirm delete", 
            $"Are you sur you want to delete the entry {ctx.Entry.Issuer}", 
            MessageDialogStyle.AffirmativeAndNegative,
            new MetroDialogSettings()
            {
                AffirmativeButtonText = Resources.DialogConfirmYesButton,
                NegativeButtonText = Resources.DialogConfirmNoButton,
            });

        if (result != MessageDialogResult.Affirmative) return;

        codeEntriesService.Entries.Entries.Remove(ctx.Entry);
		codeEntriesService.SaveData();
		RefreshSource();
	}

    private async Task AskForPassword(object sender, AsyncPasswordEventArgs e)
    {
        e.Password = await dialogCoordinator.ShowInputAsync(this, "Password required", "The file is protected by a password");
    }

    public void Dispose()
    {
        jsonImporter.OnAskForPassword -= AskForPassword;
    }
}
