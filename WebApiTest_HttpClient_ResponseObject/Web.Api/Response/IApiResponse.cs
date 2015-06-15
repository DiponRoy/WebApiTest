using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Response
{
    public interface IApiResponse<TSource> : IResponse<TSource>
    {
        int StatusCode { get; set; }
    }
}
