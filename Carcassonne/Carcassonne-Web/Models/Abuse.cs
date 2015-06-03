using System;
using System.ComponentModel.DataAnnotations;

namespace Carcassonne_Web.Models
{
    public class Abuse
    {
        [Key]
        public int ID { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Display(Name = "Reden")]
        public string Reason { get; set; }
    }
}