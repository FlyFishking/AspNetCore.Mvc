using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.WebCore.Helper
{
    /// <summary>
    /// Not all frameworks are created equal (fx1.1 vs fx2.0,
    /// micro-framework, compact-framework,
    /// silverlight, etc). This class simply wraps up a few things that would
    /// otherwise make the real code unnecessarily messy, providing fallback
    /// implementations if necessary.
    /// </summary>
    internal sealed class Helpers
    {
        public static readonly Type[] EmptyTypes = Type.EmptyTypes;

        public static StringBuilder AppendLine(StringBuilder builder)
        {
            return builder.AppendLine();
        }

        public static void Sort(int[] keys, object[] values)
        {
            bool flag;
            do
            {
                flag = false;
                for (int index = 1; index < keys.Length; ++index)
                {
                    if (keys[index - 1] > keys[index])
                    {
                        int key = keys[index];
                        keys[index] = keys[index - 1];
                        keys[index - 1] = key;
                        object obj = values[index];
                        values[index] = values[index - 1];
                        values[index - 1] = obj;
                        flag = true;
                    }
                }
            }
            while (flag);
        }

        internal static MemberInfo GetInstanceMember(TypeInfo declaringType, string name)
        {
            MemberInfo[] member = declaringType.AsType().GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            switch (member.Length)
            {
                case 0:
                    return (MemberInfo)null;
                case 1:
                    return member[0];
                default:
                    throw new AmbiguousMatchException(name);
            }
        }

        internal static MethodInfo GetInstanceMethod(Type declaringType, string name)
        {
            foreach (MethodInfo method in declaringType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.Name == name)
                    return method;
            }
            return (MethodInfo)null;
        }

        internal static MethodInfo GetInstanceMethod(TypeInfo declaringType, string name)
        {
            return Helpers.GetInstanceMethod(declaringType.AsType(), name);
        }

        internal static MethodInfo GetStaticMethod(Type declaringType, string name)
        {
            foreach (MethodInfo method in declaringType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.Name == name)
                    return method;
            }
            return (MethodInfo)null;
        }

        internal static MethodInfo GetStaticMethod(TypeInfo declaringType, string name)
        {
            return Helpers.GetStaticMethod(declaringType.AsType(), name);
        }

        internal static MethodInfo GetStaticMethod(Type declaringType, string name, Type[] parameterTypes)
        {
            foreach (MethodInfo method in declaringType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.Name == name && Helpers.IsMatch(method.GetParameters(), parameterTypes))
                    return method;
            }
            return (MethodInfo)null;
        }

        internal static MethodInfo GetInstanceMethod(Type declaringType, string name, Type[] parameterTypes)
        {
            foreach (MethodInfo method in declaringType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.Name == name && Helpers.IsMatch(method.GetParameters(), parameterTypes))
                    return method;
            }
            return (MethodInfo)null;
        }

        internal static MethodInfo GetInstanceMethod(TypeInfo declaringType, string name, Type[] types)
        {
            return Helpers.GetInstanceMethod(declaringType.AsType(), name, types);
        }

        internal static bool IsSubclassOf(Type type, Type baseClass)
        {
            return type.GetTypeInfo().IsSubclassOf(baseClass);
        }

        internal static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        internal static bool IsValueType(Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        internal static bool IsSealed(Type type)
        {
            return type.GetTypeInfo().IsSealed;
        }

        internal static bool IsClass(Type type)
        {
            return type.GetTypeInfo().IsClass;
        }

        internal static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        internal static MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic, bool allowInternal)
        {
            if (property == (PropertyInfo)null)
                return (MethodInfo)null;
            MethodInfo methodInfo = property.GetMethod;
            if (!nonPublic && methodInfo != (MethodInfo)null && !methodInfo.IsPublic)
                methodInfo = (MethodInfo)null;
            return methodInfo;
        }

        internal static MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic, bool allowInternal)
        {
            if (property == (PropertyInfo)null)
                return (MethodInfo)null;
            MethodInfo methodInfo = property.SetMethod;
            if (!nonPublic && methodInfo != (MethodInfo)null && !methodInfo.IsPublic)
                methodInfo = (MethodInfo)null;
            return methodInfo;
        }

        private static bool IsMatch(ParameterInfo[] parameters, Type[] parameterTypes)
        {
            if (parameterTypes == null)
                parameterTypes = Helpers.EmptyTypes;
            if (parameters.Length != parameterTypes.Length)
                return false;
            for (int index = 0; index < parameters.Length; ++index)
            {
                if (parameters[index].ParameterType != parameterTypes[index])
                    return false;
            }
            return true;
        }

        internal static ConstructorInfo GetConstructor(Type type, Type[] parameterTypes, bool nonPublic)
        {
            return Helpers.GetConstructor(type.GetTypeInfo(), parameterTypes, nonPublic);
        }

        internal static ConstructorInfo GetConstructor(TypeInfo type, Type[] parameterTypes, bool nonPublic)
        {
            return ((IEnumerable<ConstructorInfo>)Helpers.GetConstructors(type, nonPublic)).SingleOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>)(ctor => Helpers.IsMatch(ctor.GetParameters(), parameterTypes)));
        }

        internal static ConstructorInfo[] GetConstructors(TypeInfo typeInfo, bool nonPublic)
        {
            return typeInfo.DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>)(c =>
            {
                if (!c.IsStatic)
                    return ((nonPublic ? 0 : (c.IsPublic ? 1 : 0)) | (nonPublic ? 1 : 0)) != 0;
                return false;
            })).ToArray<ConstructorInfo>();
        }

        internal static PropertyInfo GetProperty(Type type, string name, bool nonPublic)
        {
            return Helpers.GetProperty(type.GetTypeInfo(), name, nonPublic);
        }

        internal static PropertyInfo GetProperty(TypeInfo type, string name, bool nonPublic)
        {
            return type.GetDeclaredProperty(name);
        }
        internal static object ParseEnum(Type type, string value)
        {
            return Enum.Parse(type, value, true);
        }
  
        internal static MemberInfo[] GetInstanceFieldsAndProperties(Type type, bool publicOnly)
        {
            BindingFlags bindingAttr = publicOnly ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            PropertyInfo[] properties = type.GetProperties(bindingAttr);
            FieldInfo[] fields = type.GetFields(bindingAttr);
            MemberInfo[] memberInfoArray = new MemberInfo[fields.Length + properties.Length];
            properties.CopyTo((Array)memberInfoArray, 0);
            fields.CopyTo((Array)memberInfoArray, properties.Length);
            return memberInfoArray;
        }

        internal static Type GetMemberType(MemberInfo member)
        {
            PropertyInfo propertyInfo;
            if ((object)(propertyInfo = member as PropertyInfo) != null)
                return propertyInfo.PropertyType;
            FieldInfo fieldInfo = member as FieldInfo;
            if ((object)fieldInfo == null)
                return (Type)null;
            return fieldInfo.FieldType;
        }

        internal static bool IsAssignableFrom(Type target, Type type)
        {
            return target.IsAssignableFrom(type);
        }

        internal static Assembly GetAssembly(Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        internal static byte[] GetBuffer(MemoryStream ms)
        {
            ArraySegment<byte> buffer;
            if (!ms.TryGetBuffer(out buffer))
                throw new InvalidOperationException("Unable to obtain underlying MemoryStream buffer");
            if (buffer.Offset != 0)
                throw new InvalidOperationException("Underlying MemoryStream buffer was not zero-offset");
            return buffer.Array;
        }
  }
}