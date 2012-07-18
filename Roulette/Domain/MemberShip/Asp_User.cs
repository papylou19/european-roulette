using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain.MemberShip
{
    [Table("aspnet_Users")]
    public class Asp_User
    {
        [Key]
        public Guid UserId { get; set; }
        public String UserName { get; set; }
        public string LoweredUserName { get; set; }
    }
}
