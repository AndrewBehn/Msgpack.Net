using Msgpack.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msgpack
{
    public class ConverterCache 
    {
        private readonly List<MsgpackConverter> _cache;

        public ConverterCache()
        {
            _cache = new List<MsgpackConverter>
            {
                new PrimitiveConverter(this),
                new NullableConverter(this),
                new EnumToIntConverter(this),
                new DefaultArrayConverter(this),
                new DefaultObjectConverter(this)
            };
        }

        public MsgpackConverter GetConverter(Type key) => _cache.First(c => c.CanConvert(key));

        public void AddConverter(MsgpackConverter converter) => _cache.Insert(0, converter);

        public static readonly ConverterCache Default = new ConverterCache();
    }
}
