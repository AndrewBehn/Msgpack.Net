using System;
using System.IO;
using Msgpack.Extensions;
using Msgpack.Serializer;
using Msgpack.Token;

namespace Msgpack
{
    public static class MsgpackConvert
    {
        private static readonly MsgpackTokenParser _parser = new MsgpackTokenParser();

        public static object Deserialize(byte[] input, Type objectType, MsgpackSerializerSettings settings = null)
        {
            var serializerSettings = settings ?? MsgpackSerializerSettings.Default;
            using (var stream = new MemoryStream(input))
            {
                //var hexSTring = input.ToHexString();
                var token = _parser.ReadToken(stream);
                var reader = new MsgpackTokenReader(token);
                var serializer = new MsgpackSerializer(serializerSettings);
                return serializer.DeserializeObject(reader, objectType);
            }
        }

        public static T Deserialize<T>(byte[] input, MsgpackSerializerSettings settings = null)
        {
            var serializerSettings = settings ?? MsgpackSerializerSettings.Default;
            using (var stream = new MemoryStream(input))
            {
                //var hexSTring = input.ToHexString();
                var token = _parser.ReadToken(stream);
                var reader = new MsgpackTokenReader(token);
                var serializer = new MsgpackSerializer(serializerSettings);
                return serializer.DeserializeObject<T>(reader);
            }
        }

        public static byte[] Serialize(object input, Type objectType, MsgpackSerializerSettings settings = null)
        {
            var serializerSettings = settings ?? MsgpackSerializerSettings.Default;
            using (var stream = new MemoryStream())
            {
                var writer = new MsgpackStreamWriter(stream);
                var serializer = new MsgpackSerializer(serializerSettings);
                serializer.Serialize(writer, input, objectType);

                return stream.ToArray();
            }
        }

        public static byte[] Serialize<T>(T input, MsgpackSerializerSettings settings = null)
        {
            var serializerSettings = settings ?? MsgpackSerializerSettings.Default;
            using (var stream = new MemoryStream())
            {
                var writer = new MsgpackStreamWriter(stream);
                var serializer = new MsgpackSerializer(serializerSettings);
                serializer.Serialize(writer, input);

                return stream.ToArray();
            }
        }
    }
}