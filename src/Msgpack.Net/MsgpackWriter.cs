using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msgpack
{
    public abstract class MsgpackWriter
    {
        public abstract void WriteObjectSize(int numObjects);
        public abstract void WriteArraySize(int numObjects);
        public abstract void WriteNil();
        public abstract void WriteValue(sbyte? value);
        public abstract void WriteValue(short? value);
        public abstract void WriteValue(int? value);
        public abstract void WriteValue(long? value);
        public abstract void WriteValue(byte? value);
        public abstract void WriteValue(ushort? value);
        public abstract void WriteValue(uint? value);
        public abstract void WriteValue(ulong? value);
        public abstract void WriteValue(bool? value);
        public abstract void WriteValue(float? value);
        public abstract void WriteValue(double? value);
        public abstract void WriteValue(byte[] value);
        public abstract void WriteValue(string value);
    }
}
