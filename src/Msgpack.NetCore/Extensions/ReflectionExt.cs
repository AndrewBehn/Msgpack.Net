using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Msgpack.Extensions
{
    public static class ReflectionExt
    {
        public static bool IsNullableType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static bool IsDictionary(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>));
        }

        public static Tuple<Type, Type> GetDictionaryTypes(Type dictionaryType)
        {
            return new Tuple<Type, Type>(dictionaryType.GetGenericArguments()[0],
                dictionaryType.GetGenericArguments()[1]);
        }

        public static IEnumerable<MemberInfo> GetSettablePropertiesAndFields(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            return
                type.GetFields(bindingFlags).Concat<MemberInfo>(type.GetProperties(bindingFlags).Where(p => p.CanWrite));
        }

        public static IEnumerable<MemberInfo> GetPropertiesAndFields(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            return type.GetFields(bindingFlags).Cast<MemberInfo>().Concat(type.GetProperties(bindingFlags));
        }
    }
}