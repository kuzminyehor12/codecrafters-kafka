using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace src;

internal partial class Program
{
    public class ApiVersionsResponse : KafkaResponse<ApiVersionsResponseBody>
    {
        public ApiVersionsResponse(KafkaResponseHeader header, ApiVersionsResponseBody body) : base(header, body)
        {
        }

        public override byte[] CreateBuffer()
        {
            int messageSize = Size();
            byte[] buffer = new byte[sizeof(int) + messageSize];
            
            int offset = 0;
            byte[] messageSizeInBytes = new byte[sizeof(int)];

            BinaryPrimitives.WriteInt32BigEndian(messageSizeInBytes, messageSize);
            Buffer.BlockCopy(messageSizeInBytes, 0, buffer, offset, messageSizeInBytes.Length);
            offset += Marshal.SizeOf(messageSize);

            Buffer.BlockCopy(Header.CorrelationIdInBytes, 0, buffer, offset, Header.CorrelationIdInBytes.Length);
            offset += Marshal.SizeOf(Header.CorrelationId);

            Buffer.BlockCopy(Body.ErrorCodeInBytes, 0, buffer, offset, Body.ErrorCodeInBytes.Length);
            offset += Marshal.SizeOf(Body.ErrorCode);

            buffer[offset++] = (byte)Body.ApiDescriptions.Length;

            foreach (var apiDescription in Body.ApiDescriptions)
            {
                Buffer.BlockCopy(apiDescription.ApiKeyInBytes, 0, buffer, offset, apiDescription.ApiKeyInBytes.Length);
                offset += Marshal.SizeOf(apiDescription.ApiKey);

                Buffer.BlockCopy(apiDescription.MinApiVersionInBytes, 0, buffer, offset, apiDescription.MinApiVersionInBytes.Length);
                offset += Marshal.SizeOf(apiDescription.MinApiVersion);

                Buffer.BlockCopy(apiDescription.MaxApiVersionInBytes, 0, buffer, offset, apiDescription.MaxApiVersionInBytes.Length);
                offset += Marshal.SizeOf(apiDescription.MaxApiVersion);

                buffer[offset++] = 0x00; // tag buffer
            }

            Buffer.BlockCopy(Body.ThrottleTimeInBytes, 0, buffer, offset, Body.ThrottleTimeInBytes.Length);
            offset += Marshal.SizeOf(Body.ThrottleTime);

            buffer[offset] = 0x00; // tag buffer

            return buffer;
        }
    }
}
