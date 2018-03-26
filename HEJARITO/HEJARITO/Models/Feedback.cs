using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        [Required]
        [Range(0, 5, ErrorMessage = "Betyg bör vara på skala 0-5")]
        public int Grade { get; set; }
        [Required]
        public int StudentDocumentId { get; set; }
        public virtual StudentDocument StudentDocument { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    } 
}