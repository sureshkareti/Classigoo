﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Classigoo.Controllers
{
    //Authorize]
    public class AddsController : ApiController
    {

        public IHttpActionResult GetAllAdds()
        {
            IList<tbl_Adds> Adds = null;

            using (var ctx = new ClassigooEntities())
            {

                Adds = ctx.tbl_Adds.ToList();
            }


            if (Adds.Count == 0)
            {
                return NotFound();
            }

            return Ok(Adds);
        }
    


    // GET api/values
    public IEnumerable<string> Get()
        {



            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
