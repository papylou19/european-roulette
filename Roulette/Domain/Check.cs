using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [Table("Check")]
    public class Check
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ContractNumber { get; set; }
        public double PossibleWinning { get; set; }
        public string PossibleWinningString { get; set; }
        public int GameID { get; set; }
        public Guid UserId { get; set; }
        public string BoardCurrentStates { get; set; }
        public int Stake { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
