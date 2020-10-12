using System;

namespace AudioDownloader.WpfClient
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class TimeRangeViewModel
	{
		public TimeViewModel Start { get; set; }
		public TimeViewModel End { get; set; }

		public TimeRangeViewModel() { }

		public TimeRangeViewModel(TimeViewModel start, TimeViewModel end)
		{
			Start = start;
			End = end;
		}

		public TimeRangeViewModel
		(
			double endTime,
			Action<double, TimeSpan> onValueChanged,
			Action<string, TimeSpan?> onDisplayChanged
		) : this
		(
			start: new TimeViewModel(0, onValueChanged, onDisplayChanged),
			end: new TimeViewModel(endTime, onValueChanged, onDisplayChanged)
		) { }
	}
}
