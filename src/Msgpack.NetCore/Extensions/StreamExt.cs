using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Msgpack.Extensions
{
    public static class StreamExt
    {
        public static TokenType PeekType(this Stream stream)
        {
            var firstByte = PeekFirstByte(stream);
            return firstByte.LookupType();
        }

        public static byte PeekFirstByte(this Stream stream)
        {
            var firstByte = stream.ReadByte();
            if (firstByte < 0)
                throw new EndOfStreamException();

            stream.Seek(-1, SeekOrigin.Current);
            return (byte) firstByte;
        }

        public static byte[] Take(this Stream stream, int count)
        {
            var buffer = new byte[count];
            stream.Read(buffer, 0, count);
            return buffer;
        }

        public static void WriteAll(this Stream stream, IEnumerable<byte> input)
        {
            var enumerable = input as byte[] ?? input.ToArray();
            stream.Write(enumerable.ToArray(), 0, enumerable.Count());
        }


        public static string ToHexString(this byte[] ba)
        {
            var hex = new StringBuilder();
            hex.Append("b'");
            foreach (var b in ba)
                hex.Append(@"\x" + b.ToString("X2"));
            hex.Append("'");
            return hex.ToString();
        }
    }
}