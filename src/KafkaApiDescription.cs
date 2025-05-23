using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace src;

internal partial class Program
{
    public class KafkaApiDescription : ISizable
    {
        private static readonly object _instanceLock = new object();
        private byte[] _apiKeyInBytes = [];
        private byte[] _minApiVersionInBytes = [];
        private byte[] _maxApiVersionInBytes = [];

        public short ApiKey { get; set; }

        public byte[] ApiKeyInBytes
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_apiKeyInBytes == null || _apiKeyInBytes.Length == 0)
                    {
                        _apiKeyInBytes = new byte[sizeof(short)];
                        BinaryPrimitives.WriteInt16BigEndian(_apiKeyInBytes, ApiKey);
                    }

                    return _apiKeyInBytes;
                }
            }
        }

        public short MinApiVersion { get; set; }

        public byte[] MinApiVersionInBytes
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_minApiVersionInBytes == null || _minApiVersionInBytes.Length == 0)
                    {
                        _minApiVersionInBytes = new byte[sizeof(short)];
                        BinaryPrimitives.WriteInt16BigEndian(_minApiVersionInBytes, MinApiVersion);
                    }

                    return _minApiVersionInBytes;
                }
            }
        }

        public short MaxApiVersion { get; set; }

         public byte[] MaxApiVersionInBytes
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_maxApiVersionInBytes == null || _maxApiVersionInBytes.Length == 0)
                    {
                        _maxApiVersionInBytes = new byte[sizeof(short)];
                        BinaryPrimitives.WriteInt16BigEndian(_maxApiVersionInBytes, MaxApiVersion);
                    }

                    return _maxApiVersionInBytes;
                }
            }
        }

        public int Size() => Marshal.SizeOf(ApiKey) + Marshal.SizeOf(MinApiVersion) + Marshal.SizeOf(MaxApiVersion);
    }
}