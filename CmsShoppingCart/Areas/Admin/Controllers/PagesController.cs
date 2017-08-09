using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare a list of PageVM
            List<PageVM> pagesList;

            // Init the list
            using (Db db = new Db())
            {
                pagesList = db.Pages.ToList().OrderBy(p => p.Sorting).Select(p => new PageVM(p)).ToList();
            }
            // return view with list
            return View(pagesList);
        }
    }
}