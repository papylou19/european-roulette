using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain.MemberShip
{
    [Table("aspnet_Roles")]
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }

        public Guid ApplicationId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }

        public ICollection<UsersInRole> UsersInRoles { get; set; }
    }

}
