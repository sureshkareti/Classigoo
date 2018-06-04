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
using Classigoo.Models;

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
                    classigooEntities.FillAds(add.Category, add.SubCategory, add.State, add.District, add.Mandal, add.NearestArea, add.Title, add.Type, add.Status, add.UserId, Output);

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
            catch (Exception ex)
            {
                Library.WriteLog("At Api agricultural vehicles create", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
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
            catch (Exception ex)
            {
                Library.WriteLog("At Api construction vehicles create", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
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
            catch (Exception ex)
            {
                Library.WriteLog("At Api transportation vehicles create", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
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
            catch (Exception ex)
            {
                Library.WriteLog("At Api passenger vehicles create", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
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
            catch (Exception ex)
            {
                Library.WriteLog("At APi Realestate create", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [ActionName("DeleteAdd")]
        public IHttpActionResult DeleteAdd( string[] tempArray)
        {
            string type = tempArray[0];
            string id = tempArray[1];

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int addId = Convert.ToInt32(id);
                    Add objAdd = classigooEntities.Adds.Find(addId);

                    if (objAdd != null)
                    {
                        if (type == "Real Estate")
                        {
                            RealEstate objRealEstate = classigooEntities.RealEstates.First(x => x.AddId == objAdd.AddId);
                            if (objRealEstate != null)
                            {
                                classigooEntities.RealEstates.Remove(objRealEstate);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Construction Vehicles")
                        {
                            ConstructionVehicle objCV = classigooEntities.ConstructionVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objCV != null)
                            {
                                classigooEntities.ConstructionVehicles.Remove(objCV);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Transportation Vehicles")
                        {
                            TransportationVehicle objTV = classigooEntities.TransportationVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objTV != null)
                            {
                                classigooEntities.TransportationVehicles.Remove(objTV);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Agricultural Vehicles")
                        {
                            AgriculturalVehicle objAV = classigooEntities.AgriculturalVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objAV != null)
                            {
                                classigooEntities.AgriculturalVehicles.Remove(objAV);
                                classigooEntities.SaveChanges();
                            }

                        }
                        else if (type == "Passenger Vehicles")
                        {
                            PassengerVehicle objPV = classigooEntities.PassengerVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objPV != null)
                            {
                                classigooEntities.PassengerVehicles.Remove(objPV);
                                classigooEntities.SaveChanges();
                            }
                        }


                        classigooEntities.Adds.Remove(objAdd);
                        classigooEntities.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Delete Add", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.OK);
        }


        [HttpGet]
        [ActionName("GetAdd")]
        public IHttpActionResult GetAdd(string addId)
        {
           
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    Add objAdd = classigooEntities.Adds.Find(id);

                    if (objAdd != null)
                    {
                        return Ok(objAdd);
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Get Adds Table add", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [ActionName("GetRealEstate")]
        public IHttpActionResult GetRealEstate(string addId)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);                    
                    RealEstate objRealestae = (RealEstate)classigooEntities.RealEstates.Where(a => a.AddId == id).Select(x => x);

                    if (objRealestae != null)
                    {
                        return Ok(objRealestae);
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Get Realestate Table add", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }
    }
}
