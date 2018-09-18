using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Platform.Core.Extention
{
    public static class AssemblerExtension
    {
        /// <summary>
        /// All assemblies by reference
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> GetAppDomainAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        public static ConstructorInfo GetConstructor(this Type type, Type[] parameterTypes, bool nonPublic)
        {
            return type.GetTypeInfo().GetConstructor(parameterTypes, nonPublic);
        }

        public static ConstructorInfo GetConstructor(this TypeInfo type, Type[] parameterTypes, bool nonPublic)
        {
            return type.GetConstructors(nonPublic)
                .SingleOrDefault(ctor => IsMatch(ctor.GetParameters(), parameterTypes));
        }

        public static ConstructorInfo[] GetConstructors(this TypeInfo typeInfo, bool nonPublic)
        {
            return typeInfo.DeclaredConstructors.Where(c =>
            {
                if (!c.IsStatic)
                    return ((nonPublic ? 0 : (c.IsPublic ? 1 : 0)) | (nonPublic ? 1 : 0)) != 0;
                return false;
            }).ToArray();
        }

        public static PropertyInfo GetProperty(this Type type, string name, bool nonPublic)
        {
            return type.GetTypeInfo().GetProperty(name, nonPublic);
        }

        public static PropertyInfo GetProperty(this TypeInfo type, string name, bool nonPublic)
        {
            return type.GetDeclaredProperty(name);
        }

        public static MemberInfo GetInstanceMember(this TypeInfo declaringType, string name)
        {
            var member = declaringType.AsType().GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            switch (member.Length)
            {
                case 0:
                    return null;
                case 1:
                    return member[0];
                default:
                    throw new AmbiguousMatchException(name);
            }
        }

        public static MethodInfo GetInstanceMethod(this Type declaringType, string name)
        {
            var instanceFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return declaringType.GetMethods(instanceFlags).FirstOrDefault(method => method.Name == name);
        }

        public static MethodInfo GetInstanceMethod(this TypeInfo declaringType, string name)
        {
            return declaringType.AsType().GetInstanceMethod(name);
        }

        public static MethodInfo GetInstanceMethod(this Type declaringType, string name, Type[] parameterTypes)
        {
            var instanceFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return declaringType.GetMethods(instanceFlags).FirstOrDefault(method => method.Name == name && IsMatch(method.GetParameters(), parameterTypes));
        }

        public static MethodInfo GetInstanceMethod(this TypeInfo declaringType, string name, Type[] types)
        {
            return declaringType.AsType().GetInstanceMethod(name, types);
        }

        public static MethodInfo GetStaticMethod(this Type declaringType, string name)
        {
            var staticFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return declaringType.GetMethods(staticFlags).FirstOrDefault(method => method.Name == name);
        }

        public static MethodInfo GetStaticMethod(this TypeInfo declaringType, string name)
        {
            return declaringType.AsType().GetStaticMethod(name);
        }

        public static MethodInfo GetStaticMethod(this Type declaringType, string name, Type[] parameterTypes)
        {
            var staticFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return declaringType.GetMethods(staticFlags).FirstOrDefault(method => method.Name == name && IsMatch(method.GetParameters(), parameterTypes));
        }

        public static MethodInfo GetGetMethod(this PropertyInfo property, bool nonPublic, bool allowInternal)
        {
            if (property == null)
                return null;
            var methodInfo = property.GetMethod;
            if (!nonPublic && methodInfo != null && !methodInfo.IsPublic)
                methodInfo = null;
            return methodInfo;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo property, bool nonPublic, bool allowInternal)
        {
            if (property == null)
                return null;
            var methodInfo = property.SetMethod;
            if (!nonPublic && methodInfo != null && !methodInfo.IsPublic)
                methodInfo = null;
            return methodInfo;
        }

        public static MemberInfo[] GetInstanceFieldsAndProperties(this Type type, bool publicOnly)
        {
            var bindingAttr = publicOnly ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var properties = type.GetProperties(bindingAttr);
            var fields = type.GetFields(bindingAttr);
            var memberInfoArray = new MemberInfo[fields.Length + properties.Length];
            properties.CopyTo(memberInfoArray, 0);
            fields.CopyTo(memberInfoArray, properties.Length);
            return memberInfoArray;
        }

        public static Type GetMemberType(this MemberInfo member)
        {
            PropertyInfo propertyInfo;
            if ((propertyInfo = member as PropertyInfo) != null)
                return propertyInfo.PropertyType;
            var fieldInfo = member as FieldInfo;
            return fieldInfo?.FieldType;
        }

        public static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        public static object ParseEnum(Type type, string value)
        {
            return Enum.Parse(type, value, true);
        }

        public static bool IsAssignableFrom(this Type target, Type type)
        {
            return target.IsAssignableFrom(type);
        }

        public static bool IsValueType(Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        public static bool IsSealed(Type type)
        {
            return type.GetTypeInfo().IsSealed;
        }

        public static bool IsClass(Type type)
        {
            return type.GetTypeInfo().IsClass;
        }

        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsSubclassOf(Type type, Type baseClass)
        {
            return type.GetTypeInfo().IsSubclassOf(baseClass);
        }

        public static void Sort(int[] keys, object[] values)
        {
            bool flag;
            do
            {
                flag = false;
                for (var index = 1; index < keys.Length; ++index)
                    if (keys[index - 1] > keys[index])
                    {
                        var key = keys[index];
                        keys[index] = keys[index - 1];
                        keys[index - 1] = key;
                        var obj = values[index];
                        values[index] = values[index - 1];
                        values[index - 1] = obj;
                        flag = true;
                    }
            }
            while (flag);
        }

        /// <summary>
        /// Find all classed of the specify type
        /// </summary>
        /// <param name="assignTypeFrom">base type of interface</param>
        /// <returns></returns>
        public static IEnumerable<Type> FindClassesOfInterfaceType(this Type assignTypeFrom)
        {
            var list = new List<Type>();
            foreach (var item in GetAppDomainAssemblies())
            {
                var typesToRegister = item.GetTypes()
                    .Where(type => !string.IsNullOrEmpty(type.Namespace) && type.GetInterface(assignTypeFrom.Name) == assignTypeFrom)
                    .ToList();
                if (typesToRegister.Any())
                    list.AddRange(typesToRegister);
            }
            return list;
        }

        private static bool IsMatch(ParameterInfo[] parameters, Type[] parameterTypes)
        {
            if (parameterTypes == null)
                parameterTypes = Type.EmptyTypes;
            if (parameters.Length != parameterTypes.Length)
                return false;
            return !parameters.Where((t, index) => t.ParameterType != parameterTypes[index]).Any();
        }
    }
}