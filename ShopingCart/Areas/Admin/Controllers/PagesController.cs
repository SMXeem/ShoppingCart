using ShopingCart.Models.Data;
using ShopingCart.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> pagelist;
            using (Db db = new Db())
            {
                pagelist = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            return View(pagelist);
        }
        //Add Page
        [HttpGet]
        public ActionResult AddPage()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddPage(PageVM aPage)
        {
            // Check model state
            if (! ModelState.IsValid)
            {
                return View(aPage);
            }
            using(Db db=new Db())
            {
                // Declare slug
                string slug;

                // Init pageDTO
                PagesDt dto = new PagesDt();

                // DTO title
                dto.Title = aPage.Title;

                // Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(aPage.Slug))
                {
                    slug = aPage.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = aPage.Slug.Replace(" ", "-").ToLower();
                }

                // Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == aPage.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Title or Slug already exists");
                    return View(aPage);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = aPage.Body;
                dto.HasSidebar = aPage.HasSidebar;
                dto.Sorting = 100;

                // Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "Page Added";

            // Redirect


            return RedirectToAction("AddPage");
        }
    }
}