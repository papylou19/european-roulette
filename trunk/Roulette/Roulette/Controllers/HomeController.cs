using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;
using System.Web.Helpers;
using Domain.Helpers;

namespace Roulette.Controllers
{
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

        public JsonResult GetStakes()
        {
            return Json(Stakes);
        }
    }
}
