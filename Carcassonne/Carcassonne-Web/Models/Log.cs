using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.Models
{
    public class Log
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "LogMessage")]
        public string Message { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        public LogType Category { get; set; }
        public string CategoryAttribute { get; set; }

    }

    public enum LogType
    {
        Game, Room,Security
    }
}