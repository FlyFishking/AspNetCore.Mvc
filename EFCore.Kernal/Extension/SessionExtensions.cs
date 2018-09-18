using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EFCore.Kernal.Extension
{
    public static class SessionExtensions
    {
        public static T GetViaProtoBuf<T>(this ISession session, string key) where T : class
        {
            if (!session.TryGetValue(key, out var byteArray)) return null;

            using (var memoryStream = new MemoryStream(byteArray))
            {
                var obj = default(T);//ProtoBuf.Serializer.Deserialize<T>(memoryStream);
                return obj;
            }
        }

        public static void SetViaProtoBuf<T>(this ISession session, string key, T value) where T : class
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
//                    ProtoBuf.Serializer.Serialize(memoryStream, value);
                    var byteArray = memoryStream.ToArray();
                    session.Set(key, byteArray);
                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }
    }
}