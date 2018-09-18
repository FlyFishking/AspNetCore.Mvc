using System;
using System.IO;

namespace Microsoft.WebCore.Extention
{
    public static class MemboryExtension
    {
        public static byte[] GetBuffer(this MemoryStream ms)
        {
            if (!ms.TryGetBuffer(out var buffer))
                throw new InvalidOperationException("Unable to obtain underlying MemoryStream buffer");
            if (buffer.Offset != 0)
                throw new InvalidOperationException("Underlying MemoryStream buffer was not zero-offset");
            return buffer.Array;
        }
    }
}