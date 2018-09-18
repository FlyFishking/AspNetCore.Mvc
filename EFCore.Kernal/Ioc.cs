using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace EFCore.Kernal
{
    public class Ioc : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
//            builder.RegisterType(null).AsSelf().EnableInterfaceInterceptors();
        }
    }
}
