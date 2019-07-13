using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NozomDashBoard.Models;
using Microsoft.AspNet.Identity;

namespace NozomDashBoard.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult ProjectSelection()
        {
            
            return View(new HomeModel());
        }

        [HttpPost]
        public ActionResult ProjectSelection(HomeModel model)
        {
            if (User.Identity.GetUserName() == null)
            {
                return RedirectToAction("Account", "Account");
            }
            else if (User.Identity.GetUserName() == "Admin")
            {
                return RedirectToAction("AdminIndex", "DashBoard", new { ProjectID = model.m_ProjectsResult });

            }
            if (model == null)
            {
                return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في تحميل البيانات" });
                
            }

            return RedirectToAction("Index", "DashBoard", new { ProjectID = model.m_ProjectsResult });

        }

        [HttpGet]
        public ActionResult ErrorPage(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }

        [HttpGet]
        public ActionResult CloseWindow()
        {
            return View();
        }
    }
}