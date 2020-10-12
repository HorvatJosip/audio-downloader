using System;

namespace AudioDownloader.WpfClient
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class TimeViewModel
	{
		public const string TimeSpanFormat = @"hh\:mm\:ss\.fff";

		private readonly Action<double, TimeSpan> _onValueChanged;
		private readonly Action<string, TimeSpan?> _onDisplayChanged;

		public TimeViewModel() : this(0) { }

		public TimeViewModel(double initialValue)
		{
			Value = initialValue;
		}

		public TimeViewModel(Action<double, TimeSpan> onValueChanged, Action<string, TimeSpan?> onDisplayChanged)
			: this(0, onValueChanged, onDisplayChanged) { }

		public TimeViewModel(double initialValue, Action<double, TimeSpan> onValueChanged, Action<string, TimeSpan?> onDisplayChanged)
			: this(initialValue)
		{
			_onValueChanged = onValueChanged;
			_onDisplayChanged = onDisplayChanged;
		}

		private double _value = 0.00000002; // Allow initial 0 to go through and trigger the setter
		public double Value
		{
			get => _value;
			set
			{
				if (Equals(value, _value)) return;

				_value = value;

				var time = TimeSpan.FromMilliseconds(value);

				_onValueChanged?.Invoke(value, time);

				Display = Format(time);
			}
		}

		private string _display;
		public string Display
		{
			get => _display;
			set
			{
				if (Equals(value, _display)) return;

				_display = value;

				if (TimeSpan.TryParse(value, out var time))
				{
					_onDisplayChanged?.Invoke(value, time);

					Value = time.TotalMilliseconds;
				}
				else
				{
					_onDisplayChanged?.Invoke(value, null);
				}
			}
		}

		public TimeSpan Time => TimeSpan.FromMilliseconds(Value);

		public static string Format(TimeSpan value)
			=> string.Format($"{{0:{TimeSpanFormat}}}", value);
	}
}
