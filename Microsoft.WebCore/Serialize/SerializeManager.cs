namespace Microsoft.WebCore.Serialize
{
    public class SerializeManager
    {
        private readonly SerializeProvider provider;
        public SerializeManager(SerializeProvider provider)
        {
            this.provider = provider;
        }

        public virtual string Serialize<T>(T model)
        {
            return provider.Serialize(model);
        }

        public virtual void Serialize<T>(T model, string filePath)
        {
            provider.Serialize(model, filePath);
        }

        public virtual T Deserialize<T>(string content)
        {
            return provider.Deserialize<T>(content);
        }

        public virtual T DeserializeFromUri<T>(string inputUri)
        {
            return provider.DeserializeFromUri<T>(inputUri);
        }
    }
}