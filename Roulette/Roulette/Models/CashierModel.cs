using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Roulette.Models
{
    public class CashierModel
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        [Range(1, 300, ErrorMessage = "The Percent must be between 1 and 300.")]
        public int NumberPercent { get; set; }

        [Required]
        [Remote("UserNameCheck","Cashier",ErrorMessage="This username is alredy taken!")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}