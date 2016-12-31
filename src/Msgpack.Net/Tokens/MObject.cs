using Msgpack.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msgpack.Token
{
    public class MObject : MToken // MContainer
    {
        public MObject(TokenType type, IEnumerable<KeyValuePair<MToken, MToken>> properties) : base(type)
        {
            Properties = properties;
        }

        public IEnumerable<KeyValuePair<MToken, MToken>> Properties { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{TokenType}, Size = {Properties.Count()}");

            foreach (var p in Properties)
            {
                var propNameString = p.Key.ToString();

                builder.Append("\t" + propNameString + " : ");

                var propValueString = p.Value.ToString()
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                var first = true;
                foreach (var c in propValueString)
                    if (first)
                    {
                        builder.AppendLine(c);
                        first = false;
                    }
                    else
                    {
                        builder.AppendLine("\t" + c);
                    }
            }

            return builder.ToString();
        }

        public override void WriteTo(MsgpackWriter writer, MsgpackSerializer serializer = null)
        {
            serializer = serializer ?? new MsgpackSerializer();

            writer.WriteObjectSize(Properties.Count());
            foreach (var proeprty in Properties)
            {
                proeprty.Key.WriteTo(writer, serializer);
                proeprty.Value.WriteTo(writer, serializer);
            }
        }

        public override IEnumerator<MToken> GetEnumerator()
        {
            List<MToken> tokens = new List<MToken>() { this };
            foreach (var p in Properties)
            {
                tokens.AddRange(p.Key);
                tokens.AddRange(p.Value);
            }

            return tokens.GetEnumerator();
        }

        public static MObject Parse(byte[] input)
        {
            using (var stream = new MemoryStream(input))
            {
                var token = MsgpackTokenParser.Instance.ReadToken(stream);
                return (MObject)token;
            }
        }
    }
}
