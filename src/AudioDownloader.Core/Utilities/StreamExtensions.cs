using System.IO;
using System.Threading.Tasks;

namespace AudioDownloader.Core
{
	public static class StreamExtensions
	{
		public static void WriteChunk(this Stream stream, byte[] chunk)
			=> stream.Write(chunk, 0, chunk.Length);

		public static async Task WriteChunkAsync(this Stream stream, byte[] chunk)
			=> await stream.WriteAsync(chunk, 0, chunk.Length);
	}
}
