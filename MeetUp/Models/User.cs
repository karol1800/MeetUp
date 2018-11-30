using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetUp.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Display(Name = "Login")]
        public string login { get; set; }
        [Required(ErrorMessage = "Pole wymagane!")]
        [StringLength(20, ErrorMessage = "Imię może mieć max 20 znaków.")]

        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Potwierdź hasło")]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string confirmedpassword { get; set; }

        [Display(Name = "Nazwisko")]
        public string surname { get; set; }

        [Display(Name = "Wiek")]
        [Range(18, 125, ErrorMessage = "Wiek nie może być ujemny")]
        public int age { get; set; }

    }
}