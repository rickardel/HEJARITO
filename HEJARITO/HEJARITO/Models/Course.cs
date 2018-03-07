using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "Kursnamn")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<Module> Modules { get; set; }

        //public virtual ICollection<Document> Documents { get; set; }

    }
}