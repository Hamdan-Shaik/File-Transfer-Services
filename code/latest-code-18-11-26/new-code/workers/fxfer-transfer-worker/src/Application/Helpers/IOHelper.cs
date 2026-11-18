namespace Fxfer.TransferWorker.Application.Helpers;

public static class IOHelper
{
    public static Stream GetStream(string stringData)
    {
        var stream = new MemoryStream();

        using (var writer = new StreamWriter(stream, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false), 1024, leaveOpen: true))
        {
            writer.Write(stringData);
            writer.Flush();
        }

        stream.Position = 0;
        return stream;
    }

    public static string GetString(Stream inputStream)
    {
        using (var reader = new StreamReader(inputStream))
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

