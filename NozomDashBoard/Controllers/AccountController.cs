using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NozomDashBoard.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace NozomDashBoard.Controllers
{
    public class AccountController : Controller
    {
        private NozomDashBoardEntities db = new NozomDashBoardEntities();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [HttpGet]
        public ActionResult Account()
        {
            TempData["isAuth"] = true;
            return View(new Account_Model());
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [HttpPost]
        public ActionResult Account(Account_Model model)
        {
            if (model != null)
            {
                var items = db.Users.ToList();

                try
                {
                    // Verification.    
                    if (ModelState.IsValid)
                    {
                        // Initialization.    
                        var loginInfo = db.CheckAuthurizationInfo(model.m_UserNameResult, model.m_PassWord).ToList();
                        // Verification.    
                        if (loginInfo != null && loginInfo.Count() > 0)
                        {
                            // Initialization.    
                            var logindetails = loginInfo.First();

                            // Login In.    
                            this.SignInUser(logindetails, false);
                            // Redirection.
                            TempData["isAuth"] = true;
                            return RedirectToAction("ProjectSelection", "Home");
                        }
                        else
                        {
                            // Setting.    
                            ModelState.AddModelError(string.Empty, "خطأ فى كلمة المرور");
                            TempData["isAuth"] = false;
                            return View(new Account_Model());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Info    
                    Console.Write(ex);
                }
                // If we got this far, something failed, redisplay form    
                return this.View(model);

            }
            return null;
        }

        public ActionResult LogOff()
        {
            LogOffUser();
            return RedirectToAction("Account");
        }
        private void LogOffUser()
        {
            try
            {
                // Setting.    
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        #region Sign In method.    
        /// <summary>  
        /// Sign In User method.    
        /// </summary>  
        /// <param name="username">Username parameter.</param>  
        /// <param name="isPersistent">Is persistent parameter.</param>  
        private void SignInUser(string username, bool isPersistent)
        {
            // Initialization.    
            var claims = new List<Claim>();
            try
            {
                // Setting    
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.    
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        #endregion
    }
}