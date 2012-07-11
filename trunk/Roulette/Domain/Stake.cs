using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [Table("Stake")]
    public class Stake
    {
        public int Id { get; set; }
        public long ContractNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public double Coefficient { get; set; }
        public int Sum { get; set; }
        public string Type { get; set; }
        public byte Number { get; set; }
        public bool IsWinningTicket { get; set; }
        public bool IsPayed { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int GameId { get; set; }

        [ForeignKey("Id")]
        public Game Game { get; set; }
    }
}
