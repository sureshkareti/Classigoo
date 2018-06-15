using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
                    classigooEntities.FillAds(add.Category, add.SubCategory, add.State, add.District, add.Mandal, add.NearestArea, add.Title, add.Type, add.Status, add.UserId, Output);

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
                Library.WriteLog("exception at delete add postdboperations", ex);
                return false;

            }

            return true;
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
    }
}