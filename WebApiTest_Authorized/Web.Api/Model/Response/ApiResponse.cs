using System;

namespace Web.Api.Model.Response
{
    public class ApiResponse<TSource> : Response<TSource>
    {
        /*sets response data*/
        public ApiResponse(TSource data)
            : base(data)
        {
        }

        /*sets error*/
        public ApiResponse(Exception exception)
            : base(exception)
        {
        }
    }
}