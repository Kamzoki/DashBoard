//TODO: realtime updating dashboard

//TODO: design task details
//TODO: design task operations
//TODO: design signout button in Index
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NozomDashBoard.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace NozomDashBoard.Controllers
{
    public class DashBoardController : Controller
    {
        public enum ClientState
        {
            UpToDate,
            OutDated
        };

        public static ClientState Mansy = ClientState.UpToDate;
        public static ClientState Abdullah = ClientState.UpToDate;
        public static ClientState Omar = ClientState.UpToDate;
        public static ClientState Admin = ClientState.UpToDate;

        private NozomDashBoardEntities db = new NozomDashBoardEntities();

        #region Helper
        private DashBoardModel InitializeDashBoard(ref int? ProjectID, bool isAdmin)
        {
            DashBoardModel model = new DashBoardModel(ProjectID);

            model.m_AllTasks = db.EquireTasks("TaskStates", false, ProjectID, null).ToList();
            model.isAdmin = isAdmin;

            return model;
        }
        private bool? isUserAccissable(ref int? userID)
        {
            string givenUserName;

            switch (userID)
            {
                case 2: givenUserName = "Ahmad Abdullatef"; break;
                case 3: givenUserName = "Omar Arf"; break;
                case 4: givenUserName = "Abdullah Saad"; break;
                case 5: givenUserName = "Ahmad Mansy"; break;
                default: givenUserName = null; break;
            }

            if (givenUserName == null)
            {
                return null;
            }
            else if (User.Identity.GetUserName() == givenUserName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult UpdateClient()
        {
            switch (User.Identity.GetUserName())
            {
                case "Admin":
                    if (Admin == ClientState.OutDated)
                    {
                        Admin = ClientState.UpToDate;
                        return Json(new { isOutDated = "true" });
                    }
                    return Json(new { isOutDated = "false"});
                case "Omar Arf":
                    if (Omar == ClientState.OutDated)
                    {
                        Omar = ClientState.UpToDate;
                        return Json(new { isOutDated = "true" });
                    }
                    return Json(new { isOutDated = "false" });
                case "Abdullah Saad":
                    if (Abdullah == ClientState.OutDated)
                    {
                        Abdullah = ClientState.UpToDate;
                        return Json(new { isOutDated = "true" });
                    }
                    return Json(new { isOutDated = "false" });
                case "Ahmad Mansy":
                    if (Mansy == ClientState.OutDated)
                    {
                        Mansy = ClientState.UpToDate;
                        return Json(new { isOutDated = "true" });
                    }
                    return Json(new { isOutDated = "false" });
                default:
                    return Json(new { isOutDated = "NULL" });
            }
        }

        public void UpdateClientState()
        {
            if (User.Identity.GetUserName() == "Admin")
            {
                Mansy = ClientState.OutDated;
                Abdullah = ClientState.OutDated;
                Omar = ClientState.OutDated;
            }

            else if (User.Identity.GetUserName() == "Omar Arf")
            {
                Mansy = ClientState.OutDated;
                Abdullah = ClientState.OutDated;
                Admin = ClientState.OutDated;
            }

            else if (User.Identity.GetUserName() == "Ahmad Mansy")
            {
                Abdullah = ClientState.OutDated;
                Omar = ClientState.OutDated;
                Admin = ClientState.OutDated;
            }

            else if (User.Identity.GetUserName() == "Abdullah Saad")
            {
                Mansy = ClientState.OutDated;
                Omar = ClientState.OutDated;
                Admin = ClientState.OutDated;
            }
        }
        #endregion

        #region DashBoard
        [HttpGet]
        public ActionResult Index(int? ProjectID)
        {

            //Main page for end-users; It gets all The unfinished tasks and stores them in m_AllTasks list
            //If the end-user is an Admin, redirect to AdminIndex page
            if (User.Identity.GetUserName() != null)
            {
                if (ProjectID != null)
                {
                    return View(InitializeDashBoard(ref ProjectID, false));
                }
                return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في تحميل البيانات" });
            }
            else
            {
                return RedirectToAction("Account", "Account");
            }

        }

        [HttpGet]
        public ActionResult AdminIndex(int? ProjectID)
        {
            //Opens the dashboard with extra tools for administration
            if (User.Identity.GetUserName() == null)
            {
                return RedirectToAction("Account", "Account");
            }

            if (ProjectID == null)
            {
                return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في تحميل البيانات" });
            }

            return View(InitializeDashBoard(ref ProjectID, true));
        }

        [HttpGet]
        public ActionResult TaskDetails (int? userID)
        {
            if (User.Identity.GetUserName() != null)
            {
                //Opens A page of task details for end-users
                DashBoardModel model = (DashBoardModel)TempData["model"];
                if (model.isAccissable == null)
                {
                    return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في تحميل البيانات" });
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Account", "Account");
            }
        }
        #endregion

        #region AddTask

        [HttpGet]
        public ActionResult AddTask(int? ProjectID)
        {
            if (User.Identity.GetUserName() != null)
            {
                //Opens a form of adding a new task for admin user
                DashBoardModel model = new DashBoardModel(ProjectID, true);
                return View(model);
            }
            else
            {
                return RedirectToAction("Account", "Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddTask(DashBoardModel recivedModel)
        {
            //Post new tasks to the database
            if (recivedModel != null)
            {
                db.DashBoardData.Add(recivedModel.m_Task);
                await db.SaveChangesAsync();
                UpdateClientState();
                if (User.Identity.GetUserName() == "Admin")
                {
                    return RedirectToAction("AdminIndex", new { ProjectID = recivedModel.m_CurrentProjectID });
                }
                return RedirectToAction("Index", new { ProjectID = recivedModel.m_CurrentProjectID});
            }
            return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في الوصول إلى الشبكة" });
        }
        #endregion

        #region EditTask
        [HttpGet]
        public ActionResult EditTask(int? taskID, int? ProjectID, int? userID)
        {
            if (User.Identity.GetUserName() != null)
            {
                //Opens a page to edit some task by admin user.
                //If the user is not an admin, redirect to TaskDetails
                if (taskID != null || ProjectID != null)
                {
                    DashBoardModel model = new DashBoardModel(ProjectID);
                    model.m_Task = db.DashBoardData.Find(taskID);
                    model.isAccissable = isUserAccissable(ref userID);

                    if (User.Identity.GetUserName() != "Admin")
                    {
                        TempData["model"] = model;
                        return RedirectToAction("TaskDetails");
                    }
                    return View(model);
                }
                return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في تحميل البيانات" });
            }
            else
            {
                return RedirectToAction("Account", "Account");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> EditTask (DashBoardModel recivedModel)
        {
            //Submit changes to the database.
            if (recivedModel != null)
            {
                db.EditTask(recivedModel.m_Task.id, recivedModel.m_Task.Title, recivedModel.m_Task.Details,
                            recivedModel.m_Task.StartingDate, recivedModel.m_Task.DeadLine, recivedModel.m_Task.isFinished,
                            recivedModel.m_Task.Dependancy, recivedModel.m_Task.Notes, recivedModel.m_Task.UserId);
                await db.SaveChangesAsync();
                UpdateClientState();
                if (recivedModel.isAccissable == true)
                {
                    return RedirectToAction("CloseWindow", "Home");
                }
                return RedirectToAction("Index", new { ProjectID = recivedModel.m_CurrentProjectID});
            }
            return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في الوصول إلى الشبكة" });
        }

        [HttpPost]
        public async Task<ActionResult> FinishTask(int? taskid, int? ProjectID)
        {
            //Sets isFinished = 1 for the taskid passed
            if (taskid != null || ProjectID != null)
            {
                db.SetTaskFinished(taskid);
                await db.SaveChangesAsync();
                UpdateClientState();
                if (User.Identity.GetUserName() == "Admin")
                {
                    return RedirectToAction("AdminIndex", new { ProjectID = ProjectID });
                }
                return RedirectToAction("Index", new {ProjectID = ProjectID });
            }
            return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في الوصول إلى الشبكة" });
        }

        #endregion

        #region DeleteTasks

        [HttpGet]
        public ActionResult DeleteTasks(int? ProjectID)
        {
            if (User.Identity.GetUserName() == null)
            {
                return RedirectToAction("Account", "Account");
            }
            if (ProjectID == null)
            {
                return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في الوصول إلى الشبكة" });
            }
            //Opens a page of task deletion adminitration.
            return View(new DeletionModel(ProjectID));
        }

        [HttpPost]
        public ActionResult DeleteTasks(DeletionModel recievedModel)
        {
            //Deletes list of tasks based on dates given.
            if (recievedModel != null)
            {
                db.DeleteTasks(recievedModel.m_StartingDate, recievedModel.m_EndingDate);
                db.SaveChanges();
                if (User.Identity.GetUserName() == "Admin")
                {
                    return RedirectToAction("AdminIndex", new { ProjectID = recievedModel.m_CurrentProjectID });
                }
                return RedirectToAction("Index", new {ProjectID = recievedModel.m_CurrentProjectID });
            }
            return RedirectToAction("ErrorPage", "Home", new { msg = "خطأ في الوصول إلى الشبكة" });
        }
        #endregion

    }
}