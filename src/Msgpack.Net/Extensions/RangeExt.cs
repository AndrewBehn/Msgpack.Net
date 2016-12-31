namespace Msgpack.Extensions
{
    public static class RangeExt
    {
        public static bool WithinByte(this ulong val)
        {
            return /*val >= byte.MinValue &&*/ val <= byte.MaxValue;
        }

        public static bool WithinByte(this long val)
        {
            return (val >= byte.MinValue) && (val <= byte.MaxValue);
        }

        public static bool WithinByte(this double val)
        {
            return (val >= byte.MinValue) && (val <= byte.MaxValue);
        }

        public static bool WithinSByte(this ulong val)
        {
            return /*val >= byte.MinValue &&*/ val <= (ulong) sbyte.MaxValue;
        }

        public static bool WithinSByte(this long val)
        {
            return (val >= sbyte.MinValue) && (val <= sbyte.MaxValue);
        }

        public static bool WithinSByte(this double val)
        {
            return (val >= sbyte.MinValue) && (val <= sbyte.MaxValue);
        }

        public static bool WithinUshort(this ulong val)
        {
            return /*val >= byte.MinValue &&*/ val <= ushort.MaxValue;
        }

        public static bool WithinUshort(this long val)
        {
            return (val >= ushort.MinValue) && (val <= ushort.MaxValue);
        }

        public static bool WithinUshort(this double val)
        {
            return (val >= ushort.MinValue) && (val <= ushort.MaxValue);
        }


        public static bool WithinShort(this ulong val)
        {
            return /*val >= short.MinValue &&*/ val <= (ulong) short.MaxValue;
        }

        public static bool WithinShort(this long val)
        {
            return (val >= short.MinValue) && (val <= short.MaxValue);
        }

        public static bool WithinShort(this double val)
        {
            return (val >= short.MinValue) && (val <= short.MaxValue);
        }


        public static bool WithinUint(this ulong val)
        {
            return /*val >= byte.MinValue &&*/ val <= uint.MaxValue;
        }

        public static bool WithinUint(this long val)
        {
            return (val >= uint.MinValue) && (val <= uint.MaxValue);
        }

        public static bool WithinUint(this double val)
        {
            return (val >= uint.MinValue) && (val <= uint.MaxValue);
        }


        public static bool WithinInt(this ulong val)
        {
            return /*val >= byte.MinValue &&*/ val <= int.MaxValue;
        }

        public static bool WithinInt(this long val)
        {
            return (val >= int.MinValue) && (val <= int.MaxValue);
        }

        public static bool WithinInt(this double val)
        {
            return (val >= int.MinValue) && (val <= int.MaxValue);
        }

        public static bool WithinULong(this long val)
        {
            return val >= 0 /*&& (ulong)val <= ulong.MaxValue*/;
        }

        public static bool WithinULong(this double val)
        {
            return (val >= 0) && (val <= ulong.MaxValue);
        }


        public static bool WithinLong(this ulong val)
        {
            return /*val >= byte.MinValue &&*/ val <= long.MaxValue;
        }

        public static bool WithinLong(this double val)
        {
            return (val >= long.MinValue) && (val <= long.MaxValue);
        }


        public static bool WithinFloat(this ulong val)
        {
            return (val >= float.MinValue) && (val <= float.MaxValue);
        }

        public static bool WithinFloat(this long val)
        {
            return (val >= float.MinValue) && (val <= float.MaxValue);
        }

        public static bool WithinFloat(this double val)
        {
            return (val >= float.MinValue) && (val <= float.MaxValue);
        }


        public static bool WithinDouble(this ulong val)
        {
            return (val >= double.MinValue) && (val <= double.MaxValue);
        }

        public static bool WithinDouble(this long val)
        {
            return (val >= double.MinValue) && (val <= double.MaxValue);
        }
    }
}