using System.Collections.Generic;
using System.Web.Http;
using Db;

namespace Web.Api.Controllers
{
    public class ModelsController : ApiController
    {
        // POST api/models/add
        [HttpPost]
        public void Add([FromBody] User value)
        {
        }

        // POST api/models/remove
        [HttpPost]
        public void Remove([FromUri] User value)
        {
        }

        // PUT api/models/replace
        [HttpPut]
        public void Replace([FromUri] int id, [FromBody] User value)
        {
        }
    }
}
