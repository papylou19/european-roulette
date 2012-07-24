﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend.Facade.Interfaces;
using Backend.DataContext;
using Domain;
using Domain.Helpers;
using Backend.Helper;

namespace Backend.Facade.Implementations
{
    public class RouletteFacade : IRouletteFacade
    {
        private RouletteContext ctx;

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
            var current = ctx.GameStates.FirstOrDefault();
            int state = 0;

            if (current == null || current.State == 1)
            {
                var cashiers =  ctx.Cashiers.ToList();
                foreach (var cashier in cashiers)
                {
                    ctx.Games.Add(new Game
                    {
                        CashierId = cashier.Id
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
                    if (current.State == 1)
                    {
                        var cashiers = ctx.Cashiers.ToList();
                        foreach (var cashier in cashiers)
                        {
                            ctx.Games.Where(p=>p.CashierId == cashier.Id).OrderByDescending(p => p.Id).Take(1).FirstOrDefault().Number = Convert.ToByte(new Random().Next(1, 37)); // HARD CODE
                        }
                    }

                    current.StartTime = DateTime.Now;
                    current.State = current.State != 1 ? Convert.ToInt16(current.State + 1) : Convert.ToInt16(0);
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
            return ctx.Games.Where(p=>p.Number != null && p.CashierId == cashierId).OrderByDescending(p => p.Id).Take(10).OrderBy(p => p.Id).ToArray();
        }


        public bool AddCashier(int percent,Guid userId)
        {
            try
            {
                ctx.Cashiers.Add(new Cashier
                {
                    NumberPercent = percent,
                    UserId =userId
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
            return ctx.Cashiers.ToList();
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
            return ctx.Games.FirstOrDefault(p => p.CashierId == cashierId && p.Number!=null) == null ? 0 :ctx.Games.Where(p => p.CashierId == cashierId && p.Number!=null).Max(p => p.Id) + 1;
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
                check.ContractNumber = ctx.Identities.FirstOrDefault(p => p.Name == "ContractNumber").Value; ;
                check.UserId = userId;
                check.BoardCurrentStates = board;
                foreach (var item in stakes)
                {
                    check.PossibleWinningString += Constant.Coeffecent[item.Type].ToString() + '*' + item.Price.ToString() + "+";
                    check.PossibleWinning += Constant.Coeffecent[item.Type] * item.Price;
                    check.stake += item.Price;
                }

                check.GameID = ctx.Games.Where(p => p.CashierId == cashierId).OrderByDescending(p => p.Id).FirstOrDefault().Id;
                check.CreateDate = DateTime.Now;
                check.PossibleWinningString = check.PossibleWinningString.Remove(check.PossibleWinningString.Length - 1);
                ctx.Checks.Add(check);

                ctx.Identities.FirstOrDefault(p => p.Name == "ContractNumber").Value++;
                ctx.SaveChanges();
                return check.ContractNumber;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public Check GetCheck(long contracNumber)
        {
            return ctx.Checks.FirstOrDefault(m => m.ContractNumber == contracNumber);
        }


        public double CountPercent(string currentUserName)
        {
            
            var sum =  (from cashier in ctx.Cashiers
                       join game in ctx.Games on cashier.Id equals game.CashierId
                       join stake in ctx.Stakes on game.Id equals stake.GameId
                       where cashier.User.UserName == currentUserName
                       select stake.Sum).Sum();

            var winnerSum = (from cashier in ctx.Cashiers
                             join game in ctx.Games on cashier.Id equals game.CashierId
                             join stake in ctx.Stakes on game.Id equals stake.GameId
                             where cashier.User.UserName == currentUserName && stake.IsWinningTicket==true
                             select stake.PossibleWinning).Sum();

            return winnerSum*100/sum;
        }


        public KeyValuePair<double, List<int>> CountWinningNumber(int gameId, int number)
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
            try
            {
                foreach (var item in stakes)
                {
                    ctx.Stakes.FirstOrDefault(m => m.Id == item).IsWinningTicket = true;
                }
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public List<Stake> CheckWinner(long contractNumber)
        {
            return ctx.Stakes.Where(m => m.ContractNumber == contractNumber && m.IsWinningTicket == true).ToList();
           
        }


        public bool Pay(long contractNumber)
        {
            try
            {
                ctx.Stakes.Where(m => m.ContractNumber == contractNumber && m.IsWinningTicket == true).ToList().ForEach(m => { m.IsPayed = true; m.PaymentDate = DateTime.Now; });
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }
}