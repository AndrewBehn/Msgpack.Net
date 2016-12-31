using Msgpack;
using Msgpack.Token;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class ParserConfig : IDisposable
    {
        private MemoryStream _written = new MemoryStream();

        public ParserConfig()
        {
            Writer = new MsgpackStreamWriter(_written);
        }

        public MsgpackStreamWriter Writer { get; }
        public Stream AllBytes => new MemoryStream(_written.ToArray());
        public MToken ReadBack() => new MsgpackTokenParser().ReadToken(AllBytes);
        public void Dispose() => _written.Dispose();
    }

    public class MsgpackTokenParserTests
    {
        [Fact]
        public void ReadingNil()
        {
            using (var config = new ParserConfig())
            {
                config.Writer.WriteNil();
                var token = config.ReadBack();

                Assert.IsType<MValue>(token);
                Assert.Equal(TokenType.Nil, token.TokenType);
            }
        }

        #region boolean types
        [Fact]
        public void ReadingTrue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<bool>(true, config.Writer.WriteValue, TokenType.True);
            }
        }

        [Fact]
        public void ReadingFalse()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<bool>(false, config.Writer.WriteValue, TokenType.False);
            }
        }
        #endregion

        #region numeric types
        [Fact]
        public void ReadingPositiveFixintMinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<byte>(0, config.Writer.WriteValue, TokenType.PositiveFixint);
            }
        }

        [Fact]
        public void ReadingPositiveFixintMaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<byte>(127, config.Writer.WriteValue, TokenType.PositiveFixint);
            }
        }

        [Fact]
        public void ReadingUint8MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<byte>(128, config.Writer.WriteValue, TokenType.Uint8);
            }
        }

        [Fact]
        public void ReadingUint8MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<byte>(byte.MaxValue, config.Writer.WriteValue, TokenType.Uint8);
            }
        }

        [Fact]
        public void ReadingUint16MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<ushort>(byte.MaxValue + 1, config.Writer.WriteValue, TokenType.Uint16);
            }
        }

        [Fact]
        public void ReadingUint16MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<ushort>(ushort.MaxValue, config.Writer.WriteValue, TokenType.Uint16);
            }
        }

        [Fact]
        public void ReadingUint32MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<uint>(ushort.MaxValue + 1, config.Writer.WriteValue, TokenType.Uint32);
            }
        }

        [Fact]
        public void ReadingUint32MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<uint>(uint.MaxValue, config.Writer.WriteValue, TokenType.Uint32);
            }
        }

        [Fact]
        public void ReadingUint64MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<ulong>((ulong)uint.MaxValue + 1, config.Writer.WriteValue, TokenType.Uint64);
            }
        }

        [Fact]
        public void ReadingUint64MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<ulong>(ulong.MaxValue, config.Writer.WriteValue, TokenType.Uint64);
            }
        }


        [Fact]
        public void ReadingNegativeFixintMinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<sbyte>(-32, config.Writer.WriteValue, TokenType.NegativeFixint);
            }
        }

        [Fact]
        public void ReadingNegativeFixintMaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<sbyte>(-1, config.Writer.WriteValue, TokenType.NegativeFixint);
            }
        }


        [Fact]
        public void ReadingInt8MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<sbyte>(sbyte.MinValue, config.Writer.WriteValue, TokenType.Int8);
            }
        }

        [Fact]
        public void ReadingInt8MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<sbyte>(-33, config.Writer.WriteValue, TokenType.Int8);
            }
        }

        [Fact]
        public void ReadingInt16MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<short>(short.MinValue, config.Writer.WriteValue, TokenType.Int16);
            }
        }

        [Fact]
        public void ReadingInt16MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<short>(sbyte.MinValue - 1, config.Writer.WriteValue, TokenType.Int16);
            }
        }

        [Fact]
        public void ReadingInt32MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<int>(int.MinValue, config.Writer.WriteValue, TokenType.Int32);
            }
        }

        [Fact]
        public void ReadingInt32MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<int>(short.MinValue - 1, config.Writer.WriteValue, TokenType.Int32);
            }
        }

        [Fact]
        public void ReadingInt64MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<long>(long.MinValue, config.Writer.WriteValue, TokenType.Int64);
            }
        }

        [Fact]
        public void ReadingInt64MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<long>((long)int.MinValue - 1, config.Writer.WriteValue, TokenType.Int64);
            }
        }

        [Fact]
        public void ReadingFloat32MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<float>(float.MinValue, config.Writer.WriteValue, TokenType.Float32);
            }
        }

        [Fact]
        public void ReadingFloat32MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<float>(float.MaxValue, config.Writer.WriteValue, TokenType.Float32);
            }
        }

        [Fact]
        public void ReadingFloat64MinValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<double>(double.MinValue, config.Writer.WriteValue, TokenType.Float64);
            }
        }

        [Fact]
        public void ReadingFloat64MaxValue()
        {
            using (var config = new ParserConfig())
            {
                config.WriteReadAndAssertNullable<double>(double.MaxValue, config.Writer.WriteValue, TokenType.Float64);
            }
        }
        #endregion

        #region string types
        [Fact]
        public void ReadingFixStrValue()
        {
            using (var config = new ParserConfig())
            {
                var token = config.WriteReadAndAssertType("string", config.Writer.WriteValue, TokenType.FixStr);
            }
        }

        [Fact]
        public void ReadingEmptryStrValue()
        {
            WriteReadAndAssertString(0, TokenType.FixStr);
        }

        [Fact]
        public void ReadingMaxLengthFixStrValue()
        {
            WriteReadAndAssertString(31, TokenType.FixStr);
        }


        [Fact]
        public void ReadingStr8Min()
        {
            WriteReadAndAssertString(32, TokenType.Str8);
        }

        [Fact]
        public void ReadingStr8Max()
        {
            WriteReadAndAssertString(byte.MaxValue, TokenType.Str8);
        }

        [Fact]
        public void ReadingStr16Min()
        {
            WriteReadAndAssertString(byte.MaxValue + 1, TokenType.Str16);
        }

        [Fact]
        public void ReadingStr16Max()
        {
            WriteReadAndAssertString(ushort.MaxValue, TokenType.Str16);
        }

        [Fact]
        public void ReadingStr32Min()
        {
            WriteReadAndAssertString(ushort.MaxValue + 1, TokenType.Str32);
        }

        //[Fact]
        //public void ReadingStr32Max()
        //{
        //    WriteReadAndAssertString(uint.MaxValue , TokenType.Str32);
        //}


        public static void WriteReadAndAssertString(int length, TokenType type)
        {
            using (var config = new ParserConfig())
            {
                //var length = ushort.MaxValue + 1;
                var value = new string('*', (int)length);
                Assert.Equal(Encoding.UTF8.GetBytes(value).Length, length);
                var token = config.WriteReadAndAssertType(value, config.Writer.WriteValue, type);
            }
        }

        #endregion

        #region binary types

        [Fact]
        public void ReadingEmptryBinary8Min()
        {
            WriteReadAndAssertBinary(byte.MinValue, TokenType.Binary8);
        }

        [Fact]
        public void ReadingEmptryBinary8Max()
        {
            WriteReadAndAssertBinary(byte.MaxValue, TokenType.Binary8);
        }


        [Fact]
        public void ReadingEmptryBinary16Min()
        {
            WriteReadAndAssertBinary(byte.MaxValue + 1, TokenType.Binary16);
        }

        [Fact]
        public void ReadingEmptryBinary16Max()
        {
            WriteReadAndAssertBinary(ushort.MaxValue, TokenType.Binary16);
        }

        [Fact]
        public void ReadingEmptryBinary32Min()
        {
            WriteReadAndAssertBinary(ushort.MaxValue + 1, TokenType.Binary32);
        }

        public static void WriteReadAndAssertBinary(int length, TokenType type)
        {
            using (var config = new ParserConfig())
            {
                var value = new byte[length];
                var token = config.WriteReadAndAssertType(new byte[length], config.Writer.WriteValue, type);
            }
        }



        #endregion

        #region array types
        
        [Fact]
        public void ReadingFixArrayMin()
        {
            WriteReadAndAssertArray(0, TokenType.FixArray);
        }

        [Fact]
        public void ReadingFixArrayMax()
        {
            WriteReadAndAssertArray(15, TokenType.FixArray);
        }

        [Fact]
        public void ReadingArray16Min()
        {
            WriteReadAndAssertArray(16, TokenType.Array16);
        }

        [Fact]
        public void ReadingArray16Max()
        {
            WriteReadAndAssertArray(ushort.MaxValue, TokenType.Array16);
        }

        [Fact]
        public void ReadingArray32Min()
        {
            WriteReadAndAssertArray(ushort.MaxValue + 1, TokenType.Array32);
        }


        public static void WriteReadAndAssertArray(int length, TokenType type)
        {
            using (var c = new ParserConfig())
            {
                c.Writer.WriteArraySize(length);
                for (int i = 0; i < length; i++)
                    c.Writer.WriteNil();

                var token = c.ReadBack();
                Assert.IsType<MArray>(token);
                Assert.Equal(type, token.TokenType);

                var arrayToken = (MArray)token;
                Assert.True(arrayToken.Items.All(i => i.TokenType == TokenType.Nil));
            }
        }
        #endregion

        #region map types

        [Fact]
        public void ReadingFixMapMin()
        {
            WriteReadAndAssertMap(0, TokenType.FixMap);
        }

        [Fact]
        public void ReadingFixMapMax()
        {
            WriteReadAndAssertMap(15, TokenType.FixMap);
        }

        [Fact]
        public void ReadingMap16Min()
        {
            WriteReadAndAssertMap(16, TokenType.Map16);
        }

        [Fact]
        public void ReadingMap16Max()
        {
            WriteReadAndAssertMap(ushort.MaxValue, TokenType.Map16);
        }

        [Fact]
        public void ReadingMap32Min()
        {
            WriteReadAndAssertMap(ushort.MaxValue + 1, TokenType.Map32);
        }



        public static void WriteReadAndAssertMap(int length, TokenType type)
        {
            using (var c = new ParserConfig())
            {
                c.Writer.WriteObjectSize(length);
                for (int i = 0; i < length; i++)
                {
                    c.Writer.WriteValue(i);
                    c.Writer.WriteNil();
                }

                var token = c.ReadBack();
                Assert.IsType<MObject>(token);
                Assert.Equal(type, token.TokenType);

                var arrayToken = (MObject)token;
                Assert.True(arrayToken.Properties.All(i => i.Value.TokenType == TokenType.Nil));
            }
        }




        #endregion

    }
    public static class Ext
    {
        public static MValue WriteReadAndAssertType<T>(this ParserConfig config, T value, Action<T> writeAction, TokenType tokenType) where T : class
        {
            writeAction(value);
            var token = config.ReadBack();

            Assert.IsType<MValue>(token);
            Assert.Equal(tokenType, token.TokenType);

            var valToken = (MValue)token;
            Assert.Equal(value, (T)valToken.Value);
            return valToken;
        }

        public static MValue WriteReadAndAssertNullable<T>(this ParserConfig config, T value, Action<T?> writeAction, TokenType tokenType) where T : struct
        {
            writeAction(value);
            var token = config.ReadBack();

            Assert.IsType<MValue>(token);
            Assert.Equal(tokenType, token.TokenType);

            var valToken = (MValue)token;
            Assert.Equal(value, (T)valToken.Value);
            return valToken;
        }
    }
}
