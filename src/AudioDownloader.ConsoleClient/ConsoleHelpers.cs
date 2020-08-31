using System;
using System.ComponentModel;
using static System.Console;

namespace AudioDownloader.ConsoleClient
{
	public static class ConsoleHelpers
	{
		private const char HEADER_WRAPPER_CHAR = '-';
		private const char HEADER_UNDERLINE_CHAR = '=';
		private const int HEADER_WRAPPER_LENGTH = 30;
		private static readonly string _headerWrapper = new string(HEADER_WRAPPER_CHAR, HEADER_WRAPPER_LENGTH);

		public static void PrintHeader(string text, bool newLineBefore = true, bool newLineAfter = true)
		{
			var header = $"{_headerWrapper} {text} {_headerWrapper}";
			var underline = new string(HEADER_UNDERLINE_CHAR, header.Length);

			if (newLineBefore)
			{
				WriteLine();
			}

			WriteLine(underline);
			WriteLine(header);
			WriteLine(underline);

			if (newLineAfter)
			{
				WriteLine();
			}
		}

		public static bool Confirm(string message, ConsoleKey negativeKey = ConsoleKey.N, bool invert = false)
		{
			Write(message);

			var result = ReadKey(true).Key != negativeKey;

			WriteLine();

			return invert ? !result : result;
		}

		public static T Read<T>(string message, Func<string, T> converter, Func<T, bool> isValid)
		{
			if (isValid == null || converter == null) return default;

			message = $"{Environment.NewLine}{message}";

			while (true)
			{
				Write(message);

				var input = ReadLine();

				var converted = converter(input);

				if (isValid(converted)) return converted;
			}
		}

		public static T Read<T>(string message, Func<T, bool> isValid)
		{
			var converter = TypeDescriptor.GetConverter(typeof(T));

			return Read(message, input => (T)converter.ConvertFromInvariantString(input), isValid);
		}

		public static TimeSpan ReadTimeSpan(string message)
			=> Read
			(
				message,
				input =>
				{
					TimeSpan.TryParse(input, out var output);
					return output;
				},
				output => output >= TimeSpan.Zero
			);
	}
}
