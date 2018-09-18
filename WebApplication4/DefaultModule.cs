using Autofac;
using EFCore.Kernal.ProviderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApplication4
{
    //    public class IocModule : Module
    //    {
    //        protected override void Load(ContainerBuilder builder)
    //        {
    //            //            var assemblys = Assembly.Load("WebApplication1");
    //            //            var baseType = typeof(IDiTest);
    //            //            builder.RegisterAssemblyTypes(assemblys)
    //            //                .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
    //            //                .AsImplementedInterfaces().InstancePerLifetimeScope();
    //
    ////            builder.RegisterType<DiTest>().As<IDiTest>();//.PropertiesAutowired();
    //            builder.RegisterType<StudentService>().AsImplementedInterfaces();
    ////            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
    //            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
    //        }
    //    }
    public class DefaultModule : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, List<Type> listType)
        {
            //            builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
            //                .As<IValuesService>()
            //                .InstancePerLifetimeScope();

            //            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            //            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            var service = listType.Where(r => r.Name.EndsWith("Service")).ToArray();
            var repository = listType.Where(r => r.Name.EndsWith("Repository")).ToArray();
            var ctsAttr = listType.Where(r => r.GetCustomAttribute<DependencyRegisterAttribute>() != null).ToArray();
            builder.RegisterTypes(ctsAttr).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterTypes(repository).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterTypes(service).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
        }

        public int Order { get; } = 1;
    }
    public class DependencyRegisterAttribute : Attribute
    {

    }
}