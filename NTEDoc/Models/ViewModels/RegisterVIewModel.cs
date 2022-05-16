using Microsoft.AspNetCore.Identity;
using NTEDoc.DataRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NTEDoc.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Morate uneti Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Morate uneti Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Morate uneti Ime")]
        [Display(Name = "Ime")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Morate uneti Prezime")]
        [Display(Name = "Prezime")]
        public string LastName { get; set; }

        [Display(Name = "Izaberite sektor")]
        public int? SektorId { get; set; }
        public Sector Sektor { get; set; }

        [Required(ErrorMessage = "Morate izabrati ulogu")]
        [Display(Name = "Izaberite ulogu")]
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }



    }
}
