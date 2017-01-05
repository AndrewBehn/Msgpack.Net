using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Msgpack.Exceptions;
using Msgpack.Extensions;
using Msgpack.Token;

namespace Msgpack
{
    public class MsgpackStreamReader : MsgpackReader
    {
        private MsgpackTokenReader _internalReader;

        public MsgpackStreamReader(Stream stream)
        {
            _internalReader = new MsgpackTokenReader(MTokenParser.Instance.ReadToken(stream));
        }

        public override int ReadArraySize() => _internalReader.ReadArraySize();

        public override bool? ReadBool() => _internalReader.ReadBool();

        public override byte? ReadByte() => _internalReader.ReadByte();

        public override byte[] ReadBytes() => _internalReader.ReadBytes();

        public override double? ReadDouble() => _internalReader.ReadDouble();

        public override float? ReadFloat() => _internalReader.ReadFloat();

        public override int? ReadInt() => _internalReader.ReadInt();

        public override long? ReadLong() => _internalReader.ReadLong();

        public override MToken ReadNext() => _internalReader.ReadNext();

        public override int ReadObjectSize() => _internalReader.ReadObjectSize();

        public override sbyte? ReadSByte() => _internalReader.ReadSByte();

        public override short? ReadShort() => _internalReader.ReadShort();

        public override string ReadString() => _internalReader.ReadString();

        public override uint? ReadUInt() => _internalReader.ReadUInt();

        public override ulong? ReadULong() => _internalReader.ReadULong();

        public override ushort? ReadUShort() => _internalReader.ReadUShort();
    }


    public class MsgpackTokenReader : MsgpackReader
    {
        public MsgpackTokenReader(MToken token)
        {
            _tokenEnumerator = token.GetEnumerator();
        }

        public MToken Token { get; private set; }

        private readonly IEnumerator<MToken> _tokenEnumerator;

        public override MToken ReadNext()
        {
            if (_tokenEnumerator.MoveNext())
            {
                Token = _tokenEnumerator.Current;
                return Token;
            }
            else
                throw new InvalidOperationException();
        }

        public override int ReadArraySize()
        {
            ReadNext();
            var array = Token as MArray;
            return array.Items.Count();
        }

        public override int ReadObjectSize()
        {
            ReadNext();
            var obj = Token as MObject;
            return obj.Properties.Count();
        }

        public override bool? ReadBool()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            bool boolValue;
            if (TryCoerceBool(out boolValue))
                return boolValue;

            string stringValue;
            if (TryCoerceString(out stringValue))
                switch (stringValue.ToLower())
                {
                    case "true":
                        return true;
                    case "false":
                        return false;
                }

            throw new MsgpackReaderException();
        }


