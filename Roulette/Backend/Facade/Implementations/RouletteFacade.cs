using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend.Facade.Interfaces;
using Backend.DataContext;
using Domain;
using Domain.Helpers;
using Backend.Helper;
using System.Data.Entity;

namespace Backend.Facade.Implementations
{
    public class RouletteFacade : IRouletteFacade
    {
        private RouletteContext ctx;

        public RouletteFacade()
        {
        }

        public RouletteFacade(RouletteContext context)
        {
            this.ctx = context;
        }

        private GameState SetState(GameState state)
        {
            ctx.GameStates.Add(state);
            try
            {
                ctx.SaveChanges();
                return state;
            }
            catch 
            {
                return null;
            }
        }

        public GameState GetCurrentState()
        {
            return ctx.GameStates.FirstOrDefault();
        }

        public int? ChangeGameState()
        {
            using (var ctx = new RouletteContext())
            {
                var current = ctx.GameStates.FirstOrDefault();
                int state = 0;

                if (current == null || current.State == 1)
                {
                    var cashiers = ctx.Cashiers.ToList();
                    foreach (var cashier in cashiers)
                    {
                        ctx.Games.Add(new Game
                        {
                            CashierId = cashier.Id,
                            GameNumber = ctx.Games.Where(p => p.CashierId == cashier.Id).FirstOrDefault() != null ? ctx.Games.Where(p => p.CashierId == cashier.Id).Max(p => p.GameNumber) + 1 : Constants.GAME_FIRST_NUMBER
                        });
                    }
                }

                if (current == null)
                {
                    SetState(new GameState()
                    {
                        State = 0,
                        StartTime = DateTime.Now
                    });
                }
                else
                {
                    current.StartTime = DateTime.Now;
                    current.State = current.State != 1 ? (short)(current.State + 1) : (short)0;
                    state = current.State;
                }

                try
                {
                    ctx.SaveChanges();
                    return state;
                }
                catch
                {
                    return null;
                }
            }
        }

        public Cashier GetCashierByUserId(Guid userId)
        {
            using (var ctx = new RouletteContext())
            {
                return ctx.Cashiers.FirstOrDefault(p => p.UserId == userId);
            }
        }



