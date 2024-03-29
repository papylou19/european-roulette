﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Helpers;
using Domain;

namespace Backend.Facade.Interfaces
{
    public interface IRouletteFacade
    {
        bool CreateStake(StakeDTO[] stakes, long contractNumber, Guid CashierId);
        bool AddCashier(int percent,Guid userId);
        List<Cashier> GetAllCashier();
        int? ChangeGameState();
        GameState GetCurrentState();
        Game[] GetLastHistory(Guid userId);
        bool DeleteCashier(int id);
        Cashier GetCashierByUserId(Guid userId);
        bool EditCashier(int id, string oldUserName, string newUserName, string password, int percent);
        Cashier GetCashierByUserName(string userName);
        Cashier GetCahierById(int id);
        Guid GetUserId(string username);
        int GetCurrentRoundNumber(Guid userId);
        KeyValuePair<double, List<int>> CountWinningNumber(int gameId, int number);
        List<Stake> GetStakes(long contractNumber);
        long CreateCheck(StakeDTO[] stakes, string board, Guid userId);
        Check GetCheck(long contracNumber);
        double CountPercent(Guid userId);
        bool CheckNumber(int number, int winNumber,string type);
        bool MakeWinner(List<int> stakes);
        List<Stake> CheckWinner(long contractNumber);
        bool Pay(long contractNumber);
        int GetCurrentGameId(Guid userId);
        void WriteWinnerNumber(int gameId,int winNumber);
        byte? GetWinner(int gameId);
        Report GetReportsByDate(DateTime startDate, DateTime endDate, Guid cashierId);
        Report GetReportsByDate(DateTime startDate, DateTime endDate, int cashierId);
        int GetAmountOfBet(Guid userId);
        double GetAmountOfPayOut(Guid userId);

        bool RemoveCheck(long contractNumber);

        bool RemoveGameRefuse(DateTime removeUntil);
    }
}
