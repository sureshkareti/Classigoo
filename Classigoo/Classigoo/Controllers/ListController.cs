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


            //for girdListView
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

            addColl.AddsGrid = gridList;

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

        public ActionResult ApplyFilter(string filterOptions, string category, string location)
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
                    addColl = FilterRE(filterColl, location);
                    break;
                case Constants.AgriculturalVehicle:
                   addColl = FilterAV(filterColl, location);
                    break;
                case Constants.ConstructionVehicle:
                     addColl = FilterCV(filterColl, location);
                    break;
                case Constants.TransportationVehicle:
                    addColl = FilterTV(filterColl, location);
                    break;
                case Constants.PassengerVehicle:
                   addColl = FilterPV(filterColl, location);
                    break;
                default:
                    addColl = FilterCategoryNotSelect(location);
                    break;
            }

            return PartialView("_FillSearchResults", addColl);
        }

        public AddsModel FilterRE(Dictionary<string, object> filterOptions, string location)
        {

            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> realestateColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string pricefrom = filterOptions["pricefrom"].ToString();
                string bedrooms = filterOptions["bedrooms"].ToString();
                //  string construction = filterOptions["construction"].ToString();
                string listedby = filterOptions["listedby"].ToString();
                string furnishing = filterOptions["furnishing"].ToString();

                totalRowCount = db.RealEstates.Where(RealEstate =>

               // (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               //(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
                (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                 (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)



                ).Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  where
                                    // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                                    (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                                    (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true) &&
                                    (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                                    (location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.RealEstates.Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  //where
                                  //(location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = realestateColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterAV(Dictionary<string, object> filterOptions, string location)
        {

            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> avColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string pricefrom = filterOptions["pricefrom"].ToString();
                string bedrooms = filterOptions["bedrooms"].ToString();
                //  string construction = filterOptions["construction"].ToString();
                string listedby = filterOptions["listedby"].ToString();
                string furnishing = filterOptions["furnishing"].ToString();

                totalRowCount = db.AgriculturalVehicles.Where(AV =>

               // (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               //(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               (pricefrom != "Price From" ? AV.Price == pricefrom : true) 
                //(bedrooms != "Bed Rooms" ? AV.Bedrooms == bedrooms : true) &&
                //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
               // (listedby != "Listed By" ? AV.ListedBy == listedby : true) &&
                // (furnishing != "Furnishing" ? AV.Furnishing == furnishing : true)



                ).Count();

                avColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  where
                                    // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                                    (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                                    (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true) &&
                                    (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                                    (location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.RealEstates.Count();

                avColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  //where
                                  //(location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = avColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterCV(Dictionary<string, object> filterOptions, string location)
        {

            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> realestateColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string pricefrom = filterOptions["pricefrom"].ToString();
                string bedrooms = filterOptions["bedrooms"].ToString();
                //  string construction = filterOptions["construction"].ToString();
                string listedby = filterOptions["listedby"].ToString();
                string furnishing = filterOptions["furnishing"].ToString();

                totalRowCount = db.RealEstates.Where(RealEstate =>

               // (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               //(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
                (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                 (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)



                ).Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  where
                                    // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                                    (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                                    (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true) &&
                                    (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                                    (location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.RealEstates.Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  //where
                                  //(location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = realestateColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterTV(Dictionary<string, object> filterOptions, string location)
        {

            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> realestateColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string pricefrom = filterOptions["pricefrom"].ToString();
                string bedrooms = filterOptions["bedrooms"].ToString();
                //  string construction = filterOptions["construction"].ToString();
                string listedby = filterOptions["listedby"].ToString();
                string furnishing = filterOptions["furnishing"].ToString();

                totalRowCount = db.RealEstates.Where(RealEstate =>

               // (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               //(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
                (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                 (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)



                ).Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  where
                                    // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                                    (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                                    (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true) &&
                                    (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                                    (location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.RealEstates.Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  //where
                                  //(location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = realestateColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterPV(Dictionary<string, object> filterOptions, string location)
        {

            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            List<CustomAdd> realestateColl = new List<CustomAdd>();
            int totalRowCount = 0;
            if (filterOptions.Count() > 0)
            {
                string pricefrom = filterOptions["pricefrom"].ToString();
                string bedrooms = filterOptions["bedrooms"].ToString();
                //  string construction = filterOptions["construction"].ToString();
                string listedby = filterOptions["listedby"].ToString();
                string furnishing = filterOptions["furnishing"].ToString();

                totalRowCount = db.RealEstates.Where(RealEstate =>

               // (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
               //(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
               (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
                (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                 (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)



                ).Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  where
                                    // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                                    (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                                    (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true) &&
                                    (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true) &&
                                    (location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            else
            {
                totalRowCount = db.RealEstates.Count();

                realestateColl = (from RealEstate in db.RealEstates
                                  join add in db.Adds on RealEstate.AddId equals add.AddId
                                  //where
                                  //(location != "All India" ? add.Mandal == location : true)
                                  orderby RealEstate.AddId
                                  select new CustomAdd
                                  {
                                      AddId = add.AddId,
                                      Location = add.Mandal,
                                      CreatedDate = add.Created.ToString(),
                                      Title = add.Title,
                                      Description = RealEstate.Description
                                  }).Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();
            }
            addColl.Adds = realestateColl;
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