        public bool CreateStake(StakeDTO[] stakes,long contractNumber,Guid userId)
        {
            var cashierId = ctx.Cashiers.FirstOrDefault(p => p.UserId == userId).Id;
            try
            {
                foreach(StakeDTO stake in stakes)
                {
                    var coefficient = Constant.Coeffecent[stake.Type];
                    ctx.Stakes.Add(new Stake
                    {                        
                        ContractNumber = contractNumber,
                        CreateDate = DateTime.Now,
                        Coefficient = coefficient,
                        Number = stake.Id,
                        Sum = stake.Price,
                        Type = stake.Type.ToString(),
                        PossibleWinning = stake.Price*coefficient,
                        GameId = ctx.Games.Where(p => p.CashierId == cashierId).OrderByDescending(p => p.Id).FirstOrDefault().Id
                    });
                }
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Game[] GetLastHistory(Guid userId)
        {
            var cashierId = ctx.Cashiers.FirstOrDefault(p => p.UserId == userId).Id;

            //skipping current game
            return ctx.Games.Where(p=>p.CashierId == cashierId).OrderByDescending(p => p.Id).Skip(1).Take(10).OrderBy(p => p.Id).OrderByDescending(p => p.Id).ToArray();
        }

        public bool AddCashier(int percent,Guid userId)
        {
            try
            {
                ctx.Cashiers.Add(new Cashier
                {
                    NumberPercent = percent,
                    UserId = userId,
                    LastChangeDate = DateTime.Now,
                });

                ctx.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }


        public List<Cashier> GetAllCashier()
        {
            using (var ctx = new RouletteContext())
            {
                return ctx.Cashiers.Include(m => m.User).ToList();
            }
        }


        public bool DeleteCashier(int id)
        {
            try
            {
                var item = ctx.Cashiers.FirstOrDefault(m => m.Id == id);
                ctx.Cashiers.Remove(item);
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool EditCashier(int id, string oldUserName,string newUserName, string password,int percent)
        {
            try
            {
                var cashier = ctx.Cashiers.FirstOrDefault(m => m.Id == id);
                cashier.NumberPercent = percent;
                cashier.User.UserName = newUserName;
                cashier.User.LoweredUserName = newUserName.ToLower();
                ctx.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Cashier GetCashierByUserName(string userName)
        {
            return ctx.Cashiers.FirstOrDefault(m => m.User.UserName == userName);
        }


        public Cashier GetCahierById(int id)
        {
            return ctx.Cashiers.FirstOrDefault(m => m.Id == id);
        }

        public Guid GetUserId(string userName)
        {
            return ctx.Cashiers.FirstOrDefault(m => m.User.UserName == userName).UserId;
        }

        public int GetCurrentRoundNumber(Guid userId)
        {
            var cashierId = ctx.Cashiers.FirstOrDefault(p=>p.UserId == userId).Id;
            return ctx.Games.Count(p => p.CashierId == cashierId) >= 2 ? ctx.Games.Where(p => p.CashierId == cashierId).Max(p => p.GameNumber) : Constants.GAME_FIRST_NUMBER;
        }


        public List<Stake> GetStakes(long contractNumber)
        {
            return ctx.Stakes.Where(m => m.ContractNumber == contractNumber).ToList();
        }


        public long CreateCheck(StakeDTO[] stakes, string board, Guid userId)
        {
            try
            {
                var check = new Check();

                var cashierId = ctx.Cashiers.FirstOrDefault(p => p.UserId == userId).Id;
                check.ContractNumber = ctx.Identities.FirstOrDefault(p => p.Name == "ContractNumber").Value;
                check.UserId = userId;
                check.BoardCurrentStates = board;
                foreach (var item in stakes)
                {
                    check.PossibleWinningString += Constant.Coeffecent[item.Type].ToString() + '*' + item.Price.ToString() + " + ";
                    check.PossibleWinning += Constant.Coeffecent[item.Type] * item.Price;
                    check.Stake += item.Price;
                }

                check.GameID = ctx.Games.Where(p => p.CashierId == cashierId).OrderByDescending(p => p.Id).FirstOrDefault().Id;
                check.CreateDate = DateTime.Now;
                check.PossibleWinningString = check.PossibleWinningString.Remove(check.PossibleWinningString.Length - 3);
                ctx.Checks.Add(check);

                ctx.Identities.FirstOrDefault(p => p.Name == "ContractNumber").Value++;
                ctx.SaveChanges();
                return check.ContractNumber;
            }
            catch
            {
                return 0;
            }
        }


        public Check GetCheck(long contracNumber)
        {
            return ctx.Checks.FirstOrDefault(m => m.ContractNumber == contracNumber);
        }

        public double CountPercent(Guid userId)
        {
            using (var ctx = new RouletteContext())
            {
                double winnerSum = 0;
                var winStake = (from cashier in ctx.Cashiers
                                join game in ctx.Games on cashier.Id equals game.CashierId
                                join stake in ctx.Stakes on game.Id equals stake.GameId
                                where cashier.UserId == userId && stake.IsWinningTicket == true
                                select new { PossibleWinning = stake.PossibleWinning }).ToList();


                var test = (from cashier in ctx.Cashiers // TODO check why this not wotking without 
                            join game in ctx.Games on cashier.Id equals game.CashierId
                            join stake in ctx.Stakes on game.Id equals stake.GameId
                            where cashier.UserId == userId
                            select stake.Sum).ToList();
                var sum = test.Sum();

                if (winStake.Count != 0)
                {
                    winnerSum = winStake.Sum(m => m.PossibleWinning);
                }

                return winnerSum * 100 / sum;
            }
        }


        public KeyValuePair<double, List<int>> CountWinningNumber(int gameId, int number)
        {
            using (var ctx = new RouletteContext())
            {
                var stakes = ctx.Stakes.Where(m => m.GameId == gameId).ToList();
                double count = 0;
                List<int> winStakes = new List<int>();
                foreach (var item in stakes)
                {
                    if (CheckNumber(item.Number, number, item.Type))
                    {
                        count += item.PossibleWinning;
                        winStakes.Add(item.Id);
                    }
                }
                return new KeyValuePair<double, List<int>>(count, winStakes);
            }
        }

       public bool CheckNumber(int number, int winNumber,string type)
        {
            switch (type)
            {
                case "SingleElement":
                    {
                        if (winNumber == number)
                            return true;
                        break;
                    }
                case "HorizontalPair":
                    {
                        if (winNumber == number || winNumber == number + 3)
                        {
                            return true;
                        }
                        break;
                    }
                case "VerticalPair":
                    {
                        if (winNumber == number || winNumber == number - 1)
                        {
                            return true;
                        }
                        break;
                    }
                case "HorizontalWithZeroPair":
                    {
                        if (winNumber == number || winNumber == 0)
                        {
                            return true;
                        }
                        break;
                    }
                case "HorizontalWithZeroTrips":
                    {

                        if (winNumber == number || winNumber == number - 1 || winNumber == 0)
                        {
                            return true;
                        }

                        break;
                    }
                case "Quads":
                    {
                        for (int i = -1; i < 4; i++)
                        {

                            if (winNumber == number + i)
                            {
                                return true;
                          
                            }
                            if (i == 0)
                            {
                                i++;
                            }
                        }
                        break;
                    }
                case "TwelveElements":
                    {
                        for (int i = -2; i < 10; i++)
                        {
                            if (winNumber == number + i)
                            {
                                return true;
                             
                            }

                        }
                        break;
                    }
                case "EvenOrOdd":
                    {

                        for (int i = 0; i <= 36; i += 2)
                        {
                            if (winNumber == number + i)
                            {
                                return true;
                           
                            }
                        }

                        break;
                    }
                case "BlackOrRed":
                    {
                        var red = new int[] { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
                        var black = new int[] { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 29, 28, 31, 33, 35 };
                        if (number == 3)
                        {
                            if (red.Contains(winNumber))
                            {
                                return true;
                 
                            }
                            break;
                        }
                        else
                        {
                            if (black.Contains(winNumber))
                            {
                                return true;
                            }
                            break;
                        }


                    }
                case "EighteenElements":
                    {
                        for (int i = -2; i < 17; i++)
                        {
                            if (winNumber == number + i)
                            {
                                return true;
                            }

                        }
                        break;
                    }
                case "HorizontalLine":
                    {
                        for (int i = 0; i <= 33; i += 3)
                        {
                            if (winNumber == number + i)
                            {
                                return true;
                            }

                        }
                        break;
                    }
                case "VerticalTrips":
                    {
                        for (int i = 0; i > -3; i--)
                        {
                            if (winNumber == number + i)
                            {
                                return true;
                            }

                        }
                        break;
                    }
                case "TwoVerticalTrips":
                    {
                        for (int i = -2; i < 4; i++)
                        {
                            if (winNumber == number + i)
                            {
                                return true;
                            }

                        }
                        break;
                    }
            }
            return false;
        }

        public bool MakeWinner(List<int> stakes)
        {
            using (var ctx = new RouletteContext())
            {
                try
                {
                    foreach (var item in stakes)
                    {
                        ctx.Stakes.FirstOrDefault(m => m.Id == item).IsWinningTicket = true;
                    }
                    ctx.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void WriteWinnerNumber(int gameId, int winNumber)
        {
            using (var ctx = new RouletteContext())
            {
                ctx.Games.FirstOrDefault(p => p.Id == gameId).Number = Convert.ToByte(winNumber);
                ctx.SaveChanges();
            }
        }

        public List<Stake> CheckWinner(long contractNumber)
        {
            return ctx.Stakes.Where(m => m.ContractNumber == contractNumber && m.IsWinningTicket == true).ToList();
           
        }

        public byte? GetWinner(int gameId) 
        {
            return ctx.Games.FirstOrDefault(p => p.Id == gameId).Number;
        }

        public bool Pay(long contractNumber)
        {
            try
            {
                ctx.Stakes.Where(m => m.ContractNumber == contractNumber && m.IsWinningTicket == true).ToList().ForEach(m => { m.IsPayed = true; m.PaymentDate = DateTime.Now; });
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetCurrentGameId(Guid userId)
        {
            using (var ctx = new RouletteContext())
            {
                var cashierId = ctx.Cashiers.FirstOrDefault(p => p.UserId == userId).Id;
                return ctx.Games.FirstOrDefault(p => p.CashierId == cashierId) != null ? ctx.Games.Where(p => p.CashierId == cashierId).Max(p => p.Id) : Constants.GAME_FIRST_NUMBER;
            }
        }

        public Report GetReportsByDate(DateTime startDate, DateTime endDate, int cashierId)
        {
            var cashier = ctx.Cashiers.Find(cashierId);
            if (cashier != null)
            {
                return GetReportsByDate(startDate, endDate, cashier.UserId);
            }
            else
            {
                return null;
            }
        }

        public Report GetReportsByDate(DateTime startDate, DateTime endDate, Guid UserId)
        {
            endDate = endDate.AddDays(1);
            
            var stakes = (from stake in ctx.Stakes
                       where stake.CreateDate >= startDate && stake.CreateDate <= endDate && stake.Game.Cashier.UserId == UserId 
                       group stake by stake.ContractNumber into check
                       select new
                       {
                           ContractNumber = check.Key,
                           CreateDate = check.FirstOrDefault().CreateDate,
                           Sum = check.Sum(m => m.Sum),
                           PossibleWinning = check.Sum(m => m.PossibleWinning),
                           IsWinningTicket = check.FirstOrDefault(m => m.IsWinningTicket) != null ? true : false,
                           PaymentDate = check.FirstOrDefault(m => m.IsPayed) != null ? check.FirstOrDefault(m => m.PaymentDate != null).PaymentDate : null,
                           IsPayed = check.Count(m => m.IsPayed) > 0 ? true : false,
                           WinningSum = check.Sum(m => m.IsWinningTicket ? m.PossibleWinning : 0)
                       });

           

            var reports = new Report();
            reports.Stakes = new List<Stake>();

            foreach (var item in stakes)
            {
                reports.Stakes.Add(new Stake
                {
                    ContractNumber = item.ContractNumber,
                    CreateDate = item.CreateDate,
                    Sum = item.Sum,
                    PossibleWinning = item.PossibleWinning,
                    IsWinningTicket = item.IsWinningTicket,
                    PaymentDate = item.PaymentDate,
                    IsPayed = item.IsPayed,
                    WinningSum = item.WinningSum
                });

            }

            reports.Stake = stakes.Sum(m => m.Sum);
            reports.CountStake = stakes.Count();
            var payedStake = stakes.Where(m => (m.IsWinningTicket && m.IsPayed)).ToList();
            if (payedStake != null && payedStake.Count > 0)
            {
                reports.WinSum = payedStake.Sum(m => m.WinningSum);
            }
            else {
                reports.WinSum = 0;
            }
            reports.WaitingSum = stakes.Where(m => m.IsWinningTicket == true).Sum(m => m.WinningSum) - reports.WinSum;

            return reports;
        }


        public int GetAmountOfBet(Guid userId)
        {
            var sumQuery = (from cashier in ctx.Cashiers
                            join game in ctx.Games on cashier.Id equals game.CashierId
                            join stake in ctx.Stakes on game.Id equals stake.GameId
                            where cashier.UserId == userId
                            select stake.Sum);

            var sum = sumQuery.Count() != 0 ? sumQuery.Sum() : 0;

            return sum;
        }

        public double GetAmountOfPayOut(Guid userId)
        {
            var winnerQuery = (from cashier in ctx.Cashiers
                               join game in ctx.Games on cashier.Id equals game.CashierId
                               join stake in ctx.Stakes on game.Id equals stake.GameId
                               where cashier.UserId == userId && stake.IsWinningTicket == true
                               select stake.PossibleWinning);

            var winnerSum = winnerQuery.Count() != 0 ? winnerQuery.Sum() : 0;

            return winnerSum;
        }


        public bool RemoveCheck(long contractNumber)
        {
            var entry = ctx.Checks.Find(contractNumber);
            ctx.Entry(entry).State = System.Data.EntityState.Deleted;
            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }


        public bool RemoveGameRefuse(DateTime removeUntil)
        {            
            using (ctx = new RouletteContext())
            {
                foreach (var entry in ctx.Games.Where(m => (m.Stackes.Count == 0) || (m.Stackes.FirstOrDefault(n => n.CreateDate > removeUntil) == null)))
                {
                    ctx.Entry(entry).State = System.Data.EntityState.Deleted;
                }
                try
                {
                    ctx.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }            
        }
    }
}
