﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace Classigoo.Controllers
{
    public class ListController : Controller
    {
        // GET: List
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View(GetAdds(1));
        }
        [HttpPost]
        public ActionResult Index(FormCollection coll)
        {
            FiterOptions filterOptions = new FiterOptions();
            filterOptions.Category = coll["category"];
            filterOptions.Location = coll["location"];
            filterOptions.SearchKeyword = coll["searchKeyword"];
            filterOptions.Type = coll["type"];
            ViewBag.FilterValues = filterOptions;
           // ApplyFilter("", 1, coll["category"], coll["location"], coll["searchKeyword"], coll["type"])
            return View(GetAdds(1));
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DisplayAdds(int currentPageIndex)
        {
           
           return PartialView("_FillSearchResults", GetAdds(currentPageIndex));

        }
        private AddsModel GetAdds(int currentPage)
        {
            int maxRows = Constants.NoOfAddsPerPage;
            ClassigooEntities db = new ClassigooEntities();
            
            AddsModel addColl = new AddsModel();
            List<CustomAdd> coll = new List<CustomAdd>();
            List<Add> addsByPage = (from add in db.Adds
                                    where add.Status==Constants.ActiveSatus
                                    select add)
                        .OrderBy(add => add.Created)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            foreach (var add in addsByPage)
            {
                coll.Add(CheckCategory(add));
            }

            addColl.Adds = coll;
            addColl.AddsGrid = GetGridAdds(coll);

            double pageCount = (double)((decimal)db.Adds.Count() / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;

            return addColl;

        }

        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
            customAdd.Location = add.Mandal+","+add.State;

            DateTime dtTemp = add.Created.Value;

            customAdd.CreatedDate = dtTemp.ToString("MMMM") + ", " + dtTemp.Day + ", " + dtTemp.Year; // .mon.ToLongDateString();
            customAdd.AddId = add.AddId;
            customAdd.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(add.Title); 
            customAdd.Category = add.Category;
            switch (add.Category)
            {
                case Constants.RealEstate:
                    {
                        foreach (var item in add.RealEstates)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }               
                case Constants.TransportationVehicle:
                    {
                        foreach (var item in add.TransportationVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case Constants.ConstructionVehicle:
                    {
                        foreach (var item in add.ConstructionVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case Constants.AgriculturalVehicle:
                    {
                        foreach (var item in add.AgriculturalVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case Constants.PassengerVehicle:
                    {
                        foreach (var item in add.PassengerVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }

            }
            return customAdd;
        }
        public List<List<CustomAdd>> GetGridAdds(List<CustomAdd> coll)
        {
            List<List<CustomAdd>> gridList = new List<List<CustomAdd>>();
            List<CustomAdd> tempAddColl = new List<CustomAdd>();
            int count = 0;
            foreach (CustomAdd customAdd in coll)
            {
                if (count == 3)
                {
                    gridList.Add(tempAddColl);
                    tempAddColl = new List<CustomAdd>();
                    count = 0;
                }
                tempAddColl.Add(customAdd);
                count++;
            }
            gridList.Add(tempAddColl);
            return gridList;
        }

        public ActionResult ApplyFilter(string filterOptions,int pageNum, string category, string location,string keyword,string type)
        {
            Dictionary<string, object> filterColl = new Dictionary<string, object>();
            JavaScriptSerializer j = new JavaScriptSerializer();
            object filters = j.Deserialize(filterOptions, typeof(object));
            if (filters.ToString()!="")
            {
                filterColl = (Dictionary<string, object>)filters;
            }

            AddsModel addColl = new AddsModel();
            switch (category)
            {
                case "Select Category":
                    addColl = FilterCategoryNotSelect(location, keyword, type, pageNum);
                    break;
                case Constants.RealEstate:
                    addColl = FilterRE(filterColl,location,keyword,type,pageNum);
                    break;
                case Constants.AgriculturalVehicle:
                   addColl = FilterAV(filterColl, location, keyword, type, pageNum);
                    break;
                case Constants.ConstructionVehicle:
                     addColl = FilterCV(filterColl, location, keyword, type, pageNum);
                    break;
                case Constants.TransportationVehicle:
                    addColl = FilterTV(filterColl, location, keyword, type, pageNum);
                    break;
                case Constants.PassengerVehicle:
                   addColl = FilterPV(filterColl, location, keyword, type, pageNum);
                    break;
                default:
                    addColl = FilterCategoryNotSelect(location, keyword, type, pageNum);
                    break;
            }

            return PartialView("_FillSearchResults", addColl);
        }

        public AddsModel FilterRE(Dictionary<string, object> filterOptions, string location,string keyword,string type,int pageNum)
        {
            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> realestateColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string subCategory = filterOptions["subCategory"].ToString();
                string furnishing = filterOptions["furnishing"].ToString();
                string availability = filterOptions["availability"].ToString();
                string listedBy = filterOptions["listedBy"].ToString();
                string squareFeets = filterOptions["squareFeets"].ToString();
                string priceFrom = filterOptions["priceFrom"].ToString();
                string priceTo = filterOptions["priceTo"].ToString();
                string bedRooms = filterOptions["bedRooms"].ToString();

                totalRowCount = (from RE in db.RealEstates
                                 join add in db.Adds on RE.AddId equals add.AddId
                                 where
                   (subCategory != "Select Category" ? RE.SubCategory == subCategory : true) &&
                (furnishing != "Furnishing" ? RE.Furnishing == furnishing : true) &&
                 (availability != "Construction Status" ? RE.Availability == availability : true) &&
                (listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                 (squareFeets != "Builtup Area" ? RE.SquareFeets == squareFeets : true) &&
                 (priceFrom != "Price From" ? (Convert.ToInt32(RE.Price)) >= 100: true) &&
                 (priceTo != "Price To" ? (Convert.ToInt32(RE.Price)) >= 100 : true) &&
                 (bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&

                             ((location != "" ? add.State == location : true) ||
                                  (location != "" ? add.District == location : true) ||
                                  (location != "" ? add.Mandal == location : true)) &&
                                  (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                  (keyword != "" ? add.Title.Contains(keyword) : true)
                                 select add.AddId).Count();
                   
                realestateColl = (from RE in db.RealEstates
                                  join add in db.Adds on RE.AddId equals add.AddId
                                  where
                 (subCategory != "Select Category" ? RE.SubCategory == subCategory : true) &&
               (furnishing != "Furnishing" ? RE.Furnishing == furnishing : true) &&
                (availability != "Construction Status" ? RE.Availability == availability : true) &&
               (listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                (squareFeets != "Builtup Area" ? RE.SquareFeets == squareFeets : true) &&
                (priceFrom != "Price From" ? RE.Price == priceFrom : true) &&
                (priceTo != "Price To" ? RE.Price == priceTo : true) &&
                (bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true)&&
                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                 (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                  orderby add.Created
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RE.Description,
                                      ImgUrlPrimary = RE.ImgUrlPrimary,
                                      Price = RE.Price,
                                      Category = Constants.RealEstate
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
               
            }
            else
            {
                totalRowCount = GetAddsCount(location, keyword, type, Constants.RealEstate);
                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  where
                                 ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true))&&
                                 (add.Type == type) &&
                                 (add.Status == Constants.ActiveSatus)&&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                  orderby add.Created
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description,
                                      ImgUrlPrimary = RealEstate.ImgUrlPrimary,
                                      Price = RealEstate.Price,
                                      Category = Constants.RealEstate
                                  }).Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();
               
            }
            addColl.Adds = realestateColl;
            addColl.AddsGrid = GetGridAdds(realestateColl);
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterAV(Dictionary<string, object> filterOptions, string location, string keyword, string type, int pageNum)
        {

            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> avColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string subCategory = filterOptions["subCategory"].ToString();
                string company = filterOptions["company"].ToString();
                string priceFrom = filterOptions["priceFrom"].ToString();
                string priceTo = filterOptions["priceTo"].ToString();

                totalRowCount = (from AV in db.AgriculturalVehicles
                                 join add in db.Adds on AV.AddId equals add.AddId
                                 where
                   (subCategory != "Select Category" ? AV.SubCategory == subCategory : true) &&
                (company != "All" ? AV.Company == company : true) &&
                 (priceFrom != "Price From" ? (Convert.ToInt32(AV.Price)) >= 100 : true) &&
                 (priceTo != "Price To" ? Convert.ToInt32(AV.Price) >= 100 : true) &&
                             ((location != "" ? add.State == location : true) ||
                                  (location != "" ? add.District == location : true) ||
                                  (location != "" ? add.Mandal == location : true)) &&
                                  (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                  (keyword != "" ? add.Title.Contains(keyword) : true)
                                 select add.AddId).Count();

                avColl = (from AV in db.AgriculturalVehicles
                                  join add in db.Adds on AV.AddId equals add.AddId
                                  where
                 (subCategory != "Select Category" ? AV.SubCategory == subCategory : true) &&
                (company != "All" ? AV.Company == company : true) &&
                (priceFrom != "Price From" ? AV.Price == priceFrom : true) &&
                (priceTo != "Price To" ? AV.Price == priceTo : true) &&
                
                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                 (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                  orderby add.Created
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = AV.Description,
                                      ImgUrlPrimary = AV.ImgUrlPrimary,
                                      Price = AV.Price,
                                      Category = Constants.RealEstate
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
               
                totalRowCount = GetAddsCount(location, keyword, type, Constants.AgriculturalVehicle);
                avColl = (from AV in db.AgriculturalVehicles
                                  join add in db.Adds on AV.AddId equals add.AddId
                          where
                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                          orderby add.Created
                          select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = AV.Description,
                                      ImgUrlPrimary = AV.ImgUrlPrimary,
                                      Price = AV.Price,
                                      Category=Constants.AgriculturalVehicle
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = avColl;
            addColl.AddsGrid = GetGridAdds(avColl);
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);
            
            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterCV(Dictionary<string, object> filterOptions, string location, string keyword, string type, int pageNum)
        {

            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> cvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string subCategory = filterOptions["subCategory"].ToString();
                string company = filterOptions["company"].ToString();
                string priceFrom = filterOptions["priceFrom"].ToString();
                string priceTo = filterOptions["priceTo"].ToString();

                totalRowCount = (from CV in db.ConstructionVehicles
                                 join add in db.Adds on CV.AddId equals add.AddId
                                 where
                   (subCategory != "Select Category" ? CV.SubCategory == subCategory : true) &&
                (company != "All" ? CV.Company == company : true) &&
                 (priceFrom != "Price From" ? (Convert.ToInt32(CV.Price)) >= 100: true) &&
                 (priceTo != "Price To" ? (Convert.ToInt32(CV.Price)) >= 100: true) &&
                             ((location != "" ? add.State == location : true) ||
                                  (location != "" ? add.District == location : true) ||
                                  (location != "" ? add.Mandal == location : true)) &&
                                  (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                  (keyword != "" ? add.Title.Contains(keyword) : true)
                                 select add.AddId).Count();

                cvColl = (from CV in db.ConstructionVehicles
                          join add in db.Adds on CV.AddId equals add.AddId
                          where
         (subCategory != "Select Category" ? CV.SubCategory == subCategory : true) &&
        (company != "All" ? CV.Company == company : true) &&
        (priceFrom != "Price From" ? CV.Price == priceFrom : true) &&
        (priceTo != "Price To" ? CV.Price == priceTo : true) &&

                        ((location != "" ? add.State == location : true) ||
                         (location != "" ? add.District == location : true) ||
                         (location != "" ? add.Mandal == location : true)) &&
                         (add.Type == type) &&
                         (add.Status == Constants.ActiveSatus) &&
                         (keyword != "" ? add.Title.Contains(keyword) : true)
                          orderby add.Created
                          select new CustomAdd
                          {
                              AddId = add.AddId,
                              Location = add.Mandal + "," + add.State,
                              CreatedDate = add.Created.ToString(),
                              Title = add.Title,
                              Description = CV.Description,
                              ImgUrlPrimary = CV.ImgUrlPrimary,
                              Price = CV.Price,
                              Category = Constants.RealEstate
                          }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = GetAddsCount(location, keyword, type, Constants.ConstructionVehicle);

                cvColl = (from CV in db.ConstructionVehicles
                                  join add in db.Adds on CV.AddId equals add.AddId
                          where
                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                          orderby add.Created
                          select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = CV.Description,
                                      ImgUrlPrimary = CV.ImgUrlPrimary,
                                      Price = CV.Price,
                                      Category = Constants.ConstructionVehicle
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = cvColl;
            addColl.AddsGrid = GetGridAdds(cvColl);
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterTV(Dictionary<string, object> filterOptions, string location, string keyword, string type, int pageNum)
        {

            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> tvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string subCategory = filterOptions["subCategory"].ToString();
                string company = filterOptions["company"].ToString();
                string priceFrom = filterOptions["priceFrom"].ToString();
                string priceTo = filterOptions["priceTo"].ToString();

                totalRowCount = (from AV in db.TransportationVehicles
                                 join add in db.Adds on AV.AddId equals add.AddId
                                 where
                   (subCategory != "Select Category" ? AV.SubCategory == subCategory : true) &&
                (company != "All" ? AV.Company == company : true) &&
                 (priceFrom != "Price From" ? (Convert.ToInt32(AV.Price)) >= 100 : true) &&
                 (priceTo != "Price To" ? (Convert.ToInt32(AV.Price)) >= 100: true) &&
                             ((location != "" ? add.State == location : true) ||
                                  (location != "" ? add.District == location : true) ||
                                  (location != "" ? add.Mandal == location : true)) &&
                                  (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                  (keyword != "" ? add.Title.Contains(keyword) : true)
                                 select add.AddId).Count();

                tvColl = (from TV in db.TransportationVehicles
                          join add in db.Adds on TV.AddId equals add.AddId
                          where
         (subCategory != "Select Category" ? TV.SubCategory == subCategory : true) &&
        (company != "All" ? TV.Company == company : true) &&
        (priceFrom != "Price From" ? TV.Price == priceFrom : true) &&
        (priceTo != "Price To" ? TV.Price == priceTo : true) &&

                        ((location != "" ? add.State == location : true) ||
                         (location != "" ? add.District == location : true) ||
                         (location != "" ? add.Mandal == location : true)) &&
                         (add.Type == type) &&
                         (add.Status == Constants.ActiveSatus) &&
                         (keyword != "" ? add.Title.Contains(keyword) : true)
                          orderby add.Created
                          select new CustomAdd
                          {
                              AddId = add.AddId,
                              Location = add.Mandal + "," + add.State,
                              CreatedDate = add.Created.ToString(),
                              Title = add.Title,
                              Description = TV.Description,
                              ImgUrlPrimary = TV.ImgUrlPrimary,
                              Price = TV.Price,
                              Category = Constants.RealEstate
                          }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = GetAddsCount(location, keyword, type, Constants.TransportationVehicle);

                tvColl = (from TV in db.TransportationVehicles
                                  join add in db.Adds on TV.AddId equals add.AddId
                          where
                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                          orderby add.Created
                          select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = TV.Description,
                                      ImgUrlPrimary = TV.ImgUrlPrimary,
                                      Price = TV.Price,
                                      Category = Constants.TransportationVehicle
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = tvColl;
            addColl.AddsGrid = GetGridAdds(tvColl);
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterPV(Dictionary<string, object> filterOptions, string location, string keyword, string type, int pageNum)
        {

            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> pvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string subCategory= filterOptions["subCategory"].ToString();
                string company = filterOptions["company"].ToString();
                string priceFrom = filterOptions["priceFrom"].ToString();
                string priceTo = filterOptions["priceTo"].ToString();
                string yearFrom = filterOptions["yearFrom"].ToString();
                string yearTo = filterOptions["yearTo"].ToString();
                string kmFrom = filterOptions["kmFrom"].ToString();
                string kmTo = filterOptions["kmTo"].ToString();
                string model = filterOptions["model"].ToString();

                totalRowCount = (from PV in db.PassengerVehicles
                                 join add in db.Adds on PV.AddId equals add.AddId
                                 where
                   (subCategory != "Select Category" ? PV.SubCategory == subCategory : true) &&
                (company != "Furnishing" ? PV.Company == company : true) &&
                 (priceFrom != "Price From" ? (Convert.ToInt32(PV.Price)) >= 100 : true) &&
                 (priceTo != "Price To" ? (Convert.ToInt32(PV.Price)) >= 100 : true) &&
                 (yearFrom != "Price From" ? (Convert.ToInt32(PV.Year)) >= (Convert.ToInt32(yearFrom)) : true) &&
                 (yearTo != "Price To" ? (Convert.ToInt32(PV.Year)) >= (Convert.ToInt32(yearTo)) : true) &&
                 (kmFrom != "Price From" ? (Convert.ToInt32(PV.KMDriven)) >= (Convert.ToInt32(kmFrom)) : true) &&
                 (kmTo != "Price To" ? (Convert.ToInt32(PV.KMDriven)) >= (Convert.ToInt32(kmTo)) : true) &&
                 (model != "Bed Rooms" ? PV.Model == model : true) &&

                             ((location != "" ? add.State == location : true) ||
                                  (location != "" ? add.District == location : true) ||
                                  (location != "" ? add.Mandal == location : true)) &&
                                  (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                  (keyword != "" ? add.Title.Contains(keyword) : true)
                                 select add.AddId).Count();

                pvColl = (from PV in db.PassengerVehicles
                                  join add in db.Adds on PV.AddId equals add.AddId
                                  where
                 (subCategory != "Select Category" ? PV.SubCategory == subCategory : true) &&
                (company != "Furnishing" ? PV.Company == company : true) &&
                 (priceFrom != "Price From" ? (Convert.ToInt32(PV.Price)) >= (Convert.ToInt32(priceFrom)) : true) &&
                 (priceTo != "Price To" ? (Convert.ToInt32(PV.Price)) >= (Convert.ToInt32(priceTo)) : true) &&
                 (yearFrom != "Price From" ? (Convert.ToInt32(PV.Year)) >= (Convert.ToInt32(yearFrom)) : true) &&
                 (yearTo != "Price To" ? (Convert.ToInt32(PV.Year)) >= (Convert.ToInt32(yearTo)) : true) &&
                 (kmFrom != "Price From" ? (Convert.ToInt32(PV.KMDriven)) >= (Convert.ToInt32(kmFrom)) : true) &&
                 (kmTo != "Price To" ? (Convert.ToInt32(PV.KMDriven)) >= (Convert.ToInt32(kmTo)) : true) &&
                 (model != "Bed Rooms" ? PV.Model == model : true) &&
                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                 (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                  orderby add.Created
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = PV.Description,
                                      ImgUrlPrimary = PV.ImgUrlPrimary,
                                      Price = PV.Price,
                                      Category = Constants.RealEstate
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();

            }
            else
            {
                totalRowCount = GetAddsCount(location, keyword, type, Constants.PassengerVehicle);

                pvColl = (from PV in db.PassengerVehicles
                                  join add in db.Adds on PV.AddId equals add.AddId
                          where
                        
                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                          
                          orderby add.Created
                          select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal + "," + add.State,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = PV.Description,
                                      ImgUrlPrimary = PV.ImgUrlPrimary,
                                      Price = PV.Price,
                                      Category = Constants.PassengerVehicle
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = pvColl;
            addColl.AddsGrid = GetGridAdds(pvColl);
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterCategoryNotSelect(string location,string keyword, string type, int pageNum)
        {
            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
            ClassigooEntities db = new ClassigooEntities();
          int  totalRowCount = GetAddsCount(location, keyword, type, "Select Category");
            AddsModel addColl = new AddsModel();
            List<CustomAdd> coll = new List<CustomAdd>();
            List<Add> addsByPage = (from add in db.Adds 
                                    where

                                ((location != "" ? add.State == location : true) ||
                                 (location != "" ? add.District == location : true) ||
                                 (location != "" ? add.Mandal == location : true)) &&
                                 (add.Type == type) &&
                                  (add.Status == Constants.ActiveSatus) &&
                                 (keyword != "" ? add.Title.Contains(keyword) : true)
                                  orderby add.Created select add).Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();
            foreach (var add in addsByPage)
            {
                coll.Add(CheckCategory(add));
            }
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);
            addColl.Adds = coll;
            addColl.AddsGrid = GetGridAdds(coll);
            addColl.CurrentPageIndex = currentPage;
            return addColl;
        }

        public int GetAddsCount(string location, string keyword, string type,string category)
        {
            ClassigooEntities db = new ClassigooEntities();
            int count = 0;
            if (category == "Select Category")
            {
              count = db.Adds.Where(add =>
             ((location != "" ? add.State == location : true) ||
             (location != "" ? add.District == location : true) ||
             (location != "" ? add.Mandal == location : true)) &&
             (add.Type == type) &&
              (add.Status == Constants.ActiveSatus) &&
             (keyword != "" ? add.Title.Contains(keyword) : true)).Count();
            }
            else
            {
               count = db.Adds.Where(add =>
              ((location != "" ? add.State == location : true) ||
              (location != "" ? add.District == location : true) ||
              (location != "" ? add.Mandal == location : true)) &&
              (add.Type == type) &&
               (add.Status == Constants.ActiveSatus) &&
              (keyword != "" ? add.Title.Contains(keyword) : true) &&
              (add.Category == category)).Count();
            }
            return count;
        }
    }
}