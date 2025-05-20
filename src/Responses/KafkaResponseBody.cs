using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace src;

internal partial class Program
{
    public abstract class KafkaResponseBody : ISizable
    {
        private readonly object _bodyLock = new object();
        private byte[] _errorCodeInBytes = [];
        private byte[] _throttleTimeInBytes = [];

        public short ErrorCode { get; set; }

        public byte[] ErrorCodeInBytes
        {
            get
            {
                lock (_bodyLock)
                {
                    if (_errorCodeInBytes is null || _errorCodeInBytes.Length == 0)
                    {
                        _errorCodeInBytes = new byte[sizeof(short)];
                        BinaryPrimitives.WriteInt16BigEndian(_errorCodeInBytes, ErrorCode);
                    }

                    return _errorCodeInBytes;
                }
            }
        }

        public int ThrottleTime { get; set; }

        public byte[] ThrottleTimeInBytes
        {
            get
            {
                lock (_bodyLock)
                {
                    if (_throttleTimeInBytes is null || _throttleTimeInBytes.Length == 0)
                    {
                        _throttleTimeInBytes = new byte[sizeof(int)];
                        BinaryPrimitives.WriteInt32BigEndian(_throttleTimeInBytes, ThrottleTime);
                    }

                    return _throttleTimeInBytes;
                }
            }
        }

        protected KafkaResponseBody(short errorCode, int throttleTime)
        {
            ErrorCode = errorCode;
            ThrottleTime = throttleTime;
        }

        public virtual int Size() => Marshal.SizeOf(ErrorCode) + Marshal.SizeOf(ThrottleTime) + sizeof(byte);
    }
}