using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha ett namn")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "Aktivitetsnamn")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Aktivitetstyp")]
        public int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha ett startdatum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Deadlinedatum")]
        
        //TM 2018-03-09 10:04 Nullable DateTime!
        public Nullable<DateTime> DeadlineDate { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha ett slutdatum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }
        //public virtual ICollection<Document> Documents { get; set; }
    }
}