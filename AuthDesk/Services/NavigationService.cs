using System.Windows.Controls;
using System.Windows.Navigation;

using AuthDesk.Contracts.Services;
using AuthDesk.Contracts.ViewModels;

namespace AuthDesk.Services;

public class NavigationService : INavigationService
{
    private readonly IPageService pageService;
    private Frame frame;
    private object lastParameterUsed;

    public event EventHandler<string> Navigated;

    public bool CanGoBack => frame.CanGoBack;

    public NavigationService(IPageService pageService)
    {
        this.pageService = pageService;
    }

    public void Initialize(Frame shellFrame)
    {
        if (frame == null)
        {
            frame = shellFrame;
            frame.Navigated += OnNavigated;
        }
    }

    public void UnsubscribeNavigation()
    {
        frame.Navigated -= OnNavigated;
        frame = null;
    }

    public void GoBack()
    {
        if (frame.CanGoBack)
        {
            var vmBeforeNavigation = frame.GetDataContext();
            frame.GoBack();
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }
        }
    }

    public bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false)
    {
        var pageType = pageService.GetPageType(pageKey);

        if (frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(lastParameterUsed)))
        {
            frame.Tag = clearNavigation;
            var page = pageService.GetPage(pageKey);
            var navigated = frame.Navigate(page, parameter);
            if (navigated)
            {
                lastParameterUsed = parameter;
                var dataContext = frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }

    public void CleanNavigation()
        => frame.CleanNavigation();

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            bool clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.CleanNavigation();
            }

            var dataContext = frame.GetDataContext();
            if (dataContext is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.ExtraData);
            }

            Navigated?.Invoke(sender, dataContext.GetType().FullName);
        }
    }
}
