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
        public IHttpActionResult GetAdds(string location)
        {
            var adds = new List<Add>();
            if(location== "All India")
            {
                adds = db.Adds.ToList();
            }
            else
            {
                 adds = db.Adds.Where(a => a.Location == location).ToList();
            }
            
            if (adds.Count > 0)
                return Ok(adds);
            else
                return NotFound();


        }
    }
}
