using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Helpers;
using Roulette.Models;
using Roulette.Utilits;
using Backend;
using SignalR;
using SignalR.Hubs;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Roulette.Controllers
{
    [Authorize]
    public class StakeController : ControllerBase
    {
        public ActionResult Index()
        {
            TableModel model = new TableModel
            {
                Colors = ColorFields,
            };

            ViewBag.CurrentRound = GetCurrentRound();

            return View(model);
        }

        private int GetCurrentRound()
        {
            return Unit.RouletteSrvc.GetCurrentRoundNumber(CurrentUserId);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CreateStake(string stake, string currentState)
        {
            StakeDTO[] stakeDTOs = ((List<StakeDTO>)Newtonsoft.Json.JsonConvert.DeserializeObject(stake, typeof(List<StakeDTO>))).ToArray();
            if (stakeDTOs != null && stakeDTOs.FirstOrDefault(m => m.Price <= 0) == null)
            {
                bool success = false;
                long contractNumber = 0;
                var state = Unit.RouletteSrvc.GetCurrentState();
                if (state != null && state.State != Constants.RollingState)
                {
                    var boardForCheck = Regex.Replace(BoardCurrentStates[CurrentUserName], @"\w+-chip.png", @"check-chip.gif");
                    contractNumber = Unit.RouletteSrvc.CreateCheck(stakeDTOs, boardForCheck, CurrentUserId);
                    if (contractNumber != 0)
                    {
                        success = Unit.RouletteSrvc.CreateStake(stakeDTOs, contractNumber, CurrentUserId);
                    }
                }

                // Send socket for sinchronization
                var context = GlobalHost.ConnectionManager.GetHubContext<QuestionsHub>();
                context.Clients.newQuestion("");
                return Json(new { success = success, contractNumber = contractNumber });
            }
            return Json(new { success = false });
        }
        
        public ActionResult Report(DateTime startDate, DateTime endDate, int? cashierId)
        {
            if (cashierId.HasValue && Roles.IsUserInRole("Admin"))
            {
                var model = new ReportModel();
                model.Reports = Unit.RouletteSrvc.GetReportsByDate(startDate, endDate, cashierId.Value);
                return PartialView("_Report", model);
            }
            else 
            {
                var model = new ReportModel();
                model.Reports = Unit.RouletteSrvc.GetReportsByDate(startDate, endDate, CurrentUserId);
                return PartialView("_Report", model);
            }
        }

    
        public ActionResult Check(long contractNumber)
        {
            var check = Unit.RouletteSrvc.GetCheck(contractNumber);
            if (check != null)
            {
                var model = new CheckModel
                {
                    CurrentStake = check.BoardCurrentStates,
                    GameId = check.GameID,
                    CashierId = check.UserId,
                    ContractNumber = "978" + check.ContractNumber,
                    Sum = check.Stake,
                    Winning = check.PossibleWinning,
                    WinningString = check.PossibleWinningString,
                    CreateDate = check.CreateDate
                };

               if (!BoardCurrentStates.ContainsKey(CurrentUserName))
                {
                    BoardCurrentStates.Add(CurrentUserName, "");
                }

                BoardCurrentStates[CurrentUserName] = "";

                return View(model);
            }
            return Content("");
        }

        public JsonResult NextNumber()
        {
            int gameId = GetCurrentGameId();
            byte? winNumber = Unit.RouletteSrvc.GetWinner(gameId);
            return Json(new { nextNumber = winNumber });
        }

        [HttpPost]
        public ActionResult CheckWinner(string barCode)
        {
            long contractNumber;
            if (long.TryParse(barCode.Substring(3), out contractNumber))
            {
                 var stakes = Unit.RouletteSrvc.CheckWinner(contractNumber);
                 if (stakes.Count!=0)
                 {
                     foreach (var item in stakes)
                     {
                         if (!item.IsPayed)
                         {
                             return Json(new { isWinner = true, winning = "Winning  price is " + stakes.Select(m => m.PossibleWinning).Sum() });
                         }
                         else
                         {
                             return Json(new { isWinner = false, winning = "The ticket has already payed!" });
                         }
                     }
                 }
                 else
                 {
                     return Json(new { isWinner = false, winning = "The ticket didn't win!" });
                 }
               
            }
            return Json(new { isWinner = false, winning = "Error according when check code. Please try later." });
        }

        public ActionResult PayWinner(string barCode)
        {
            long contractNumber;
            if (long.TryParse(barCode.Substring(3), out contractNumber))
            {
               return Json(new { isPayed=Unit.RouletteSrvc.Pay(contractNumber) });

            }
           return Json(new { isPayed = false });
        }

        public ActionResult Barcode(string code)
        {
            var ean13 = new Ean13(code);
            Bitmap bmp = ean13.CreateBitmap();

            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Png);
            return File(stream.ToArray(), "image/png");
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

            // Send socket for sinchronization
            var context = GlobalHost.ConnectionManager.GetHubContext<QuestionsHub>();
            context.Clients.newQuestion(currentState);

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

        public int GetCurrentGameId()
        {
            return Unit.RouletteSrvc.GetCurrentGameId(CurrentUserId);
        }

    }

    public class QuestionsHub : Hub
    {

    }
}
