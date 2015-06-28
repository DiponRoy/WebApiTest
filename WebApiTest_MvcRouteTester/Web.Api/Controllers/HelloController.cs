using System.Web.Http;

namespace Web.Api.Controllers
{
    public class HelloController : ApiController
    {
        [HttpGet]
        public string Index()
        {
            return "Hello";
        }

        [HttpGet]
        [Route("api/hi")]
        public string Route()
        {
            return "Hi";
        }
    }
}
