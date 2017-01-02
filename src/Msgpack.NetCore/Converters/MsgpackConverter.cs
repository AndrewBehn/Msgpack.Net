using System;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public abstract class MsgpackConverter
    {
        protected ConverterCache ConverterCache;

        protected MsgpackConverter(ConverterCache converterCache)
        {
            ConverterCache = converterCache;
        }

        public abstract bool CanConvert(Type objectType);

        public abstract void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackSerializerSettings settings);

        public abstract object ReadMsgpack(MsgpackReader reader, Type objectType);

        public T ReadMsgpack<T>(MsgpackReader reader)
        {
            return (T) ReadMsgpack(reader, typeof(T));
        }
    }
}