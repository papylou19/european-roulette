using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Timers;
using Backend;

namespace Roulette
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        Timer timer;
        protected static UnitOfWork Unit { get; private set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogIn", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            Unit = new UnitOfWork();
            timer = new Timer(30000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int? state = new UnitOfWork().RouletteSrvc.ChangeGameState();
            var cashierUserIds = Unit.RouletteSrvc.GetAllCashier().Select(p => p.UserId);

            if (state == 1)
            {
                foreach (var userId in cashierUserIds)
                {
                    var percent = Unit.RouletteSrvc.GetCashierByUserId(userId).NumberPercent;
                    var currentPercent = Unit.RouletteSrvc.CountPercent(userId);
                    var numberDic = new Dictionary<int, double>();
                    var stakeDict = new Dictionary<int, List<int>>();
                    var gameId = Unit.RouletteSrvc.GetCurrentGameId(userId);

                    for (int i = 0; i < 37; i++)
                    {
                        var count = Unit.RouletteSrvc.CountWinningNumber(gameId, i);
                        numberDic.Add(i, count.Key);
                        stakeDict.Add(i, count.Value);
                    }

                    if (percent > currentPercent)
                    {
                        numberDic = (from pair in numberDic
                                     orderby pair.Value descending
                                     select pair).ToDictionary(pair => pair.Key, pair => pair.Value);
                    }
                    else
                    {
                        numberDic = (from pair in numberDic
                                     orderby pair.Value descending
                                     select pair).ToDictionary(pair => pair.Key, pair => pair.Value);
                    }

                    int winNumber = numberDic.ElementAt(new Random().Next(0, 9)).Key;

                    Unit.RouletteSrvc.WriteWinnerNumber(gameId,winNumber);
                    Unit.RouletteSrvc.MakeWinner(stakeDict[winNumber]);
                }
            }
        }

    }
}