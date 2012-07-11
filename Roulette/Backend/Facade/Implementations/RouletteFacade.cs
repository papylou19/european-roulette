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

        public bool CreateStake(StakeDTO[] stakes)
        {
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
                        GameId = 10 // HARD CODE
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
    }
}
