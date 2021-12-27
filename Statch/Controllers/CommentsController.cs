using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Statch.DAL;
using Statch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Statch.Controllers
{
    public class CommentsController : Controller
    {
        private UsersDAL userContext = new UsersDAL();
        public async Task<ActionResult> Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "User"))
            {
                ViewData["Role"] = null;
            }
            else
            {
                ViewData["Role"] = "User";
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://commentsapisdd.azurewebsites.net/");
            HttpResponseMessage response = await client.GetAsync("/api/comments");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Comments> commentsList = JsonConvert.DeserializeObject<List<Comments>>(data);
                return View(commentsList);
            }
            else
            {
                return View(new List<Comments>());
            }
        }
        public ActionResult Create()
        {
            //Check if the Role of the user is a Judge
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["userName"] = userContext.GetUserName(HttpContext.Session.GetString("LoginID"));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comments c)
        {
            ViewData["userName"] = userContext.GetUserName(HttpContext.Session.GetString("LoginID"));

            if (ModelState.IsValid)
            {
                c.Id = 0;
                c.UserName = userContext.GetUserName(HttpContext.Session.GetString("LoginID"));
                HttpClient client = new HttpClient();
                client.BaseAddress = new
                Uri("https://commentsapisdd.azurewebsites.net/");
                string json = JsonConvert.SerializeObject(c);
                StringContent commentscontent = new StringContent(json, UnicodeEncoding.UTF8,
                "application/json");
                HttpResponseMessage response = await client.PostAsync("/api/comments",
                commentscontent);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Failed to add comment!";
                    return RedirectToAction("Index", "Comments");
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                TempData["Message"] = "";
                return View(c);
            }
        }
    }
}
