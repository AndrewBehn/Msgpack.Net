using System;

namespace Msgpack.Exceptions
{
    public class MsgpackWriterException : Exception
    {
        public MsgpackWriterException()
        {
        }

        public MsgpackWriterException(string message) : base(message)
        {
        }

        public MsgpackWriterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}