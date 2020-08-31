namespace AudioDownloader.Infrastructure
{
	public abstract class Result<TSelf> where TSelf : Result<TSelf>, new()
	{
		public string Error { get; private set; }

		public static TSelf FromError(string error) => new TSelf
		{
			Error = error
		};
	}
}
