using Autofac;
using EFCore.Service.Contract;
using EFCore.Service.Implement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EFCore.Kernal.ProviderModule;

namespace WebApplication2.Kernel
{
    public class DefaultModule : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, List<Type> listType)
        {
//            builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
//                .As<IValuesService>()
//                .InstancePerLifetimeScope();

            //            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            //            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            var service = listType.Where(r => r.Name.EndsWith("Service")
                            || r.Name.EndsWith("Repository")
                            || r.GetCustomAttribute<DependencyRegisterAttribute>() != null)
                            .ToArray();
            builder.RegisterTypes(service).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
        }

        public int Order { get; } = 1;
    }

    public class DependencyRegisterAttribute : Attribute
    {

    }
}