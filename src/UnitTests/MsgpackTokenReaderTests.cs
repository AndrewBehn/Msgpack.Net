using Msgpack;
using Msgpack.Exceptions;
using Msgpack.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    //public class ParserConfig : IDisposable
    //{
    //    private MemoryStream _written = new MemoryStream();

    //    public ParserConfig()
    //    {
    //        Writer = new MsgpackStreamWriter(_written);
    //    }

    //    public MsgpackStreamWriter Writer { get; }
    //    public Stream AllBytes => new MemoryStream(_written.ToArray());
    //    public MToken ReadBack() => new MsgpackTokenParser().ReadToken(AllBytes);
    //    public void Dispose() => _written.Dispose();
    //}

    public class MsgpackTokenReaderTests
    {
        [Fact]
        public void ReadBoolFromNil()
        {
            var reader = new MsgpackTokenReader(MValue.Nil);
            var val = reader.ReadBool();
            Assert.Equal(null, val);
        }

        [Fact]
        public void ReadBoolFromTrue()
        {
            var reader = new MsgpackTokenReader(MValue.True);
            var boolVal = reader.ReadBool();
            Assert.Equal(true, (bool)boolVal);
        }

        [Fact]
        public void ReadBoolFromFalse()
        {
            var reader = new MsgpackTokenReader(MValue.False);
            var boolVal = reader.ReadBool();
            Assert.Equal(false, (bool)boolVal);
        }

        [Fact]
        public void ReadBoolFromStringFalse()
        {
            var reader = new MsgpackTokenReader(new MValue(TokenType.FixStr, "false"));
            var boolVal = reader.ReadBool();
            Assert.Equal(false, (bool)boolVal);

            reader = new MsgpackTokenReader(new MValue(TokenType.FixStr, "FALSE"));
            boolVal = reader.ReadBool();
            Assert.Equal(false, (bool)boolVal);
        }

        [Fact]
        public void ReadBoolFromStringTrue()
        {
            var reader = new MsgpackTokenReader(new MValue(TokenType.FixStr, "true"));
            var boolVal = reader.ReadBool();
            Assert.Equal(true, (bool)boolVal);

            reader = new MsgpackTokenReader(new MValue(TokenType.FixStr, "TRUE"));
            boolVal = reader.ReadBool();
            Assert.Equal(true, (bool)boolVal);
        }

        [Fact]
        public void ReadByte()
        {
            MsgpackTokenReader reader = null;

            reader = new MsgpackTokenReader(new MValue(TokenType.PositiveFixint, (byte)0));
            Assert.Equal(0, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Uint8, byte.MaxValue));
            Assert.Equal(byte.MaxValue, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Uint16, ushort.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());


            reader = new MsgpackTokenReader(new MValue(TokenType.Uint32, uint.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());

            reader = new MsgpackTokenReader(new MValue(TokenType.Uint64, ulong.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());

            reader = new MsgpackTokenReader(new MValue(TokenType.NegativeFixint, (sbyte)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());


            reader = new MsgpackTokenReader(new MValue(TokenType.Int8, (sbyte)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());

            reader = new MsgpackTokenReader(new MValue(TokenType.Int8, (sbyte)1));
            Assert.Equal(1, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Int8, sbyte.MaxValue));
            Assert.Equal((byte)sbyte.MaxValue, reader.ReadByte().Value);


            reader = new MsgpackTokenReader(new MValue(TokenType.Int16, (short)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());

            reader = new MsgpackTokenReader(new MValue(TokenType.Int16, (short)1));
            Assert.Equal(1, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Int16, short.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());


            reader = new MsgpackTokenReader(new MValue(TokenType.Int32, (int)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());
            
            reader = new MsgpackTokenReader(new MValue(TokenType.Int32, (int)1));
            Assert.Equal(1, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Int32, int.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());


            reader = new MsgpackTokenReader(new MValue(TokenType.Int64, (long)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());

            reader = new MsgpackTokenReader(new MValue(TokenType.Int64, (long)1));
            Assert.Equal(1, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Int64, long.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());
            

            reader = new MsgpackTokenReader(new MValue(TokenType.Float32, (float)1));
            Assert.Equal(1, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Float32, (float)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());

            reader = new MsgpackTokenReader(new MValue(TokenType.Float32, float.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());


            reader = new MsgpackTokenReader(new MValue(TokenType.Float64, (double)1));
            Assert.Equal(1, reader.ReadByte().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Float64, (double)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());
            
            reader = new MsgpackTokenReader(new MValue(TokenType.Float64, double.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadByte());
        }

        //TODO:  fill out for other types

        [Fact]
        public void ReadUShort()
        {
            MsgpackTokenReader reader = null;

            reader = new MsgpackTokenReader(new MValue(TokenType.PositiveFixint, (byte)0));
            Assert.Equal((ushort)0, reader.ReadUShort().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Uint8, byte.MaxValue));
            Assert.Equal(byte.MaxValue, reader.ReadUShort().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Uint16, ushort.MaxValue));
            Assert.Equal(ushort.MaxValue, reader.ReadUShort().Value);

            reader = new MsgpackTokenReader(new MValue(TokenType.Uint32, uint.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadUShort());

            reader = new MsgpackTokenReader(new MValue(TokenType.Uint64, ulong.MaxValue));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadUShort());

            reader = new MsgpackTokenReader(new MValue(TokenType.NegativeFixint, (sbyte)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadUShort());

            reader = new MsgpackTokenReader(new MValue(TokenType.Int8, (sbyte)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadUShort());

            reader = new MsgpackTokenReader(new MValue(TokenType.Int16, (short)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadUShort());

            reader = new MsgpackTokenReader(new MValue(TokenType.Int32, (int)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadUShort());

            reader = new MsgpackTokenReader(new MValue(TokenType.Int64, (long)-1));
            Assert.Throws<MsgpackReaderException>(() => reader.ReadUShort());
        }

    }
}
