using Msgpack.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace Msgpack.Token
{
    public abstract class MToken : IEnumerable<MToken>
    {
        protected MToken(TokenType tokenType)
        {
            TokenType = tokenType;
        }

        public TokenType TokenType { get; }

        public byte[] ToBytes(MsgpackSerializer serializer = null)
        {
            using (var s = new MemoryStream())
            {
                var writer = new MsgpackStreamWriter(s);
                WriteTo(writer, null);
                return s.ToArray();
            }
        }

        public abstract void WriteTo(MsgpackWriter writer, MsgpackSerializer serializer = null);

        public T ToObject<T>(MsgpackSerializer serializer = null)
        {
            return (T)ToObject(typeof(T), serializer);
        }


        private object ToObject(Type type, MsgpackSerializer serializer = null)
        {
            var reader = new MsgpackTokenReader(this);
            serializer = serializer ?? new MsgpackSerializer();
            return serializer.DeserializeObject(reader, type);
        }
        
        public abstract IEnumerator<MToken> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
