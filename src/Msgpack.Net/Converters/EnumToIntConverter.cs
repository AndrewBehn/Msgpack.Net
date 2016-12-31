using System;
using System.Reflection;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public class EnumToIntConverter : MsgpackConverter
    {
        public EnumToIntConverter(ConverterCache converterCache) : base(converterCache)
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.GetTypeInfo().IsEnum;
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackSerializerSettings settings)
        {
            var backingPrimitiveType = Enum.GetUnderlyingType(objectType);
            var convertedValue = Convert.ChangeType(value, backingPrimitiveType);

            var converter = ConverterCache.GetConverter(backingPrimitiveType);
            converter.WriteMsgpack(writer, convertedValue, backingPrimitiveType, settings);
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var backingPrimitiveType = Enum.GetUnderlyingType(objectType);
            var converter = ConverterCache.GetConverter(backingPrimitiveType);
            var primitiveValue = converter.ReadMsgpack(reader, backingPrimitiveType);

            var enumValues = Enum.GetValues(objectType);
            foreach (var v in enumValues)
                if (Convert.ChangeType(v, backingPrimitiveType).Equals(primitiveValue))
                    return v;

            throw new InvalidOperationException();
        }
    }
}