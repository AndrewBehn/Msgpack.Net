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
                new PrimitiveConverter(),
                new NullableConverter(),
                new EnumToIntConverter(),
                new DefaultArrayConverter(),
                new DefaultObjectConverter()
            };

            foreach (var p in _cache)
                p.Initialize(this);
        }

        public ConverterCache(MsgpackConverter[] converters) : this()
        {
            if (converters != null)
            {
                foreach (var c in converters)
                {
                    c.Initialize(this);
                    _cache.Insert(0, c);
                }
            }
        }

        public MsgpackConverter GetConverter(Type key) => _cache.First(c => c.CanConvert(key));

        public static readonly ConverterCache Default = new ConverterCache();
    }
}
