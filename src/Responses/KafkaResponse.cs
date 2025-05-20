namespace src;

internal partial class Program
{
    public abstract class KafkaResponse<TBody> : ISizable
        where TBody : KafkaResponseBody
    {
        public int MessageSize { get; set; }

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