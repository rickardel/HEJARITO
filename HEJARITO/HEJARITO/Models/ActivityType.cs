using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class ActivityType
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "Activitetstypsnamn")]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}