using Msgpack.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Msgpack;
using Msgpack.Serializer;
using Msgpack.Token;
using Msgpack.Attrributes;
using System.IO;
using UnitTests.Helpers;

namespace UnitTests
{  

    public class TypeConverterAttributeTests
    {
        [Fact]
        public void SerializingTypeWithTypeConverterAttributeUsesDeclaredTypeConverter()
        {
            var bytes = MsgpackConvert.Serialize<DecoratedThing>(new DecoratedThing()
            {
                ShortProperty = 1,
                StringProperty = "propString"
            });

            using (var s = new MemoryStream(bytes))
            {
                var parser = new MTokenParser();
                var token = parser.ReadToken(s);
                var enumerator = token.GetEnumerator();

                enumerator.MoveNext();
                Assert.Equal(TokenType.FixArray, enumerator.Current.TokenType);
                enumerator.MoveNext();
                Assert.Equal(TokenType.FixStr, enumerator.Current.TokenType);
                enumerator.MoveNext();
                Assert.Equal(TokenType.PositiveFixint, enumerator.Current.TokenType);
            }
        }

        [Fact]
        public void DeserializingTypeWithTypeConverterAttributesUsesDeclaredTypeConverter()
        {
            using (var s = new MemoryStream())
            {
                var writer = new MsgpackStreamWriter(s);
                writer.WriteArraySize(2);
                writer.WriteValue("propString");
                writer.WriteValue((short)1);

                var thing = MsgpackConvert.Deserialize<DecoratedThing>(s.ToArray());
                Assert.Equal("propString", thing.StringProperty);
                Assert.Equal(1, thing.ShortProperty);
                Assert.Equal(s.Length, s.Position);

            }
        }
    }
}
