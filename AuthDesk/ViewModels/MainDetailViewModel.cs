using AuthDesk.Contracts.ViewModels;
using AuthDesk.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AuthDesk.ViewModels;

public class MainDetailViewModel : ObservableObject, INavigationAware
{
    private CodeEntry item;

    public CodeEntry Item
    {
        get { return item; }
        set { SetProperty(ref item, value); }
    }

    public MainDetailViewModel()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string entryId)
        {
            var data = App.Current.Properties["entries"] as CodeEntries;
            if (data != null)
            {
                Item = data.Entries.FirstOrDefault(i => i.Id == entryId);
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
