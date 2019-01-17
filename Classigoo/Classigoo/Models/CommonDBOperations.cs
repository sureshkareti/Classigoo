using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

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
                                   .Take(9).ToList();
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At getting similar adds", ex);
            }
            return addColl;
        }
        
        public SubCategoryCount GetSubCatCount(string category,string location, string type, string keyword,string subCategory, string company,string reFilters)
        {
            SubCategoryCount objSubCatCount = new SubCategoryCount();
            try
            {
                objSubCatCount.AVSubCat = new AV();
                objSubCatCount.CVSubCat = new CV();
                objSubCatCount.TVSubCat = new TV();
                objSubCatCount.PVSubCat = new PV();
                objSubCatCount.RESubCat = new RE();
                switch (category)
                {
                    case "Select Category":
                        objSubCatCount.AVSubCat = GetAVSubCount(location, type, keyword, subCategory, company);
                        objSubCatCount.CVSubCat = GetCVSubCount(location, type, keyword, subCategory, company);
                        objSubCatCount.TVSubCat = GetTVSubCount(location, type, keyword, subCategory, company);
                        objSubCatCount.PVSubCat = GetPVSubCount(location, type, keyword, subCategory, company);
                        objSubCatCount.RESubCat = GetRESubCount(location, type, keyword, subCategory, company, reFilters);
                        break;
                    case Constants.RealEstate:
                        objSubCatCount.RESubCat = GetRESubCount(location, type, keyword, subCategory, company, reFilters);

                        break;
                    case Constants.AgriculturalVehicle:
                        objSubCatCount.AVSubCat = GetAVSubCount(location, type, keyword, subCategory, company);
                        break;
                    case Constants.ConstructionVehicle:
                        objSubCatCount.CVSubCat = GetCVSubCount(location, type, keyword, subCategory, company);
                        break;
                    case Constants.TransportationVehicle:
                        objSubCatCount.TVSubCat = GetTVSubCount(location, type, keyword, subCategory, company);
                        break;
                    case Constants.PassengerVehicle:
                        objSubCatCount.PVSubCat = GetPVSubCount(location, type, keyword, subCategory, company);
                        break;
                    default:
                      //  addColl = FilterCategoryNotSelect(location, keyword, type, pageNum);
                        break;
                }
            

                //common subcategories count
             
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Getsubcatcount commondboperations", ex);
            }
            return objSubCatCount;
         }
        public AV GetAVSubCount(string location,string type,string keyword,string subCategory,string company)
        {
            AV objAV = new AV();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    #region AV
                    objAV.BorewellMachineCount = (from AV in db.AgriculturalVehicles
                                                  join add in db.Adds on AV.AddId equals add.AddId
                                                  where
                                           (subCategory == "Borewell Machines" && company != "All" ? AV.Company == company : true) &&
                                              ((location != "" ? add.State == location : true) ||
                                                   (location != "" ? add.District == location : true) ||
                                                   (location != "" ? add.Mandal == location : true)) &&
                                                   (add.Type == type) &&
                                                   (add.Status == Constants.ActiveSatus) &&
                                                   (add.SubCategory == "Borewell Machines") &&
                                                   (keyword != "" ? add.Title.Contains(keyword) : true)
                                                  select add.AddId).Count();

                    objAV.TractorsCount = (from AV in db.AgriculturalVehicles
                                           join add in db.Adds on AV.AddId equals add.AddId
                                           where
                                     (subCategory == "Tractors" && company != "All" ? AV.Company == company : true) &&
                                       ((location != "" ? add.State == location : true) ||
                                            (location != "" ? add.District == location : true) ||
                                            (location != "" ? add.Mandal == location : true)) &&
                                            (add.Type == type) &&
                                            (add.Status == Constants.ActiveSatus) &&
                                            (add.SubCategory == "Tractors") &&
                                            (keyword != "" ? add.Title.Contains(keyword) : true)
                                           select add.AddId).Count();

                    objAV.DozerCount = (from AV in db.AgriculturalVehicles
                                        join add in db.Adds on AV.AddId equals add.AddId
                                        where
                                  (subCategory == "Dozers" && company != "All" ? AV.Company == company : true) &&
                                    ((location != "" ? add.State == location : true) ||
                                         (location != "" ? add.District == location : true) ||
                                         (location != "" ? add.Mandal == location : true)) &&
                                         (add.Type == type) &&
                                         (add.Status == Constants.ActiveSatus) &&
                                         (add.SubCategory == "Dozers") &&

                                         (keyword != "" ? add.Title.Contains(keyword) : true)
                                        select add.AddId).Count();
                    objAV.HarvesterCount = (from AV in db.AgriculturalVehicles
                                            join add in db.Adds on AV.AddId equals add.AddId
                                            where
                                      (subCategory == "Combine Harvesters" && company != "All" ? AV.Company == company : true) &&
                                        ((location != "" ? add.State == location : true) ||
                                             (location != "" ? add.District == location : true) ||
                                             (location != "" ? add.Mandal == location : true)) &&
                                             (add.Type == type) &&
                                             (add.Status == Constants.ActiveSatus) &&
                                             (add.SubCategory == "Combine Harvesters") &&

                                             (keyword != "" ? add.Title.Contains(keyword) : true)
                                            select add.AddId).Count();
                    objAV.BackhoeLoaderCount = (from AV in db.AgriculturalVehicles
                                                join add in db.Adds on AV.AddId equals add.AddId
                                                where
                                            (subCategory == "Backhoe Loaders" && company != "All" ? AV.Company == company : true) &&
                                            ((location != "" ? add.State == location : true) ||
                                                 (location != "" ? add.District == location : true) ||
                                                 (location != "" ? add.Mandal == location : true)) &&
                                                 (add.Type == type) &&
                                                 (add.Status == Constants.ActiveSatus) &&
                                                 (add.SubCategory == "Backhoe Loaders") &&

                                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                                select add.AddId).Count();
                    objAV.ExcavatorsCount = (from AV in db.AgriculturalVehicles
                                             join add in db.Adds on AV.AddId equals add.AddId
                                             where
                                      (subCategory == "Excavators" && company != "All" ? AV.Company == company : true) &&
                                         ((location != "" ? add.State == location : true) ||
                                              (location != "" ? add.District == location : true) ||
                                              (location != "" ? add.Mandal == location : true)) &&
                                              (add.Type == type) &&
                                              (add.Status == Constants.ActiveSatus) &&
                                              (add.SubCategory == "Excavators") &&

                                              (keyword != "" ? add.Title.Contains(keyword) : true)
                                             select add.AddId).Count();
                    #endregion
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At GetAVSubcatcount", ex);
            }

             return objAV;
            
        }

        public CV GetCVSubCount(string location, string type, string keyword, string subCategory, string company)
        {
            CV objCV = new CV();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    #region CV
                    objCV.TractorsCount = (from CV in db.ConstructionVehicles
                                           join add in db.Adds on CV.AddId equals add.AddId
                                           where
                                        (subCategory == "Tractors" && company != "All" ? CV.Company == company : true) &&
                                       ((location != "" ? add.State == location : true) ||
                                            (location != "" ? add.District == location : true) ||
                                            (location != "" ? add.Mandal == location : true)) &&
                                            (add.Type == type) &&
                                            (add.Status == Constants.ActiveSatus) &&
                                            (add.SubCategory == "Tractors") &&

                                            (keyword != "" ? add.Title.Contains(keyword) : true)
                                           select add.AddId).Count();

                    objCV.DozerCount = (from CV in db.ConstructionVehicles
                                        join add in db.Adds on CV.AddId equals add.AddId
                                        where
                                    (subCategory == "Dozers" && company != "All" ? CV.Company == company : true) &&
                                    ((location != "" ? add.State == location : true) ||
                                         (location != "" ? add.District == location : true) ||
                                         (location != "" ? add.Mandal == location : true)) &&
                                         (add.Type == type) &&
                                         (add.Status == Constants.ActiveSatus) &&
                                         (add.SubCategory == "Dozers") &&

                                         (keyword != "" ? add.Title.Contains(keyword) : true)
                                        select add.AddId).Count();

                    objCV.BackhoeLoaderCount = (from CV in db.ConstructionVehicles
                                                join add in db.Adds on CV.AddId equals add.AddId
                                                where
                                            (subCategory == "Backhoe Loaders" && company != "All" ? CV.Company == company : true) &&
                                            ((location != "" ? add.State == location : true) ||
                                                 (location != "" ? add.District == location : true) ||
                                                 (location != "" ? add.Mandal == location : true)) &&
                                                 (add.Type == type) &&
                                                 (add.Status == Constants.ActiveSatus) &&
                                                 (add.SubCategory == "Backhoe Loaders") &&

                                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                                select add.AddId).Count();

                    objCV.ExcavatorsCount = (from CV in db.ConstructionVehicles
                                             join add in db.Adds on CV.AddId equals add.AddId
                                             where
                                         (subCategory == "Excavators" && company != "All" ? CV.Company == company : true) &&
                                         ((location != "" ? add.State == location : true) ||
                                              (location != "" ? add.District == location : true) ||
                                              (location != "" ? add.Mandal == location : true)) &&
                                              (add.Type == type) &&
                                              (add.Status == Constants.ActiveSatus) &&
                                              (add.SubCategory == "Excavators") &&

                                              (keyword != "" ? add.Title.Contains(keyword) : true)
                                             select add.AddId).Count();

                    objCV.WheelLoaderCount = (from CV in db.ConstructionVehicles
                                              join add in db.Adds on CV.AddId equals add.AddId
                                              where
                                          (subCategory == "Wheel Loaders" && company != "All" ? CV.Company == company : true) &&
                                          ((location != "" ? add.State == location : true) ||
                                               (location != "" ? add.District == location : true) ||
                                               (location != "" ? add.Mandal == location : true)) &&
                                               (add.Type == type) &&
                                               (add.Status == Constants.ActiveSatus) &&
                                               (add.SubCategory == "Wheel Loaders") &&

                                               (keyword != "" ? add.Title.Contains(keyword) : true)
                                              select add.AddId).Count();
                    objCV.CraneCount = (from CV in db.ConstructionVehicles
                                        join add in db.Adds on CV.AddId equals add.AddId
                                        where
                                    (subCategory == "Cranes" && company != "All" ? CV.Company == company : true) &&
                                    ((location != "" ? add.State == location : true) ||
                                         (location != "" ? add.District == location : true) ||
                                         (location != "" ? add.Mandal == location : true)) &&
                                         (add.Type == type) &&
                                         (add.Status == Constants.ActiveSatus) &&
                                         (add.SubCategory == "Cranes") &&

                                         (keyword != "" ? add.Title.Contains(keyword) : true)
                                        select add.AddId).Count();

                    objCV.TransitMixerCount = (from CV in db.ConstructionVehicles
                                               join add in db.Adds on CV.AddId equals add.AddId
                                               where
                                           (subCategory == "Transit Mixers" && company != "All" ? CV.Company == company : true) &&
                                           ((location != "" ? add.State == location : true) ||
                                                (location != "" ? add.District == location : true) ||
                                                (location != "" ? add.Mandal == location : true)) &&
                                                (add.Type == type) &&
                                                (add.Status == Constants.ActiveSatus) &&
                                                (add.SubCategory == "Transit Mixers") &&

                                                (keyword != "" ? add.Title.Contains(keyword) : true)
                                               select add.AddId).Count();


                    objCV.SoilCompactorCount = (from CV in db.ConstructionVehicles
                                                join add in db.Adds on CV.AddId equals add.AddId
                                                where
                                               (subCategory == "Soil Compactors" && company != "All" ? CV.Company == company : true) &&
                                            ((location != "" ? add.State == location : true) ||
                                                 (location != "" ? add.District == location : true) ||
                                                 (location != "" ? add.Mandal == location : true)) &&
                                                 (add.Type == type) &&
                                                 (add.Status == Constants.ActiveSatus) &&
                                                 (add.SubCategory == "Soil Compactors") &&

                                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                                select add.AddId).Count();
                    objCV.TippersCount = (from CV in db.ConstructionVehicles
                                          join add in db.Adds on CV.AddId equals add.AddId
                                          where
                                      (subCategory == "Tippers" && company != "All" ? CV.Company == company : true) &&
                                      ((location != "" ? add.State == location : true) ||
                                           (location != "" ? add.District == location : true) ||
                                           (location != "" ? add.Mandal == location : true)) &&
                                           (add.Type == type) &&
                                           (add.Status == Constants.ActiveSatus) &&
                                           (add.SubCategory == "Tippers") &&

                                           (keyword != "" ? add.Title.Contains(keyword) : true)
                                          select add.AddId).Count();
                    #endregion
                    
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At GetCVSubcount", ex);
            }
            return objCV;
        }

        public TV GetTVSubCount(string location, string type, string keyword, string subCategory, string company)
        {
            TV objTV = new TV();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    #region TV
                    objTV.Autos3wheelerCount = (from TV in db.TransportationVehicles
                                                join add in db.Adds on TV.AddId equals add.AddId
                                                where
                                             (subCategory == "Autos - 3 wheelers" && company != "All" ? TV.Company == company : true) &&
                                            ((location != "" ? add.State == location : true) ||
                                                 (location != "" ? add.District == location : true) ||
                                                 (location != "" ? add.Mandal == location : true)) &&
                                                 (add.Type == type) &&
                                                 (add.Status == Constants.ActiveSatus) &&
                                                 (add.SubCategory == "Autos - 3 wheelers") &&
                                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                                select add.AddId).Count();
                    objTV.MiniTrucks4wheelerCount = (from TV in db.TransportationVehicles
                                                     join add in db.Adds on TV.AddId equals add.AddId
                                                     where
                                                 (subCategory == "Mini Trucks - 4 wheelers" && company != "All" ? TV.Company == company : true) &&
                                                 ((location != "" ? add.State == location : true) ||
                                                      (location != "" ? add.District == location : true) ||
                                                      (location != "" ? add.Mandal == location : true)) &&
                                                      (add.Type == type) &&
                                                      (add.Status == Constants.ActiveSatus) &&
                                                      (add.SubCategory == "Mini Trucks - 4 wheelers") &&
                                                      (keyword != "" ? add.Title.Contains(keyword) : true)
                                                     select add.AddId).Count();
                    objTV.LorryTrucksCount = (from TV in db.TransportationVehicles
                                              join add in db.Adds on TV.AddId equals add.AddId
                                              where
                                          (subCategory == "Lorry Trucks" && company != "All" ? TV.Company == company : true) &&
                                          ((location != "" ? add.State == location : true) ||
                                               (location != "" ? add.District == location : true) ||
                                               (location != "" ? add.Mandal == location : true)) &&
                                               (add.Type == type) &&
                                               (add.Status == Constants.ActiveSatus) &&
                                               (add.SubCategory == "Lorry Trucks") &&
                                               (keyword != "" ? add.Title.Contains(keyword) : true)
                                              select add.AddId).Count();
                    objTV.DCMTrucksCount = (from TV in db.TransportationVehicles
                                            join add in db.Adds on TV.AddId equals add.AddId
                                            where
                                        (subCategory == "DCM Trucks" && company != "All" ? TV.Company == company : true) &&
                                        ((location != "" ? add.State == location : true) ||
                                             (location != "" ? add.District == location : true) ||
                                             (location != "" ? add.Mandal == location : true)) &&
                                             (add.Type == type) &&
                                             (add.Status == Constants.ActiveSatus) &&
                                             (add.SubCategory == "DCM Trucks") &&
                                             (keyword != "" ? add.Title.Contains(keyword) : true)
                                            select add.AddId).Count();
                    #endregion

                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At GetCVSubCount", ex);
            }
            return objTV;
        }
        public PV GetPVSubCount(string location, string type, string keyword,  string subCategory, string company)
        {
            PV objPV = new PV();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    #region PV
                    objPV.AutosCount = (from PV in db.PassengerVehicles
                                        join add in db.Adds on PV.AddId equals add.AddId
                                        where
                                    (subCategory == "Autos" && company != "All" ? PV.Company == company : true) &&
                                    ((location != "" ? add.State == location : true) ||
                                         (location != "" ? add.District == location : true) ||
                                         (location != "" ? add.Mandal == location : true)) &&
                                         (add.Type == type) &&
                                         (add.Status == Constants.ActiveSatus) &&
                                         (add.SubCategory == "Autos") &&
                                         (keyword != "" ? add.Title.Contains(keyword) : true)
                                        select add.AddId).Count();
                    objPV.CarsCount = (from PV in db.PassengerVehicles
                                       join add in db.Adds on PV.AddId equals add.AddId
                                       where
   (subCategory == "Cars" && company != "All" ? PV.Company == company : true) &&
                                   ((location != "" ? add.State == location : true) ||
                                        (location != "" ? add.District == location : true) ||
                                        (location != "" ? add.Mandal == location : true)) &&
                                        (add.Type == type) &&
                                        (add.Status == Constants.ActiveSatus) &&
                                        (add.SubCategory == "Cars") &&
                                        (keyword != "" ? add.Title.Contains(keyword) : true)
                                       select add.AddId).Count();

                    objPV.TravelVansCount = (from PV in db.PassengerVehicles
                                             join add in db.Adds on PV.AddId equals add.AddId
                                             where
                                         (subCategory == "Travel Vans" && company != "All" ? PV.Company == company : true) &&
                                         ((location != "" ? add.State == location : true) ||
                                              (location != "" ? add.District == location : true) ||
                                              (location != "" ? add.Mandal == location : true)) &&
                                              (add.Type == type) &&
                                              (add.Status == Constants.ActiveSatus) &&
                                              (add.SubCategory == "Travel Vans") &&
                                              (keyword != "" ? add.Title.Contains(keyword) : true)
                                             select add.AddId).Count();

                    objPV.BikesCount = (from PV in db.PassengerVehicles
                                        join add in db.Adds on PV.AddId equals add.AddId
                                        where
                                    (subCategory == "Bikes" && company != "All" ? PV.Company == company : true) &&
                                    ((location != "" ? add.State == location : true) ||
                                         (location != "" ? add.District == location : true) ||
                                         (location != "" ? add.Mandal == location : true)) &&
                                         (add.Type == type) &&
                                         (add.Status == Constants.ActiveSatus) &&
                                         (add.SubCategory == "Bikes") &&
                                         (keyword != "" ? add.Title.Contains(keyword) : true)
                                        select add.AddId).Count();
                    #endregion

                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At GetPvSubcount", ex);
            }
            return objPV;
        }
        public RE GetRESubCount(string location, string type, string keyword,  string subCategory, string company,string reFilters)
        {
            RE objRE = new RE();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    #region RE
                    Dictionary<string, object> filterColl = new Dictionary<string, object>();
                    JavaScriptSerializer j = new JavaScriptSerializer();
                    object filters = j.Deserialize(reFilters, typeof(object));
                    string availability = "Construction Status";
                    string listedBy = "Listed By";
                   // int priceFrom = 0;
                   // int priceTo = 0;
                    string bedRooms = "Bed Rooms";
                    if (filters.ToString() != "")
                    {
                        filterColl = (Dictionary<string, object>)filters;
                        subCategory = filterColl["subCategory"].ToString();
                        availability = filterColl["availability"].ToString();
                        listedBy = filterColl["listedBy"].ToString();
                        //priceFrom = Convert.ToInt32(filterColl["priceFrom"]);
                        //priceTo = Convert.ToInt32(filterColl["priceTo"]);
                        bedRooms = filterColl["bedRooms"].ToString();
                    }
                    objRE.ApartmentsCount = (from RE in db.RealEstates
                                             join add in db.Adds on RE.AddId equals add.AddId
                                             where
                              (subCategory == "Apartments" && availability != "Construction Status" ? RE.Availability == availability : true) &&
                             (subCategory == "Apartments" && listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                              //(subCategory == "Apartments" && priceFrom != 0 ? RE.Price >= priceFrom : true) &&
                             // (subCategory == "Apartments" && priceTo != 0 ? RE.Price <= priceTo : true) &&
                              (subCategory == "Apartments" && bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&
                                         ((location != "" ? add.State == location : true) ||
                                              (location != "" ? add.District == location : true) ||
                                              (location != "" ? add.Mandal == location : true)) &&
                                              (add.Type == type) &&
                                              (add.Status == Constants.ActiveSatus) &&
                                              (add.SubCategory == "Apartments") &&
                                              (keyword != "" ? add.Title.Contains(keyword) : true)
                                             select add.AddId).Count();

                    objRE.PlotsLandCount = (from RE in db.RealEstates
                                            join add in db.Adds on RE.AddId equals add.AddId
                                            where
                       (subCategory == "Plots/Land" && availability != "Construction Status" ? RE.Availability == availability : true) &&
                       (subCategory == "Plots/Land" && listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                        //(subCategory == "Plots/Land" && priceFrom != 0 ? RE.Price >= priceFrom : true) &&
                       // (subCategory == "Plots/Land" && priceTo != 0 ? RE.Price <= priceTo : true) &&
                        (subCategory == "Plots/Land" && bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&
                                        ((location != "" ? add.State == location : true) ||
                                             (location != "" ? add.District == location : true) ||
                                             (location != "" ? add.Mandal == location : true)) &&
                                             (add.Type == type) &&
                                             (add.Status == Constants.ActiveSatus) &&
                                             (add.SubCategory == "Plots/Land") &&
                                             (keyword != "" ? add.Title.Contains(keyword) : true)
                                            select add.AddId).Count();
                    objRE.AgriculturalLandCount = (from RE in db.RealEstates
                                                   join add in db.Adds on RE.AddId equals add.AddId
                                                   where
                                   (subCategory == "Agricultural Land" && availability != "Construction Status" ? RE.Availability == availability : true) &&
                               (subCategory == "Agricultural Land" && listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                               // (subCategory == "Agricultural Land" && priceFrom != 0 ? RE.Price >= priceFrom : true) &&
                               // (subCategory == "Agricultural Land" && priceTo != 0 ? RE.Price <= priceTo : true) &&
                                (subCategory == "Agricultural Land" && bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&
                                               ((location != "" ? add.State == location : true) ||
                                                    (location != "" ? add.District == location : true) ||
                                                    (location != "" ? add.Mandal == location : true)) &&
                                                    (add.Type == type) &&
                                                    (add.Status == Constants.ActiveSatus) &&
                                                    (add.SubCategory == "Agricultural Land") &&
                                                    (keyword != "" ? add.Title.Contains(keyword) : true)
                                                   select add.AddId).Count();
                    objRE.ShopsOfficesCount = (from RE in db.RealEstates
                                               join add in db.Adds on RE.AddId equals add.AddId
                                               where
                      (subCategory == "Shops & Offices" && availability != "Construction Status" ? RE.Availability == availability : true) &&
                    (subCategory == "Shops & Offices" && listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                    // (subCategory == "Shops & Offices" && priceFrom != 0 ? RE.Price >= priceFrom : true) &&
                   //  (subCategory == "Shops & Offices" && priceTo != 0 ? RE.Price <= priceTo : true) &&
                     (subCategory == "Shops & Offices" && bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&
                                           ((location != "" ? add.State == location : true) ||
                                                (location != "" ? add.District == location : true) ||
                                                (location != "" ? add.Mandal == location : true)) &&
                                                (add.Type == type) &&
                                                (add.Status == Constants.ActiveSatus) &&
                                                (add.SubCategory == "Shops & Offices") &&
                                                (keyword != "" ? add.Title.Contains(keyword) : true)
                                               select add.AddId).Count();
                    objRE.IndependentHousesVillasCount = (from RE in db.RealEstates
                                                          join add in db.Adds on RE.AddId equals add.AddId
                                                          where
                                      (subCategory == "Independent Houses & Villas" && availability != "Construction Status" ? RE.Availability == availability : true) &&
                                   (subCategory == "Independent Houses & Villas" && listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                                   // (subCategory == "Independent Houses & Villas" && priceFrom != 0 ? RE.Price >= priceFrom : true) &&
                                   // (subCategory == "Independent Houses & Villas" && priceTo != 0 ? RE.Price <= priceTo : true) &&
                                    (subCategory == "Independent Houses & Villas" && bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&
                                                      ((location != "" ? add.State == location : true) ||
                                                           (location != "" ? add.District == location : true) ||
                                                           (location != "" ? add.Mandal == location : true)) &&
                                                           (add.Type == type) &&
                                                           (add.Status == Constants.ActiveSatus) &&
                                                           (add.SubCategory == "Independent Houses & Villas") &&
                                                           (keyword != "" ? add.Title.Contains(keyword) : true)
                                                          select add.AddId).Count();

                    objRE.HostelsPGCount = (from RE in db.RealEstates
                                            join add in db.Adds on RE.AddId equals add.AddId
                                            where
          (subCategory == "Hostels & PG" && availability != "Construction Status" ? RE.Availability == availability : true) &&
          (subCategory == "Hostels & PG" && listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
           //(subCategory == "Hostels & PG" && priceFrom != 0 ? RE.Price >= priceFrom : true) &&
           //(subCategory == "Hostels & PG" && priceTo != 0 ? RE.Price <= priceTo : true) &&
           (subCategory == "Hostels & PG" && bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&
                                        ((location != "" ? add.State == location : true) ||
                                             (location != "" ? add.District == location : true) ||
                                             (location != "" ? add.Mandal == location : true)) &&
                                             (add.Type == type) &&
                                             (add.Status == Constants.ActiveSatus) &&
                                             (add.SubCategory == "Hostels & PG") &&
                                             (keyword != "" ? add.Title.Contains(keyword) : true)
                                            select add.AddId).Count();


                    #endregion

                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Getresubcatcount",ex);
            }
            return objRE;
        }

        //public SubCategoryCount GetSubCatCount()
        //{
        //    SubCategoryCount objSubCatCount = new SubCategoryCount();
        //    try
        //    {
        //        using (ClassigooEntities db = new ClassigooEntities())
        //        {
        //            #region AV
        //           AV objAV = new AV();
        //            objAV.BorewellMachineCount=db.Adds.Where(add => add.Category == Constants.AgriculturalVehicle).
        //            Where(add => add.SubCategory == "Borewell Machine").
        //            Where(add => add.Status == Constants.ActiveSatus).Count().ToString();



        //            objAV.TractorsCount = db.Adds./*Where(add => add.Category == Constants.AgriculturalVehicle).*/
        //Where(add => add.SubCategory == "Tractors").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objAV.DozerCount = db.Adds./*Where(add => add.Category == Constants.AgriculturalVehicle).*/
        //Where(add => add.SubCategory == "Dozer").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objAV.HarvesterCount = db.Adds.Where(add => add.Category == Constants.AgriculturalVehicle).
        //Where(add => add.SubCategory == "Combine Harvester").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objAV.BackhoeLoaderCount = db.Adds./*.Where(add => add.Category == Constants.AgriculturalVehicle).*/
        //Where(add => add.SubCategory == "Backhoe Loader").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objAV.ExcavatorsCount = db.Adds./*Where(add => add.Category == Constants.AgriculturalVehicle).*/
        //Where(add => add.SubCategory == "Excavators").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objSubCatCount.AVSubCat = objAV;
        //            #endregion

        //            #region CV
        //            CV objCV = new CV();
        //            objCV.TractorsCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
        //      Where(add => add.SubCategory == "Tractors").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.DozerCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
        //     Where(add => add.SubCategory == "Dozers").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.BackhoeLoaderCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
        //     Where(add => add.SubCategory == "Backhoe_Loader").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.ExcavatorsCount = db.Adds./*Where(add => add.Category == Constants.ConstructionVehicle).*/
        //     Where(add => add.SubCategory == "Excavators").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.WheelLoaderCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
        //     Where(add => add.SubCategory == "Wheel Loader").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.CraneCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
        //     Where(add => add.SubCategory == "Crane").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.TransitMixerCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
        //     Where(add => add.SubCategory == "Transit Mixer").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.SoilCompactorCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
        //     Where(add => add.SubCategory == "Soil Compactor").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objCV.TippersCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).
        //     Where(add => add.SubCategory == "Tippers").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objSubCatCount.CVSubCat = objCV;
        //            #endregion

        //            #region TV
        //            TV objTV = new TV();
        //            objTV.Autos3wheelerCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
        //     Where(add => add.SubCategory == "Autos - 3 wheeler").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objTV.MiniTrucks4wheelerCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
        //     Where(add => add.SubCategory == "Mini Trucks - 4 wheeler").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objTV.LorryTrucksCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
        //     Where(add => add.SubCategory == "Lorry Trucks").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objTV.DCMTrucksCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).
        //     Where(add => add.SubCategory == "DCM Trucks").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objSubCatCount.TVSubCat = objTV;
        //            #endregion

        //            #region PV
        //            PV objPV = new PV();
        //         objPV.AutosCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
        //           Where(add => add.SubCategory == "Autos").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objPV.CarsCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
        //          Where(add => add.SubCategory == "Cars").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objPV.TravelVansCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
        //          Where(add => add.SubCategory == "Travel Vans").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objPV.BikesCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).
        //          Where(add => add.SubCategory == "Bikes").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objSubCatCount.PVSubCat = objPV;
        //            #endregion

        //            #region RE
        //            RE objRE = new RE();
        //        objRE.ApartmentsCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
        //       Where(add => add.SubCategory == "Apartments").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objRE.PlotsLandCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
        //      Where(add => add.SubCategory == "Plots/Land").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objRE.AgriculturalLandCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
        //      Where(add => add.SubCategory == "Agricultural Land").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objRE.ShopsOfficesCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
        //      Where(add => add.SubCategory == "Shops & Offices").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objRE.IndependentHousesVillasCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
        //      Where(add => add.SubCategory == "Independent Houses & Villas").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objRE.HostelsPGCount = db.Adds.Where(add => add.Category == Constants.RealEstate).
        //      Where(add => add.SubCategory == "Hostels & PG").Where(add => add.Status == Constants.ActiveSatus).Count().ToString();
        //            objSubCatCount.RESubCat = objRE;
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Library.WriteLog("exception at get subcategory count commondboperations", ex);
        //    }

        //    return objSubCatCount;
        //}
    }
}