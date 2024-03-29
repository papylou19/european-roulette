﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using System.Web.Security;

namespace Roulette.Controllers
{
    public class AccountController : ControllerBase
    {
        //
        // GET: /Account/
        MembershipProvider membership = Membership.Provider;
        [HttpGet]
        public ActionResult LogIn()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: /Account/LogOut
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn");
        } 

        [HttpPost]
        public ActionResult LogIn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
               if (membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    if (returnUrl != null)
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Stake");
                }
            }
            return View();
        }

    }
}
