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

                addColl.Adds = (from add in db.Adds
                                           select add)
                            .OrderBy(add=>add.Category)
                            .Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();

                double pageCount = (double)((decimal)db.Adds.Count() / Convert.ToDecimal(maxRows));
                addColl.PageCount = (int)Math.Ceiling(pageCount);

                addColl.CurrentPageIndex = currentPage;

                return addColl;
            
        }
    }
}