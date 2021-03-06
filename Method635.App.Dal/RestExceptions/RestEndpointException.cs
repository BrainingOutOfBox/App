﻿using System;
using System.Runtime.Serialization;

namespace Method635.App.Forms.RestAccess.RestExceptions
{
    [Serializable]
    public class RestEndpointException : Exception
    {
        public RestEndpointException()
        {
        }

        public RestEndpointException(string message) : base(message)
        {
        }

        public RestEndpointException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RestEndpointException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}