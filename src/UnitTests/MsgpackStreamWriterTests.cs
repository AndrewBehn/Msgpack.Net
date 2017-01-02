using Msgpack;
using Msgpack.Token;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class StreamWriterConfig : IDisposable
    {
        private MemoryStream _written = new MemoryStream();

        public StreamWriterConfig()
        {
            Writer = new MsgpackStreamWriter(_written);
        }

        public MsgpackStreamWriter Writer { get; }
        public Stream AllBytes => new MemoryStream(_written.ToArray());
        public MToken ReadBack() => new MTokenParser().ReadToken(AllBytes);
        public void Dispose() => _written.Dispose();
    }

    public class MsgpackStreamWriterTests
    {
        [Fact]
        public void WritingMap16MinSize()
        {
            using (var config = new StreamWriterConfig())
            {
                config.Writer.WriteObjectSize(0);
                var token = config.ReadBack();
                Assert.Equal(TokenType.FixMap, token.TokenType);

                var obj = (MObject)token;
                Assert.Equal(0, obj.Properties.Count());

            }
        }


    }
}
