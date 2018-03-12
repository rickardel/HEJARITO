using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "En modul måste ha ett namn")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "ModulNamn")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "En modul måste ha ett startdatum")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "En modul måste ha ett slutdatum")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }

        [Required]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        //TM 2018-03-09 11:51 Navigational property för att kunna lista alla aktiviteter inom aktuell modul
        public virtual ICollection<Activity> Activities { get; set; } //TM (denna NP har funnits redan från början)

        //public virtual ICollection<Document> Documents { get; set; }
    }
}