using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using EFCore.Kernal.ProviderModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication4
{
    public class ControllerRegistrar : IDependencyRegistrar
    {
        public int Order { get; } = 0;

        public void Register(ContainerBuilder builder, List<Type> listType)
        {
            var controllerType = typeof(ControllerBase);
            var arrControllerType = listType.Where(t => controllerType.IsAssignableFrom(t) && t != controllerType).ToArray();
            builder.RegisterTypes(arrControllerType).PropertiesAutowired();
        }
    }
}
