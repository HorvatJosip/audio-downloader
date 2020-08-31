using System;
using System.Collections.Generic;

namespace AudioDownloader.Infrastructure
{
	public class AudioSplitOptions
	{
		public string SourceFilePath { get; set; }
		public string DestinationFilePath { get; set; }
		public TimeSpan? Start { get; set; }
		public TimeSpan? End { get; set; }
		public IEnumerable<AudioSplitRange> SplitRanges { get; set; }
	}
}
