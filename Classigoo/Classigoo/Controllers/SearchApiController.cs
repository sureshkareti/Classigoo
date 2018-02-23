using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Classigoo.Controllers
{
    public class SearchApiController : ApiController
    {
        private ClassigooEntities db = new ClassigooEntities();


        [HttpGet]
        [ActionName("GetAdds")]
        public IHttpActionResult GetAdds()
        {
            var adds = db.Adds.ToList();
            if (adds.Count > 0)
                return Ok(adds);
            else
                return NotFound();


        }
    }
}
