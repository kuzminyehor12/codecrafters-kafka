using System.Buffers.Binary;

namespace src;

internal partial class Program
{
    public interface ISizable
    {
        int Size();
    }
}