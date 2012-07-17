using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Helpers;
using Domain;

namespace Backend.Facade.Interfaces
{
    public interface IRouletteFacade
    {
        bool CreateStake(StakeDTO[] stakes);
        bool AddCashier(int percent,Guid userId);
        List<Cashier> GetAllCashier();
        int? ChangeGameState();
        GameState GetCurrentState();
        bool DeleteCashier(int id);
        bool EditCashier(int id, string oldUserName, string newUserName, string password, int percent);
        Cashier GetCashierByUserName(string userName);
        Cashier GetCahierById(int id);
    }
}
