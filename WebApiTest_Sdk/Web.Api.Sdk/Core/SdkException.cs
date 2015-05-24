﻿using System;

namespace Web.Api.Sdk.Core
{
    public class SdkException : Exception
    {
        public SdkException(string message) : base(message)
        {
        }

        public SdkException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}