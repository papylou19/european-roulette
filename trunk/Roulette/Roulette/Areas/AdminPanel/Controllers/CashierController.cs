using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using Roulette.Controllers;
using Roulette.Areas.AdminPanel.Models;
using Domain;
using System.Web.Security;

namespace Roulette.Areas.AdminPanel.Controllers
{
    [Authorize(Roles="admin")]
    public class CashierController : Roulette.Controllers.ControllerBase
    {
        //
        // GET: /AdminPanel/Home/
        MembershipProvider membership = Membership.Provider;
        public ActionResult Index()
        {
            var cashierListmodel = new List<CashierModel>();
            var cashierList = Unit.RouletteSrvc.GetAllCashier();
            foreach (var item in cashierList)
            {
                cashierListmodel.Add(new CashierModel
                {
                    NumberPercent = item.NumberPercent,
                    UserName = item.User.UserName,
                    IsApproved = membership.GetUser(item.User.UserId,true).IsApproved,
                    Id = item.Id
                });
            }
            return View(cashierListmodel);
        }

        [HttpGet]
        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Add(CashierModel model)
        {
            MembershipCreateStatus status;
            var user = membership.CreateUser(model.UserName, model.Password,"", null, null, true, null, out status);
            if (status == MembershipCreateStatus.Success)
            {
                Unit.RouletteSrvc.AddCashier(model.NumberPercent,(Guid)user.ProviderUserKey);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Disable(int id)
        {
            var user = membership.GetUser(Unit.RouletteSrvc.GetCahierById(id).User.UserName, true);
            user.IsApproved = !user.IsApproved;
            membership.UpdateUser(user);
            return Json(new { sucess = true  }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cashier = Unit.RouletteSrvc.GetCahierById(id);
            var editModel = new EditCashierModel{
                Percent = cashier.NumberPercent,
                UserName = cashier.User.UserName,
                OldUserName = cashier.User.UserName
        };

            return View(editModel);
        }

        [HttpPost]
        public ActionResult Edit(EditCashierModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != null)
                {

                    membership.ChangePassword(model.OldUserName, membership.ResetPassword(model.OldUserName,null), model.Password);
                }

                Unit.RouletteSrvc.EditCashier(model.Id, model.OldUserName, model.UserName, model.Password, model.Percent);
            }
            return RedirectToAction("Index");
        }

        public JsonResult UserNameCheck(string UserName, string OldUserName)
        {
            if (!UserName.Equals(OldUserName))
            {
                return Json(Unit.RouletteSrvc.GetCashierByUserName(UserName) == null, JsonRequestBehavior.AllowGet);
            }
           return Json(true, JsonRequestBehavior.AllowGet);
        }

    

    }
}
