using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace EFCore.Kernal.ProviderModule
{
    public class ProviderManager : IProviderManager
    {
        public virtual IContainer Container => container;
        public static ProviderManager Instance { get; } = new ProviderManager();

        public IServiceProvider Initialize(IServiceCollection services)
        {
            // Create the autofac container builder.
            var builder = new ContainerBuilder();

            builder.RegisterInstance(Instance).As<IProviderManager>().SingleInstance();
            //所有程序集 和程序集下类型
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package").ToList();//排除所有的系统程序集、Nuget下载包
            var listAllType = new List<Type>();
            libs.ForEach(t =>
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(t.Name));
                    listAllType.AddRange(assembly.GetTypes().Where(type => type != null));
                }
                catch { }
            });

            //找到所有外部 IDependencyRegistrar 实现，调用注册
            var registrarType = typeof(IDependencyRegistrar);
            var arrRegistrarType = listAllType.Where(t => registrarType.IsAssignableFrom(t) && t != registrarType).ToArray();
            var listRegistrarInstances = arrRegistrarType.Select(t => (IDependencyRegistrar)Activator.CreateInstance(t)).ToList();
            listRegistrarInstances = listRegistrarInstances.OrderBy(t => t.Order).ToList();
            listRegistrarInstances.ForEach(t => t.Register(builder, listAllType));

            //注册 ITransientDependency 实现类
            var dependencyType = typeof(ITransientDependency);
            var arrDependencyType = listAllType.Where(t => dependencyType.IsAssignableFrom(t) && t != dependencyType).ToArray();
            builder.RegisterTypes(arrDependencyType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            //if you want use auto aop, you need install Autofac.Extras.DynamicProxy to support it
            //.EnableInterfaceInterceptors();

            foreach (var type in arrDependencyType)
            {
                if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                {
                    builder.RegisterType(type).As(type.BaseType).InstancePerLifetimeScope().PropertiesAutowired();
                }
            }

            //注册 ISingletonDependency 实现类
            var singletonDependencyType = typeof(ISingletonDependency);
            var arrSingletonDependencyType = listAllType.Where(t => singletonDependencyType.IsAssignableFrom(t) && t != singletonDependencyType).ToArray();
            builder.RegisterTypes(arrSingletonDependencyType)
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired();

            foreach (var type in arrSingletonDependencyType)
            {
                if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                {
                    builder.RegisterType(type).As(type.BaseType).SingleInstance().PropertiesAutowired();
                }
            }

            //register modules
            var modules = listAllType.Where(t => t.IsClass && t.BaseType == typeof(Autofac.Module)).Select(t => t.Assembly).Distinct().ToList();
            modules.ToList().ForEach(t => builder.RegisterAssemblyModules(t));

            builder.Populate(services);
            container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// Check whether some service is registered (can be resolved)
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return string.IsNullOrEmpty(key) ? scope.Resolve<T>() : scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="parameters"></param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(params Parameter[] parameters) where T : class
        {
            var scope = Scope();
            return scope.Resolve<T>(parameters);
        }

        /// <summary>
        /// Resolve all
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved services</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return string.IsNullOrEmpty(key) ? scope.Resolve<IEnumerable<T>>().ToArray() : scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// Resolve optional
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new Exception("Unknown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {

                }
            }
            throw new Exception("No constructor  was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// Try to resolve srevice
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <param name="instance">Resolved service</param>
        /// <returns>Value indicating whether service has been successfully resolved</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// Get current scope,remember to be sure that it'll be disposed once used
        /// </summary>
        /// <returns>Scope</returns>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope();
            }
            catch (Exception)
            {
                //we can get an exception here if RequestLifetimeScope is already disposed
                //for example, requested in or after "Application_EndRequest" handler
                //but note that usually it should never happen

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }

        private IContainer container;
    }
}