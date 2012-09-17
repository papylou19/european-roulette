using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Timers;
using Backend;
using Backend.DataContext;
using Backend.Facade.Interfaces;
using Backend.Facade.Implementations;
using SignalR;
using Roulette.Controllers;

namespace Roulette
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        Timer timer;
        protected static UnitOfWork Unit { get; private set; }
        protected static RouletteFacade RouletteFcd { get; private set; }

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
            RouletteFcd = new RouletteFacade();
            timer = new Timer(90000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;

            var refuseTimer = new Timer(12 * 3600 * 1000); // elapsed once in day
            refuseTimer.Elapsed += new ElapsedEventHandler(refuseTimer_Elapsed);
            refuseTimer.Start();
            refuseTimer_Elapsed(null, null);
        }

        void refuseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!RouletteFcd.RemoveGameRefuse(DateTime.UtcNow.AddMonths(-6)))
            {
                refuseTimer_Elapsed(null, null);
            }
        }



        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int? state = RouletteFcd.ChangeGameState();
            //int? state = RouletteFcd.GetCurrentState().State;
            var cashierUserIds = RouletteFcd.GetAllCashier().Select(p => p.UserId);

            if (state == Constants.RollingState)
            {
                foreach (var userId in cashierUserIds)
                {
                    var percent = RouletteFcd.GetCashierByUserId(userId).NumberPercent;
                    var currentPercent = RouletteFcd.CountPercent(userId);
                    var numberDic = new Dictionary<int, double>();
                    //var stakeDict = new Dictionary<int, List<int>>();
                    var stakeList = new List<int>[37];
                    var gameId = RouletteFcd.GetCurrentGameId(userId);

                    for (int i = 0; i < 37; i++)
                    {
                        var count = RouletteFcd.CountWinningNumber(gameId, i);
                        numberDic.Add(i, count.Key);
                        //stakeDict.Add(i, count.Value);
                        stakeList[i] = count.Value;
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
                                     orderby pair.Value ascending
                                     select pair).ToDictionary(pair => pair.Key, pair => pair.Value);
                    }

                    var possibleVariants = 9;
                    while ((possibleVariants < 35) && (numberDic[possibleVariants] == numberDic[possibleVariants + 1]))
                    {
                        possibleVariants++;
                    }
                    int winNumber = numberDic.ElementAt(new Random().Next(0, possibleVariants)).Key;

                    RouletteFcd.WriteWinnerNumber(gameId, winNumber);
                    RouletteFcd.MakeWinner(stakeList[winNumber]);
                }

                // Send socket for sinchronization
                var context = GlobalHost.ConnectionManager.GetHubContext<QuestionsHub>();
                context.Clients.newQuestion("");
            }
        }

    }
}