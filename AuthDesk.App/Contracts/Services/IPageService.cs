using System.Windows.Controls;

namespace AuthDesk.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
