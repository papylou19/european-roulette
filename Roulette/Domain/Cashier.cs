using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Domain.MemberShip;

namespace Domain
{
    [Table("Cashier")]
    public class Cashier
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int NumberPercent { get; set; }

        [ForeignKey("UserId")]
        public virtual Asp_User User { get; set; }
     
    }
}
