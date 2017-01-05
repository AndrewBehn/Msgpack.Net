using System;
using System.IO;
using Msgpack.Extensions;
using Msgpack.Serializer;
using Msgpack.Token;
using Msgpack.Converters;

namespace Msgpack
{
    public static class MsgpackConvert
    {
        private static readonly MTokenParser _parser = new MTokenParser();

        public static object Deserialize(byte[] input, Type objectType, MsgpackConverter[] converters = null, MsgpackConverterSettings settings = null)
        {
            var serializerSettings = settings ?? MsgpackConverterSettings.Default;
            using (var stream = new MemoryStream(input))
            {
                var reader = new MsgpackStreamReader(stream);
                var serializer = new MsgpackSerializer(serializerSettings, converters);
                return serializer.DeserializeObject(reader, objectType);
            }
        }

        public static T Deserialize<T>(byte[] input, MsgpackConverter[] converters = null, MsgpackConverterSettings settings = null)
        {
            return (T)Deserialize(input, typeof(T), converters, settings);
        }

        public static byte[] Serialize(object input, Type objectType, MsgpackConverter[] converters = null, MsgpackConverterSettings settings = null)
        {
            var serializerSettings = settings ?? MsgpackConverterSettings.Default;
            using (var stream = new MemoryStream())
            {
                var writer = new MsgpackStreamWriter(stream);
                var serializer = new MsgpackSerializer(serializerSettings, converters);
                serializer.Serialize(writer, input, objectType);

                return stream.ToArray();
            }
        }

        public static byte[] Serialize<T>(T input, MsgpackConverter[] converters = null, MsgpackConverterSettings settings = null)
        {
            return Serialize(input, typeof(T), converters, settings);
        }
    }
}