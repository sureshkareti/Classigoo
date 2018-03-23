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
            foreach(var add in addsByPage)
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
            filterColl = (Dictionary<string,object>)filters;
            AddsModel addColl = new AddsModel();
            switch(category)
            {
                case "Cars":
                  addColl=  FilterCars(filterColl);
                    break;
                case "Electronics":
                    addColl = FilterCars(filterColl);
                    break;
                case "RealEstate":
                    addColl = FilterCars(filterColl);
                    break;
            }

            return PartialView("DisplayAdds", addColl);
        }
        public AddsModel FilterCars(Dictionary<string,object> filterOptions)
        {
            int currentPage = 1;
            int maxRows = 5;
            ClassigooEntities db = new ClassigooEntities();
            AddsModel addColl = new AddsModel();
            string model = filterOptions["model"].ToString();
            string price = filterOptions["price"].ToString();
            int totalRowCount = db.Cars.Where(car =>

            (model != "Model" ? car.Model == model : true) &&
             (price != "Price from" ? car.Price == price : true)


            ).Count();
            
            List<CustomAdd> carColl = (from car in db.Cars
                                       join add in db.Adds on car.AddId equals add.AddId
                                       where
            (model != "Model" ? car.Model == model : true) &&
              (price != "Price from" ? car.Price == price : true)
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
        public void FilterRealEstate(Dictionary<string, object> filterOptions)
        {

        }
        public void FilterElectronics(Dictionary<string, object> filterOptions)
        {

        }
    }
}