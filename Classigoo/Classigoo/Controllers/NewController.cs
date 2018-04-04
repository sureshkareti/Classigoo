using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Classigoo.Controllers
{
    public class NewController : Controller
    {
        // GET: New
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Detail()
        {
            return View();
        }
        public ActionResult ShowAdds()
        {
            return View(GetAdds(1));
        }
        [HttpPost]
        public ActionResult DisplayAdds(int currentPageIndex)
        {
            return PartialView(GetAdds(currentPageIndex));
        }

        private AddsModel GetAdds(int currentPage)
        {
            int maxRows = 5;
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
            double pageCount = (double)((decimal)db.Adds.Count() / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;

            return addColl;

        }
        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
            customAdd.Location = add.Location;
            customAdd.CreatedDate = add.Created.ToString();
            customAdd.AddId = add.AddId;
            switch (add.Category)
            {
                case "RealEstate":
                    {

                        foreach (var item in add.RealEstates)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Title = item.Title;

                        }
                        break;
                    }
                case "Cars":
                    {
                        foreach (var item in add.Cars)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Title = item.Title;

                        }
                        break;
                    }
                case "Electronics":
                    {
                        foreach (var item in add.Electronics)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Title = item.Title;

                        }
                        break;
                    }

            }
            return customAdd;
        }
        public ActionResult ApplyFilter(string filterOptions, string category)
        {
            Dictionary<string, object> filterColl = new Dictionary<string, object>();
            JavaScriptSerializer j = new JavaScriptSerializer();
            object filters = j.Deserialize(filterOptions, typeof(object));
            filterColl = (Dictionary<string, object>)filters;
            AddsModel addColl = new AddsModel();
            switch (category)
            {
                case "Cars":
                    addColl = FilterCar(filterColl);
                    break;
                case "Electronics":
                    addColl = FilterElectronic(filterColl);
                    break;
                case "RealEstate":
                    addColl = FilterRealEstate(filterColl);
                    break;
            }

            return PartialView("DisplayAdds", addColl);
        }
        public ActionResult ApplyFilters(string category)
        {
            //Dictionary<string, object> filterColl = new Dictionary<string, object>();
            //JavaScriptSerializer j = new JavaScriptSerializer();
            //object filters = j.Deserialize(filterOptions, typeof(object));
            //filterColl = (Dictionary<string, object>)filters;
            AddsModel addColl = new AddsModel();
            switch (category)
            {
                case "Cars":
                    addColl = FilterCars();
                    break;
                case "Electronics":
                    addColl = FilterElectronics();
                    break;
                case "RealEstate":
                    addColl = FilterRealEstates();
                    break;
            }

            return PartialView("DisplayAdds", addColl);
        }
        public AddsModel FilterCars()
        {
            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
           

            int totalRowCount = db.Cars.Count();

            List<CustomAdd> carColl = (from car in db.Cars
                                       join add in db.Adds on car.AddId equals add.AddId
                              
                                       orderby car.AddId
                                       select new CustomAdd
                                       {
                                           AddId = add.AddId,
                                           Location = add.Location,
                                           CreatedDate = add.Created.ToString(),
                                           Title = car.Title,
                                           Description = car.Description
                                       }).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            addColl.Adds = carColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;
        }
        public AddsModel FilterElectronics()
        {
            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();


            int totalRowCount = db.Electronics.Count();

            List<CustomAdd> carColl = (from Electronic in db.Electronics
                                       join add in db.Adds on Electronic.AddId equals add.AddId

                                       orderby Electronic.AddId
                                       select new CustomAdd
                                       {
                                           AddId = add.AddId,
                                           Location = add.Location,
                                           CreatedDate = add.Created.ToString(),
                                           Title = Electronic.Title,
                                           Description = Electronic.Description
                                       }).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            addColl.Adds = carColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;
        }
        public AddsModel FilterRealEstates()
        {
            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();


            int totalRowCount = db.RealEstates.Count();

            List<CustomAdd> carColl = (from RealEstate in db.RealEstates
                                       join add in db.Adds on RealEstate.AddId equals add.AddId

                                       orderby RealEstate.AddId
                                       select new CustomAdd
                                       {
                                           AddId = add.AddId,
                                           Location = add.Location,
                                           CreatedDate = add.Created.ToString(),
                                           Title = RealEstate.Title,
                                           Description = RealEstate.Description
                                       }).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            addColl.Adds = carColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;
        }
        public AddsModel FilterCar(Dictionary<string, object> filterOptions)
        {
            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
           // string brandname = filterOptions["brandname"].ToString();
            string model = filterOptions["model"].ToString();

            string price = filterOptions["price"].ToString();
          //  string priceto = filterOptions["priceto"].ToString();
            string yearfrom = filterOptions["yearfrom"].ToString();
           // string yearto = filterOptions["yearto"].ToString();
            string fuel = filterOptions["fuel"].ToString();
            string kmdriven = filterOptions["kmdriven"].ToString();
           // string kmdriven1 = filterOptions["kmdriven1"].ToString();

            int totalRowCount = db.Cars.Where(car =>

             // (brandname != "BrandName" ? car.Type == brandname : true) &&
              (model != "Model" ? car.Model == model : true) &&
              (price != "Price from" ? car.Price == price : true) &&
             // (priceto != "Price To" ? car.priceto == priceto : true) &&
              (yearfrom != "Year From" ? car.Year == yearfrom : true) &&
             // (yearto != "Year To" ? car.Year == brandname : true) &&
              (fuel != "Fuel" ? car.Fuel == fuel : true) &&
              (kmdriven != "KM Driven" ? car.KMDriven == kmdriven : true)



            ).Count();

            List<CustomAdd> carColl = (from car in db.Cars
                                       join add in db.Adds on car.AddId equals add.AddId
                                       where
             // (brandname != "BrandName" ? car.Type == brandname : true) &&
              (model != "Model" ? car.Model == model : true) &&
              (price != "Price From" ? car.Price == price : true) &&
              (yearfrom != "Year From" ? car.Year == yearfrom : true) &&
             // (fuel != "FuelType" ? car.Fuel == fuel : true) &&
             (kmdriven != "KM Driven" ? car.KMDriven == kmdriven : true)
                                       orderby car.AddId
                                       select new CustomAdd
                                       {
                                           AddId = add.AddId,
                                           Location = add.Location,
                                           CreatedDate = add.Created.ToString(),
                                           Title = car.Title,
                                           Description = car.Description
                                       }).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            addColl.Adds = carColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;
        }
        public AddsModel FilterRealEstate(Dictionary<string, object> filterOptions)
        {

            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
          //  string category = filterOptions["category"].ToString();
          //  string builtuparea = filterOptions["builtuparea"].ToString();
            string pricefrom = filterOptions["pricefrom"].ToString();
          //  string priceto = filterOptions["priceto"].ToString();
            string bedrooms = filterOptions["bedrooms"].ToString();
          //  string construction = filterOptions["construction"].ToString();
            string listedby = filterOptions["listedby"].ToString();
            string furnishing = filterOptions["furnishing"].ToString();
            
            int totalRowCount = db.RealEstates.Where(RealEstate =>

           // (category != "Bed Rooms" ? RealEstate.SubCategoryId == category : true)
           //(builtuparea != "Bed Rooms" ? RealEstate.bu == builtuparea : true)
           (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true)&&
            (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true)&&
            //(construction != "Construction Status" ? RealEstate. == bedrooms : true)&&
            (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
             (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true)
             


            ).Count();

            List<CustomAdd> realestateColl = (from RealEstate in db.RealEstates
                                              join add in db.Adds on RealEstate.AddId equals add.AddId
                                              where
                    // (pricefrom != "Price From" ? RealEstate.Price == pricefrom : true) &&
                     (listedby != "Listed By" ? RealEstate.ListedBy == listedby : true) &&
                     (furnishing != "Furnishing" ? RealEstate.Furnishing == furnishing : true) &&
                     (bedrooms != "Bed Rooms" ? RealEstate.Bedrooms == bedrooms : true)
                                              orderby RealEstate.AddId
                                              select new CustomAdd
                                              {
                                                  AddId = add.AddId,
                                                  Location = add.Location,
                                                  CreatedDate = add.Created.ToString(),
                                                  Title = RealEstate.Title,
                                                  Description = RealEstate.Description
                                              }).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            addColl.Adds = realestateColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
        public AddsModel FilterElectronic(Dictionary<string, object> filterOptions)
        {

            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();

            string pricefrom = filterOptions["pricefrom"].ToString();
          

            int totalRowCount = db.Electronics.Where(Electronic =>

           
           (pricefrom != "Price From" ? Electronic.Price == pricefrom : true)
           
            ).Count();

            List<CustomAdd> electronicColl = (from Electronic in db.Electronics
                                              join add in db.Adds on Electronic.AddId equals add.AddId
                                              where
                    (pricefrom != "Price From" ? Electronic.Price == pricefrom : true) 
                                              orderby Electronic.AddId
                                              select new CustomAdd
                                              {
                                                  AddId = add.AddId,
                                                  Location = add.Location,
                                                  CreatedDate = add.Created.ToString(),
                                                  Title = Electronic.Title,
                                                  Description = Electronic.Description
                                              }).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            addColl.Adds = electronicColl;
            double pageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(maxRows));
            addColl.PageCount = (int)Math.Ceiling(pageCount);

            addColl.CurrentPageIndex = currentPage;
            return addColl;

        }
    }
}