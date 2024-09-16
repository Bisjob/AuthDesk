using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AuthDesk.UserControls
{
    /// <summary>
    /// Logique d'interaction pour DateTimeProgressBar.xaml
    /// </summary>
    public partial class DateTimeProgressBar : UserControl
    {
        public DateTime LastExecutedDate
        {
            get { return (DateTime)GetValue(LastExecutedDateProperty); }
            set { SetValue(LastExecutedDateProperty, value); }
        }

        public static readonly DependencyProperty LastExecutedDateProperty =
            DependencyProperty.Register("LastExecutedDate", typeof(DateTime), typeof(DateTimeProgressBar), new PropertyMetadata(DateTime.Now, OnLastExecutedDateChanged));

        public TimeSpan TickInterval
        {
            get { return (TimeSpan)GetValue(TickIntervalProperty); }
            set { SetValue(TickIntervalProperty, value); UpdateProgress(); }
        }

        public static readonly DependencyProperty TickIntervalProperty =
            DependencyProperty.Register("TickInterval", typeof(TimeSpan), typeof(DateTimeProgressBar), new PropertyMetadata(TimeSpan.Zero));

        private DispatcherTimer timer = new DispatcherTimer();

        public DateTimeProgressBar()
        {
            InitializeComponent();
            Loaded += DateTimeProgressBar_Loaded;
            Unloaded += DateTimeProgressBar_Unloaded;
        }


        private static void OnLastExecutedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DateTimeProgressBar;
            control?.UpdateProgress();
        }

        private void DateTimeProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromMilliseconds(100); // Update frequency, adjust as needed
            timer.Tick += Timer_Tick;
            timer.Start();
            UpdateProgress();
        }
        private void DateTimeProgressBar_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Tick -= Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if (TickInterval.TotalMilliseconds <= 0) return;

            var now = DateTime.UtcNow;
            var timeSinceLastExecution = now - LastExecutedDate;
            var progress = Math.Min(1.0, timeSinceLastExecution.TotalMilliseconds / TickInterval.TotalMilliseconds);

            progressBar.Value = progress * progressBar.Maximum;
            progressBar.Maximum = TickInterval.TotalMilliseconds;

            if (progress >= 1)
            {
                // Optionally, do something when the interval has completed
            }
        }
    }
}
