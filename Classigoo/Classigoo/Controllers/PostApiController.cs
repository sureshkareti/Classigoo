using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Classigoo;
using System.Data.Entity.Infrastructure;

namespace Classigoo.Controllers
{
    public class PostApiController : ApiController
    {
        // POST: api/PostApi
        [ResponseType(typeof(void))]
        public IHttpActionResult PostAdd(Add add)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    classigooEntities.Adds.Add(add);
                    classigooEntities.SaveChanges();
                }
            }
            catch(DbUpdateException)
            {

            }

            return StatusCode(HttpStatusCode.Created);
        }

        // POST: api/PostApi
        [ResponseType(typeof(void))]
        public IHttpActionResult PostRealEstate(RealEstate realEstate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    classigooEntities.RealEstates.Add(realEstate);
                    classigooEntities.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {

            }

            return StatusCode(HttpStatusCode.Created);
        }

    }
}
