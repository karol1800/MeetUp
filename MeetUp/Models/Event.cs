using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MeetUp.Models
{
    public class Event
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        [Display(Name = "Nazwa:")]
        public string Name { get; set; }

        [Display(Name = "Liczba biletów:")]
        public int number { get; set; }

        [Display(Name = "Miejsce:")]
        [Required(ErrorMessage = "Pole wymagane!")]
        [StringLength(20, ErrorMessage = "Maksymalnie 20 znaków!")]
        public string palce { get; set; }

        [Display(Name = "Data:")]
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
    }
}