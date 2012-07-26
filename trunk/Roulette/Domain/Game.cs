using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [Table("Game")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public byte? Number { get; set; }
        public int CashierId { get; set; }
        public int GameNumber { get; set; }

        [ForeignKey("CashierId")]
        public virtual Cashier Cashier{get;set;}

    }
}
