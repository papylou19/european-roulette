using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roulette.Models
{
    public class CheckModel
    {
        public string CurrentStake { get; set; }
        public Guid CashierId { get; set; }
        public int GameId { get; set; }
        public string ContractNumber { get; set; }
        public int Sum { get; set; }
        public double Winning { get; set; }
        public string WinningString { get; set; }
        public DateTime CreateDate { get; set; }
    }

}