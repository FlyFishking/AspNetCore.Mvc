namespace Microsoft.WebCore.Serialize
{
    public abstract class SerializeProvider : ISerialize
    {
        public abstract string Serialize<T>(T model);
        public abstract void Serialize<T>(T model, string filePath);
        public abstract T Deserialize<T>(string content);
        public abstract T DeserializeFromUri<T>(string inputUri);
    }
}