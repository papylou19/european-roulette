using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend.Facade.Interfaces;
using Backend.DataContext;
using Domain;
using Domain.Helpers;

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

        public bool CreateStake(StakeDTO[] stakes,Guid userId)
        {
            var cashierId = ctx.Cashiers.FirstOrDefault(p => p.UserId == userId).Id;
            try
            {
                foreach(StakeDTO stake in stakes)
                {
                    ctx.Stakes.Add(new Stake
                    {
                        ContractNumber = ctx.Identities.FirstOrDefault(p=>p.Name == "ContractNumber").Value,
                        CreateDate = DateTime.Now,
                        Coefficient = 1.5, // HARD CODE
                        Number = stake.Id,
                        Sum = stake.Price,
                        Type = stake.Type.ToString(),
                        GameId = ctx.Games.Where(p => p.CashierId == cashierId).OrderByDescending(p => p.Id).FirstOrDefault().Id
                    });
                }
                ctx.Identities.FirstOrDefault(p => p.Name == "ContractNumber").Value++;
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

        public Guid GetUserId(string username)
        {
            return ctx.Users.FirstOrDefault(p => p.UserName == username).UserId;
        }

        public int GetCurrentRoundNumber(Guid userId)
        {
            var cashierId = ctx.Cashiers.FirstOrDefault(p=>p.UserId == userId).Id;
            return ctx.Games.Where(p => p.CashierId == cashierId && p.Number!=null).Max(p => p.Id) + 1;
        }
    }
}
