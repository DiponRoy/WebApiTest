using System;
using Web.Api.Model;
using Web.Api.Model.Response;

namespace Web.Api.Tests.Models
{
    public class ApiResponseTmpl<TSource> : IResponse<TSource>
    {
        public bool IsSuccess { get; set; }
        public TSource Data { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