        public override byte? ReadByte()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinByte())
                    return (byte)unsigned;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinByte())
                    return (byte)signed;
                else
                    throw new MsgpackReaderException();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinByte())
                    return (byte)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override ushort? ReadUShort()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinUshort())
                    return (ushort)unsigned;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinUshort())
                    return (ushort)signed;
                else
                    throw new MsgpackReaderException();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinUshort())
                    return (ushort)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override uint? ReadUInt()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinUint())
                    return (uint)unsigned;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinUint())
                    return (uint)signed;
                else
                    throw new MsgpackReaderException();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinUint())
                    return (uint)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override ulong? ReadULong()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                return unsigned;

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinULong())
                    return (ulong)signed;
                else
                    throw new MsgpackReaderException();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinULong())
                    return (ulong)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override sbyte? ReadSByte()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinSByte())
                    return (sbyte)unsigned;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinSByte())
                    return (sbyte)signed;
                else
                    throw new MsgpackReaderException();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinSByte())
                    return (sbyte)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override short? ReadShort()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinShort())
                    return (short)unsigned;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinShort())
                    return (short)signed;
                else
                    throw new MsgpackReaderException();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinShort())
                    return (short)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override int? ReadInt()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinInt())
                    return (int)unsigned;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinInt())
                    return (int)signed;
                else
                    throw new MsgpackReaderException();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinInt())
                    return (int)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override long? ReadLong()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinLong())
                    return (long)unsigned;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                return signed;

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinLong())
                    return (long)floating;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }

        public override string ReadString()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            string stringValue;
            if (TryCoerceString(out stringValue))
                return stringValue;

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                return unsigned.ToString();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                return unsigned.ToString();

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                return unsigned.ToString();

            bool boolValue;
            if (TryCoerceBool(out boolValue))
                return boolValue.ToString();

            throw new MsgpackReaderException();
        }

        public override byte[] ReadBytes()
        {
            ReadNext();
            byte[] binaryValue;
            if (TryCoerceBinary(out binaryValue))
                return binaryValue;

            throw new MsgpackReaderException();
        }

        public override float? ReadFloat()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                if (floating.WithinFloat())
                    return (float)floating;
                else
                    throw new MsgpackReaderException();

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinFloat())
                    return signed;
                else
                    throw new MsgpackReaderException();

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinFloat())
                    return unsigned;
                else
                    throw new MsgpackReaderException();
            throw new MsgpackReaderException();
        }

        public override double? ReadDouble()
        {
            ReadNext();
            if (TryCoerceNull())
                return null;

            double floating;
            if (TryCoerceFloatingPoint(out floating))
                return floating;

            long signed;
            if (TryCoerceSignedInteger(out signed))
                if (signed.WithinDouble())
                    return signed;
                else
                    throw new MsgpackReaderException();

            ulong unsigned;
            if (TryCoerceUnsignedInteger(out unsigned))
                if (unsigned.WithinDouble())
                    return unsigned;
                else
                    throw new MsgpackReaderException();

            throw new MsgpackReaderException();
        }


        private bool TryCoerceNull()
        {
            var mVal = Token as MValue;
            return mVal.Value == null;
        }

        private bool TryCoerceBool(out bool value)
        {
            var mVal = Token as MValue;
            switch (mVal.TokenType)
            {
                case TokenType.False:
                    value = false;
                    return true;
                case TokenType.True:
                    value = true;
                    return true;
            }
            value = default(bool);
            return false;
        }

        private bool TryCoerceUnsignedInteger(out ulong value)
        {
            //var type = _stream.PeekType();
            var mVal = Token as MValue;
            switch (mVal.TokenType)
            {
                case TokenType.PositiveFixint:
                case TokenType.Uint8:
                    {
                        value = (ulong)(byte)mVal.Value;
                        return true;
                    }
                case TokenType.Uint16:
                    {
                        value = (ulong)(ushort)mVal.Value;
                        return true;
                    }
                case TokenType.Uint32:
                    {
                        value = (ulong)(uint)mVal.Value;
                        return true;
                    }
                case TokenType.Uint64:
                    {
                        value = (ulong)mVal.Value;
                        return true;
                    }
            }
            value = default(ulong);
            return false;
        }

        private bool TryCoerceSignedInteger(out long value)
        {
            var mVal = Token as MValue;
            switch (mVal.TokenType)
            {
                case TokenType.NegativeFixint:
                    {
                        value = (long)(sbyte)mVal.Value;
                        return true;
                    }
                case TokenType.Int8:
                    {
                        value = (long)(sbyte)mVal.Value;
                        return true;
                    }
                case TokenType.Int16:
                    {
                        value = (long)(short)mVal.Value;
                        return true;
                    }
                case TokenType.Int32:
                    {
                        value = (long)(int)mVal.Value;
                        return true;
                    }
                case TokenType.Int64:
                    {
                        value = (long)mVal.Value;
                        return true;
                    }
            }
            value = default(long);
            return false;
        }

        private bool TryCoerceFloatingPoint(out double value)
        {
            var mVal = Token as MValue;
            switch (mVal.TokenType)
            {
                case TokenType.Float32:
                    {
                        value = (double)(float)mVal.Value;
                        return true;
                    }
                case TokenType.Float64:
                    {
                        value = (double)mVal.Value;
                        return true;
                    }
            }
            value = default(double);
            return false;
        }

        private bool TryCoerceString(out string value)
        {
            var mVal = Token as MValue;
            switch (mVal.TokenType)
            {
                case TokenType.FixStr:
                    {
                        value = (string)mVal.Value;
                        return true;
                    }
                case TokenType.Str8:
                    {
                        value = (string)mVal.Value;
                        return true;
                    }
                case TokenType.Str16:
                    {
                        value = (string)mVal.Value;
                        return true;
                    }
                case TokenType.Str32:
                    {
                        value = (string)mVal.Value;
                        return true;
                    }
            }
            value = null;
            return false;
        }

        private bool TryCoerceBinary(out byte[] value)
        {

            var mVal = Token as MValue;
            switch (mVal.TokenType)
            {
                case TokenType.Binary8:
                    {
                        value = (byte[])mVal.Value;
                        return true;
                    }
                case TokenType.Binary16:
                    {
                        value = (byte[])mVal.Value;
                        return true;
                    }
                case TokenType.Binary32:
                    {
                        value = (byte[])mVal.Value;
                        return true;
                    }
            }

            value = default(byte[]);
            return false;
        }
    }
}