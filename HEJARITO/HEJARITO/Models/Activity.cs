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
        public string Name { get; set; }
        [Required]
        public int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }
        //public virtual ICollection<Document> Documents { get; set; }
    }
}