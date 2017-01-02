using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Msgpack.Extensions;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public class DictionaryConverter : MsgpackConverter
    {
        public DictionaryConverter(ConverterCache converterCache) : base(converterCache)
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsDictionary();
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackSerializerSettings settings)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            var keyPropertyInfo = objectType.GetProperty("Key");
            var valPropertyInfo = objectType.GetProperty("Value");

            var enumerable = (IEnumerable) value;

            var vals = enumerable as object[] ?? enumerable.Cast<object>().ToArray();
            writer.WriteObjectSize(vals.Length);
            foreach (var val in vals)
            {
                var keyValue = keyPropertyInfo.GetValue(val);
                var valValue = valPropertyInfo.GetValue(val);

                var keyConverter = ConverterCache.GetConverter(keyPropertyInfo.PropertyType);
                var valConverter = ConverterCache.GetConverter(valPropertyInfo.PropertyType);

                keyConverter.WriteMsgpack(writer, keyValue, keyPropertyInfo.PropertyType, settings);
                valConverter.WriteMsgpack(writer, valValue, valPropertyInfo.PropertyType, settings);
            }
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}