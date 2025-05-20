using System.Runtime.InteropServices;

namespace src;

internal partial class Program
{
    public class ApiVersionsResponseBody : KafkaResponseBody
    {
        public KafkaApiDescription[] ApiDescriptions { get; }

        public ApiVersionsResponseBody(
            KafkaApiDescription[] apiDescriptions,
            short errorCode,
            int throttleTime) : base(errorCode, throttleTime)
        {
            ApiDescriptions = apiDescriptions;
        }

        public override int Size() =>
            base.Size()
            + ApiDescriptions.Sum(d => d.Size() + 1)
            + sizeof(byte);
    }
}