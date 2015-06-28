using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace Web.Api.Security
{
    public class HttpHeaderReader
    {
        public readonly IDictionary<string, IEnumerable<string>> Headers;
        public HttpHeaderReader(HttpActionContext actionContext)
        {
            Headers = actionContext.Request.Headers.ToDictionary(k => k.Key, v => v.Value);
        }

        public bool Has(string headerName)
        {
            return Headers.Any(x => x.Key == headerName);
        }

        public IEnumerable<string> Values(string headerName)
        {
            if (!Has(headerName))
            {
                return null;
            }
            return Headers.First(x => x.Key == headerName).Value;
        }

        public string Value(string headerName)
        {
            var values = Values(headerName);
            if (values == null)
            {
                return null;
            }
            return values.FirstOrDefault();
        }
    }
}