using System;
using System.Reflection;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public class EnumToStringConverter : MsgpackConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetTypeInfo().IsEnum;
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackConverterSettings settings)
        {
            var stringConverter = ConverterCache.GetConverter(typeof(string));
            stringConverter.WriteMsgpack(writer, value.ToString(), typeof(string), settings);
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var stringConverter = ConverterCache.GetConverter(typeof(string));
            var stringValue = (string)stringConverter.ReadMsgpack(reader, typeof(string));
            return Enum.Parse(objectType, stringValue);
        }
    }
}