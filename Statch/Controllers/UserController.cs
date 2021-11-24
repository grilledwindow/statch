using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Statch.DAL;
using Statch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statch.Controllers
{
    public class UserController : Controller
    {

        private UsersDAL userContext = new UsersDAL();

        public ActionResult Create()
        {
            ViewData["SalutationList"] = GetSalutations();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user)
        {
            //Lists for the View
            ViewData["SalutationList"] = GetSalutations();
            if (ModelState.IsValid)
            {
                user.UserID = userContext.Add(user);
                TempData["Success"] = "User Creation Successful!";
                return RedirectToAction("LoginView", "Home");
            }
            else
            {
                return View(user);
            }
        }
        private List<SelectListItem> GetSalutations()
        {
            List<SelectListItem> sal = new List<SelectListItem>();
            sal.Add(new SelectListItem
            {
                Value = "Dr",
                Text = "Dr"
            }); sal.Add(new SelectListItem
            {
                Value = "Mr",
                Text = "Mr"
            }); sal.Add(new SelectListItem
            {
                Value = "Ms",
                Text = "Ms"
            }); sal.Add(new SelectListItem
            {
                Value = "Mrs",
                Text = "Mrs"
            }); sal.Add(new SelectListItem
            {
                Value = "Mdm",
                Text = "Mdm"
            });

            return sal;
        }
        public ActionResult Details(int userID)
        {
            // Stop accessing the action if not logged in
            // or account not in the "User" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            userID = Convert.ToInt32(HttpContext.Session.GetString("LoginID"));
            Users user = userContext.GetDetails(userID);
            return View(user);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (HttpContext.Session.GetString("LoginID") == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Details");
            }

            Users user = userContext.GetDetails(Convert.ToInt32(HttpContext.Session.GetString("LoginID")));
            ViewData["SalutationList"] = GetSalutations();

            //If the user contains no details then redirect to Index
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users user)
        {
            ViewData["SalutationList"] = GetSalutations();
            if (ModelState.IsValid)
            {
                user.UserID = userContext.Update(user);
                TempData["Success"] = "Edit Successful!";
                return RedirectToAction("Details");
            }
            else
            {
                TempData["Message"] = "";
                return View(user);
            }

        }
    }
}
