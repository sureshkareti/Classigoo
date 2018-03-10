using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            customAdd.Createddate = add.Created.ToString();
            customAdd.AddId = add.AddId;
            switch (add.Category)
            {
                case "RealEstate":
                    { 
                         foreach(var item in add.RealEstates)
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

        
        
    }
}