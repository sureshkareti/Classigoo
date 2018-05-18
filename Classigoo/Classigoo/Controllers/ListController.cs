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
        // GET: List
        public ActionResult Index()
        {
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
            int maxRows = 10;
            ClassigooEntities db = new ClassigooEntities();
            
            AddsModel addColl = new AddsModel();
            List<CustomAdd> coll = new List<CustomAdd>();
            List<Add> addsByPage = (from add in db.Adds
                                    select add)
                        .OrderBy(add => add.Category)
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
            if (filters.ToString() != "")
            {
                filterColl = (Dictionary<string, object>)filters;
            }

            AddsModel addColl = new AddsModel();
            switch (category)
            {
                case "Select Category":
                    addColl = FilterCategoryNotSelect(location);
                    break;
                case Constants.RealEstate:
                    addColl = FilterRE(filterColl, location,keyword,type,pageNum);
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
                    addColl = FilterCategoryNotSelect(location);
                    break;
            }

            return PartialView("_FillSearchResults", addColl);
        }

        public AddsModel FilterRE(Dictionary<string, object> filterOptions, string location,string keyword,string type,int pageNum)
        {
            int currentPage = pageNum;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> realestateColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
               // string subCategory = filterOptions["subCategory"].ToString();
               // string furnishing = filterOptions["furnishing"].ToString();
               // string availability = filterOptions["constructionStatus"].ToString();
               // string listedBy = filterOptions["listedBy"].ToString();
               // string squareFeets = filterOptions["builtupArea"].ToString();
               // string priceFrom = filterOptions["priceFrom"].ToString();
               // string priceTo = filterOptions["priceTo"].ToString();
               // string bedRooms = filterOptions["bedRooms"].ToString();

               // totalRowCount = db.RealEstates.Where(RE =>

               //(subCategory != "All" ? RE.SubCategory == subCategory : true)&&
               //(furnishing != "Bed Rooms" ? RE.Furnishing == furnishing : true)&&
               // (availability != "Bed Rooms" ? RE.Availability == availability : true) &&
               //(listedBy != "Price From" ? RE.ListedBy == listedBy : true) &&
               // (squareFeets != "Bed Rooms" ? RE.SquareFeets == squareFeets : true) &&
               // (priceFrom != "Construction Status" ? RE.Price == priceFrom : true)&&
               // (priceTo != "Listed By" ? RE.Price == priceTo : true) &&
               //  (bedRooms != "Furnishing" ? RE.Bedrooms == bedRooms : true)).Count();

               // realestateColl = (from RE in db.RealEstates
               //                   join add in db.Adds on RE.AddId equals add.AddId
               //                   where
               //                     (subCategory != "All" ? RE.SubCategory == subCategory : true) &&
               //(furnishing != "Bed Rooms" ? RE.Furnishing == furnishing : true) &&
               // (availability != "Bed Rooms" ? RE.Availability == availability : true) &&
               //(listedBy != "Price From" ? RE.ListedBy == listedBy : true) &&
               // (squareFeets != "Bed Rooms" ? RE.SquareFeets == squareFeets : true) &&
               // (priceFrom != "Construction Status" ? RE.Price == priceFrom : true) &&
               // (priceTo != "Listed By" ? RE.Price == priceTo : true) &&
               //  (bedRooms != "Furnishing" ? RE.Bedrooms == bedRooms : true)
               //                   orderby RE.AddId
               //                   select new CustomAdd
               //                   {
               //                       AddId = add.AddId,
               //                       Location = add.Mandal + "," + add.State,
               //                       CreatedDate = add.Created.ToString(),
               //                       Title = add.Title,
               //                       Description = RE.Description,
               //                       ImgUrlPrimary= RE.ImgUrlPrimary,
               //                       Price= RE.Price,
               //                       Category=Constants.RealEstate
               //                   }).Skip((currentPage - 1) * maxRows)
               //             .Take(maxRows).ToList();
            }
            else
            {
            

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  where
                                 (location != "" ? add.State == location : true) &&
                                 //(location != "" ? add.District == location : true) &&
                                 //(location != "" ? add.Mandal == location : true) &&
                                 (add.Type == type) &&
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
                                  }).ToList();
                totalRowCount = realestateColl.Count();

                realestateColl=realestateColl.Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();
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
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> avColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
               // string pricefrom = filterOptions["pricefrom"].ToString();
               // totalRowCount = db.AgriculturalVehicles.Where(AV =>

               //// (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               ////(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               //(pricefrom != "Price From" ? AV.Price == pricefrom : true) 
               



               // ).Count();

               // avColl = (from AV in db.AgriculturalVehicles
               //                   join add in db.Adds on AV.AddId equals add.AddId
               //                   //where
               //                     // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                                   
               //                     //(location != "All India" ? add.Mandal == location : true)
               //                   orderby AV.AddId
               //                   select new CustomAdd
               //                   {
               //                       AddId = add.AddId,
               //                       Location = add.Mandal + "," + add.State,
               //                       CreatedDate = add.Created.ToString(),
               //                       Title = add.Title,
               //                       Description = AV.Description,
               //                       ImgUrlPrimary = AV.ImgUrlPrimary,
               //                       Price = AV.Price,
               //                       Category = Constants.AgriculturalVehicle
               //                   }).Skip((currentPage - 1) * maxRows)
               //             .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.Adds.Where(add =>
                               (location != "" ? add.State == location : true) &&
                               (location != "" ? add.District == location : true) &&
                               (location != "" ? add.Mandal == location : true) &&
                               (add.Type == type) &&
                               (add.Title.Contains(keyword))).Count();

                avColl = (from AV in db.AgriculturalVehicles
                                  join add in db.Adds on AV.AddId equals add.AddId
                          where
                        (location != "" ? add.State == location : true) &&
                        (location != "" ? add.District == location : true) &&
                        (location != "" ? add.Mandal == location : true) &&
                        (add.Type == type) &&
                        (add.Title.Contains(keyword))
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
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> cvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
               // string pricefrom = filterOptions["pricefrom"].ToString();
               // string bedrooms = filterOptions["bedrooms"].ToString();
               // //  string construction = filterOptions["construction"].ToString();
               // string listedby = filterOptions["listedby"].ToString();
               // string furnishing = filterOptions["furnishing"].ToString();

               // totalRowCount = db.ConstructionVehicles.Where(cv =>

               //// (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               ////(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               //(pricefrom != "Price From" ? cv.Price == pricefrom : true) 
               //// (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
               // //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
               //// (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
               // // (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)



               // ).Count();

               // cvColl = (from CV in db.ConstructionVehicles
               //                   join add in db.Adds on CV.AddId equals add.AddId
               //                   //where
               //                   //  // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
               //                   //  (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
               //                   //  (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true) &&
               //                   //  (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
               //                   //  (location != "All India" ? add.Mandal == location : true)
               //                   orderby CV.AddId
               //                   select new CustomAdd
               //                   {
               //                       AddId = add.AddId,
               //                       Location = add.Mandal + "," + add.State,
               //                       CreatedDate = add.Created.ToString(),
               //                       Title = add.Title,
               //                       Description = CV.Description,
               //                       ImgUrlPrimary = CV.ImgUrlPrimary,
               //                       Price = CV.Price,
               //                       Category=Constants.ConstructionVehicle
               //                   }).Skip((currentPage - 1) * maxRows)
               //             .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.Adds.Where(add =>
                               (location != "" ? add.State == location : true) &&
                               (location != "" ? add.District == location : true) &&
                               (location != "" ? add.Mandal == location : true) &&
                               (add.Type == type) &&
                               (add.Title.Contains(keyword))).Count();

                cvColl = (from CV in db.ConstructionVehicles
                                  join add in db.Adds on CV.AddId equals add.AddId
                          where
                        (location != "" ? add.State == location : true) &&
                        (location != "" ? add.District == location : true) &&
                        (location != "" ? add.Mandal == location : true) &&
                        (add.Type == type) &&
                        (add.Title.Contains(keyword))
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
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> tvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
               // string pricefrom = filterOptions["pricefrom"].ToString();
               // string bedrooms = filterOptions["bedrooms"].ToString();
               // //  string construction = filterOptions["construction"].ToString();
               // string listedby = filterOptions["listedby"].ToString();
               // string furnishing = filterOptions["furnishing"].ToString();

               // totalRowCount = db.TransportationVehicles.Where(cv =>

               //// (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               ////(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               //(pricefrom != "Price From" ? cv.Price == pricefrom : true) 
               // //(bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
               // //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
               // //(listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
               // // (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)



               // ).Count();

               // tvColl = (from TV in db.TransportationVehicles
               //                   join add in db.Adds on TV.AddId equals add.AddId
               //                   //where
               //                   //  // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
               //                   //  (listedby != "Listed By" ? TV.ListedBy == listedby : true) &&
               //                   //  (furnishing != "Furnishing" ? TV.Furnishing == furnishing : true) &&
               //                   //  (bedrooms != "Bed Rooms" ? TV.Bedrooms == bedrooms : true) &&
               //                   //  (location != "All India" ? add.Mandal == location : true)
               //                   orderby TV.AddId
               //                   select new CustomAdd
               //                   {
               //                       AddId = add.AddId,
               //                       Location = add.Mandal + "," + add.State,
               //                       CreatedDate = add.Created.ToString(),
               //                       Title = add.Title,
               //                       Description = TV.Description,
               //                       ImgUrlPrimary = TV.ImgUrlPrimary,
               //                       Price = TV.Price,
               //                       Category=Constants.TransportationVehicle
               //                   }).Skip((currentPage - 1) * maxRows)
               //             .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.Adds.Where(add =>
                                 (location != "" ? add.State == location : true) &&
                                 (location != "" ? add.District == location : true) &&
                                 (location != "" ? add.Mandal == location : true) &&
                                 (add.Type == type) &&
                                 (add.Title.Contains(keyword))).Count();

                tvColl = (from TV in db.TransportationVehicles
                                  join add in db.Adds on TV.AddId equals add.AddId
                          where
                        (location != "" ? add.State == location : true) &&
                        (location != "" ? add.District == location : true) &&
                        (location != "" ? add.Mandal == location : true) &&
                        (add.Type == type) &&
                        (add.Title.Contains(keyword))
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
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> pvColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
               // string pricefrom = filterOptions["pricefrom"].ToString();
               // string bedrooms = filterOptions["bedrooms"].ToString();
               // //  string construction = filterOptions["construction"].ToString();
               // string listedby = filterOptions["listedby"].ToString();
               // string furnishing = filterOptions["furnishing"].ToString();

               // totalRowCount = db.PassengerVehicles.Where(pv =>

               //// (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               ////(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               //(pricefrom != "Price From" ? pv.Price == pricefrom : true) 
               // //(bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
               // //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
               //// (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
               // // (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)



               // ).Count();

               // pvColl = (from PV in db.PassengerVehicles
               //                   join add in db.Adds on PV.AddId equals add.AddId
               //                   //where
               //                   //  // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
               //                   //  (listedby != "Listed By" ? PV.ListedBy == listedby : true) &&
               //                   //  (furnishing != "Furnishing" ? PV.Furnishing == furnishing : true) &&
               //                   //  (bedrooms != "Bed Rooms" ? PV.Bedrooms == bedrooms : true) &&
               //                   //  (location != "All India" ? add.Mandal == location : true)
               //                   orderby PV.AddId
               //                   select new CustomAdd
               //                   {
               //                       AddId = add.AddId,
               //                       Location = add.Mandal + "," + add.State,
               //                       CreatedDate = add.Created.ToString(),
               //                       Title = add.Title,
               //                       Description = PV.Description,
               //                       ImgUrlPrimary = PV.ImgUrlPrimary,
               //                       Price = PV.Price,
               //                       Category=Constants.PassengerVehicle
               //                   }).Skip((currentPage - 1) * maxRows)
               //             .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.Adds.Where(add =>
                               (location != "" ? add.State == location : true) &&
                               (location != "" ? add.District == location : true) &&
                               (location != "" ? add.Mandal == location : true) &&
                               (add.Type == type) &&
                               (add.Title.Contains(keyword))).Count();

                pvColl = (from PV in db.PassengerVehicles
                                  join add in db.Adds on PV.AddId equals add.AddId
                          where
                        (location != "" ? add.State == location : true) &&
                        (location != "" ? add.District == location : true) &&
                        (location != "" ? add.Mandal == location : true) &&
                        (add.Type == type) &&
                        (add.Title.Contains(keyword))
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
        public AddsModel FilterCategoryNotSelect(string location)
        {
            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();

            AddsModel addColl = new AddsModel();
            List<CustomAdd> coll = new List<CustomAdd>();
            List<Add> addsByPage = (from add in db.Adds
                                    where
                           (location != "All India" ? add.Mandal == location : true)
                                    orderby add.AddId
                                    select add)
                        .OrderBy(add => add.Category)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            foreach (var add in addsByPage)
            {
                coll.Add(CheckCategory(add));
            }
            double pageCount = (double)((decimal)db.Adds.Count() / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);
            addColl.Adds = coll;
            addColl.CurrentPageIndex = currentPage;
            return addColl;
        }


    }
}