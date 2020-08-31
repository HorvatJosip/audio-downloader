using System;

namespace AudioDownloader.Infrastructure
{
	public class AudioSplitRange
	{
		public TimeSpan Start { get; }
		public TimeSpan End { get; }

		public AudioSplitRange(TimeSpan start, TimeSpan end)
		{
			if (start < end)
			{
				Start = start;
				End = end;
			}
			else
			{
				End = start;
				Start = end;
			}
		}

		public bool Contains(TimeSpan other)
			=> other >= Start && other <= End;
	}
}
