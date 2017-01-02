using Msgpack.Attrributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Msgpack.Net.Extensions
{
    public static class AttributeExt
    {
        public static bool HasTypeConverterAttribute(this Type type) => type.GetTypeInfo().GetCustomAttribute<MsgpackTypeConverterAttribute>() != null;

        public static Type GetTypeConverterAttributeConverterType(this Type type) => type.GetTypeInfo().GetCustomAttribute<MsgpackTypeConverterAttribute>().ConverterType;
    }
}
