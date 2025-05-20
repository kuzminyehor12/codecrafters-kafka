using System.Buffers.Binary;
using System.Net.Sockets;

namespace src;

internal partial class Program
{
    public class Request
    {
        public int MessageSize { get; set; }

        public RequestHeader Header { get; set; }

        public RequestBody Body { get; set; }

        public Request(NetworkStream stream)
        {
            byte[] messageSize = new byte[sizeof(int)];
            stream.ReadExactly(messageSize, 0, messageSize.Length);
            MessageSize = BinaryPrimitives.ReadInt32BigEndian(messageSize);

            byte[] header = new byte[19];
            stream.ReadExactly(header, 0, header.Length);
            Header = new RequestHeader(header);

            // read tag buffer
            stream.ReadByte();

            byte[] body = new byte[14];
            stream.ReadExactly(body, 0, body.Length);
            Body = new RequestBody(body);

            // read tag buffer
            stream.ReadByte();
        }

        public override string ToString() => $"Message Size: {MessageSize}\n{Header}{Body}";
    }
}