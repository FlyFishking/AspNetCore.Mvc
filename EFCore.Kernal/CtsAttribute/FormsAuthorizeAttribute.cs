using System;
using System.Linq;


namespace EFCore.Kernal.CtsAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class FormsAuthorizeAttribute : Attribute
    {

    }
}