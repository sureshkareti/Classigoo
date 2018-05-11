using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
           // return View("Index",GetAdds(currentPageIndex));
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
            customAdd.Title = add.Title;
            customAdd.Category = add.Category;
            switch (add.Category)
            {
                case "Real Estate":
                    {
                        foreach (var item in add.RealEstates)
                        {
                            customAdd.Description = item.Description;
                           
                        }
                        break;
                    }               
                case "Transportation Vehicles":
                    {
                        foreach (var item in add.TransportationVehicles)
                        {
                            customAdd.Description = item.Description;

                        }
                        break;
                    }
                case "Construction Vehicles":
                    {
                        foreach (var item in add.ConstructionVehicles)
                        {
                            customAdd.Description = item.Description;

                        }
                        break;
                    }
                case "Agricultural Vehicles":
                    {
                        foreach (var item in add.AgriculturalVehicles)
                        {
                            customAdd.Description = item.Description;

                        }
                        break;
                    }
                case "Passenger Vehicles":
                    {
                        foreach (var item in add.PassengerVehicles)
                        {
                            customAdd.Description = item.Description;

                        }
                        break;
                    }

            }
            return customAdd;
        }
    }
}