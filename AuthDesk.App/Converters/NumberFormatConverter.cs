using System.Globalization;
using System.Windows.Data;

namespace AuthDesk.Converters
{
	public class NumberFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string number && number.Length == 6)
			{
				return number.Insert(3, " ");
			}
			return value; 
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
