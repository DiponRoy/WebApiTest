using System;
using Web.Api.Model;

namespace Web.Api.Sdk.Tests
{
    public class ApiResponseTmpl<TSource> : IResponse<TSource>
    {
        public bool IsSuccess { get; set; }
        public TSource Data { get; set; }
        public Exception Exception { get; set; }
    }
}
