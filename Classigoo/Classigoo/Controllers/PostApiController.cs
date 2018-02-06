using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Classigoo;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace Classigoo.Controllers
{
    public class PostApiController : ApiController
    {
        // POST: api/PostApi
        [HttpPost]     
        public int PostAdd(Add add)
        {
            int insertedAddId = 0;          

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    ObjectParameter Output = new ObjectParameter("AddId", typeof(int));
                    classigooEntities.FillAds(add.CategoryId, add.LocationId, add.UserId, Output);

                    int responceCode = classigooEntities.SaveChanges();
                    if (responceCode == 0)
                    {
                        insertedAddId =(int)Output.Value;
                    }                 
                }
            }
            catch(DbUpdateException)
            {
                return 0;
            }

            return insertedAddId;
        }

       
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
