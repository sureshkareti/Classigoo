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
        //[HttpPost]
        //public ActionResult DisplayAdds(int currentPageIndex)
        //{
        //    return PartialView(GetAdds(currentPageIndex));
        //}

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
            customAdd.Createddate = (DateTime)add.Created;
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


        public ActionResult GetDataForDatatable(JQueryDataTableParams param)
        {
            ClassigooEntities db = new ClassigooEntities();
            IQueryable<Add> memberCol = db.Adds.AsQueryable();
            int totalCount = memberCol.Count();
            IEnumerable<Add> filteredMembers = memberCol;
       
            //if (!string.IsNullOrEmpty(param.sSearch))
            //{
            //    filteredMembers = memberCol
            //            .Where(m => m.FirstName.Contains(param.sSearch) ||
            //               m.LastName.Contains(param.sSearch) ||
            //               m.Company.Contains(param.sSearch) ||
            //               m.JobTitle.Contains(param.sSearch));
            //}

            //Func<Member, string> orderingFunction = (m => param.iSortCol_0 == 0 ? m.FirstName :
            //                      sortIdx == 1 ? m.LastName :
            //                      sortIdx == 2 ? m.Company :
            //                      m.JobTitle);

            //if (param.sSortDir_0 == "asc")
            //    filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //else
            //    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);

            var displayedMembers = filteredMembers
                     .Skip(param.iDisplayStart)
                     .Take(param.iDisplayLength);

            var result = from a in displayedMembers
                         select new CustomAdd {AddId=a.AddId,Location=a.Location,Createddate=(DateTime)a.Created,
                         Title="dfadf",Description="rerwe"
                         };

            //AddsModel addColl = new AddsModel();
            //List<CustomAdd> coll = new List<CustomAdd>();
            //List<Add> addsByPage = (from add in db.Adds
            //                        select add)
            //            .OrderBy(add => add.Category)
            //            .Skip(param.iDisplayStart)
            //            .Take(param.iDisplayLength).ToList();
            //foreach (var add in addsByPage)
            //{
            //    coll.Add(CheckCategory(add));
            //}
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            },
               JsonRequestBehavior.AllowGet);
        }
    }
}