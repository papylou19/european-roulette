using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using Domain.Helpers;
using Domain;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Roulette.Utilits;

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
            bool success = false;
            long contractNumber = 0;
            var state = Unit.RouletteSrvc.GetCurrentState();
            if ( state!=null && state.State!=1)
            {
                contractNumber = Unit.RouletteSrvc.CreateCheck(stakes, BoardCurrentStates[CurrentUserName], CurrentUserId);
                if (contractNumber != 0)
                {
                    success = Unit.RouletteSrvc.CreateStake(stakes, contractNumber, CurrentUserId);
                }
            }

            return Json(new { success = success, contractNumber = contractNumber });
        }

        public ActionResult Report(DateTime startDate,DateTime endDate)
        {
            var model = new ReportModel();
            model.Reports = Unit.RouletteSrvc.GetReportsByDate(startDate, endDate);
            return PartialView("_Report",model);
        
        }

    
        public ActionResult Check(long contractNumber)
        {
            var check = Unit.RouletteSrvc.GetCheck(contractNumber);
            if (check != null)
            {
                var model = new CheckModel{
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
}
