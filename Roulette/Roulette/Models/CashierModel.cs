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
        [Display(Name = "Процент выдачи")]
        [Range(1, System.Int32.MaxValue, ErrorMessage = "The Percent must be greater than 1.")]
        public int NumberPercent { get; set; }

        public double CurrentPercent { get; set; }
        public int Bet { get; set; }
        public double PayOut { get; set; }


        [Required]
        [Display(Name = "Имя")]
        [Remote("UserNameCheck","Cashier",ErrorMessage="This username is alredy taken!")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Подтвердить Пароль")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}