using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using System;
using System.Globalization;

namespace AudioDownloader.WpfClient
{
	public class StringToBoolConverter : BaseConverter<StringToBoolConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is string str)
			{
				return str.IsNotNullOrEmpty();
			}

			return false;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
