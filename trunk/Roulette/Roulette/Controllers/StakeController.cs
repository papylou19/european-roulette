using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using Domain.Helpers;

namespace Roulette.Controllers
{
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
            bool success = Unit.RouletteSrvc.CreateStake(stakes);
            return Json(new { success = success });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RememberCurrentState(string currentState)
        {
            BoardCurrentState = currentState.Replace("highlighted","");
            return Json(new { success = true });
        }

        [HttpPost]
        public string GetStakes()
        {
            return BoardCurrentState;
        }

    }
}
