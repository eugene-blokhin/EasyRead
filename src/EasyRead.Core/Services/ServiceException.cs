using System;
using System.Runtime.Serialization;
using EasyRead.Common;

namespace EasyRead.Core.Services
{
    public class ServiceException<T> : Exception<T> 
        where T : ServiceExceptionData
    {
        public ServiceException()
        {
        }

        public ServiceException(T exceptionData) : base(exceptionData)
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, T exceptionData) : base(message, exceptionData)
        {
        }

        public ServiceException(string message, Exception inner) : base(message, inner)
        {
        }

        public ServiceException(string message, T exceptionData, Exception inner) : base(message, exceptionData, inner)
        {
        }

        protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class ServiceExceptionData : ExceptionData { }
}
