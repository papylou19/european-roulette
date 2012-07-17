using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using System.Web.Helpers;
using Domain.Helpers;
using Domain;

namespace Roulette.Controllers
{
      [Authorize]
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            TableModel model = new TableModel
            {
                Colors = InitializeColors()
            };
            return View(model);
        }

        public JsonResult GetCurrentState()
        {
            GameState state = Unit.RouletteSrvc.GetCurrentState();
            if (state == null || state.State == 1)
            {
                if (!BoardCurrentStates.ContainsKey(CurrentUserName))
                {
                    BoardCurrentStates.Add(CurrentUserName, "");
                }
                BoardCurrentStates[CurrentUserName] = "";
            }
            if (state != null)
                return Json(new { State = state.State, StartTime = state.StartTime.ToString() });
            else
                return null;
        }
    }
}
