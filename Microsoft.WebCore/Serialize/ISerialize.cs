using System.Threading.Tasks;

namespace Microsoft.WebCore.Serialize
{
    public interface ISerialize
    {
        string Serialize<T>(T model);
        void Serialize<T>(T model, string filePath);
        T Deserialize<T>(string content);
        T DeserializeFromUri<T>(string inputUri);
    }
}