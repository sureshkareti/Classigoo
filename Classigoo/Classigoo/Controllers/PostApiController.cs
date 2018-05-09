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
        [ActionName("PostAdd")]
        public int PostAdd(Add add)
        {
            int insertedAddId = 0;

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    ObjectParameter Output = new ObjectParameter("AddId", typeof(int));
                    classigooEntities.FillAds(add.Category, add.SubCategory,add.State,add.District,add.Mandal,add.NearestArea,add.Title,add.Type,add.Status,  add.UserId, Output);

                    int responceCode = classigooEntities.SaveChanges();
                    if (responceCode == 0)
                    {
                        insertedAddId = (int)Output.Value;
                    }
                }
            }
            catch (DbUpdateException)
            {
                return 0;
            }

            return insertedAddId;
        }

        [HttpPost]
        [ActionName("AgriculturalVehicle")]
        public IHttpActionResult AgriculturalVehicle(AgriculturalVehicle agriculturalVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    classigooEntities.AgriculturalVehicles.Add(agriculturalVehicle);
                    classigooEntities.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {

            }

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [ActionName("ConstructionVehicle")]
        public IHttpActionResult ConstructionVehicle(ConstructionVehicle constructionVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    classigooEntities.ConstructionVehicles.Add(constructionVehicle);
                    classigooEntities.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {

            }

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [ActionName("TransportationVehicle")]
        public IHttpActionResult TransportationVehicle(TransportationVehicle transportationVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    classigooEntities.TransportationVehicles.Add(transportationVehicle);
                    classigooEntities.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {

            }

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [ActionName("PassengerVehicle")]
        public IHttpActionResult PassengerVehicle(PassengerVehicle passengerVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    classigooEntities.PassengerVehicles.Add(passengerVehicle);
                    classigooEntities.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [ActionName("RealEstate")]
        public IHttpActionResult RealEstate(RealEstate realEstate)
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

        [HttpPost]
        [ActionName("RealEstate")]
        public IHttpActionResult DeleteAdd(string tyepe,string id)
        {
           
            try
            {

                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    //classigooEntities.RealEstates.Add(realEstate);
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
