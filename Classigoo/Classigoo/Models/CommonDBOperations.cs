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

        public SubCategoryCount GetSubCatCount()
        {
            SubCategoryCount objSubCatCount = new SubCategoryCount();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    #region AV
                   AV objAV = new AV();
                    objAV.BorewellMachineCount=db.Adds.Where(add => add.Category == Constants.AgriculturalVehicle).
                    Where(add => add.SubCategory == "Borewell Machine").
                    Where(add => add.Status == Constants.ActiveSatus).Count().ToString();



                    objAV.TractorsCount = db.Adds./*Where(add => add.Category == Constants.AgriculturalVehicle).*/
        Where(add => add.SubCategory == "Tractors").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objAV.DozerCount = db.Adds./*Where(add => add.Category == Constants.AgriculturalVehicle).*/
        Where(add => add.SubCategory == "Dozer").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objAV.HarvesterCount = db.Adds.Where(add => add.Category == Constants.AgriculturalVehicle).
        Where(add => add.SubCategory == "Combine Harvester").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objAV.BackhoeLoaderCount = db.Adds./*.Where(add => add.Category == Constants.AgriculturalVehicle).*/
        Where(add => add.SubCategory == "Backhoe Loader").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objAV.ExcavatorsCount = db.Adds./*Where(add => add.Category == Constants.AgriculturalVehicle).*/
        Where(add => add.SubCategory == "Excavators").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objSubCatCount.AVSubCat = objAV;
                    #endregion

                    #region CV
                    CV objCV = new CV();
                    objCV.TractorsCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
              Where(add => add.SubCategory == "Tractors").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.DozerCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
             Where(add => add.SubCategory == "Dozers").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.BackhoeLoaderCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
             Where(add => add.SubCategory == "Backhoe_Loader").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.ExcavatorsCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
             Where(add => add.SubCategory == "Excavators").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.WheelLoaderCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
             Where(add => add.SubCategory == "Wheel Loader").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.CraneCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
             Where(add => add.SubCategory == "Crane").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.TransitMixerCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
             Where(add => add.SubCategory == "Transit Mixer").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.SoilCompactorCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
             Where(add => add.SubCategory == "Soil Compactor").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objCV.TippersCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
             Where(add => add.SubCategory == "Tippers").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objSubCatCount.CVSubCat = objCV;
                    #endregion

                    #region TV
                    TV objTV = new TV();
                    objTV.Autos3wheelerCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
             Where(add => add.SubCategory == "Autos - 3 wheeler").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objTV.MiniTrucks4wheelerCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
             Where(add => add.SubCategory == "Mini Trucks - 4 wheeler").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objTV.LorryTrucksCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
             Where(add => add.SubCategory == "Lorry Trucks").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objTV.DCMTrucksCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
             Where(add => add.SubCategory == "DCM Trucks").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objSubCatCount.TVSubCat = objTV;
                    #endregion

                    #region PV
                    PV objPV = new PV();
                 objPV.AutosCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
                   Where(add => add.SubCategory == "Autos").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objPV.CarsCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
                  Where(add => add.SubCategory == "Cars").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objPV.TravelVansCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
                  Where(add => add.SubCategory == "Travel Vans").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objPV.BikesCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
                  Where(add => add.SubCategory == "Bikes").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objSubCatCount.PVSubCat = objPV;
                    #endregion

                    #region RE
                    RE objRE = new RE();
                objRE.ApartmentsCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
               Where(add => add.SubCategory == "Apartments").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objRE.PlotsLandCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
              Where(add => add.SubCategory == "Plots/Land").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objRE.AgriculturalLandCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
              Where(add => add.SubCategory == "Agricultural Land").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objRE.ShopsOfficesCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
              Where(add => add.SubCategory == "Shops & Offices").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objRE.IndependentHousesVillasCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
              Where(add => add.SubCategory == "Independent Houses & Villas").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objRE.HostelsPGCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
              Where(add => add.SubCategory == "Hostels & PG").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
                    objSubCatCount.RESubCat = objRE;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get subcategory count commondboperations", ex);
            }

            return objSubCatCount;
        }

        public void GetAVSubCount(string location,string type,string keyword,string category,string subCategory,string company)
        {
            using (ClassigooEntities db = new ClassigooEntities())
            {
                AV objAV = new AV();


              objAV.BorewellMachineCount = (from AV in db.AgriculturalVehicles
                                 join add in db.Adds on AV.AddId equals add.AddId
                                 where
                  // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                //(company != "All" ? AV.Company == company : true) &&
                             ((location != "" ? add.State == location : true) ||
                                  (location != "" ? add.District == location : true) ||
                                  (location != "" ? add.Mandal == location : true)) &&
                                  (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus)&&
                                  (add.SubCategory == "Borewell Machine") &&

                                  (keyword != "" ? add.Title.Contains(keyword) : true)
                                 select add.AddId).Count().ToString();




                objAV.BorewellMachineCount = db.Adds.Where(add =>
                    ((location != "" ? add.State == location : true) ||
                    (location != "" ? add.District == location : true) ||
                    (location != "" ? add.Mandal == location : true)) &&
                    (add.Type == type) &&
                     (add.Status == Constants.ActiveSatus) &&
                     (add.SubCategory == "Borewell Machine") &&
                    (keyword != "" ? add.Title.Contains(keyword) : true) &&
                    (add.Category == category)).Count().ToString();

                objAV.TractorsCount = db.Adds.Where(add =>
                           ((location != "" ? add.State == location : true) ||
                           (location != "" ? add.District == location : true) ||
                           (location != "" ? add.Mandal == location : true)) &&
                           (add.Type == type) &&
                            (add.Status == Constants.ActiveSatus) &&
                            (add.SubCategory == "Tractors") &&
                           (keyword != "" ? add.Title.Contains(keyword) : true) &&
                           (add.Category == category)).Count().ToString();

                objAV.DozerCount = db.Adds.Where(add =>
                          ((location != "" ? add.State == location : true) ||
                          (location != "" ? add.District == location : true) ||
                          (location != "" ? add.Mandal == location : true)) &&
                          (add.Type == type) &&
                           (add.Status == Constants.ActiveSatus) &&
                           (add.SubCategory == "Dozer") &&
                          (keyword != "" ? add.Title.Contains(keyword) : true) &&
                          (add.Category == category)).Count().ToString();

                objAV.HarvesterCount = db.Adds.Where(add =>
                         ((location != "" ? add.State == location : true) ||
                         (location != "" ? add.District == location : true) ||
                         (location != "" ? add.Mandal == location : true)) &&
                         (add.Type == type) &&
                          (add.Status == Constants.ActiveSatus) &&
                          (add.SubCategory == "Combine Harvester") &&
                         (keyword != "" ? add.Title.Contains(keyword) : true) &&
                         (add.Category == category)).Count().ToString();

                objAV.BackhoeLoaderCount = db.Adds.Where(add =>
                          ((location != "" ? add.State == location : true) ||
                          (location != "" ? add.District == location : true) ||
                          (location != "" ? add.Mandal == location : true)) &&
                          (add.Type == type) &&
                           (add.Status == Constants.ActiveSatus) &&
                           (add.SubCategory == "Backhoe Loader") &&
                          (keyword != "" ? add.Title.Contains(keyword) : true) &&
                          (add.Category == category)).Count().ToString();

                objAV.ExcavatorsCount = db.Adds.Where(add =>
                    ((location != "" ? add.State == location : true) ||
                    (location != "" ? add.District == location : true) ||
                    (location != "" ? add.Mandal == location : true)) &&
                    (add.Type == type) &&
                     (add.Status == Constants.ActiveSatus) &&
                     (add.SubCategory == "Excavators") &&
                    (keyword != "" ? add.Title.Contains(keyword) : true) &&
                    (add.Category == category)).Count().ToString();
            }
        }

        public void GetCVSubCount(string location, string type, string keyword, string category, string subCategory, string company)
        {
            using (ClassigooEntities db = new ClassigooEntities())
            {
                     CV objCV = new CV();
                    objCV.TractorsCount = (from CV in db.ConstructionVehicles
                                           join add in db.Adds on CV.AddId equals add.AddId
                                           where
                                       // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                       //(company != "All" ? AV.Company == company : true) &&
                                       ((location != "" ? add.State == location : true) ||
                                            (location != "" ? add.District == location : true) ||
                                            (location != "" ? add.Mandal == location : true)) &&
                                            (add.Type == type) &&
                                            (add.Status == Constants.ActiveSatus) &&
                                            (add.SubCategory == "Tractors") &&

                                            (keyword != "" ? add.Title.Contains(keyword) : true)
                                           select add.AddId).Count().ToString();

                objCV.DozerCount = (from CV in db.ConstructionVehicles
                                       join add in db.Adds on CV.AddId equals add.AddId
                                       where
                                   // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                   //(company != "All" ? AV.Company == company : true) &&
                                   ((location != "" ? add.State == location : true) ||
                                        (location != "" ? add.District == location : true) ||
                                        (location != "" ? add.Mandal == location : true)) &&
                                        (add.Type == type) &&
                                        (add.Status == Constants.ActiveSatus) &&
                                        (add.SubCategory == "Dozers") &&

                                        (keyword != "" ? add.Title.Contains(keyword) : true)
                                       select add.AddId).Count().ToString();

                objCV.BackhoeLoaderCount = (from CV in db.ConstructionVehicles
                                    join add in db.Adds on CV.AddId equals add.AddId
                                    where
                                // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                //(company != "All" ? AV.Company == company : true) &&
                                ((location != "" ? add.State == location : true) ||
                                     (location != "" ? add.District == location : true) ||
                                     (location != "" ? add.Mandal == location : true)) &&
                                     (add.Type == type) &&
                                     (add.Status == Constants.ActiveSatus) &&
                                     (add.SubCategory == "Backhoe_Loader") &&

                                     (keyword != "" ? add.Title.Contains(keyword) : true)
                                    select add.AddId).Count().ToString();

                objCV.ExcavatorsCount = (from CV in db.ConstructionVehicles
                                            join add in db.Adds on CV.AddId equals add.AddId
                                            where
                                        // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                        //(company != "All" ? AV.Company == company : true) &&
                                        ((location != "" ? add.State == location : true) ||
                                             (location != "" ? add.District == location : true) ||
                                             (location != "" ? add.Mandal == location : true)) &&
                                             (add.Type == type) &&
                                             (add.Status == Constants.ActiveSatus) &&
                                             (add.SubCategory == "Excavators") &&

                                             (keyword != "" ? add.Title.Contains(keyword) : true)
                                            select add.AddId).Count().ToString();

                objCV.WheelLoaderCount = (from CV in db.ConstructionVehicles
                                         join add in db.Adds on CV.AddId equals add.AddId
                                         where
                                     // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                     //(company != "All" ? AV.Company == company : true) &&
                                     ((location != "" ? add.State == location : true) ||
                                          (location != "" ? add.District == location : true) ||
                                          (location != "" ? add.Mandal == location : true)) &&
                                          (add.Type == type) &&
                                          (add.Status == Constants.ActiveSatus) &&
                                          (add.SubCategory == "Wheel Loader") &&

                                          (keyword != "" ? add.Title.Contains(keyword) : true)
                                         select add.AddId).Count().ToString();
                objCV.CraneCount = (from CV in db.ConstructionVehicles
                                          join add in db.Adds on CV.AddId equals add.AddId
                                          where
                                      // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                      //(company != "All" ? AV.Company == company : true) &&
                                      ((location != "" ? add.State == location : true) ||
                                           (location != "" ? add.District == location : true) ||
                                           (location != "" ? add.Mandal == location : true)) &&
                                           (add.Type == type) &&
                                           (add.Status == Constants.ActiveSatus) &&
                                           (add.SubCategory == "Crane") &&

                                           (keyword != "" ? add.Title.Contains(keyword) : true)
                                          select add.AddId).Count().ToString();

                objCV.TransitMixerCount = (from CV in db.ConstructionVehicles
                                    join add in db.Adds on CV.AddId equals add.AddId
                                    where
                                // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                //(company != "All" ? AV.Company == company : true) &&
                                ((location != "" ? add.State == location : true) ||
                                     (location != "" ? add.District == location : true) ||
                                     (location != "" ? add.Mandal == location : true)) &&
                                     (add.Type == type) &&
                                     (add.Status == Constants.ActiveSatus) &&
                                     (add.SubCategory == "Transit Mixer") &&

                                     (keyword != "" ? add.Title.Contains(keyword) : true)
                                    select add.AddId).Count().ToString();


                objCV.SoilCompactorCount = (from CV in db.ConstructionVehicles
                                           join add in db.Adds on CV.AddId equals add.AddId
                                           where
                                       // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                       //(company != "All" ? AV.Company == company : true) &&
                                       ((location != "" ? add.State == location : true) ||
                                            (location != "" ? add.District == location : true) ||
                                            (location != "" ? add.Mandal == location : true)) &&
                                            (add.Type == type) &&
                                            (add.Status == Constants.ActiveSatus) &&
                                            (add.SubCategory == "Soil Compactor") &&

                                            (keyword != "" ? add.Title.Contains(keyword) : true)
                                           select add.AddId).Count().ToString();
                objCV.TippersCount = (from CV in db.ConstructionVehicles
                                            join add in db.Adds on CV.AddId equals add.AddId
                                            where
                                        // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                        //(company != "All" ? AV.Company == company : true) &&
                                        ((location != "" ? add.State == location : true) ||
                                             (location != "" ? add.District == location : true) ||
                                             (location != "" ? add.Mandal == location : true)) &&
                                             (add.Type == type) &&
                                             (add.Status == Constants.ActiveSatus) &&
                                             (add.SubCategory == "Tippers") &&

                                             (keyword != "" ? add.Title.Contains(keyword) : true)
                                            select add.AddId).Count().ToString();












            }
        }

        public void GetTVSubCount(string location, string type, string keyword, string category, string subCategory, string company)
        {
            using (ClassigooEntities db = new ClassigooEntities())
            {
                TV objTV = new TV();
                objTV.Autos3wheelerCount = (from TV in db.TransportationVehicles
                                       join add in db.Adds on TV.AddId equals add.AddId
                                       where
                                   // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                   //(company != "All" ? AV.Company == company : true) &&
                                   ((location != "" ? add.State == location : true) ||
                                        (location != "" ? add.District == location : true) ||
                                        (location != "" ? add.Mandal == location : true)) &&
                                        (add.Type == type) &&
                                        (add.Status == Constants.ActiveSatus) &&
                                        (add.SubCategory == "Autos - 3 wheeler") &&
                                        (keyword != "" ? add.Title.Contains(keyword) : true)
                                         select add.AddId).Count().ToString();
                objTV.MiniTrucks4wheelerCount = (from TV in db.TransportationVehicles
                                            join add in db.Adds on TV.AddId equals add.AddId
                                            where
                                        // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                        //(company != "All" ? AV.Company == company : true) &&
                                        ((location != "" ? add.State == location : true) ||
                                             (location != "" ? add.District == location : true) ||
                                             (location != "" ? add.Mandal == location : true)) &&
                                             (add.Type == type) &&
                                             (add.Status == Constants.ActiveSatus) &&
                                             (add.SubCategory == "Mini Trucks - 4 wheeler") &&
                                             (keyword != "" ? add.Title.Contains(keyword) : true)
                                            select add.AddId).Count().ToString();
                objTV.LorryTrucksCount = (from TV in db.TransportationVehicles
                                                 join add in db.Adds on TV.AddId equals add.AddId
                                                 where
                                             // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                             //(company != "All" ? AV.Company == company : true) &&
                                             ((location != "" ? add.State == location : true) ||
                                                  (location != "" ? add.District == location : true) ||
                                                  (location != "" ? add.Mandal == location : true)) &&
                                                  (add.Type == type) &&
                                                  (add.Status == Constants.ActiveSatus) &&
                                                  (add.SubCategory == "Lorry Trucks") &&
                                                  (keyword != "" ? add.Title.Contains(keyword) : true)
                                                 select add.AddId).Count().ToString();
                objTV.DCMTrucksCount = (from TV in db.TransportationVehicles
                                          join add in db.Adds on TV.AddId equals add.AddId
                                          where
                                      // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                      //(company != "All" ? AV.Company == company : true) &&
                                      ((location != "" ? add.State == location : true) ||
                                           (location != "" ? add.District == location : true) ||
                                           (location != "" ? add.Mandal == location : true)) &&
                                           (add.Type == type) &&
                                           (add.Status == Constants.ActiveSatus) &&
                                           (add.SubCategory == "DCM Trucks") &&
                                           (keyword != "" ? add.Title.Contains(keyword) : true)
                                          select add.AddId).Count().ToString();










            }
        }
        public void GePVSubCount(string location, string type, string keyword, string category, string subCategory, string company)
        {
            using (ClassigooEntities db = new ClassigooEntities())
            {
                PV objPV = new PV();
                objPV.AutosCount = (from PV in db.PassengerVehicles
                                    join add in db.Adds on PV.AddId equals add.AddId
                                    where
                                // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                //(company != "All" ? AV.Company == company : true) &&
                                ((location != "" ? add.State == location : true) ||
                                     (location != "" ? add.District == location : true) ||
                                     (location != "" ? add.Mandal == location : true)) &&
                                     (add.Type == type) &&
                                     (add.Status == Constants.ActiveSatus) &&
                                     (add.SubCategory == "Autos") &&
                                     (keyword != "" ? add.Title.Contains(keyword) : true)
                                    select add.AddId).Count().ToString();
                objPV.CarsCount = (from PV in db.PassengerVehicles
                                    join add in db.Adds on PV.AddId equals add.AddId
                                    where
                                // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                //(company != "All" ? AV.Company == company : true) &&
                                ((location != "" ? add.State == location : true) ||
                                     (location != "" ? add.District == location : true) ||
                                     (location != "" ? add.Mandal == location : true)) &&
                                     (add.Type == type) &&
                                     (add.Status == Constants.ActiveSatus) &&
                                     (add.SubCategory == "Cars") &&
                                     (keyword != "" ? add.Title.Contains(keyword) : true)
                                    select add.AddId).Count().ToString();

                objPV.TravelVansCount = (from PV in db.PassengerVehicles
                                   join add in db.Adds on PV.AddId equals add.AddId
                                   where
                               // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                               //(company != "All" ? AV.Company == company : true) &&
                               ((location != "" ? add.State == location : true) ||
                                    (location != "" ? add.District == location : true) ||
                                    (location != "" ? add.Mandal == location : true)) &&
                                    (add.Type == type) &&
                                    (add.Status == Constants.ActiveSatus) &&
                                    (add.SubCategory == "Travel Vans") &&
                                    (keyword != "" ? add.Title.Contains(keyword) : true)
                                   select add.AddId).Count().ToString();

                objPV.BikesCount = (from PV in db.PassengerVehicles
                                         join add in db.Adds on PV.AddId equals add.AddId
                                         where
                                     // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                     //(company != "All" ? AV.Company == company : true) &&
                                     ((location != "" ? add.State == location : true) ||
                                          (location != "" ? add.District == location : true) ||
                                          (location != "" ? add.Mandal == location : true)) &&
                                          (add.Type == type) &&
                                          (add.Status == Constants.ActiveSatus) &&
                                          (add.SubCategory == "Bikes") &&
                                          (keyword != "" ? add.Title.Contains(keyword) : true)
                                         select add.AddId).Count().ToString();










            }
        }
        public void GeRESubCount(string location, string type, string keyword, string category, string subCategory, string company)
        {
            using (ClassigooEntities db = new ClassigooEntities())
            {
                RE objRE = new RE();
                objRE.ApartmentsCount = (from RE in db.RealEstates
                                    join add in db.Adds on RE.AddId equals add.AddId
                                    where
                                // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                //(company != "All" ? AV.Company == company : true) &&
                                ((location != "" ? add.State == location : true) ||
                                     (location != "" ? add.District == location : true) ||
                                     (location != "" ? add.Mandal == location : true)) &&
                                     (add.Type == type) &&
                                     (add.Status == Constants.ActiveSatus) &&
                                     (add.SubCategory == "Apartments") &&
                                     (keyword != "" ? add.Title.Contains(keyword) : true)
                                    select add.AddId).Count().ToString();
                
                objRE.PlotsLandCount = (from RE in db.RealEstates
                                         join add in db.Adds on RE.AddId equals add.AddId
                                         where
                                     // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                     //(company != "All" ? AV.Company == company : true) &&
                                     ((location != "" ? add.State == location : true) ||
                                          (location != "" ? add.District == location : true) ||
                                          (location != "" ? add.Mandal == location : true)) &&
                                          (add.Type == type) &&
                                          (add.Status == Constants.ActiveSatus) &&
                                          (add.SubCategory == "Plots/Land") &&
                                          (keyword != "" ? add.Title.Contains(keyword) : true)
                                         select add.AddId).Count().ToString();
                objRE.AgriculturalLandCount = (from RE in db.RealEstates
                                        join add in db.Adds on RE.AddId equals add.AddId
                                        where
                                    // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                    //(company != "All" ? AV.Company == company : true) &&
                                    ((location != "" ? add.State == location : true) ||
                                         (location != "" ? add.District == location : true) ||
                                         (location != "" ? add.Mandal == location : true)) &&
                                         (add.Type == type) &&
                                         (add.Status == Constants.ActiveSatus) &&
                                         (add.SubCategory == "Agricultural Land") &&
                                         (keyword != "" ? add.Title.Contains(keyword) : true)
                                        select add.AddId).Count().ToString();
                objRE.ShopsOfficesCount = (from RE in db.RealEstates
                                               join add in db.Adds on RE.AddId equals add.AddId
                                               where
                                           // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                           //(company != "All" ? AV.Company == company : true) &&
                                           ((location != "" ? add.State == location : true) ||
                                                (location != "" ? add.District == location : true) ||
                                                (location != "" ? add.Mandal == location : true)) &&
                                                (add.Type == type) &&
                                                (add.Status == Constants.ActiveSatus) &&
                                                (add.SubCategory == "Shops & Offices") &&
                                                (keyword != "" ? add.Title.Contains(keyword) : true)
                                               select add.AddId).Count().ToString();
                objRE.IndependentHousesVillasCount = (from RE in db.RealEstates
                                           join add in db.Adds on RE.AddId equals add.AddId
                                           where
                                       // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                       //(company != "All" ? AV.Company == company : true) &&
                                       ((location != "" ? add.State == location : true) ||
                                            (location != "" ? add.District == location : true) ||
                                            (location != "" ? add.Mandal == location : true)) &&
                                            (add.Type == type) &&
                                            (add.Status == Constants.ActiveSatus) &&
                                            (add.SubCategory == "Independent Houses & Villas") &&
                                            (keyword != "" ? add.Title.Contains(keyword) : true)
                                           select add.AddId).Count().ToString();

                objRE.HostelsPGCount = (from RE in db.RealEstates
                                                      join add in db.Adds on RE.AddId equals add.AddId
                                                      where
                                                  // (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                                                  //(company != "All" ? AV.Company == company : true) &&
                                                  ((location != "" ? add.State == location : true) ||
                                                       (location != "" ? add.District == location : true) ||
                                                       (location != "" ? add.Mandal == location : true)) &&
                                                       (add.Type == type) &&
                                                       (add.Status == Constants.ActiveSatus) &&
                                                       (add.SubCategory == "") &&
                                                       (add.SubCategory == "Hostels & PG") &&
                                                       (keyword != "" ? add.Title.Contains(keyword) : true)
                                                      select add.AddId).Count().ToString();





            }
        }


    }
}