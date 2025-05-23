using System.Buffers.Binary;

namespace src;

internal partial class Program
{
    public class KafkaResponseHeader : ISizable
    {
        private static readonly object _headerLock = new object();

        private byte[] _correlationIdInBytes = [];

        public int CorrelationId { get; set; }

        public byte[] CorrelationIdInBytes
        {
            get
            {
                lock (_headerLock)
                {
                    if (_correlationIdInBytes is null || _correlationIdInBytes.Length == 0)
                    {
                        _correlationIdInBytes = new byte[sizeof(int)];
                        BinaryPrimitives.WriteInt32BigEndian(_correlationIdInBytes, CorrelationId);
                    }

                    return _correlationIdInBytes;
                }
            }
        }

        public KafkaResponseHeader(int correlationId)
        {
            CorrelationId = correlationId;
        }

        public int Size() => sizeof(int);
    }
}