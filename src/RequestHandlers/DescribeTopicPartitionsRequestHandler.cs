namespace src;

internal partial class Program
{
    public class DescribeTopicPartitionsRequestHandler : KafkaRequestHandler
    {
        public override short MinVersion => 0;

        public override short MaxVersion => 0;

        public override short ApiKey => (short)ApiKeys.DescribeTopicPartitions;

        public override byte[] GetResponseBuffer(int correlationId, short apiVersion)
        {
            throw new NotImplementedException();
        }
    }
}