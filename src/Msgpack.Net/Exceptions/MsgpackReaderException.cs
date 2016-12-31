using System;

namespace Msgpack.Exceptions
{
    public class MsgpackReaderException : Exception
    {
        public MsgpackReaderException()
        {
        }

        public MsgpackReaderException(string message) : base(message)
        {
        }

        public MsgpackReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}