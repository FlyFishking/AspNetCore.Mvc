using Autofac;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using EFCore.Kernal.ProviderModule;

namespace WebApplication2.Kernel
{
    public class ControllerRegistrar : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder, List<Type> listType)
        {
            var pageModelType = typeof(PageModel);
            var arrControllerType = listType.Where(t => pageModelType.IsAssignableFrom(t) && t != pageModelType).ToArray();
            builder.RegisterTypes(arrControllerType).PropertiesAutowired();
        }
    }
}
