using Autofac;
using Microsoft.EFCore.Infrustructure;
using WebApplication3.Service;
using Module = Autofac.Module;

namespace WebApplication3.Kernel
{
    public class IocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //            var assemblys = Assembly.Load("WebApplication1");
            //            var baseType = typeof(IDiTest);
            //            builder.RegisterAssemblyTypes(assemblys)
            //                .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            //                .AsImplementedInterfaces().InstancePerLifetimeScope();

//            builder.RegisterType<DiTest>().As<IDiTest>();//.PropertiesAutowired();
            builder.RegisterType<StudentService>().AsImplementedInterfaces();
//            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}