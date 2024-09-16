using AuthDesk.Core.Models;
using AuthDesk.Core.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AuthDesk.Models
{
    public class CodeEntryContext : ObservableObject, IDisposable
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

		public TimeSpan CodeInterval { get; }
		

        private ICommand copyCodeCommand;
		public ICommand CopyCodeCommand
		=> copyCodeCommand ??= new RelayCommand(Copy);

        public DateTime CodeLastExecuted  { get => codeLastExecuted; set => SetProperty(ref codeLastExecuted, value); }

        private DateTime codeLastExecuted;


        private DispatcherTimer codeTimer;
        private DispatcherTimer messageTimer;

        public CodeEntryContext(CodeEntry entry)
		{
			Entry = entry;

			if (Entry.Info.Period != 0)
				CodeInterval = TimeSpan.FromSeconds((double)Entry.Info.Period);
			else
                CodeInterval = TimeSpan.FromSeconds(30);
			
			GenerateNewCode();

            codeTimer = new DispatcherTimer()
			{
				Interval = CodeInterval,
			};
			codeTimer.Tick += CodeTimer_Tick;
			codeTimer.Start();

            messageTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(2) 
			};
			messageTimer.Tick += MessageTimer_Tick;
		}

		public void GenerateNewCode()
		{
			Code = CodeGenerator.Get2FACode(Entry.Info.Secrets);
            CodeLastExecuted = DateTime.UtcNow;
        }

		private void Copy()
		{
			Clipboard.SetText(Code);
			CopyCodebuttonText = "Copied !";
			messageTimer.Start();
		}

		private void CodeTimer_Tick(object sender, EventArgs e)
		{
			GenerateNewCode();
        }

		private void MessageTimer_Tick(object sender, EventArgs e)
		{
			CopyCodebuttonText = "Copy";
			messageTimer.Stop(); // Stop the timer to prevent it from firing again
		}

        public void Dispose()
        {
            codeTimer.Tick -= CodeTimer_Tick;
            codeTimer.Stop();

            messageTimer.Tick -= MessageTimer_Tick;
            messageTimer.Stop();
        }
    }
}
