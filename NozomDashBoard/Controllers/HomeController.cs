/*
 * Description: This controller is only responsible for the projectSelection page and passing the selected ProjectID to the dashboard controller.
 * Dependancies: none.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NozomDashBoard.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace NozomDashBoard.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult ProjectSelection()
        {
            //This function opens up the home page in which the user can choose the project to access its dashboard.
            if (User.Identity.GetUserName() == "Admin")
            {
                return View(new HomeModel(true));
            }
            return View(new HomeModel(false));
        }

        [HttpPost]
        public ActionResult ProjectSelection(HomeModel model)
        {
            //This function Posts the selected ProjectId and opens the appropriate client (AdminIndex/Index) based on the logged on user.
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
        public ActionResult AddNewProject()
        {
            return View(new HomeModel());
        }

        [HttpPost]
        public async Task<ActionResult> AddNewProject(HomeModel recivedModel)
        {
            if (recivedModel.m_NewProject != null)
            {
                NozomDashBoardEntities db = new NozomDashBoardEntities();
                db.Project.Add(recivedModel.m_NewProject);
                await db.SaveChangesAsync();
                return RedirectToAction("ProjectSelection");
            }
            else
            {
                return RedirectToAction("ErrorPage", new { msg = "خطأ في الوصول إلى الشبكة" });
            }
        }

        [HttpGet]
        public ActionResult ErrorPage(string msg)
        {
            //This function returns a blank page with error msg as the one passed to it.
            ViewBag.msg = msg;
            return View();
        }

        [HttpGet]
        public ActionResult CloseWindow()
        {
            //This function is used after submit a modification to task only to close the new popup window.
            return View();
        }
    }
}