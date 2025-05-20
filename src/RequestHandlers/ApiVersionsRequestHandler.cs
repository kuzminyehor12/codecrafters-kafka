using System.Reflection;

namespace src;

internal partial class Program
{
    public class ApiVersionsRequestHandler : KafkaRequestHandler
    {
        public override short MinVersion => 0;

        public override short MaxVersion => 4;

        public override short ApiKey => (short)ApiKeys.ApiVersions;

        public override byte[] GetResponseBuffer(int correlationId, short apiVersion)
        {
            var header = new KafkaResponseHeader(correlationId);

            var body = CreateBody(apiVersion);

            var response = new ApiVersionsResponse(header, body);

            return response.CreateBuffer();
        }

        private ApiVersionsResponseBody CreateBody(short apiVersion)
        {
            short errorCode = apiVersion >= MinVersion && apiVersion <= MaxVersion ?
               ErrorCodes.OK
               : ErrorCodes.UNSUPPORTED_VERSION;

            var apiDescriptions = GetAvailableApis();

            return new ApiVersionsResponseBody(apiDescriptions, errorCode, 4915200);
        }
        
        private KafkaApiDescription[] GetAvailableApis()
        {
            var requestHandlerTypes = Assembly.GetAssembly(typeof(KafkaRequestHandler))
                .GetTypes()
                .Where(type => type.IsClass
                    && !type.IsAbstract
                    && type.IsSubclassOf(typeof(KafkaRequestHandler)));

            return requestHandlerTypes
                .Select(type => (KafkaRequestHandler?)Activator.CreateInstance(type))
                .Where(handler => handler is not null)
                .Select(handler => new KafkaApiDescription
                {
                    ApiKey = handler!.ApiKey,
                    MinApiVersion = handler!.MinVersion,
                    MaxApiVersion = handler!.MaxVersion,
                })
                .ToArray();
        }
    }
}