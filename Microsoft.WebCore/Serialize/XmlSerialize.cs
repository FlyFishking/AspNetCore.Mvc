using System.IO;
using Microsoft.WebCore.Helper;

namespace Microsoft.WebCore.Serialize
{
    public class XmlSerialize : SerializeProvider
    {
        public override string Serialize<T>(T model)
        {
            return XmlSerializeHelper.SerializeToString(model);
        }

        public override void Serialize<T>(T model, string filePath)
        {
            XmlSerializeHelper.Serialize(model,filePath);
        }

        public override T Deserialize<T>(string content)
        {
            return XmlSerializeHelper.DeserializeString<T>(content);
        }

        public override T DeserializeFromUri<T>(string inputUri)
        {
            return XmlSerializeHelper.Deserialize<T>(inputUri);
        }
    }

    public class JsonSerialize : SerializeProvider
    {
        public override string Serialize<T>(T model)
        {
            throw new System.NotImplementedException();
        }

        public override void Serialize<T>(T model, string filePath)
        {
            throw new System.NotImplementedException();
        }

        public override T Deserialize<T>(string content)
        {
            throw new System.NotImplementedException();
        }

        public override T DeserializeFromUri<T>(string inputUri)
        {
            throw new System.NotImplementedException();
        }
    }

    public class BufferSerialize : SerializeProvider, IBufferSerialize
    {
        public override string Serialize<T>(T model)
        {
            throw new System.NotImplementedException();
        }

        public override void Serialize<T>(T model, string filePath)
        {
            throw new System.NotImplementedException();
        }

        public override T Deserialize<T>(string content)
        {
            throw new System.NotImplementedException();
        }

        public override T DeserializeFromUri<T>(string inputUri)
        {
            throw new System.NotImplementedException();
        }

        public byte[] SerializeBuffer<T>(T model)
        {
            return BufferSerializeHelper.Serialize(model);
        }

        public T DeserializeBuffer<T>(byte[] buffer)
        {
            return BufferSerializeHelper.DeSerialize<T>(buffer);
        }
    }
}