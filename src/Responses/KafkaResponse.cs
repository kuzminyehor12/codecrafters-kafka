using System.Buffers.Binary;

namespace src;

internal partial class Program
{
    public abstract class KafkaResponse<TBody> : ISizable
        where TBody : KafkaResponseBody
    {
        private static readonly object _sizeLock = new object();
        private byte[] _messageSizeInBytes = [];

        public byte[] MessageSizeInBytes
        {
            get
            {
                lock (_sizeLock)
                {
                    if (_messageSizeInBytes == null || _messageSizeInBytes.Length == 0)
                    {
                        _messageSizeInBytes = new byte[sizeof(int)];
                        BinaryPrimitives.WriteInt32BigEndian(_messageSizeInBytes, Size());
                    }

                    return _messageSizeInBytes;
                }
            }
        }

        public KafkaResponseHeader Header { get; set; }

        public TBody Body { get; set; }

        protected KafkaResponse(KafkaResponseHeader header, TBody body)
        {
            Header = header;
            Body = body;
        }

        public abstract byte[] CreateBuffer();

        public virtual int Size() => Header.Size() + Body.Size();
    }
}