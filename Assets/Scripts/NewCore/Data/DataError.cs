using System;

namespace NewCore.Services
{
    public readonly struct DataError
    {
        public string Message { get; }
        public Exception Exception { get; }

        public DataError(string message, Exception exception = null) => (Message, Exception) = (message, exception);

        public DataError(Exception exception) : this(exception.Message, exception)
        {
        }
    }
}