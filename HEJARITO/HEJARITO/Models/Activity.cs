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

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "Aktivitetsnamn")]
        public string Name { get; set; }

        [Required]
        public int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Deadlinedatum")]
        public DateTime DeadlineDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }
        //public virtual ICollection<Document> Documents { get; set; }
    }
}