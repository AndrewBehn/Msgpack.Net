using Msgpack.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msgpack.Token
{
    public class MArray : MToken // MContainer
    {
        public MArray(TokenType type, IEnumerable<MToken> items) : base(type)
        {
            Items = items.ToArray();
        }

        public MToken[] Items { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{TokenType}, Size = {Items.Count()}");

            foreach (var i in Items)
            {
                var itemString = i.ToString()
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var c in itemString)
                    builder.AppendLine("\t" + c);
            }

            return builder.ToString();
        }

        public override void WriteTo(MsgpackWriter writer, MsgpackSerializer serializer = null)
        {
            writer.WriteArraySize(Items.Count());
            foreach (var item in Items)
                item.WriteTo(writer, serializer);
        }

        public static MArray Parse(byte[] input) => (MArray)MTokenParser.Instance.ReadToken(input);

        public static MArray Load(MsgpackReader reader) => (MArray)reader.ReadNext();

        public override IEnumerator<MToken> GetEnumerator()
        {
            List<MToken> tokens = new List<MToken>() { this };
            foreach (var i in Items)
            {
                tokens.AddRange(i);
            }

            return tokens.GetEnumerator();
        }
    }
}
