using System.IO;

namespace Microsoft.WebCore.Helper
{
    public static class BufferSerializeHelper
    {
        public static byte[] Serialize<T>(T souce)
        {
            byte[] result = null;
            if (souce == null)
            {
                result = new byte[0];
            }
            try
            {
                using (var ms = new MemoryStream())
                {
                    //                    ProtoBuf.Serializer.Serialize<T>(ms, souce);
                    result = ms.ToArray();
                }
            }
            catch
            {
                result = new byte[0];
            }
            return result;
        }

        public static T DeSerialize<T>(byte[] arryByte)
        {
            var result = default(T);
            if (arryByte == null || arryByte.Length == 0) return result;
            try
            {
                using (var ms = new MemoryStream(arryByte))
                {
                    //                    result = ProtoBuf.Serializer.Deserialize<T>(ms);
                }
            }
            catch
            {
                result = default(T);
            }
            return result;
        }
    }
}