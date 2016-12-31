using System;
using System.Collections.Generic;
using Msgpack.Serializer;

namespace Msgpack.Converters
{
    public class PrimitiveConverter : MsgpackConverter
    {
        public static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap =
            new Dictionary<Type, PrimitiveTypeCode>
            {
                {typeof(bool), PrimitiveTypeCode.Boolean},
                {typeof(sbyte), PrimitiveTypeCode.SByte},
                {typeof(short), PrimitiveTypeCode.Int16},
                {typeof(ushort), PrimitiveTypeCode.UInt16},
                {typeof(int), PrimitiveTypeCode.Int32},
                {typeof(byte), PrimitiveTypeCode.Byte},
                {typeof(uint), PrimitiveTypeCode.UInt32},
                {typeof(long), PrimitiveTypeCode.Int64},
                {typeof(ulong), PrimitiveTypeCode.UInt64},
                {typeof(float), PrimitiveTypeCode.Single},
                {typeof(double), PrimitiveTypeCode.Double},
                {typeof(string), PrimitiveTypeCode.String},
                {typeof(byte[]), PrimitiveTypeCode.Bytes}
            };

        public PrimitiveConverter(ConverterCache converterCache) : base(converterCache)
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return TypeCodeMap.ContainsKey(objectType);
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackSerializerSettings settings)
        {
            PrimitiveTypeCode typeCode;
            if (!TypeCodeMap.TryGetValue(objectType, out typeCode))
                throw new InvalidOperationException();

            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean:
                    writer.WriteValue((bool) value);
                    break;
                case PrimitiveTypeCode.SByte:
                    writer.WriteValue((sbyte) value);
                    break;
                case PrimitiveTypeCode.Int16:
                    writer.WriteValue((short) value);
                    break;
                case PrimitiveTypeCode.UInt16:
                    writer.WriteValue((ushort) value);
                    break;
                case PrimitiveTypeCode.Int32:
                    writer.WriteValue((int) value);
                    break;
                case PrimitiveTypeCode.Byte:
                    writer.WriteValue((byte) value);
                    break;
                case PrimitiveTypeCode.UInt32:
                    writer.WriteValue((uint) value);
                    break;
                case PrimitiveTypeCode.Int64:
                    writer.WriteValue((long) value);
                    break;
                case PrimitiveTypeCode.UInt64:
                    writer.WriteValue((ulong) value);
                    break;
                case PrimitiveTypeCode.Single:
                    writer.WriteValue((float) value);
                    break;
                case PrimitiveTypeCode.Double:
                    writer.WriteValue((double) value);
                    break;
                case PrimitiveTypeCode.String:
                    writer.WriteValue((string) value);
                    break;
                case PrimitiveTypeCode.Bytes:
                    writer.WriteValue((byte[]) value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var typeCode = TypeCodeMap[objectType];
            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean:
                    return reader.ReadBool();
                case PrimitiveTypeCode.SByte:
                    return reader.ReadSByte();
                case PrimitiveTypeCode.Int16:
                    return reader.ReadShort();
                case PrimitiveTypeCode.UInt16:
                    return reader.ReadUShort();
                case PrimitiveTypeCode.Int32:
                    return reader.ReadInt();
                case PrimitiveTypeCode.Byte:
                    return reader.ReadByte();
                case PrimitiveTypeCode.UInt32:
                    return reader.ReadUInt();
                case PrimitiveTypeCode.Int64:
                    return reader.ReadLong();
                case PrimitiveTypeCode.UInt64:
                    return reader.ReadULong();
                case PrimitiveTypeCode.Single:
                    return reader.ReadFloat();
                case PrimitiveTypeCode.Double:
                    return reader.ReadDouble();
                case PrimitiveTypeCode.String:
                    return reader.ReadString();
                case PrimitiveTypeCode.Bytes:
                    return reader.ReadBytes();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}