using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msgpack
{
    public class MsgpackTextWriter : MsgpackWriter
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public string Value => _sb.ToString();

        public override void WriteObjectSize(int numObjects)
        {
            _sb.AppendLine($"Object Size: {numObjects}");
        }

        public override void WriteArraySize(int numObjects)
        {
            _sb.AppendLine($"Array Size: {numObjects}");
        }

        public override void WriteNil()
        {
            _sb.AppendLine("Nil");
        }

        public override void WriteValue(sbyte? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(short? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(int? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(long? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(byte? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(ushort? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(uint? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(ulong? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(bool? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(float? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(double? value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(byte[] value)
        {
            _sb.AppendLine(value.ToString());
        }

        public override void WriteValue(string value)
        {
            _sb.AppendLine(value);
        }
    }
}
