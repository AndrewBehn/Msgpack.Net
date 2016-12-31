using System;

namespace Msgpack.Extensions
{
    public static class MsgpackTypeLookupExt
    {
        public static TokenType LookupType(this byte f)
        {
            if (f <= 0x7f)
                return TokenType.PositiveFixint;
            if ((0xe0 <= f) && (f <= 0xff))
                return TokenType.NegativeFixint;
            if ((0x80 <= f) && (f <= 0x8f))
                return TokenType.FixMap;
            if ((0x90 <= f) && (f <= 0x9f))
                return TokenType.FixArray;
            if ((0xa0 <= f) && (f <= 0xbf))
                return TokenType.FixStr;

            switch (f)
            {
                case 0xc0:
                    return TokenType.Nil;
                case 0xc1:
                    throw new InvalidOperationException();
                case 0xc2:
                    return TokenType.False;
                case 0xc3:
                    return TokenType.True;
                case 0xc4:
                    return TokenType.Binary8;
                case 0xc5:
                    return TokenType.Binary16;
                case 0xc6:
                    return TokenType.Binary32;
                //case 0xc7:
                //    return TokenType.Ext8;
                //case 0xc8:
                //    return TokenType.Ext16;
                //case 0xc9:
                //    return TokenType.Ext32;
                case 0xca:
                    return TokenType.Float32;
                case 0xcb:
                    return TokenType.Float64;
                case 0xcc:
                    return TokenType.Uint8;
                case 0xcd:
                    return TokenType.Uint16;
                case 0xce:
                    return TokenType.Uint32;
                case 0xcf:
                    return TokenType.Uint64;
                case 0xd0:
                    return TokenType.Int8;
                case 0xd1:
                    return TokenType.Int16;
                case 0xd2:
                    return TokenType.Int32;
                case 0xd3:
                    return TokenType.Int64;
                //case 0xd4:
                //    return TokenType.Fixext1;
                //case 0xd5:
                //    return TokenType.Fixext2;
                //case 0xd6:
                //    return TokenType.Fixext4;
                //case 0xd7:
                //    return TokenType.Fixext8;
                //case 0xd8:
                //    return TokenType.Fixext16;
                case 0xd9:
                    return TokenType.Str8;
                case 0xda:
                    return TokenType.Str16;
                case 0xdb:
                    return TokenType.Str32;
                case 0xdc:
                    return TokenType.Array16;
                case 0xdd:
                    return TokenType.Array32;
                case 0xde:
                    return TokenType.Map16;
                case 0xdf:
                    return TokenType.Map32;
                default:
                    throw new Exception();
            }
        }
    }
}