using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo.Models
{
    public class CommonDBOperations
    {
        public RealEstate GetRealEstate(string addId)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    RealEstate objRealestae = (RealEstate)classigooEntities.RealEstates.SingleOrDefault(a => a.AddId == id);

                    if (objRealestae != null)
                    {
                        return objRealestae;
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get realestate postdboperations", ex);

            }

            return null;
        }

        public PassengerVehicle GetPV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    PassengerVehicle objPassengerVehicle = classigooEntities.PassengerVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objPassengerVehicle != null)
                    {
                        return objPassengerVehicle;
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get pv postdboperations", ex);

            }

            return null;
        }

        public ConstructionVehicle GetCV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    ConstructionVehicle objConstructionVehicle = classigooEntities.ConstructionVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objConstructionVehicle != null)
                    {
                        return objConstructionVehicle;
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get cv postdboperations", ex);

            }

            return null;
        }

        public TransportationVehicle GetTV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    TransportationVehicle objTransportationVehicle = classigooEntities.TransportationVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objTransportationVehicle != null)
                    {
                        return objTransportationVehicle;
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get tv postdboperations", ex);

            }

            return null;
        }

        public AgriculturalVehicle GetAV(string addId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    AgriculturalVehicle objAgriculturalVehicle = classigooEntities.AgriculturalVehicles.SingleOrDefault(a => a.AddId == id);

                    if (objAgriculturalVehicle != null)
                    {
                        return objAgriculturalVehicle;
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get av postdboperations", ex);

            }

            return null;
        }

        public Add GetAdd(string addId)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(addId);
                    Add objAdd = classigooEntities.Adds.SingleOrDefault(a => a.AddId == id);

                    if (objAdd != null)
                    {
                        return objAdd;
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get add postdboperations", ex);
            }

            return null;

        }

        public List<Add> GetSimilarAdds(string category,string subCategory,string type)
        {
            List<Add> addColl = new List<Add>();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    addColl = db.Adds.Where(add => add.SubCategory.ToLower() == subCategory.ToLower())
                        .Where(add=>add.Category.ToLower()==category.ToLower())
                        .Where(add=>add.Type.ToLower()==type.ToLower())
                        .Where(add=>add.Status==Constants.ActiveSatus)
                        .OrderByDescending(add => add.Created)
                                   .Take(4).ToList();
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At getting similar adds", ex);
            }
            return addColl;
        }
    }
}