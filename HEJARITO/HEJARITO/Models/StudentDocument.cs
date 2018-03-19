using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class StudentDocument
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ett dokument måste ha ett namn")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "DokumentNamn")]
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime UploadDate { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        [Required(ErrorMessage = "En fil måste väljas")]
        [StringLength(255)]
        public string FileName { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
    }
}