using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain.MemberShip
{
    [Table("aspnet_Membership")]
    public class ASP_Membership
    {
        [Key]
        public Guid UserId { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public bool IsApproved { get; set; }
        [Column("LoweredEmail")]
        public string Email { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
