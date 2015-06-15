using System;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Web.Api.Response
{
    internal class ApiResponse<TSource> : Response<TSource>, IApiResponse<TSource>
    {
        public int StatusCode { get; set; }

        /*sets response data*/
        public ApiResponse(TSource data)
            : base(data)
        {
            StatusCode = 200;
        }

        /*sets error*/
        public ApiResponse(Exception exception)
            : base(exception)
        {
            StatusCode = 500;

        }
        public ApiResponse(HttpException httpException)
            : base(httpException)
        {
            StatusCode = httpException.GetHttpCode();
        }

    }
}