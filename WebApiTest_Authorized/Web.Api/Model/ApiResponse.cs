using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Api.Model
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