using Classigoo.Models;
using System;
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
        private ClassigooEntities db = new ClassigooEntities();
        // GET: List
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Index(string category = "Select Category", string type = "Rent")
        {
            try
            {
                FiterOptions filterOptions = new FiterOptions();
                filterOptions.Category = category;
                filterOptions.Location = "";
                filterOptions.SearchKeyword = "";
                filterOptions.Type = type;
                ViewBag.FilterValues = filterOptions;
            }
            catch(Exception ex)
            {
                Library.WriteLog("At List index",ex);
            }
            return ApplyFilter("\"\"", 1, category, "", "", type, true);
        }
        [HttpPost]
        public ActionResult Index(FormCollection coll)
        {
            try
            {
                FiterOptions filterOptions = new FiterOptions();
                filterOptions.Category = coll["category"];
                filterOptions.Location = coll["location"];
                filterOptions.SearchKeyword = coll["searchKeyword"];
                filterOptions.Type = coll["type"];
                ViewBag.FilterValues = filterOptions;
            }
            catch(Exception ex)
            {
                Library.WriteLog("At List index post",ex);
            }
            return ApplyFilter("\"\"", 1, coll["category"], coll["location"], coll["searchKeyword"], coll["type"], true);
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
            try
            {
                customAdd.Location = add.Mandal+", " +add.District + ", " + add.State;

                DateTime dtTemp = add.Created.Value;
                customAdd.CreatedDate = dtTemp.ToString("MMMM") + ", " + dtTemp.Day + ", " + dtTemp.Year; // .mon.ToLongDateString();
                customAdd.AddId = add.AddId;
                customAdd.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(add.Title);
                customAdd.Category = add.Category;
                CommonDBOperations objCommonDbOperations = new CommonDBOperations();
                switch (add.Category)
                {
                    case Constants.RealEstate:
                        {
                            #region RealEstates
                            RealEstate re = objCommonDbOperations.GetRealEstate(add.AddId.ToString());
                            if(re!=null)
                            {
                                customAdd.Description = re.Description;
                                customAdd.ImgUrlPrimary = re.ImgUrlPrimary;
                                customAdd.Price = re.Price;
                            }
                            #endregion
                            break;
                        }
                    case Constants.TransportationVehicle:
                        {
                            #region TransportationVehicles
                            TransportationVehicle tv = objCommonDbOperations.GetTV(add.AddId.ToString());
                            if(tv!=null)
                            {
                                customAdd.Description = tv.Description;
                                customAdd.ImgUrlPrimary = tv.ImgUrlPrimary;
                                customAdd.Price = tv.Price;
                            }
                            #endregion
                            break;
                        }
                    case Constants.ConstructionVehicle:
                        {
                            #region ConstructionVehicles
                            ConstructionVehicle cv = objCommonDbOperations.GetCV(add.AddId.ToString());
                           if(cv!=null)
                            {
                                customAdd.Description = cv.Description;
                                customAdd.ImgUrlPrimary = cv.ImgUrlPrimary;
                                customAdd.Price = cv.Price;
                            }
                            #endregion
                            break;
                        }
                    case Constants.AgriculturalVehicle:
                        {
                            #region AgriculturalVehicles
                            AgriculturalVehicle av = objCommonDbOperations.GetAV(add.AddId.ToString());
                            if(av!=null)
                            {
                                customAdd.Description = av.Description;
                                customAdd.ImgUrlPrimary = av.ImgUrlPrimary;
                                customAdd.Price = av.Price;
                            }
                            #endregion
                            break;
                        }
                    case Constants.PassengerVehicle:
                        {
                            #region PassengerVehicles
                            PassengerVehicle pv = objCommonDbOperations.GetPV(add.AddId.ToString());
                            if(pv!=null)
                            {
                                customAdd.Description = pv.Description;
                                customAdd.ImgUrlPrimary = pv.ImgUrlPrimary;
                                customAdd.Price = pv.Price;
                            }
                            #endregion
                            break;
                        }

                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At checking category list controller",ex);
            }
            return customAdd;
        }
        public List<List<CustomAdd>> GetGridAdds(List<CustomAdd> coll)
        {
            List<List<CustomAdd>> gridList = new List<List<CustomAdd>>();
            List<CustomAdd> tempAddColl = new List<CustomAdd>();
            int count = 0;
            try
            {
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
            }
            catch(Exception ex)
            {
                Library.WriteLog("At adding adds in grid",ex);
            }
            return gridList;
        }

        public ActionResult ApplyFilter(string filterOptions, int pageNum, string category, string location, string keyword, string type, bool isSearchFrmHomePage)
        {
            AddsModel addColl = new AddsModel();
            Dictionary<string, object> filterColl = new Dictionary<string, object>();
            JavaScriptSerializer j = new JavaScriptSerializer();
            try
            {
            
                object filters = j.Deserialize(filterOptions, typeof(object));
                if (filters.ToString() != "")
                {
                    filterColl = (Dictionary<string, object>)filters;
                }
                switch (category)
                {
                    case "Select Category":
                        addColl = FilterCategoryNotSelect(location, keyword, type, pageNum);
                        break;
                    case Constants.RealEstate:
                        addColl = FilterRE(filterColl, location, keyword, type, pageNum);
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
            }
            catch(Exception ex)
            {
                Library.WriteLog("At apply filter searched from home pagem? - " + isSearchFrmHomePage,ex);

            }
            if (isSearchFrmHomePage)
            {
                return View("Index", addColl);
            }
            else
            {
                return PartialView("_FillSearchResults", addColl);
            }
        }

        public AddsModel FilterRE(Dictionary<string, object> filterOptions, string location, string keyword, string type, int pageNum)
        {
            
            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
            //ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> realestateColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                try
                {
                    string subCategory = filterOptions["subCategory"].ToString();
                    string furnishing = filterOptions["furnishing"].ToString();
                    string availability = filterOptions["availability"].ToString();
                    string listedBy = filterOptions["listedBy"].ToString();
                    int squareFeetsFrom = Convert.ToInt32(filterOptions["squareFeetsFrom"]);
                    int squareFeetsTo = Convert.ToInt32(filterOptions["squareFeetsTo"]);
                    int priceFrom = Convert.ToInt32(filterOptions["priceFrom"]);
                    int priceTo = Convert.ToInt32(filterOptions["priceTo"]);
                    string bedRooms = filterOptions["bedRooms"].ToString();

                    totalRowCount = (from RE in db.RealEstates
                                     join add in db.Adds on RE.AddId equals add.AddId
                                     where
                       (subCategory != "Select Category" ? RE.SubCategory == subCategory : true) &&
                    (furnishing != "Furnishing" ? RE.Furnishing == furnishing : true) &&
                     (availability != "Construction Status" ? RE.Availability == availability : true) &&
                    (listedBy != "Listed By" ? RE.ListedBy == listedBy : true) &&
                     (squareFeetsFrom != 0 ? RE.SquareFeets >= squareFeetsFrom : true) &&
                     (squareFeetsTo != 0 ? RE.SquareFeets <= squareFeetsTo : true) &&
                     (priceFrom != 0 ? RE.Price >= priceFrom : true) &&
                     (priceTo != 0 ? RE.Price <= priceTo : true) &&
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
                    (squareFeetsFrom != 0 ? RE.SquareFeets >= squareFeetsFrom : true) &&
                     (squareFeetsTo != 0 ? RE.SquareFeets <= squareFeetsTo : true) &&
                    (priceFrom != 0 ? RE.Price >= priceFrom : true) &&
                    (priceTo != 0 ? RE.Price <= priceTo : true) &&
                    (bedRooms != "Bed Rooms" ? RE.Bedrooms == bedRooms : true) &&
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
                catch(Exception ex)
                {
                    Library.WriteLog("At Filter RE with filter options", ex);
                }

            }
            else
            {
                try
                {
                    totalRowCount = GetAddsCount(location, keyword, type, Constants.RealEstate);
                    realestateColl = (from RealEstate in db.RealEstates
                                      join add in db.Adds on RealEstate.AddId equals add.AddId
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
                                          Description = RealEstate.Description,
                                          ImgUrlPrimary = RealEstate.ImgUrlPrimary,
                                          Price = RealEstate.Price,
                                          Category = Constants.RealEstate
                                      }).Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();
                }
                catch(Exception ex)
                {
                    Library.WriteLog("Ar Fiter RE without filter options", ex);
                }

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
            //ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> avColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                try
                {
                    string subCategory = filterOptions["subCategory"].ToString();
                    string company = filterOptions["company"].ToString();
                    int priceFrom = Convert.ToInt32(filterOptions["priceFrom"]);
                    int priceTo = Convert.ToInt32(filterOptions["priceTo"]);

                    totalRowCount = (from AV in db.AgriculturalVehicles
                                     join add in db.Adds on AV.AddId equals add.AddId
                                     where
                       (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
                    (company != "All" ? AV.Company == company : true) &&
                     (priceFrom != 0 ? AV.Price >= priceFrom : true) &&
                     (priceTo != 0 ? AV.Price <= priceTo : true) &&
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
             (subCategory != "All" ? AV.SubCategory == subCategory : true) &&
            (company != "All" ? AV.Company == company : true) &&
           (priceFrom != 0 ? AV.Price >= priceFrom : true) &&
             (priceTo != 0 ? AV.Price <= priceTo : true) &&

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
                                  Category = Constants.AgriculturalVehicle
                              }).Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();
                }
                catch(Exception ex)
                {
                    Library.WriteLog("At Filter AV with filter options", ex);
                }
            }
            else
            {
                try
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
                                  Category = Constants.AgriculturalVehicle
                              }).Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();
                }
                catch(Exception ex)
                {
                    Library.WriteLog("Ar Fiter AV without filter options", ex);
                }
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
         //   ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> cvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                try
                {
                    string subCategory = filterOptions["subCategory"].ToString();
                    string company = filterOptions["company"].ToString();
                    int priceFrom = Convert.ToInt32(filterOptions["priceFrom"]);
                    int priceTo = Convert.ToInt32(filterOptions["priceTo"]);

                    totalRowCount = (from CV in db.ConstructionVehicles
                                     join add in db.Adds on CV.AddId equals add.AddId
                                     where
                       (subCategory != "All" ? CV.SubCategory == subCategory : true) &&
                    (company != "All" ? CV.Company == company : true) &&
                     (priceFrom != 0 ? CV.Price >= priceFrom : true) &&
                     (priceTo != 0 ? CV.Price <= priceTo : true) &&
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
             (subCategory != "All" ? CV.SubCategory == subCategory : true) &&
            (company != "All" ? CV.Company == company : true) &&
            (priceFrom != 0 ? CV.Price >= priceFrom : true) &&
                     (priceTo != 0 ? CV.Price <= priceTo : true) &&

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
                catch(Exception ex)
                {
                    Library.WriteLog("At Filter CV with filter options", ex);
                }
            }
            else
            {
                try
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
                catch(Exception ex)
                {
                    Library.WriteLog("Ar Fiter CV without filter options", ex);
                }
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
          //  ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> tvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                try
                {
                    string subCategory = filterOptions["subCategory"].ToString();
                    string company = filterOptions["company"].ToString();
                    int priceFrom = Convert.ToInt32(filterOptions["priceFrom"]);
                    int priceTo = Convert.ToInt32(filterOptions["priceTo"]);

                    totalRowCount = (from TV in db.TransportationVehicles
                                     join add in db.Adds on TV.AddId equals add.AddId
                                     where
                       (subCategory != "All" ? TV.SubCategory == subCategory : true) &&
                    (company != "All" ? TV.Company == company : true) &&
                    (priceFrom != 0 ? TV.Price >= priceFrom : true) &&
                     (priceTo != 0 ? TV.Price <= priceTo : true) &&
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
             (subCategory != "All" ? TV.SubCategory == subCategory : true) &&
            (company != "All" ? TV.Company == company : true) &&
            (priceFrom != 0 ? TV.Price >= priceFrom : true) &&
                     (priceTo != 0 ? TV.Price <= priceTo : true) &&

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
                catch(Exception ex)
                {
                    Library.WriteLog("At Filter TV with filter options", ex);
                }
            }
            else
            {
                try
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
                catch(Exception ex)
                {
                    Library.WriteLog("Ar Fiter TV without filter options", ex);
                }
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
        //    ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> pvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                try
                {
                    string subCategory = filterOptions["subCategory"].ToString();
                    string company = filterOptions["company"].ToString();
                    int priceFrom = Convert.ToInt32(filterOptions["priceFrom"]);
                    int priceTo = Convert.ToInt32(filterOptions["priceTo"]);
                    int yearFrom = Convert.ToInt32(filterOptions["yearFrom"]);
                    int yearTo = Convert.ToInt32(filterOptions["yearTo"]);
                    int kmFrom = Convert.ToInt32(filterOptions["kmFrom"]);
                    int kmTo = Convert.ToInt32(filterOptions["kmTo"]);
                    string model = filterOptions["model"].ToString();
                    string fuelType = filterOptions["fuelType"].ToString();

                    totalRowCount = (from PV in db.PassengerVehicles
                                     join add in db.Adds on PV.AddId equals add.AddId
                                     where
                       (subCategory != "All" ? PV.SubCategory == subCategory : true) &&
                    (company != "All" ? PV.Company == company : true) &&
                     (priceFrom != 0 ? PV.Price >= priceFrom : true) &&
                     (priceTo != 0 ? PV.Price <= priceTo : true) &&
                     (yearFrom != 0 ? PV.Year >= yearFrom : true) &&
                     (yearTo != 0 ? PV.Year <= yearTo : true) &&
                     (kmFrom != 0 ? PV.KMDriven >= kmFrom : true) &&
                     (kmTo != 0 ? PV.KMDriven <= kmTo : true) &&
                     (model != "All" ? PV.Model == model : true) &&
                     (fuelType != "All" ? PV.FuelType == fuelType : true) &&

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
            (subCategory != "All" ? PV.SubCategory == subCategory : true) &&
            (company != "All" ? PV.Company == company : true) &&
             (priceFrom != 0 ? PV.Price >= priceFrom : true) &&
             (priceTo != 0 ? PV.Price <= priceTo : true) &&
             (yearFrom != 0 ? PV.Year >= yearFrom : true) &&
             (yearTo != 0 ? PV.Year <= yearTo : true) &&
             (kmFrom != 0 ? PV.KMDriven >= kmFrom : true) &&
             (kmTo != 0 ? PV.KMDriven <= kmTo : true) &&
             (model != "All" ? PV.Model == model : true) &&
              (fuelType != "All" ? PV.FuelType == fuelType : true) &&
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
                catch (Exception ex)
                {
                    Library.WriteLog("At Filter PV with filter options", ex);
                }

            }
            else
            {
                try
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
                catch (Exception ex)
                {
                    Library.WriteLog("Ar Fiter PV without filter options",ex);
                }
                addColl.Adds = pvColl;
                addColl.AddsGrid = GetGridAdds(pvColl);
                double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
                addColl.PageCount = (int)Math.Ceiling(pageCount);

                addColl.CurrentPageIndex = currentPage; 
            }
            return addColl;
        }
        public AddsModel FilterCategoryNotSelect(string location, string keyword, string type, int pageNum)
        {
            int currentPage = pageNum;
            int maxRows = Constants.NoOfAddsPerPage;
           // ClassigooEntities db = new ClassigooEntities();
            int totalRowCount = GetAddsCount(location, keyword, type, "Select Category");
            AddsModel addColl = new AddsModel();
            List<CustomAdd> coll = new List<CustomAdd>();
            try
            {
                List<Add> addsByPage = (from add in db.Adds
                                        where

                                    ((location != "" ? add.State == location : true) ||
                                     (location != "" ? add.District == location : true) ||
                                     (location != "" ? add.Mandal == location : true)) &&
                                     (add.Type == type) &&
                                      (add.Status == Constants.ActiveSatus) &&
                                     (keyword != "" ? add.Title.Contains(keyword) : true)
                                        orderby add.Created
                                        select add).Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();
                foreach (var add in addsByPage)
                {
                    coll.Add(CheckCategory(add));
                }
                double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
                addColl.PageCount = (int)Math.Ceiling(pageCount);
                addColl.Adds = coll;
                addColl.AddsGrid = GetGridAdds(coll);
                addColl.CurrentPageIndex = currentPage;
            }
            catch(Exception ex)
            {
                Library.WriteLog("At filter adds when no category selected",ex);
            }
            return addColl;
        }

        public int GetAddsCount(string location, string keyword, string type, string category)
        {
        //    ClassigooEntities db = new ClassigooEntities();
            int count = 0;
            try
            {
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
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Get adds count", ex);
            }
            return count;
        }

        public JsonResult GetCount()
        {
            Dictionary<string, int> countColl = new Dictionary<string, int>();
            try
            {
                //ClassigooEntities db = new ClassigooEntities();
                int pvRentCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).Where(add => add.Type == "Rent").Where(add => add.Status == Constants.ActiveSatus).Count();
                int pvSaleCount = db.Adds.Where(add => add.Category == Constants.PassengerVehicle).Where(add => add.Type == "Sale").Where(add => add.Status == Constants.ActiveSatus).Count();
                int avRentCount = db.Adds.Where(add => add.Category == Constants.AgriculturalVehicle).Where(add => add.Type == "Rent").Where(add => add.Status == Constants.ActiveSatus).Count();
                int avSaleCount = db.Adds.Where(add => add.Category == Constants.AgriculturalVehicle).Where(add => add.Type == "Sale").Where(add => add.Status == Constants.ActiveSatus).Count();
                int tvRentCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).Where(add => add.Type == "Rent").Where(add => add.Status == Constants.ActiveSatus).Count();
                int tvSaleCount = db.Adds.Where(add => add.Category == Constants.TransportationVehicle).Where(add => add.Type == "Sale").Where(add => add.Status == Constants.ActiveSatus).Count();
                int cvRentCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).Where(add => add.Type == "Rent").Where(add => add.Status == Constants.ActiveSatus).Count();
                int cvSaleCount = db.Adds.Where(add => add.Category == Constants.ConstructionVehicle).Where(add => add.Type == "Sale").Where(add => add.Status == Constants.ActiveSatus).Count();
                int reRentCount = db.Adds.Where(add => add.Category == Constants.RealEstate).Where(add => add.Type == "Rent").Where(add => add.Status == Constants.ActiveSatus).Count();
                int reSaleCount = db.Adds.Where(add => add.Category == Constants.RealEstate).Where(add => add.Type == "Sale").Where(add => add.Status == Constants.ActiveSatus).Count();
                countColl.Add("pvRentCount", pvRentCount); countColl.Add("pvSaleCount", pvSaleCount); countColl.Add("avRentCount", avRentCount);
                countColl.Add("avSaleCount", avSaleCount); countColl.Add("tvRentCount", tvRentCount); countColl.Add("tvSaleCount", tvSaleCount);
                countColl.Add("cvRentCount", cvRentCount); countColl.Add("cvSaleCount", cvSaleCount); countColl.Add("reRentCount", reRentCount);
                countColl.Add("reSaleCount", reSaleCount);
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Get num of adds in home page", ex);
            }
            return Json(countColl, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult PreviewAdd(string addId)
        {
            CustomAdd customAdd = new CustomAdd();
            try
            {
                CommonDBOperations db = new CommonDBOperations();
                Add  add= db.GetAdd(addId);
                customAdd.Location = add.Mandal + "," + add.State;
                customAdd.CreatedDate = add.Created.ToString();
                customAdd.AddId = add.AddId;
                customAdd.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(add.Title);
                #region RealEstates
                if (add.Category == Constants.RealEstate)
                {
                    RealEstate re = db.GetRealEstate(add.AddId.ToString());
                    if(re!=null)
                    {
                        customAdd.RE = re;
                        customAdd.Description = re.Description;
                        customAdd.Price = re.Price;
                        customAdd.Category = Constants.RealEstate;
                        customAdd.ImgUrlPrimary = re.ImgUrlPrimary;
                        customAdd.ImgUrlSeconday = re.ImgUrlSeconday;
                        customAdd.ImgUrlThird = re.ImgUrlThird;
                        customAdd.ImgUrlFourth = re.ImgUrlFourth;
                    }
                }
                #endregion
                #region TransportationVehicles
                else if (add.Category ==Constants.TransportationVehicle)
                {
                    TransportationVehicle tv = db.GetTV(add.AddId.ToString());
                    if (tv != null)
                    {
                        customAdd.Description = tv.Description;
                        customAdd.Price = tv.Price;
                        customAdd.Category = Constants.TransportationVehicle;
                        customAdd.ImgUrlPrimary = tv.ImgUrlPrimary;
                        customAdd.ImgUrlSeconday = tv.ImgUrlSeconday;
                        customAdd.ImgUrlThird = tv.ImgUrlThird;
                        customAdd.ImgUrlFourth = tv.ImgUrlFourth;
                        customAdd.Company = tv.Company;
                    }
                }
                #endregion
                #region ConstructionVehicles
                else if (add.Category == Constants.ConstructionVehicle)
                {
                    ConstructionVehicle cv = db.GetCV(add.AddId.ToString());
                    if (cv != null)
                    {
                        customAdd.Description = cv.Description;
                        customAdd.Price = cv.Price;
                        customAdd.Category = Constants.ConstructionVehicle;
                        customAdd.ImgUrlPrimary = cv.ImgUrlPrimary;
                        customAdd.ImgUrlSeconday = cv.ImgUrlSeconday;
                        customAdd.ImgUrlThird = cv.ImgUrlThird;
                        customAdd.ImgUrlFourth = cv.ImgUrlFourth;
                        customAdd.Company = cv.Company;
                    }
                }
                #endregion
                #region AgriculturalVehicles
                else if (add.Category == Constants.AgriculturalVehicle)
                {
                    AgriculturalVehicle av = db.GetAV(add.AddId.ToString());
                    if (av != null)
                    {
                        customAdd.Description = av.Description;
                        customAdd.Price = av.Price;
                        customAdd.Category = Constants.AgriculturalVehicle;
                        customAdd.ImgUrlPrimary = av.ImgUrlPrimary;
                        customAdd.ImgUrlSeconday = av.ImgUrlSeconday;
                        customAdd.ImgUrlThird = av.ImgUrlThird;
                        customAdd.ImgUrlFourth = av.ImgUrlFourth;
                        customAdd.Company = av.Company;
                    }
                  
                }
                #endregion
                #region PassengerVehicles
                else if (add.Category == Constants.PassengerVehicle)
                {
                    PassengerVehicle pv = db.GetPV(add.AddId.ToString());
                    if (pv != null)
                    {
                        customAdd.PV = pv;
                        customAdd.Description = pv.Description;
                        customAdd.Price = pv.Price;
                        customAdd.Category = Constants.PassengerVehicle;
                        customAdd.ImgUrlPrimary = pv.ImgUrlPrimary;
                        customAdd.ImgUrlSeconday = pv.ImgUrlSeconday;
                        customAdd.ImgUrlThird = pv.ImgUrlThird;
                        customAdd.ImgUrlFourth = pv.ImgUrlFourth;
                    }
                   
                }
                #endregion
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Preview add addid- " + addId, ex);
            }
            return View(customAdd);
        }

        [HttpPost]
        public ActionResult AddChat(string usermessage,string AddId)
        {
            bool status = false;
            try
            {
                MessageDBOperations msgDbObj = new MessageDBOperations();
                Guid toUserId = (Guid)msgDbObj.GetAddOwnerUserId(Convert.ToInt32(AddId));
                UserController useCntObj = new UserController();
                useCntObj.ControllerContext = new ControllerContext(this.Request.RequestContext, useCntObj);
                Guid frmUserId = useCntObj.GetUserId();
                Message msg = new Message();
                msg.AdId = Convert.ToInt32(AddId);
                msg.CreatedOn = CustomActions.GetCurrentISTTime();
                msg.FromUserId = frmUserId;
                msg.ToUserId = toUserId;
                msg.RequestorUserId = frmUserId;
                msg.Message1 = usermessage;
                status = msgDbObj.AddChat(msg);         
            }
            catch(Exception ex)
            {
                Library.WriteLog("At AddChat",ex);
            }
            if (status)
            {
                return RedirectToAction("Home", "User");
            }
            else
            {
                return View();
            }
        }
    }
}