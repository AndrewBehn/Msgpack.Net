using Msgpack.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Msgpack.Token
{
    public static class RangeValidation
    {
        public static readonly IReadOnlyDictionary<TokenType, Type> TypeMap = new Dictionary<TokenType, Type>()
        {
            [TokenType.Nil] = typeof(object),
            [TokenType.True] = typeof(bool),
            [TokenType.False] = typeof(bool),
            [TokenType.PositiveFixint] = typeof(byte),
            [TokenType.Uint8] = typeof(byte),
            [TokenType.Uint16] = typeof(ushort),
            [TokenType.Uint32] = typeof(uint),
            [TokenType.Uint64] = typeof(ulong),
            [TokenType.NegativeFixint] = typeof(sbyte),
            [TokenType.Int8] = typeof(sbyte),
            [TokenType.Int16] = typeof(short),
            [TokenType.Int32] = typeof(int),
            [TokenType.Int64] = typeof(long),
            [TokenType.Float32] = typeof(float),
            [TokenType.Float64] = typeof(double),
            [TokenType.FixStr] = typeof(string),
            [TokenType.Str8] = typeof(string),
            [TokenType.Str16] = typeof(string),
            [TokenType.Str32] = typeof(string),
            [TokenType.Binary8] = typeof(byte[]),
            [TokenType.Binary16] = typeof(byte[]),
            [TokenType.Binary32] = typeof(byte[]),
        };
    }
    public class MValue : MToken
    {
        public static readonly MValue Nil = new MValue(TokenType.Nil, (object)null);
        public static readonly MValue True = new MValue(TokenType.True, true);
        public static readonly MValue False = new MValue(TokenType.False, false);

        private MValue(TokenType tokenType, object value) : base(tokenType)
        {
            // validateType
            if (!RangeValidation.TypeMap.ContainsKey(tokenType))
                throw new ArgumentException();

            if (value != null)
                if (RangeValidation.TypeMap[tokenType] != value.GetType())
                    throw new ArgumentException($"{tokenType} requires a value parameter of type {value.GetType()}");

            Value = value;
        }

        public MValue(TokenType type, bool value) : this(type, (object)value)
        {
            // todo - refactor
            if (type == TokenType.True && !value)
                throw new ArgumentException();

            if (type == TokenType.False && value)
                throw new ArgumentException();
        }

        public MValue(TokenType type, byte value) : this(type, (object)value) { }
        public MValue(TokenType type, ushort value) : this(type, (object)value) { }
        public MValue(TokenType type, uint value) : this(type, (object)value) { }
        public MValue(TokenType type, ulong value) : this(type, (object)value) { }
        public MValue(TokenType type, sbyte value) : this(type, (object)value) { }
        public MValue(TokenType type, short value) : this(type, (object)value) { }
        public MValue(TokenType type, int value) : this(type, (object)value) { }
        public MValue(TokenType type, long value) : this(type, (object)value) { }
        public MValue(TokenType type, float value) : this(type, (object)value) { }
        public MValue(TokenType type, double value) : this(type, (object)value) { }
        public MValue(TokenType type, string value) : this(type, (object)value) { }
        public MValue(TokenType type, byte[] value) : this(type, (object)value) { }

        public object Value { get; }

        public override string ToString() =>  string.Format($"{TokenType}: {Value}");

        public override void WriteTo(MsgpackWriter writer, MsgpackSerializer serializer = null)
        {
            serializer = serializer ?? new MsgpackSerializer();
            serializer.Serialize(writer, Value, Value.GetType());
        }

        public override IEnumerator<MToken> GetEnumerator() => new List<MToken>() { this }.GetEnumerator();

        public static MValue Parse(byte[] input) => (MValue)MTokenParser.Instance.ReadToken(input);

        public static MValue Load(MsgpackTokenReader reader) => (MValue)reader.ReadNext();
    }
}
