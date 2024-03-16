using AuthDesk.Contracts.Services;
using AuthDesk.Core.Models;
using AuthDesk.Core.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AuthDesk.Models
{
	public class CodeEntryContext : ObservableObject
	{
		public CodeEntry Entry { get; }

		public string Id => Entry.Id;

		private string code;
		public string Code 
		{ 
			get => code; 
			private set
			{
				code = value;
				OnPropertyChanged(nameof(Code));
			}
		}

		private string copyCodebuttonText = "Copy";
		public string CopyCodebuttonText
		{
			get => copyCodebuttonText;
			private set
			{
				copyCodebuttonText = value;
				OnPropertyChanged(nameof(CopyCodebuttonText));
			}
		}

		private ICommand copyCodeCommand;
		public ICommand CopyCodeCommand
		=> copyCodeCommand ??= new RelayCommand(Copy);

		private DispatcherTimer messageTimer;

		public CodeEntryContext(CodeEntry entry)
		{
			Entry = entry;
			code = CodeGenerator.Get2FACode(Entry.Info.Secrets);

			messageTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(2) 
			};
			messageTimer.Tick += MessageTimer_Tick;
		}

		public void GenerateNewCode()
		{
			Code = CodeGenerator.Get2FACode(Entry.Info.Secrets); 
		}

		private void Copy()
		{
			Clipboard.SetText(Code);
			CopyCodebuttonText = "Copied !";
			messageTimer.Start();
		}
		private void MessageTimer_Tick(object sender, EventArgs e)
		{
			CopyCodebuttonText = "Copy";
			messageTimer.Stop(); // Stop the timer to prevent it from firing again
		}
	}
}
