using System;

namespace EFCore.Kernal.Common
{
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message)
        {

        }
        public BusinessException(int hResult, string message)
            : base(message)
        {
            base.HResult = hResult;
        }
    }
}