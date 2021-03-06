﻿using System;
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

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new Db())
            {
                // Declare Slug
                string slug;

                // Init pageDTO
                var dto = new PageDTO();

                // DTO Title
                dto.Title = model.Title;

                // Init Slug
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title || x.Slug == model.Slug))
                {
                    ModelState.AddModelError(string.Empty, "That title or slug already exists");
                    return View(model);
                }

                // DTO the rest                
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
               
                // Save Dto           
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "You have added a new page!";

            // Redirect
            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // Declare PageVM
            PageVM model;
           
            using (var db = new Db())
            {
                // Get the page
                var dto = db.Pages.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist.");
                }

                // Init PageVM
                model = new PageVM(dto);
            }

            // Return view with Model
            return View(model);
        }

        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // Check ModelState
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get page id
            var id = model.Id;

            // Init slug
            var slug = "home";

            using (Db db = new Db())
            {
                // Get the Page
                var dto = db.Pages.Find(id);

                // DTO the title
                dto.Title = model.Title;

                //
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                // Mare sure title and slug are unique
                if (db.Pages.Where(p => p.Id != id).Any(p => p.Title == model.Title) ||
                    db.Pages.Where(p => p.Id != id).Any(p => p.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }
                
                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                // Save the DTO
                db.SaveChanges();
            }

            // Set TempData Message
            TempData["SM"] = "You have edited the page!";

            // Redirect
            return RedirectToAction("EditPage");
        }
    }
}