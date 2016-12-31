using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Msgpack.Exceptions;
using Msgpack.Extensions;

namespace Msgpack.Token
{
    public class MsgpackTokenParser
    {
        public static readonly MsgpackTokenParser Instance = new MsgpackTokenParser();

        public MToken ReadToken(Stream stream)
        {
            var type = stream.PeekType();
            switch (type)
            {
                case TokenType.Nil:
                    return ReadNil(stream);
                case TokenType.False:
                    return ReadFalse(stream);
                case TokenType.True:
                    return ReadTrue(stream);
                case TokenType.PositiveFixint:
                    return new MValue(type, ReadPositiveFixInt(stream));
                case TokenType.Uint8:
                    return new MValue(type, ReadUInt8(stream));
                case TokenType.Uint16:
                    return new MValue(type, ReadUInt16(stream));
                case TokenType.Uint32:
                    return new MValue(type, ReadUInt32(stream));
                case TokenType.Uint64:
                    return new MValue(type, ReadUInt64(stream));
                case TokenType.NegativeFixint:
                    return new MValue(type, ReadNegativeFixInt(stream));
                case TokenType.Int8:
                    return new MValue(type, ReadInt8(stream));
                case TokenType.Int16:
                    return new MValue(type, ReadInt16(stream));
                case TokenType.Int32:
                    return new MValue(type, ReadInt32(stream));
                case TokenType.Int64:
                    return new MValue(type, ReadInt64(stream));
                case TokenType.Float32:
                    return new MValue(type, ReadFloat32(stream));
                case TokenType.Float64:
                    return new MValue(type, ReadFloat64(stream));
                case TokenType.Binary8:
                    return new MValue(type, ReadBinary8(stream));
                case TokenType.Binary16:
                    return new MValue(type, ReadBinary16(stream));
                case TokenType.Binary32:
                    return new MValue(type, ReadBinary32(stream));

                //case TokenType.Fixext1:
                //case TokenType.Fixext2:
                //case TokenType.Fixext4:
                //case TokenType.Fixext8:
                //case TokenType.Fixext16:
                //    break;

                //case TokenType.Ext8:
                //case TokenType.Ext16:
                //case TokenType.Ext32:
                //    break;

                case TokenType.FixStr:
                    return new MValue(type, ReadFixStr(stream));
                case TokenType.Str8:
                    return new MValue(type, ReadStr8(stream));
                case TokenType.Str16:
                    return new MValue(type, ReadStr16(stream));
                case TokenType.Str32:
                    return new MValue(type, ReadStr32(stream));

                case TokenType.FixArray:
                    return ReadFixArray(stream);
                case TokenType.Array16:
                    return ReadArray16(stream);
                case TokenType.Array32:
                    return ReadArray32(stream);

                case TokenType.FixMap:
                    return ReadFixMap(stream);
                case TokenType.Map16:
                    return ReadMap16(stream);
                case TokenType.Map32:
                    return ReadMap32(stream);

                default:
                    throw new InvalidOperationException();
            }
        }

        private static MToken ReadTrue(Stream stream)
        {
            stream.Take(1);
            return MValue.True;
        }

        private static MToken ReadFalse(Stream stream)
        {
            stream.Take(1);
            return MValue.False;
        }

        private static MToken ReadNil(Stream stream)
        {
            stream.Take(1);
            return MValue.Nil;
        }

        private MArray ReadFixArray(Stream stream)
        {
            var size = ReadFixArraySize(stream);
            var elements = new List<MToken>();
            for (var i = 0; i < size; i++)
                elements.Add(ReadToken(stream));
            
            return new MArray(TokenType.FixArray, elements);
        }

        private MArray ReadArray16(Stream stream)
        {
            var size = ReadArray16Size(stream);
            var elements = new List<MToken>();
            for (var i = 0; i < size; i++)
                elements.Add(ReadToken(stream));
            
            return new MArray(TokenType.Array16, elements);
        }

        private MArray ReadArray32(Stream stream)
        {
            var size = ReadArray32Size(stream);
            var elements = new List<MToken>();
            for (var i = 0; i < size; i++)
                elements.Add(ReadToken(stream));
            
            return new MArray(TokenType.Array32, elements);
        }


        private MObject ReadFixMap(Stream stream)
        {
            var size = ReadFixMapSize(stream);
            var properties = new Dictionary<MToken, MToken>();
            for (var i = 0; i < size; i++)
                properties.Add(ReadToken(stream), ReadToken(stream));
   
            return new MObject(TokenType.FixMap, properties);
        }

