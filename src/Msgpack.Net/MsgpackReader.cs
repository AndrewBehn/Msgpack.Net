using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msgpack
{
    public abstract class MsgpackReader
    {
        public abstract int ReadArraySize();
        public abstract int ReadObjectSize();
        public abstract void ReadNext();
        public abstract bool? ReadBool();
        public abstract byte? ReadByte();
        public abstract ushort? ReadUShort();
        public abstract uint? ReadUInt();
        public abstract ulong? ReadULong();
        public abstract sbyte? ReadSByte();
        public abstract short? ReadShort();
        public abstract int? ReadInt();
        public abstract long? ReadLong();
        public abstract string ReadString();
        public abstract byte[] ReadBytes();
        public abstract float? ReadFloat();
        public abstract double? ReadDouble();
    }
}
