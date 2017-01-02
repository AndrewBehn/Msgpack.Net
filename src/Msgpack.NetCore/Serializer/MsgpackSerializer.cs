using System;
using System.Collections.Generic;
using System.Linq;
using Msgpack.Converters;

namespace Msgpack.Serializer
{
    public class MsgpackSerializer
    {
        private readonly ConverterCache _cache = new ConverterCache();
        private readonly MsgpackSerializerSettings _settings;

        public MsgpackSerializer(MsgpackSerializerSettings settings)
        {
            _settings = settings;
        }

        public MsgpackSerializer() : this(MsgpackSerializerSettings.Default)
        {
        }

        public void Serialize<T>(MsgpackWriter writer, T value)
        {
            Serialize(writer, value, typeof(T));
        }

        public void Serialize(MsgpackWriter writer, object value, Type objectType)
        {
            var converter = _cache.GetConverter(objectType);
            converter.WriteMsgpack(writer, value, objectType, _settings);
        }

        public T DeserializeObject<T>(MsgpackReader reader)
        {
            return (T)DeserializeObject(reader, typeof(T));
        }

        public object DeserializeObject(MsgpackReader reader, Type objectType)
        {
            var converter = _cache.GetConverter(objectType);
            return converter.ReadMsgpack(reader, objectType);
        }
    }
}