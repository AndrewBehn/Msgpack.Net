using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msgpack.Attrributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MsgpackIgnoreAttribute : Attribute
    {
    }
}
