using System;
using Msgpack.Extensions;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public class NullableConverter : MsgpackConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsNullableType();
        }

        public override void WriteMsgpack(MsgpackWriter streamWriter, object value, Type objectType,
            MsgpackConverterSettings settings)
        {
            if (value == null)
            {
                streamWriter.WriteNil();
                return;
            }

            var underlyingType = Nullable.GetUnderlyingType(objectType);
            var primitiveConverter = ConverterCache.GetConverter(underlyingType);
            primitiveConverter.WriteMsgpack(streamWriter, value, underlyingType, settings);
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var underlyingType = Nullable.GetUnderlyingType(objectType);
            var primitiveConverter = ConverterCache.GetConverter(underlyingType);
            return primitiveConverter.ReadMsgpack(reader, underlyingType);
        }
    }
}