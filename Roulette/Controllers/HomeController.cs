using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Models;

namespace Roulette.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TableModel model = new TableModel
            {
                Colors = new ColorField[] {
                    new ColorField(1,true),new ColorField(2,false),new ColorField(3,true),new ColorField(4,false),new ColorField(5,true),
                    new ColorField(6,false),new ColorField(7,true),new ColorField(8,false),new ColorField(9,true),new ColorField(10,false),
                    new ColorField(11,false),new ColorField(12,true),new ColorField(13,false),new ColorField(14,true),new ColorField(15,false),
                    new ColorField(16,true),new ColorField(17,false),new ColorField(18,true),new ColorField(19,true),new ColorField(20,false),
                    new ColorField(21,true),new ColorField(22,false),new ColorField(23,true),new ColorField(24,false),new ColorField(25,true),
                    new ColorField(26,false),new ColorField(27,true),new ColorField(28,false),new ColorField(29,false),new ColorField(30,true),
                    new ColorField(31,false),new ColorField(32,true),new ColorField(33,false),new ColorField(34,true),new ColorField(35,false),
                    new ColorField(36,true)}
            };
            return View(model);
        }
    }
}
