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
using System.Globalization;

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
                    classigooEntities.FillAds(add.Category, add.SubCategory, add.State, add.District, add.Mandal, add.NearestArea, add.Title, add.Type, add.Status, add.UserId,add.Created,add.PostedBy, Output);

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
        [ActionName("UpdateAdd")]
        public IHttpActionResult UpdateAdd(Add add)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {

                    Add objAdd = classigooEntities.Adds.SingleOrDefault(a => a.AddId == add.AddId);

                    if (objAdd != null)
                    {
                        objAdd.Category = add.Category;
                        objAdd.SubCategory = add.SubCategory;
                        objAdd.State = add.State;
                        objAdd.District = add.District;
                        objAdd.Mandal = add.Mandal;
                        objAdd.NearestArea = add.NearestArea;
                        objAdd.Title = add.Title;
                        objAdd.Type = add.Type;


                        //classigooEntities.RealEstates.Attach(objRealestae);
                        //classigooEntities.Entry(objRealestae).Property(x => x.ImgUrlPrimary).IsModified = true;

                        int response = classigooEntities.SaveChanges();
                        if (response == 1)
                        {
                            return StatusCode(HttpStatusCode.OK);
                        }
                        else
                        {
                            return StatusCode(HttpStatusCode.ExpectationFailed);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Update Add", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.ExpectationFailed);
        }

        [HttpPost]
        [ActionName("DeleteAdd")]
        public IHttpActionResult DeleteAdd(string[] tempArray)
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
                    Add objAdd = classigooEntities.Adds.SingleOrDefault(a => a.AddId == id);

                    if (objAdd != null)
                    {

                        //CompleteAdd objCompleteAdd = new CompleteAdd();

                        ////objCompleteAdd.add = objAdd;
                        //objCompleteAdd.user = objAdd.User;
                        ////objCompleteAdd.agriculturalVehicle =(AgriculturalVehicle) objAdd.AgriculturalVehicles;
                        ////objCompleteAdd.transportationVehicle =(TransportationVehicle) objAdd.TransportationVehicles;
                        //if (objAdd.RealEstates.Count == 1)
                        //{
                        //    objCompleteAdd.realEstate = (RealEstate)objAdd.RealEstates.ToList()[0];
                        //}                                          
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

        [HttpPost]
        [ActionName("GetRealEstate")]
        public IHttpActionResult GetRealEstate(string[] addId)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId[0]);
                    RealEstate objRealestae = (RealEstate)classigooEntities.RealEstates.SingleOrDefault(a => a.AddId == id);

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

        [HttpGet]
        [ActionName("GetPV")]
        public IHttpActionResult GetPV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    PassengerVehicle objPassengerVehicle = classigooEntities.PassengerVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objPassengerVehicle != null)
                    {
                        return Ok(objPassengerVehicle);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Get Passenger Vehicles ", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [ActionName("GetCV")]
        public IHttpActionResult GetCV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    ConstructionVehicle objConstructionVehicle = classigooEntities.ConstructionVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objConstructionVehicle != null)
                    {
                        return Ok(objConstructionVehicle);
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Get ConstructionVehicle ", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [ActionName("GetTV")]
        public IHttpActionResult GetTV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    TransportationVehicle objTransportationVehicle = classigooEntities.TransportationVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objTransportationVehicle != null)
                    {
                        return Ok(objTransportationVehicle);
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Get TransportationVehicle ", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [ActionName("GetAV")]
        public IHttpActionResult GetAV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    AgriculturalVehicle objAgriculturalVehicle = classigooEntities.AgriculturalVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objAgriculturalVehicle != null)
                    {
                        return Ok(objAgriculturalVehicle);
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi Get AgriculturalVehicle ", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [ActionName("DeleteImage")]
        public IHttpActionResult DeleteImage(string[] addArray)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    #region RealEstate


                    if (addArray[0] == Constants.RealEstate)
                    {
                        int id = Convert.ToInt32(addArray[1]);
                        RealEstate objRealestae = classigooEntities.RealEstates.SingleOrDefault(a => a.AddId == id);


                        if (objRealestae != null)
                        {
                            if (addArray[2] == "1")
                            {
                                //objRealestae.ImgUrlPrimary = string.Empty;

                                objRealestae.ImgUrlPrimary = objRealestae.ImgUrlSeconday;
                                objRealestae.ImgUrlSeconday = objRealestae.ImgUrlThird;
                                objRealestae.ImgUrlThird = objRealestae.ImgUrlFourth;

                                objRealestae.ImgUrlFourth = string.Empty;

                            }
                            else if (addArray[2] == "2")
                            {
                                //objRealestae.ImgUrlSeconday = string.Empty;

                                objRealestae.ImgUrlSeconday = objRealestae.ImgUrlThird;
                                objRealestae.ImgUrlThird = objRealestae.ImgUrlFourth;
                                objRealestae.ImgUrlFourth = string.Empty;

                            }
                            else if (addArray[2] == "3")
                            {
                                //objRealestae.ImgUrlThird = string.Empty;

                                objRealestae.ImgUrlThird = objRealestae.ImgUrlFourth;
                                objRealestae.ImgUrlFourth = string.Empty;

                            }
                            else if (addArray[2] == "4")
                            {
                                objRealestae.ImgUrlFourth = string.Empty;

                            }

                            //classigooEntities.RealEstates.Attach(objRealestae);
                            //classigooEntities.Entry(objRealestae).Property(x => x.ImgUrlPrimary).IsModified = true;


                            int response = classigooEntities.SaveChanges();
                            if (response == 1)
                            {
                                RealEstate objRealestaeUpdated = classigooEntities.RealEstates.SingleOrDefault(a => a.AddId == id);
                                string[] allImages = new string[4];
                                allImages[0] = objRealestaeUpdated.ImgUrlPrimary;
                                allImages[1] = objRealestaeUpdated.ImgUrlSeconday;
                                allImages[2] = objRealestaeUpdated.ImgUrlThird;
                                allImages[3] = objRealestaeUpdated.ImgUrlFourth;

                                return Ok(allImages);
                                //return StatusCode(HttpStatusCode.OK);
                            }
                            else
                            {
                                return StatusCode(HttpStatusCode.ExpectationFailed);
                            }
                        }

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi delete image from database", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }


        [HttpPost]
        [ActionName("ChangeDefaultImage")]
        public IHttpActionResult ChangeDefaultImage(string[] addArray)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    #region RealEstate


                    if (addArray[0] == Constants.RealEstate)
                    {
                        int id = Convert.ToInt32(addArray[1]);
                        RealEstate objRealestae = classigooEntities.RealEstates.SingleOrDefault(a => a.AddId == id);


                        if (objRealestae != null)
                        {

                            if (addArray[2] == "2")
                            {
                                objRealestae.ImgUrlPrimary = objRealestae.ImgUrlSeconday;
                                objRealestae.ImgUrlSeconday = objRealestae.ImgUrlThird;
                                objRealestae.ImgUrlThird = objRealestae.ImgUrlFourth;

                                objRealestae.ImgUrlFourth = string.Empty;

                            }
                            else if (addArray[2] == "3")
                            {
                                objRealestae.ImgUrlPrimary = objRealestae.ImgUrlThird;
                                objRealestae.ImgUrlThird = objRealestae.ImgUrlFourth;
                                objRealestae.ImgUrlFourth = string.Empty;
                            }
                            else if (addArray[2] == "4")
                            {
                                objRealestae.ImgUrlPrimary = objRealestae.ImgUrlFourth;
                                objRealestae.ImgUrlFourth = string.Empty;

                            }

                            //classigooEntities.RealEstates.Attach(objRealestae);
                            //classigooEntities.Entry(objRealestae).Property(x => x.ImgUrlPrimary).IsModified = true;


                            int response = classigooEntities.SaveChanges();
                            if (response == 1)
                            {

                                RealEstate objRealestaeUpdated = classigooEntities.RealEstates.SingleOrDefault(a => a.AddId == id);
                                string[] allImages = new string[4];
                                allImages[0] = objRealestaeUpdated.ImgUrlPrimary;
                                allImages[1] = objRealestaeUpdated.ImgUrlSeconday;
                                allImages[2] = objRealestaeUpdated.ImgUrlThird;
                                allImages[3] = objRealestaeUpdated.ImgUrlFourth;

                                return Ok(allImages);
                            }
                            else
                            {
                                return StatusCode(HttpStatusCode.ExpectationFailed);
                            }
                        }

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At APi delete image from database", ex);
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }

            return StatusCode(HttpStatusCode.NotFound);
        }
    }
}
