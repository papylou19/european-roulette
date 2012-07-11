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
        public JsonResult RefreshStakes(StakeDTO[] stakes)
        {
            Stakes = stakes != null ? stakes.ToList() : new List<StakeDTO>();
            return Json(new { success = true });
        }

    }
}
