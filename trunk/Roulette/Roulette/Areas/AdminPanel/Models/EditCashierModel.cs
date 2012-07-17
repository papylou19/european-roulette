using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace Roulette.Areas.AdminPanel.Models
{
    public class EditCashierModel
    {
        public int Id { get; set; }
        public string OldUserName { get; set; }

        [Required]
        [Remote("UserNameCheck", "Cashier", AdditionalFields="OldUserName" ,ErrorMessage = "This username is alredy taken!")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Range(1,300)]
        public int Percent { get; set; }
    }

   
}