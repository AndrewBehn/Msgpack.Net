using System;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public abstract class MsgpackConverter
    {
        protected ConverterCache ConverterCache;

        protected MsgpackConverter() { }

        public void Initialize(ConverterCache cache)
        {
            ConverterCache = cache;
        }

        public abstract bool CanConvert(Type objectType);

        public abstract void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackConverterSettings settings);

        public abstract object ReadMsgpack(MsgpackReader reader, Type objectType);

        public T ReadMsgpack<T>(MsgpackReader reader)
        {
            return (T)ReadMsgpack(reader, typeof(T));
        }
    }
}