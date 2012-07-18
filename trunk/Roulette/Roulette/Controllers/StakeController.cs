using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using Domain.Helpers;

namespace Roulette.Controllers
{
    [Authorize]
    public class StakeController : ControllerBase
    {
        public ActionResult Index()
        {
            TableModel model = new TableModel
            {
                Colors = InitializeColors()
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult CreateStake(StakeDTO[] stakes)
        {
            bool success = Unit.RouletteSrvc.CreateStake(stakes,CurrentUserId);
            return Json(new { success = success });
        }

        //[HttpPost]
        //[ValidateInput(false)]
        public ActionResult Check()
        {
            CheckModel model = new CheckModel();
            model.CurrentStake = BoardCurrentStates[CurrentUserName];
           return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RememberCurrentState(string currentState)
        {
            if (!BoardCurrentStates.ContainsKey(CurrentUserName))
            {
                BoardCurrentStates.Add(CurrentUserName, "");
            }
            BoardCurrentStates[CurrentUserName] = currentState.Replace("highlighted", "");
            return Json(new { success = true });
        }

        [HttpPost]
        public string GetStakes()
        {
            if (!BoardCurrentStates.Keys.Contains(CurrentUserName))
            {
                return "";
            }

            return BoardCurrentStates[CurrentUserName];
        }

    }
}
