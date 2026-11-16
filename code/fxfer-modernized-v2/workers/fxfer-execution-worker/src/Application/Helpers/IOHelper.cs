using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Fxfer.ExecutionWorker.Application.Helpers;

[ExcludeFromCodeCoverage]
public class IOHelper
{
    public static Stream GetStream(string stringData)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(stringData);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
        }
    }

    public static string GetString(Stream inputStream)
    {
        using (StreamReader reader = new StreamReader(inputStream))
        {
            return reader.ReadToEnd();
        }
    }

    public static void WriteStream(Stream inputStream, ref byte[] dataBytes)
    {
        using (Stream outputStream = inputStream)
        {
            outputStream.Write(dataBytes, 0, dataBytes.Length);
        }
    }
}

