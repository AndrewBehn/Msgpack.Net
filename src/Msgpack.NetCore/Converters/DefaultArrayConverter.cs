using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public class DefaultArrayConverter : MsgpackConverter
    {
        public DefaultArrayConverter(ConverterCache converterCache) : base(converterCache)
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.GetTypeInfo().GetInterfaces().Any(i => i == typeof(IEnumerable));
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackSerializerSettings settings)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            var enumerable = (IEnumerable) value;

            var vals = enumerable as object[] ?? enumerable.Cast<object>().ToArray();
            writer.WriteArraySize(vals.Length);
            foreach (var val in vals)
            {
                var valType = val.GetType();
                var valConverter = ConverterCache.GetConverter(valType);
                valConverter.WriteMsgpack(writer, val, valType, MsgpackSerializerSettings.Default);
            }
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var itemType = objectType.GenericTypeArguments[0];
            var itemConverter = ConverterCache.GetConverter(itemType);
            var fullType = typeof(List<>).MakeGenericType(itemType);
            var list = (IList) Activator.CreateInstance(fullType);

            var size = reader.ReadArraySize();
            for (var i = 0; i < size; i++)
                list.Add(itemConverter.ReadMsgpack(reader, itemType));

            return list;
        }
    }
}