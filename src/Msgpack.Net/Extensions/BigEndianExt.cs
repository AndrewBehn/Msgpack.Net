using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Msgpack.Extensions
{
    public static class BigEndianExt
    {
        public static IEnumerable<byte> GetBigEndianBytes(this ushort val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static IEnumerable<byte> GetBigEndianBytes(this uint val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static IEnumerable<byte> GetBigEndianBytes(this ulong val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static IEnumerable<byte> GetBigEndianBytes(this short val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static IEnumerable<byte> GetBigEndianBytes(this int val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static IEnumerable<byte> GetBigEndianBytes(this long val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static IEnumerable<byte> GetBigEndianBytes(this float val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static IEnumerable<byte> GetBigEndianBytes(this double val)
        {
            var bytes = BitConverter.GetBytes(val);
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        public static ushort ReadBigEndianUInt16(this Stream stream)
        {
            var bytes = stream.Take(2);
            return BitConverter.ToUInt16(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static uint ReadBigEndianUInt32(this Stream stream)
        {
            var bytes = stream.Take(4);
            return BitConverter.ToUInt32(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static ulong ReadBigEndianUInt64(this Stream stream)
        {
            var bytes = stream.Take(8);
            return BitConverter.ToUInt64(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static short ReadBigEndianInt16(this Stream stream)
        {
            var bytes = stream.Take(2);
            return BitConverter.ToInt16(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static int ReadBigEndianInt32(this Stream stream)
        {
            var bytes = stream.Take(4);
            return BitConverter.ToInt32(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static long ReadBigEndianInt64(this Stream stream)
        {
            var bytes = stream.Take(8);
            return BitConverter.ToInt64(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static float ReadBigEndianFloat32(this Stream stream)
        {
            var bytes = stream.Take(4);
            return BitConverter.ToSingle(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static double ReadBigEndianFloat64(this Stream stream)
        {
            var bytes = stream.Take(8);
            return BitConverter.ToDouble(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);
        }

        public static ushort ToBigEndianUInt16(this byte[] sizeBytes)
        {
            if (sizeBytes.Length != 2)
                throw new InvalidOperationException();

            return BitConverter.ToUInt16(BitConverter.IsLittleEndian ? sizeBytes.Reverse().ToArray() : sizeBytes, 0);
        }

        public static uint ToBigEndianUInt32(this byte[] sizeBytes)
        {
            if (sizeBytes.Length != 4)
                throw new InvalidOperationException();

            return BitConverter.ToUInt32(BitConverter.IsLittleEndian ? sizeBytes.Reverse().ToArray() : sizeBytes, 0);
        }
    }
}