using System.Buffers.Binary;
using System.Text;

namespace src;

internal partial class Program
{
    public class RequestHeader
    {
        public int CorrelationId { get; set; }

        public short ApiKey { get; set; }

        public short ApiVersion { get; set; }

        public string ClientId { get; set; }

        public RequestHeader(byte[] header)
        {
            ApiKey = BinaryPrimitives.ReadInt16BigEndian(header[..2]);
            ApiVersion = BinaryPrimitives.ReadInt16BigEndian(header[2..4]);
            CorrelationId = BinaryPrimitives.ReadInt32BigEndian(header[4..8]);

            // client id length
            _ = header[7..10];
            ClientId = Encoding.UTF8.GetString(header[10..19]);
        }

        public override string ToString() =>
$@"Api Key: {ApiKey}{Environment.NewLine}
Api Version: {ApiVersion}{Environment.NewLine}
Correlation Id: {CorrelationId}{Environment.NewLine}
Client Id: {ClientId}{Environment.NewLine}";
    }
}