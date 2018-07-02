using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;

namespace Classigoo.Models
{
    public class PostDBOperations
    {

        public int PostAdd(Add add)
        {
            int insertedAddId = 0;

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    ObjectParameter Output = new ObjectParameter("AddId", typeof(int));
                    classigooEntities.FillAds(add.Category, add.SubCategory, add.State, add.District, add.Mandal, add.NearestArea, add.Title, add.Type, add.Status, add.UserId, add.Created, Output);

                    int responceCode = classigooEntities.SaveChanges();
                    if (responceCode == 0)
                    {
                        insertedAddId = (int)Output.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at post add postdboperations", ex);
            }

            return insertedAddId;
        }

        public bool RealEstate(RealEstate realEstate)
        {
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
                Library.WriteLog("exception at adding realestate postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool AgriculturalVehicle(AgriculturalVehicle agriculturalVehicle)
        {
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
                Library.WriteLog("exception at adding AV postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool ConstructionVehicle(ConstructionVehicle constructionVehicle)
        {

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
                Library.WriteLog("exception at adding CV postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool TransportationVehicle(TransportationVehicle transportationVehicle)
        {

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
                Library.WriteLog("exception at adding TV postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool PassengerVehicle(PassengerVehicle passengerVehicle)
        {

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
                Library.WriteLog("exception at adding PV postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool DeleteAdd(string[] tempArray)
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

                                DeleteImageLocal(new List<string>() { objRealEstate.ImgUrlPrimary, objRealEstate.ImgUrlSeconday, objRealEstate.ImgUrlThird, objRealEstate.ImgUrlFourth });


                                classigooEntities.RealEstates.Remove(objRealEstate);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Construction Vehicles")
                        {
                            ConstructionVehicle objCV = classigooEntities.ConstructionVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objCV != null)
                            {
                                DeleteImageLocal(new List<string>() { objCV.ImgUrlPrimary, objCV.ImgUrlSeconday, objCV.ImgUrlThird, objCV.ImgUrlFourth });


                                classigooEntities.ConstructionVehicles.Remove(objCV);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Transportation Vehicles")
                        {
                            TransportationVehicle objTV = classigooEntities.TransportationVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objTV != null)
                            {
                                DeleteImageLocal(new List<string>() { objTV.ImgUrlPrimary, objTV.ImgUrlSeconday, objTV.ImgUrlThird, objTV.ImgUrlFourth });

                                classigooEntities.TransportationVehicles.Remove(objTV);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Agricultural Vehicles")
                        {
                            AgriculturalVehicle objAV = classigooEntities.AgriculturalVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objAV != null)
                            {
                                DeleteImageLocal(new List<string>() { objAV.ImgUrlPrimary, objAV.ImgUrlSeconday, objAV.ImgUrlThird, objAV.ImgUrlFourth });

                                classigooEntities.AgriculturalVehicles.Remove(objAV);
                                classigooEntities.SaveChanges();
                            }

                        }
                        else if (type == "Passenger Vehicles")
                        {
                            PassengerVehicle objPV = classigooEntities.PassengerVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objPV != null)
                            {
                                DeleteImageLocal(new List<string>() { objPV.ImgUrlPrimary, objPV.ImgUrlSeconday, objPV.ImgUrlThird, objPV.ImgUrlFourth });

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
                Library.WriteLog("exception at delete add postdboperations", ex);
                return false;

            }

            return true;
        }

        public bool DeleteAdd(string addIdForDelete)
        {
            string id = addIdForDelete;
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int addId = Convert.ToInt32(id);
                    Add objAdd = classigooEntities.Adds.Find(addId);
                    string type = objAdd.Category;
                    if (objAdd != null)
                    {
                        if (type == "Real Estate")
                        {
                            RealEstate objRealEstate = classigooEntities.RealEstates.First(x => x.AddId == objAdd.AddId);
                            if (objRealEstate != null)
                            {

                                DeleteImageLocal(new List<string>() { objRealEstate.ImgUrlPrimary, objRealEstate.ImgUrlSeconday, objRealEstate.ImgUrlThird, objRealEstate.ImgUrlFourth });


                                classigooEntities.RealEstates.Remove(objRealEstate);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Construction Vehicles")
                        {
                            ConstructionVehicle objCV = classigooEntities.ConstructionVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objCV != null)
                            {
                                DeleteImageLocal(new List<string>() { objCV.ImgUrlPrimary, objCV.ImgUrlSeconday, objCV.ImgUrlThird, objCV.ImgUrlFourth });


                                classigooEntities.ConstructionVehicles.Remove(objCV);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Transportation Vehicles")
                        {
                            TransportationVehicle objTV = classigooEntities.TransportationVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objTV != null)
                            {
                                DeleteImageLocal(new List<string>() { objTV.ImgUrlPrimary, objTV.ImgUrlSeconday, objTV.ImgUrlThird, objTV.ImgUrlFourth });

                                classigooEntities.TransportationVehicles.Remove(objTV);
                                classigooEntities.SaveChanges();
                            }
                        }
                        else if (type == "Agricultural Vehicles")
                        {
                            AgriculturalVehicle objAV = classigooEntities.AgriculturalVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objAV != null)
                            {
                                DeleteImageLocal(new List<string>() { objAV.ImgUrlPrimary, objAV.ImgUrlSeconday, objAV.ImgUrlThird, objAV.ImgUrlFourth });

                                classigooEntities.AgriculturalVehicles.Remove(objAV);
                                classigooEntities.SaveChanges();
                            }

                        }
                        else if (type == "Passenger Vehicles")
                        {
                            PassengerVehicle objPV = classigooEntities.PassengerVehicles.First(x => x.AddId == objAdd.AddId);
                            if (objPV != null)
                            {
                                DeleteImageLocal(new List<string>() { objPV.ImgUrlPrimary, objPV.ImgUrlSeconday, objPV.ImgUrlThird, objPV.ImgUrlFourth });

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
                Library.WriteLog("exception at delete complete add postdboperations", ex);
                return false;

            }

            return true;
        }

        public bool DeleteAddWhenCategoryChange(string addIdForDelete)
        {
            string id = addIdForDelete;
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int addId = Convert.ToInt32(id);
                    Add objAdd = classigooEntities.Adds.Find(addId);
                    string type = objAdd.Category;
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
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at delete for categorychnage add postdboperations", ex);
                return false;

            }

            return true;
        }

        public bool DeleteImageLocal(List<string> urls)
        {
            string domain = string.Empty;
            foreach (string url in urls)
            {
                try
                {
                    //this is for delete localImage
                    domain = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + url;

                    //this is for delete subdomain image
                    //string[] pathSplits = Server.MapPath("~").Split('\\');
                    //string urlForSubdomain = url.Remove(0, 7);
                    //domain = pathSplits[0] + "\\" + pathSplits[1] + "\\" + pathSplits[2] + "\\" + urlForSubdomain;

                    FileInfo file = new FileInfo(domain);

                    if (file.Exists)//check file exsit or not
                    {
                        file.Delete();

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Library.WriteLog("At Deleting physical image ImgName - " + domain, ex);

                }
            }

            return false;
        }

        public bool UpdateAdd(Add add)
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

                        int response = classigooEntities.SaveChanges();
                        if (response == 1)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update add postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool UpdateRealEstate(RealEstate realEstate)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(realEstate.AddId);
                    RealEstate objRealestae = (RealEstate)classigooEntities.RealEstates.SingleOrDefault(a => a.AddId == id);

                    if (objRealestae != null)
                    {
                        objRealestae.SubCategory = realEstate.SubCategory;

                        objRealestae.Price = Convert.ToInt32(realEstate.Price);
                        objRealestae.Availability = realEstate.Availability;
                        objRealestae.ListedBy = realEstate.ListedBy;
                        objRealestae.Furnishing = realEstate.Furnishing;
                        objRealestae.Bedrooms = realEstate.Bedrooms;
                        objRealestae.SquareFeets = Convert.ToInt32(realEstate.SquareFeets);
                        objRealestae.Squareyards = Convert.ToInt32(realEstate.Squareyards);
                        objRealestae.Acres = Convert.ToDecimal(realEstate.Acres);
                        objRealestae.Description = realEstate.Description;

                        if (realEstate.ImgUrlPrimary != string.Empty)
                            objRealestae.ImgUrlPrimary = realEstate.ImgUrlPrimary;

                        if (realEstate.ImgUrlSeconday != string.Empty)
                            objRealestae.ImgUrlSeconday = realEstate.ImgUrlSeconday;

                        if (realEstate.ImgUrlThird != string.Empty)
                            objRealestae.ImgUrlThird = realEstate.ImgUrlThird;

                        if (realEstate.ImgUrlFourth != string.Empty)
                            objRealestae.ImgUrlFourth = realEstate.ImgUrlFourth;

                        int response = classigooEntities.SaveChanges();
                        if (response == 1)
                        {
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update realestate postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool UpdateAV(AgriculturalVehicle agriculturalV)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(agriculturalV.AddId);
                    AgriculturalVehicle objAV = (AgriculturalVehicle)classigooEntities.AgriculturalVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objAV != null)
                    {
                        objAV.Company = agriculturalV.Company;
                        objAV.OtherCompany = agriculturalV.OtherCompany;
                        objAV.SubCategory = agriculturalV.SubCategory;

                        objAV.Model = agriculturalV.Model;
                        objAV.ManufacturingYear = agriculturalV.ManufacturingYear;

                        objAV.Price = Convert.ToInt32(agriculturalV.Price);
                        objAV.Description = agriculturalV.Description;

                        if (agriculturalV.ImgUrlPrimary != string.Empty)
                            objAV.ImgUrlPrimary = agriculturalV.ImgUrlPrimary;

                        if (agriculturalV.ImgUrlSeconday != string.Empty)
                            objAV.ImgUrlSeconday = agriculturalV.ImgUrlSeconday;

                        if (agriculturalV.ImgUrlThird != string.Empty)
                            objAV.ImgUrlThird = agriculturalV.ImgUrlThird;

                        if (agriculturalV.ImgUrlFourth != string.Empty)
                            objAV.ImgUrlFourth = agriculturalV.ImgUrlFourth;

                        int response = classigooEntities.SaveChanges();
                        if (response == 1)
                        {
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update AgriculturalVehicle postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool UpdateTV(TransportationVehicle transportationV)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(transportationV.AddId);
                    TransportationVehicle objTV = (TransportationVehicle)classigooEntities.TransportationVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objTV != null)
                    {
                        objTV.Company = transportationV.Company;
                        objTV.OtherCompany = transportationV.OtherCompany;
                        objTV.SubCategory = transportationV.SubCategory;

                        objTV.Model = transportationV.Model;
                        objTV.ManufacturingYear = transportationV.ManufacturingYear;

                        objTV.Price = Convert.ToInt32(transportationV.Price);
                        objTV.Description = transportationV.Description;

                        if (transportationV.ImgUrlPrimary != string.Empty)
                            objTV.ImgUrlPrimary = transportationV.ImgUrlPrimary;

                        if (transportationV.ImgUrlSeconday != string.Empty)
                            objTV.ImgUrlSeconday = transportationV.ImgUrlSeconday;

                        if (transportationV.ImgUrlThird != string.Empty)
                            objTV.ImgUrlThird = transportationV.ImgUrlThird;

                        if (transportationV.ImgUrlFourth != string.Empty)
                            objTV.ImgUrlFourth = transportationV.ImgUrlFourth;

                        int response = classigooEntities.SaveChanges();
                        if (response == 1)
                        {
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update TransportationVehicle postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool UpdatePV(PassengerVehicle passengerV)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(passengerV.AddId);
                    PassengerVehicle objPV = (PassengerVehicle)classigooEntities.PassengerVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objPV != null)
                    {
                        objPV.Company = passengerV.Company;
                        objPV.OtherCompany = passengerV.OtherCompany;
                        objPV.SubCategory = passengerV.SubCategory;

                        objPV.Model = passengerV.Model;

                        objPV.Year = passengerV.Year;

                        objPV.FuelType = passengerV.FuelType;
                        objPV.KMDriven = passengerV.KMDriven;

                        objPV.Price = Convert.ToInt32(passengerV.Price);
                        objPV.Description = passengerV.Description;

                        if (passengerV.ImgUrlPrimary != string.Empty)
                            objPV.ImgUrlPrimary = passengerV.ImgUrlPrimary;

                        if (passengerV.ImgUrlSeconday != string.Empty)
                            objPV.ImgUrlSeconday = passengerV.ImgUrlSeconday;

                        if (passengerV.ImgUrlThird != string.Empty)
                            objPV.ImgUrlThird = passengerV.ImgUrlThird;

                        if (passengerV.ImgUrlFourth != string.Empty)
                            objPV.ImgUrlFourth = passengerV.ImgUrlFourth;

                        int response = classigooEntities.SaveChanges();
                        if (response == 1)
                        {
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update TransportationVehicle postdboperations", ex);
                return false;
            }

            return true;
        }

        public bool UpdateCV(ConstructionVehicle constructionV)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(constructionV.AddId);
                    ConstructionVehicle objCV = (ConstructionVehicle)classigooEntities.ConstructionVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objCV != null)
                    {
                        objCV.Company = constructionV.Company;
                        objCV.OtherCompany = constructionV.OtherCompany;
                        objCV.SubCategory = constructionV.SubCategory;

                        objCV.Model = constructionV.Model;
                        objCV.ManufacturingYear = constructionV.ManufacturingYear;

                        objCV.Price = Convert.ToInt32(constructionV.Price);
                        objCV.Description = constructionV.Description;

                        if (constructionV.ImgUrlPrimary != string.Empty)
                            objCV.ImgUrlPrimary = constructionV.ImgUrlPrimary;

                        if (constructionV.ImgUrlSeconday != string.Empty)
                            objCV.ImgUrlSeconday = constructionV.ImgUrlSeconday;

                        if (constructionV.ImgUrlThird != string.Empty)
                            objCV.ImgUrlThird = constructionV.ImgUrlThird;

                        if (constructionV.ImgUrlFourth != string.Empty)
                            objCV.ImgUrlFourth = constructionV.ImgUrlFourth;

                        int response = classigooEntities.SaveChanges();
                        if (response == 1)
                        {
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update ConstructionVehicle postdboperations", ex);
                return false;
            }

            return true;
        }

        public string[] DeleteImage(string[] addArray)
        {
            string[] allImages = new string[4];
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

                                allImages[0] = objRealestaeUpdated.ImgUrlPrimary;
                                allImages[1] = objRealestaeUpdated.ImgUrlSeconday;
                                allImages[2] = objRealestaeUpdated.ImgUrlThird;
                                allImages[3] = objRealestaeUpdated.ImgUrlFourth;

                                return allImages;
                                //return StatusCode(HttpStatusCode.OK);
                            }

                        }

                    }

                    #endregion

                    #region CV


                    if (addArray[0] == Constants.ConstructionVehicle)
                    {
                        int id = Convert.ToInt32(addArray[1]);
                        ConstructionVehicle objRealestae = classigooEntities.ConstructionVehicles.SingleOrDefault(a => a.AddId == id);


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
                                ConstructionVehicle objRealestaeUpdated = classigooEntities.ConstructionVehicles.SingleOrDefault(a => a.AddId == id);

                                allImages[0] = objRealestaeUpdated.ImgUrlPrimary;
                                allImages[1] = objRealestaeUpdated.ImgUrlSeconday;
                                allImages[2] = objRealestaeUpdated.ImgUrlThird;
                                allImages[3] = objRealestaeUpdated.ImgUrlFourth;

                                return allImages;
                                //return StatusCode(HttpStatusCode.OK);
                            }

                        }

                    }

                    #endregion

                    #region TV


                    if (addArray[0] == Constants.TransportationVehicle)
                    {
                        int id = Convert.ToInt32(addArray[1]);
                        TransportationVehicle objRealestae = classigooEntities.TransportationVehicles.SingleOrDefault(a => a.AddId == id);


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
                                TransportationVehicle objRealestaeUpdated = classigooEntities.TransportationVehicles.SingleOrDefault(a => a.AddId == id);

                                allImages[0] = objRealestaeUpdated.ImgUrlPrimary;
                                allImages[1] = objRealestaeUpdated.ImgUrlSeconday;
                                allImages[2] = objRealestaeUpdated.ImgUrlThird;
                                allImages[3] = objRealestaeUpdated.ImgUrlFourth;

                                return allImages;
                                //return StatusCode(HttpStatusCode.OK);
                            }

                        }

                    }

                    #endregion

                    #region AV


                    if (addArray[0] == Constants.AgriculturalVehicle)
                    {
                        int id = Convert.ToInt32(addArray[1]);
                        AgriculturalVehicle objRealestae = classigooEntities.AgriculturalVehicles.SingleOrDefault(a => a.AddId == id);


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
                                AgriculturalVehicle objRealestaeUpdated = classigooEntities.AgriculturalVehicles.SingleOrDefault(a => a.AddId == id);

                                allImages[0] = objRealestaeUpdated.ImgUrlPrimary;
                                allImages[1] = objRealestaeUpdated.ImgUrlSeconday;
                                allImages[2] = objRealestaeUpdated.ImgUrlThird;
                                allImages[3] = objRealestaeUpdated.ImgUrlFourth;

                                return allImages;
                                //return StatusCode(HttpStatusCode.OK);
                            }

                        }

                    }

                    #endregion

                    #region PV


                    if (addArray[0] == Constants.PassengerVehicle)
                    {
                        int id = Convert.ToInt32(addArray[1]);
                        PassengerVehicle objRealestae = classigooEntities.PassengerVehicles.SingleOrDefault(a => a.AddId == id);


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
                                PassengerVehicle objRealestaeUpdated = classigooEntities.PassengerVehicles.SingleOrDefault(a => a.AddId == id);

                                allImages[0] = objRealestaeUpdated.ImgUrlPrimary;
                                allImages[1] = objRealestaeUpdated.ImgUrlSeconday;
                                allImages[2] = objRealestaeUpdated.ImgUrlThird;
                                allImages[3] = objRealestaeUpdated.ImgUrlFourth;

                                return allImages;
                                //return StatusCode(HttpStatusCode.OK);
                            }

                        }

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At delete image from database", ex);

            }

            return allImages;
        }


    }
}