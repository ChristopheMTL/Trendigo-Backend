using System;
using System.Runtime.Serialization;
using IMS.Common.Core.Enumerations;

namespace IMS.Common.Core.Exceptions
{
    [Serializable]
    public class IMSException : Exception
    {
        public IMSException()
        : base() { }
    
        public IMSException(string message)
            : base(message) { }
    
        public IMSException(string format, int errorCode)
            : base(string.Format(format, errorCode)) { }
    
        public IMSException(string message, Exception innerException)
            : base(message, innerException) { }

        public IMSException(IMSCodeMessage errorEnum) : this(errorEnum.ToString(), (int)errorEnum) { }
    
        public IMSException(string format, Exception innerException, int errorCode)
            : base(string.Format(format, errorCode), innerException) { }
    
        protected IMSException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public int ErrorCode { get; internal set; }
    }
}
