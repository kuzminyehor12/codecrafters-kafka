namespace src;

internal partial class Program
{
    public abstract class KafkaRequestHandler
    {
        public abstract short MinVersion { get; }

        public abstract short MaxVersion { get; }

        public abstract short ApiKey { get; }

        public abstract byte[] GetResponseBuffer(int correlationId, short apiVersion);
    }
}