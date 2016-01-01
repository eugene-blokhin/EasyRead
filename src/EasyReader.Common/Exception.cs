using System;
using System.Runtime.Serialization;

namespace EasyReader.Common
{
    [Serializable]
    public class Exception<T> : Exception
        where T : ExceptionData
    {
        public Exception()
        {
        }

        public Exception(T exceptionData)
        {
            ExceptionData = exceptionData;
        }

        public Exception(string message) : base(message)
        {
        }

        public Exception(string message, T exceptionData) : base(message)
        {
            ExceptionData = exceptionData;
        }

        public Exception(string message, Exception inner) : base(message, inner)
        {
        }

        public Exception(string message, T exceptionData, Exception inner) : base(message, inner)
        {
            ExceptionData = exceptionData;
        }

        protected Exception(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public T ExceptionData { get; }
    }

    [Serializable]
    public abstract class ExceptionData
    {
    }
}