        private MObject ReadMap16(Stream stream)
        {
            var size = ReadMap16Size(stream);
            var properties = new Dictionary<MToken, MToken>();
            for (var i = 0; i < size; i++)
                properties.Add(ReadToken(stream), ReadToken(stream));
   
            return new MObject(TokenType.Map16, properties);
        }

        private MObject ReadMap32(Stream stream)
        {
            var size = ReadMap32Size(stream);
            var properties = new Dictionary<MToken, MToken>();
            for (var i = 0; i < size; i++)
                properties.Add(ReadToken(stream), ReadToken(stream));
   
            return new MObject(TokenType.Map32, properties);
        }

        private string ReadStr8(Stream stream)
        {
            stream.Take(1);
            var size = stream.Take(1).Single();
            var value = Encoding.UTF8.GetString(stream.Take(size));
            return value;
        }

        private string ReadStr16(Stream stream)
        {
            stream.Take(1);
            var size = stream.ReadBigEndianUInt16();
            var value = Encoding.UTF8.GetString(stream.Take(size));
            return value;
        }

        private string ReadStr32(Stream stream)
        {
            stream.Take(1);
            var size = stream.ReadBigEndianUInt32();
            if (size > int.MaxValue)
                throw new MsgpackReaderException();
            var value = Encoding.UTF8.GetString(stream.Take((int)size));
            return value;
        }

        private string ReadFixStr(Stream stream)
        {
            var first = stream.Take(1).Single();
            var size = 160 ^ first;
            var value = Encoding.UTF8.GetString(stream.Take(size));
            return value;
        }

        private int ReadFixArraySize(Stream stream)
        {
            int value;
            var first = stream.Take(1).Single();
            value = 144 ^ first;
            return value;
        }

        private int ReadArray16Size(Stream stream)
        {
            int value;
            stream.Take(1);
            value = stream.ReadBigEndianUInt16();
            return value;
        }

        private int ReadArray32Size(Stream stream)
        {
            int value;
            stream.Take(1);
            var size = stream.ReadBigEndianUInt32();
            if (size > int.MaxValue)
                throw new MsgpackReaderException();

            value = (int)size;
            return value;
        }

        private int ReadFixMapSize(Stream stream)
        {
            var first = stream.Take(1).Single();
            return 128 ^ first;
        }

        private int ReadMap16Size(Stream stream)
        {
            int value;
            stream.Take(1);
            value = stream.ReadBigEndianUInt16();
            return value;
        }

        private int ReadMap32Size(Stream stream)
        {
            int value;
            stream.Take(1);
            var size = stream.ReadBigEndianUInt32();
            if (size > int.MaxValue)
                throw new MsgpackReaderException();
            value = (int)size;
            return value;
        }

        private byte[] ReadBinary8(Stream stream)
        {
            stream.Take(1);
            var size = stream.Take(1).Single();
            return stream.Take(size);
        }

        private byte[] ReadBinary16(Stream stream)
        {
            stream.Take(1);
            var size = stream.ReadBigEndianUInt16();
            return stream.Take(size);
        }

        private byte[] ReadBinary32(Stream stream)
        {
            stream.Take(1);
            var size = stream.ReadBigEndianUInt32();
            if (size > int.MaxValue)
                throw new MsgpackReaderException();
            return stream.Take((int)size);
        }


        private float ReadFloat32(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianFloat32();
        }

        private double ReadFloat64(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianFloat64();
        }

        private byte ReadPositiveFixInt(Stream stream)
        {
            return stream.Take(1).Single();
        }

        private sbyte ReadNegativeFixInt(Stream stream)
        {
            var first = stream.Take(1).Single();
            return (sbyte)first;
        }

        private byte ReadUInt8(Stream stream)
        {
            stream.Take(1);
            return stream.Take(1).Single();
        }

        private ushort ReadUInt16(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianUInt16();
        }

        private uint ReadUInt32(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianUInt32();
        }

        private ulong ReadUInt64(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianUInt64();
        }

        private sbyte ReadInt8(Stream stream)
        {
            stream.Take(1);
            return (sbyte)stream.Take(1).Single();
        }

        private short ReadInt16(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianInt16();
        }

        private int ReadInt32(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianInt32();
        }

        private long ReadInt64(Stream stream)
        {
            stream.Take(1);
            return stream.ReadBigEndianInt64();
        }
    }
}