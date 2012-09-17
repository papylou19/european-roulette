using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using System.Web.Helpers;
using Domain.Helpers;
using Domain;
using Backend;
using SignalR;

namespace Roulette.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            TableModel model = new TableModel
            {
                Colors = ColorFields,
                History = Unit.RouletteSrvc.GetLastHistory(CurrentUserId)
            };

            return View(model);
        }

        public JsonResult GetCurrentState()
        {
            GameState state = Unit.RouletteSrvc.GetCurrentState();
            if (state == null || state.State == Constants.RollingState)
            {
                if (!BoardCurrentStates.ContainsKey(CurrentUserName))
                {
                    BoardCurrentStates.Add(CurrentUserName, "");
                }
                BoardCurrentStates[CurrentUserName] = "";

            } 

            if (state != null)
                return Json(new { State = state.State, StartTime = state.StartTime.ToString(DateTimeFormat), CurrentTime = DateTime.UtcNow.ToString(DateTimeFormat), RoundeTime = (DateTime.UtcNow - state.StartTime).TotalMilliseconds });
            else
                return null; 
        }

        public ActionResult GetHistory()
        {
            var model = new TableModel() {
                History = Unit.RouletteSrvc.GetLastHistory(CurrentUserId),
                Colors = ColorFields,
            };
            return PartialView("_History", model);
        }

        public int GetCurrentRound()
        {
            return Unit.RouletteSrvc.GetCurrentRoundNumber(CurrentUserId);
        }
    }
}
