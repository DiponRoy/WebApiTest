using System;

namespace Web.Api.Model.Response
{
    public interface IResponse<TSource>
    {
        bool IsSuccess { get; set; }
        TSource Data { get; set; }
        Exception Exception { get; set; }
        string Message { get; set; }
    }
}
