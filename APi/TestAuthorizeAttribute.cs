using System;
using Microsoft.AspNetCore.Authorization;

namespace APi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class TestAuthorizeAttribute : AuthorizeAttribute
    {

        public string Permission { get; set; }

        public TestAuthorizeAttribute(string permission)
        {
            Permission = permission;
        }
    }
}