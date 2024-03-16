using AuthDesk.ViewModels;
using System.Windows.Controls;

namespace AuthDesk.Views
{
	/// <summary>
	/// Logique d'interaction pour AddEntryPage.xaml
	/// </summary>
	public partial class AddEntryPage : Page
	{
		public AddEntryPage(AddEntryViewModel viewModel)
		{
			InitializeComponent();
			this.DataContext = viewModel;
		}
	}
}
