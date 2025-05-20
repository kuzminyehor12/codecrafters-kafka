using System.Net;
using System.Net.Sockets;

namespace src;

internal partial class Program
{
    private static void Main(string[] args)
    {
        // You can use print statements as follows for debugging, they'll be visible when running tests.
        Console.WriteLine("Logs from your program will appear here!");

        // Uncomment this block to pass the first stage
        TcpListener server = new TcpListener(IPAddress.Any, 9092);
        server.Start();

        using TcpClient client = server.AcceptTcpClient(); // wait for client
        using NetworkStream stream = client.GetStream();

        var request = new Request(stream);

        Console.WriteLine(request);

        var requestHandler = CreateHandler(request.Header.ApiKey);

        byte[] response = requestHandler?.GetResponseBuffer(request.Header.CorrelationId, request.Header.ApiVersion) ?? [];

        Console.WriteLine(BitConverter.ToString(response));
        
        stream.Write(response);

        stream.Close();
    }

    private static KafkaRequestHandler? CreateHandler(short apiKey)
    {
        return (ApiKeys)apiKey switch
        {
            ApiKeys.ApiVersions => new ApiVersionsRequestHandler(),
            ApiKeys.DescribeTopicPartitions => new DescribeTopicPartitionsRequestHandler(),
            _ => null
        };
    }
}