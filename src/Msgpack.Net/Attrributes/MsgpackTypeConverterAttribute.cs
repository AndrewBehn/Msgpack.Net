using Msgpack.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Msgpack.Attrributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class MsgpackTypeConverterAttribute : Attribute
    {
        public MsgpackTypeConverterAttribute(Type converterType)
        {
            if (!typeof(MsgpackConverter).IsAssignableFrom(converterType))
                throw new ArgumentException($"{converterType} does not inherit {typeof(MsgpackConverter)}");

            this.ConverterType = converterType;
        }

        public Type ConverterType { get; }
    }
}
