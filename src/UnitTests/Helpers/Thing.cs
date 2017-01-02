using Msgpack;
using Msgpack.Attrributes;
using Msgpack.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Helpers
{
    [MsgpackTypeConverter(typeof(DecoratedThingConverter))]
    public class DecoratedThing
    {
        public string StringProperty { get; set; }
        public short ShortProperty { get; set; }
    }

    public class UndecoratedThing
    {
        public string StringProperty { get; set; }
        public short ShortProperty { get; set; }
    }

    public class DecoratedThingConverter : MsgpackConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DecoratedThing);
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var two = reader.ReadArraySize();
            return new DecoratedThing()
            {
                StringProperty = reader.ReadString(),
                ShortProperty = (short)reader.ReadShort(),
            };
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType, MsgpackConverterSettings settings)
        {
            writer.WriteArraySize(2);
            var thing = (DecoratedThing)value;
            writer.WriteValue(thing.StringProperty);
            writer.WriteValue(thing.ShortProperty);
        }
    }

    public class UndecoratedThingConverter : MsgpackConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(UndecoratedThing);
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var two = reader.ReadArraySize();
            return new UndecoratedThing()
            {
                StringProperty = reader.ReadString(),
                ShortProperty = (short)reader.ReadShort(),
            };
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType, MsgpackConverterSettings settings)
        {
            writer.WriteArraySize(2);
            var thing = (UndecoratedThing)value;
            writer.WriteValue(thing.StringProperty);
            writer.WriteValue(thing.ShortProperty);
        }
    }

}
