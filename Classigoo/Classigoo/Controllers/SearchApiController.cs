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
        public IHttpActionResult GetAdds(string location,string category)
        {
            var adds = new List<Add>();
            if(location== "All India")
            {
                adds = db.Adds.ToList();
            }
            else
            {
                 //adds = db.Adds.Where(a=>a.Category==category).Where(a=>a.Location==location).ToList();
            }
            
            if (adds.Count > 0)
                return Ok(adds);
            else
                return NotFound();


        }
        //[HttpGet]
        //[ActionName("GetCategories")]
        //public IHttpActionResult GetCategories()
        //{
        //    List<Category> categoryColl = new List<Category>();
            
        //    categoryColl= db.Categories.ToList();
        //    if (categoryColl.Count > 0)
        //        return Ok(categoryColl);
        //    else
        //        return NotFound();
        //}

        //[HttpGet]
        //[ActionName("GetLocations")]
        //public IHttpActionResult GetLocations()
        //{
        //    List<Location> locationColl = new List<Location>();

        //    locationColl = db.Locations.ToList();

        //    if (locationColl.Count > 0)
        //        return Ok(locationColl);
        //    else
        //        return NotFound();
        //}
    }
}
