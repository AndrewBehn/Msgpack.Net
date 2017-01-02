using System;
using System.Collections.Generic;
using System.Linq;
using Msgpack.Converters;
using Msgpack.Net.Extensions;

namespace Msgpack.Serializer
{
    public class MsgpackSerializer
    {
        private readonly ConverterCache _cache;
        private readonly MsgpackConverterSettings _settings;

        public MsgpackSerializer(MsgpackConverterSettings settings, MsgpackConverter[] converters = null)
        {
            _settings = settings;
            _cache = new ConverterCache(converters);
        }

        public MsgpackSerializer() : this(MsgpackConverterSettings.Default)
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
            MsgpackConverter converter = null;
            if (objectType.HasTypeConverterAttribute())
                converter = (MsgpackConverter)Activator.CreateInstance(objectType.GetTypeConverterAttributeConverterType());
            else
                converter = _cache.GetConverter(objectType);
            return converter.ReadMsgpack(reader, objectType);
        }
    }
}