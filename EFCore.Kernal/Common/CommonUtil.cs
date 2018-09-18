using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using EFCore.Kernal.Extension;
using Microsoft.WebCore.Extention;

namespace EFCore.Kernal.Common
{
    public class CommonUtil
    {
        public static int MakeMemberNum(int cardType, int areaCode, int areaMemberId)
        {
            var iMakeMemberNum = 0;
            var bytes = new byte[4];

            var cardTypeByte = (byte)cardType;
            var areaCodeByte = (byte)areaCode;
            var areaMemberIdBytes = BitConverter.GetBytes(areaMemberId);

            cardTypeByte = (byte)(cardTypeByte << 7);
            bytes[0] = (byte)(cardTypeByte | (areaCodeByte >> 4));
            if (areaMemberIdBytes != null && areaMemberIdBytes.Length == 4)
            {
                if (BitConverter.IsLittleEndian)
                {
                    bytes[1] = (byte)((areaCodeByte << 4) | areaMemberIdBytes[2]);
                    bytes[2] = areaMemberIdBytes[1];
                    bytes[3] = areaMemberIdBytes[0];
                }
                else
                {
                    bytes[1] = (byte)((areaCodeByte << 4) | areaMemberIdBytes[1]);
                    bytes[2] = areaMemberIdBytes[2];
                    bytes[3] = areaMemberIdBytes[3];
                }
            }

            bytes = ConvertToBigEndianOrder(bytes);
            iMakeMemberNum = BitConverter.ToInt32(bytes, 0);
            return iMakeMemberNum;
        }

        /// <summary>
        /// 小端转大端
        /// </summary>
        /// <param name="leBuffer"></param>
        /// <returns></returns>
        public static byte[] ConvertToBigEndianOrder(byte[] leBuffer)
        {
            if (leBuffer != null && leBuffer.Length > 1 && BitConverter.IsLittleEndian)
            {
                var beBuffer = new byte[leBuffer.Length];
                for (var i = 0; i < leBuffer.Length; i++)
                {
                    beBuffer[beBuffer.Length - 1 - i] = leBuffer[i];
                }
                return beBuffer;
            }
            return leBuffer;
        }

        
        public static string ConvertToHexadecimal(int num)
        {
            var buffer = ConvertToBigEndianOrder(BitConverter.GetBytes(num));
            return ConvertToHexadecimal(buffer);
        }
        public static string ConvertToHexadecimal(long num)
        {
            var buffer = ConvertToBigEndianOrder(BitConverter.GetBytes(num));
            return ConvertToHexadecimal(buffer);
        }

        public static string ConvertToHexadecimal(byte[] buffer)
        {
            var result = buffer.Select(t => t.ToString("X2")).JoinString("");
            return result;
        }

        public static int GetRandomSeed()
        {
            var rndBytes = new byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        /// <summary>
        /// just for test
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRandomString(int len)
        {
            var chars = "0123456789";
            var reValue = string.Empty;
            var rnd = new Random(GetRandomSeed());
            while (reValue.Length < len)
            {
                string s1 = chars[rnd.Next(0, chars.Length)].ToString();
                if (reValue.IndexOf(s1) == -1) reValue += s1;
            }
            return reValue;
        }
    }
}
