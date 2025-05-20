using System.Text;

namespace src;

internal partial class Program
{
    public class RequestBody
    {
        public string ClientId { get; set; }

        public int ClientSoftwareVersion { get; set; }

        public RequestBody(byte[] content)
        {
            _ = content[0]; // read length
            ClientId = Encoding.UTF8.GetString(content[1..10]);

            _ = content[10]; // read length
            ClientSoftwareVersion = content[11] << 16 | content[12] << 8 | content[13]; // casting 3 bytes into Int32
        }

        public override string ToString() =>
@$"Client Id: {ClientId}{Environment.NewLine}
Client Software Version: {ClientSoftwareVersion}";
    }
}