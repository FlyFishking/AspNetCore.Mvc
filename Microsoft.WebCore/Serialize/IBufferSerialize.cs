namespace Microsoft.WebCore.Serialize
{
    public interface IBufferSerialize
    {
        byte[] SerializeBuffer<T>(T model);
        T DeserializeBuffer<T>(byte[] buffer);
    }

    public interface IStringSerialize
    {
        string Serialize<T>(T model);
        T Deserialize<T>(string content);

    }

    public interface IFileSerialize
    {
        void Serialize<T>(T model, string filePath);
        T DeserializeFromUri<T>(string inputUri);

    }
}