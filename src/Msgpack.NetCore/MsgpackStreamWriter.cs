using System;
using System.IO;
using System.Text;
using Msgpack.Extensions;
using Msgpack.Token;

namespace Msgpack
{
    public class MsgpackStreamWriter : MsgpackWriter
    {
        private readonly Stream _stream;

        public MsgpackStreamWriter(Stream stream)
        {
            _stream = stream;
        }

        public override void WriteObjectSize(int numObjects)
        {
            if (numObjects < 16)
            {
                _stream.WriteByte((byte)(128 | (byte)numObjects));
                return;
            }
            if (numObjects <= ushort.MaxValue)
            {
                _stream.WriteByte(0xde);
                _stream.WriteAll(((ushort)numObjects).GetBigEndianBytes());
                return;
            }
            _stream.WriteByte(0xdf);
            _stream.WriteAll(((uint)numObjects).GetBigEndianBytes());
        }

        public override void WriteArraySize(int numObjects)
        {
            if (numObjects < 16)
            {
                _stream.WriteByte((byte)(144 | (byte)numObjects));
                return;
            }
            if (numObjects <= ushort.MaxValue)
            {
                _stream.WriteByte(0xdc);
                _stream.WriteAll(((ushort)numObjects).GetBigEndianBytes());
                return;
            }
            _stream.WriteByte(0xdd);
            _stream.WriteAll(((uint)numObjects).GetBigEndianBytes());
        }

        public override void WriteNil()
        {
            _stream.WriteAll(new byte[] { 0xc0 });
        }

        public override void WriteValue(sbyte? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteSignedInteger((sbyte)value);
        }

        public override void WriteValue(short? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteSignedInteger((short)value);
        }

        public override void WriteValue(int? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteSignedInteger((int)value);
        }

        public override void WriteValue(long? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteSignedInteger((long)value);
        }

        public override void WriteValue(byte? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteUnsignedInteger((byte)value);
        }

        public override void WriteValue(ushort? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteUnsignedInteger((ushort)value);
        }

        public override void WriteValue(uint? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteUnsignedInteger((uint)value);
        }

        public override void WriteValue(ulong? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteUnsignedInteger((ulong)value);
        }

        public override void WriteValue(bool? value)
        {
            if (value == null)
                WriteNil();
            else
                _stream.WriteAll((bool)value ? new byte[] { 0xc3 } : new byte[] { 0xc2 });
        }

        public override void WriteValue(float? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteFloatingPoint((float)value);
        }

        public override void WriteValue(double? value)
        {
            if (value == null)
                WriteNil();
            else
                WriteFloatingPoint((double)value);
        }

        public override void WriteValue(byte[] value)
        {
            if (value == null)
            {
                WriteNil();
                return;
            }

            var length = value.Length;
            if (length <= byte.MaxValue)
            {
                _stream.WriteByte(0xc4);
                _stream.WriteByte((byte)length);
                _stream.WriteAll(value);
                return;
            }
            if (length <= ushort.MaxValue)
            {
                _stream.WriteByte(0xc5);
                _stream.WriteAll(((ushort)length).GetBigEndianBytes());
                _stream.WriteAll(value);
                return;
            }

            _stream.WriteByte(0xc6);
            _stream.WriteAll(((uint)length).GetBigEndianBytes());
            _stream.WriteAll(value);
        }

        public override void WriteValue(string value)
        {
            if (value == null)
            {
                WriteNil();
                return;
            }

            var data = Encoding.UTF8.GetBytes(value);
            var length = data.Length;

            if (length < 32)
            {
                _stream.WriteByte((byte)(160 | length));
                _stream.WriteAll(data);
                return;
            }
            if (length <= byte.MaxValue)
            {
                _stream.WriteByte(0xd9);
                _stream.WriteByte((byte)length);
                _stream.WriteAll(data);
                return;
            }
            if (length <= ushort.MaxValue)
            {
                _stream.WriteByte(0xda);
                _stream.WriteAll(((ushort)length).GetBigEndianBytes());
                _stream.WriteAll(data);
                return;
            }
            _stream.WriteByte(0xdb);
            _stream.WriteAll(((uint)length).GetBigEndianBytes());
            _stream.WriteAll(data);
        }

        #region Private Parts

        private void WriteInt8(sbyte value)
        {
            _stream.WriteAll(new[] { (byte)0xd0, (byte)value });
        }

        private void WriteInt16(short value)
        {
            _stream.WriteByte(0xd1);
            _stream.WriteAll(value.GetBigEndianBytes());
        }

        private void WriteInt32(int value)
        {
            _stream.WriteByte(0xd2);
            _stream.WriteAll(value.GetBigEndianBytes());
        }

        private void WriteInt64(long value)
        {
            _stream.WriteByte(0xd3);
            _stream.WriteAll(value.GetBigEndianBytes());
        }

        private void WriteUInt8(byte value)
        {
            _stream.WriteAll(new byte[] { 0xcc, value });
        }

        private void WriteUInt16(ushort value)
        {
            _stream.WriteByte(0xcd);
            _stream.WriteAll(value.GetBigEndianBytes());
        }

        private void WriteUInt32(uint value)
        {
            _stream.WriteByte(0xce);
            _stream.WriteAll(value.GetBigEndianBytes());
        }


        private void WriteUInt64(ulong value)
        {
            _stream.WriteByte(0xcf);
            _stream.WriteAll(value.GetBigEndianBytes());
        }

        private void WriteFloatingPoint(double value)
        {
            if (value.WithinFloat())
                WriteFloat16((float)value);
            else
                WriteFloat32(value);
        }

        private void WriteFloat16(float value)
        {
            _stream.WriteByte(0xca);
            _stream.WriteAll(value.GetBigEndianBytes());
        }

        private void WriteFloat32(double value)
        {
            _stream.WriteByte(0xcb);
            _stream.WriteAll(value.GetBigEndianBytes());
        }


        private void WriteSignedInteger(long value)
        {
            if ((value < 128) && (value > 0))
                WritePositiveFixInt((byte)value);
            else if ((value > -33) && (value < 0))
                WriteNegativeFixInt((sbyte)value);
            else if (value.WithinSByte())
                WriteInt8((sbyte)value);
            else if (value.WithinShort())
                WriteInt16((short)value);
            else if (value.WithinInt())
                WriteInt32((int)value);
            else
                WriteInt64(value);
        }

        private void WriteUnsignedInteger(ulong value)
        {
            if (value < 128)
                WritePositiveFixInt((byte)value);

            else if (value.WithinByte())
                WriteUInt8((byte)value);
            else if (value.WithinUshort())
                WriteUInt16((ushort)value);
            else if (value.WithinUint())
                WriteUInt32((uint)value);
            else
                WriteUInt64(value);
        }

        private void WriteNegativeFixInt(sbyte value)
        {
            _stream.WriteByte((byte)value);
        }

        private void WritePositiveFixInt(byte value)
        {
            _stream.WriteByte(value);
        }

        #endregion
    }
}